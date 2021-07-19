using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OblivionBSAUncompressor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private List<FileListItem> fileList = new();
        private HashSet<string> addedFiles = new();

        private bool tryAddFile(string file)
        {
            if (addedFiles.Add(file))
            {
                fileList.Add(new FileListItem
                {
                    FilePath = file,
                    FileStatus = FileStatus.NotProcessed
                });

                return true;
            }

            return false;
        }

        private bool tryRemoveFile(string file)
        {
            if (addedFiles.Remove(file))
            {
                var index = fileList.FindIndex(f => f.FilePath == file);
                fileList.RemoveAt(index);

                return true;
            }

            return false;
        }

        private void RefreshFileList()
        {
            listView_Files.BeginUpdate();
            listView_Files.Items.Clear();
            foreach (var item in fileList)
            {
                var listViewItem = listView_Files.Items.Add(item.FilePath);

                switch (item.FileStatus)
                {
                    case FileStatus.NotProcessed:
                    case FileStatus.Done:
                    case FileStatus.Skipped:
                        listViewItem.SubItems.Add(item.FileStatus.ToString());
                        break;
                    case FileStatus.Error:
                        listViewItem.SubItems.Add(item.ErrorDescription);
                        break;
                    default:
                        break;
                }
            }

            listView_Files.EndUpdate();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var bsaFiles = fileNames.SelectMany(path => Directory.Exists(path) ? Directory.GetFiles(path) : new string[] { path })
                                    .Where(path => Path.GetExtension(path).ToLower() == ".bsa");
            var needUpdate = false;
            foreach (var file in bsaFiles)
            {
                needUpdate |= tryAddFile(file);
            }

            if (needUpdate)
            {
                RefreshFileList();
            }
        }

        private void button_SelectFolder_Click(object sender, EventArgs e)
        {
            using var folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                textBox_Folder.Text = folderBrowser.SelectedPath;
            }
        }

        private void checkBox_SameAsOriginalFolder_CheckedChanged(object sender, EventArgs e)
        {
            var sameAsOriginal = checkBox_SameAsOriginalFolder.Checked;
            textBox_Folder.Enabled = !sameAsOriginal;
            button_SelectFolder.Enabled = !sameAsOriginal;
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            var sameAsOriginal = checkBox_SameAsOriginalFolder.Checked;
            if (!sameAsOriginal)
            {
                if (!Directory.Exists(textBox_Folder.Text))
                {
                    MessageBox.Show("Please provide a valid folder path");
                    return;
                }
            }

            var progressForm = new ProgressForm(fileList,
                                                checkBox_LoadWholeFileToMemory.Checked,
                                                checkBox_Multithreaded.Checked,
                                                checkBox_UseMemoryStream.Checked,
                                                (uint)(1024 * 1024 * 1024 * numericUpDown_BsaSplitSize.Value),
                                                checkBox_DummyPluginGeneration.Checked,
                                                sameAsOriginal,
                                                textBox_Folder.Text);

            progressForm.ShowDialog(this);

            RefreshFileList();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            checkBox_SameAsOriginalFolder.Checked = true;
            numericUpDown_BsaSplitSize.MouseWheel += NumericUpDown_BsaSplitSize_MouseWheel;
        }

        private void NumericUpDown_BsaSplitSize_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs hme)
                hme.Handled = true;

            if (e.Delta > 0 && numericUpDown_BsaSplitSize.Value < numericUpDown_BsaSplitSize.Maximum)
                numericUpDown_BsaSplitSize.Value += numericUpDown_BsaSplitSize.Increment;
            else if (e.Delta < 0 && numericUpDown_BsaSplitSize.Value > numericUpDown_BsaSplitSize.Minimum)
                numericUpDown_BsaSplitSize.Value -= numericUpDown_BsaSplitSize.Increment;
        }

        private void toolStripMenuItem_RemoveSelected_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView_Files.SelectedItems.Count; i++)
            {
                tryRemoveFile(listView_Files.SelectedItems[i].Text);
            }

            RefreshFileList();
        }

        private void toolStripMenuItem_RemoveAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView_Files.Items.Count; i++)
            {
                tryRemoveFile(listView_Files.Items[i].Text);
            }

            RefreshFileList();
        }

        private void listView_Files_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var haveSelection = listView_Files.SelectedItems.Count > 0;
                var haveItems = listView_Files.Items.Count > 0;
                toolStripMenuItem_RemoveSelected.Enabled = haveSelection;
                toolStripMenuItem_RemoveAll.Enabled = haveItems;
            }
        }
    }
}
