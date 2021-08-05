using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OblivionBSAUncompressor
{
    public partial class ProgressForm : Form
    {
        private ProgressForm()
        {
            InitializeComponent();
        }

        public ProgressForm(List<FileListItem> fileList,
                            bool loadToMemory,
                            bool multithreaded,
                            bool useMemoryStream,
                            uint splitSize,
                            bool dummyEsp,
                            bool sameAsOrigin,
                            string destinationFolder,
                            CurrentGame currentGame) : this()
        {
            this.fileList = fileList.Where(f => f.FileStatus == FileStatus.NotProcessed || f.FileStatus == FileStatus.Error).ToList();
            this.loadToMemory = loadToMemory;
            this.multithreaded = multithreaded;
            this.useMemoryStream = useMemoryStream;
            this.splitSize = splitSize;
            this.sameAsOrigin = sameAsOrigin;
            this.destinationFolder = destinationFolder;
            this.dummyEsp = dummyEsp;
            this.currentGame = currentGame;
        }

        private readonly List<FileListItem> fileList;
        private readonly bool loadToMemory;
        private readonly bool multithreaded;
        private readonly bool useMemoryStream;
        private readonly uint splitSize;
        private readonly bool sameAsOrigin;
        private readonly string destinationFolder;
        private readonly bool dummyEsp;
        private readonly CurrentGame currentGame;
        private readonly ProgressUIVariable uiVariable = new();

        private readonly CancellationTokenSource cancellationTokenSource = new();

        private async void ProgressForm_Load(object sender, EventArgs e)
        {
            try
            {
                await DoingTask();
            }
            catch (Exception)
            {
            }
            finally
            {
                Close();
            }
        }

        private async Task DoingTask()
        {
            uiVariable.FileCount = fileList.Count;
            uiVariable.CurrentFileIndex = 0;
            uiVariable.CurrentFileName = string.Empty;
            uiVariable.BlockCount = 0;
            uiVariable.CurrentBlockIndex = 0;
            uiVariable.FileChanged = true;

            timer_GUIUpdater.Enabled = true;

            var fileProcessor = new FileProcessor(loadToMemory, multithreaded, useMemoryStream, sameAsOrigin, destinationFolder, splitSize, dummyEsp, currentGame);
            for (int i = 0; i < fileList.Count; i++)
            {
                var file = fileList[i];
                try
                {
                    // Check if BSA file does contain compressed data.
                    using (var fs = File.OpenRead(file.FilePath))
                    {
                        var inputBsa = new BSAArchive(fs);

                        uiVariable.BeginUpdate();
                        uiVariable.CurrentFileIndex = i;
                        uiVariable.CurrentFileName = file.FilePath;
                        uiVariable.BlockCount = (int)inputBsa.TotalFileCount;
                        uiVariable.CurrentBlockIndex = 0;
                        uiVariable.FileChanged = true;
                        uiVariable.EndUpdate();

                        if (fs.Length < splitSize)
                        {
                            var allUncompressed = true;
                            foreach (var item in inputBsa.GetFileRecords())
                            {
                                if (item.SelfCompressed)
                                {
                                    allUncompressed = false;
                                    break;
                                }
                            }

                            if (allUncompressed)
                            {
                                file.FileStatus = FileStatus.Skipped;
                                continue;
                            }
                        }
                    }

                    await fileProcessor.StartProcessAsync(file.FilePath, (processed) =>
                    {
                        uiVariable.CurrentBlockIndex = processed;
                    }, cancellationTokenSource.Token);

                    file.FileStatus = FileStatus.Done;
                }
                catch (Exception ex)
                {
                    file.FileStatus = FileStatus.Error;
                    file.ErrorDescription = ex.Message;
                    throw;
                }
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void timer_GUIUpdater_Tick(object sender, EventArgs e)
        {
            if (uiVariable.IsUpdating)
            {
                return;
            }

            if (uiVariable.FileChanged == true)
            {
                uiVariable.FileChanged = false;

                label_TotalProgress.Text = $"Currentfile: {uiVariable.CurrentFileName}\n{uiVariable.CurrentFileIndex} / {uiVariable.FileCount}";
                progressBar_File.Maximum = uiVariable.FileCount;
                progressBar_File.Value = uiVariable.CurrentFileIndex;
                progressBar_Current.Value = 0;
                progressBar_Current.Maximum = uiVariable.BlockCount;
            }

            progressBar_Current.Value = uiVariable.CurrentBlockIndex;
            label_CurrentProgress.Text = $"{uiVariable.CurrentBlockIndex} / {uiVariable.BlockCount}";
        }
    }
}
