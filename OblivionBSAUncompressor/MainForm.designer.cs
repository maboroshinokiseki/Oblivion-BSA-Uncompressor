
namespace OblivionBSAUncompressor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel_Main = new System.Windows.Forms.TableLayoutPanel();
            this.listView_Files = new System.Windows.Forms.ListView();
            this.fileNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_FileList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_RemoveSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_RemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox_UseMemoryStream = new System.Windows.Forms.CheckBox();
            this.checkBox_Multithreaded = new System.Windows.Forms.CheckBox();
            this.checkBox_LoadWholeFileToMemory = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel_ResultFolder = new System.Windows.Forms.TableLayoutPanel();
            this.label_Folder = new System.Windows.Forms.Label();
            this.textBox_Folder = new System.Windows.Forms.TextBox();
            this.button_SelectFolder = new System.Windows.Forms.Button();
            this.checkBox_SameAsOriginalFolder = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label_BsaSplitDesc = new System.Windows.Forms.Label();
            this.numericUpDown_BsaSplitSize = new System.Windows.Forms.NumericUpDown();
            this.label_BsaSplitGB = new System.Windows.Forms.Label();
            this.button_Start = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_DummyPluginGeneration = new System.Windows.Forms.CheckBox();
            this.comboBox_CurrentGame = new System.Windows.Forms.ComboBox();
            this.toolTip_MainForm = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel_Main.SuspendLayout();
            this.contextMenuStrip_FileList.SuspendLayout();
            this.tableLayoutPanel_ResultFolder.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_BsaSplitSize)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_Main
            // 
            this.tableLayoutPanel_Main.AutoSize = true;
            this.tableLayoutPanel_Main.ColumnCount = 1;
            this.tableLayoutPanel_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Main.Controls.Add(this.listView_Files, 0, 0);
            this.tableLayoutPanel_Main.Controls.Add(this.checkBox_UseMemoryStream, 0, 4);
            this.tableLayoutPanel_Main.Controls.Add(this.checkBox_Multithreaded, 0, 3);
            this.tableLayoutPanel_Main.Controls.Add(this.checkBox_LoadWholeFileToMemory, 0, 2);
            this.tableLayoutPanel_Main.Controls.Add(this.tableLayoutPanel_ResultFolder, 0, 1);
            this.tableLayoutPanel_Main.Controls.Add(this.flowLayoutPanel1, 0, 5);
            this.tableLayoutPanel_Main.Controls.Add(this.button_Start, 0, 7);
            this.tableLayoutPanel_Main.Controls.Add(this.flowLayoutPanel2, 0, 6);
            this.tableLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_Main.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel_Main.Name = "tableLayoutPanel_Main";
            this.tableLayoutPanel_Main.RowCount = 8;
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Main.Size = new System.Drawing.Size(600, 471);
            this.tableLayoutPanel_Main.TabIndex = 1;
            // 
            // listView_Files
            // 
            this.listView_Files.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileNames,
            this.status});
            this.listView_Files.ContextMenuStrip = this.contextMenuStrip_FileList;
            this.listView_Files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Files.FullRowSelect = true;
            this.listView_Files.HideSelection = false;
            this.listView_Files.Location = new System.Drawing.Point(2, 2);
            this.listView_Files.Margin = new System.Windows.Forms.Padding(2);
            this.listView_Files.Name = "listView_Files";
            this.listView_Files.Size = new System.Drawing.Size(596, 290);
            this.listView_Files.TabIndex = 0;
            this.listView_Files.UseCompatibleStateImageBehavior = false;
            this.listView_Files.View = System.Windows.Forms.View.Details;
            this.listView_Files.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_Files_MouseUp);
            // 
            // fileNames
            // 
            this.fileNames.Text = "Files";
            this.fileNames.Width = 512;
            // 
            // status
            // 
            this.status.Text = "Status";
            this.status.Width = 687;
            // 
            // contextMenuStrip_FileList
            // 
            this.contextMenuStrip_FileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_RemoveSelected,
            this.toolStripMenuItem_RemoveAll});
            this.contextMenuStrip_FileList.Name = "contextMenuStrip_FileList";
            this.contextMenuStrip_FileList.Size = new System.Drawing.Size(165, 48);
            // 
            // toolStripMenuItem_RemoveSelected
            // 
            this.toolStripMenuItem_RemoveSelected.Name = "toolStripMenuItem_RemoveSelected";
            this.toolStripMenuItem_RemoveSelected.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem_RemoveSelected.Text = "Remove Selected";
            this.toolStripMenuItem_RemoveSelected.Click += new System.EventHandler(this.toolStripMenuItem_RemoveSelected_Click);
            // 
            // toolStripMenuItem_RemoveAll
            // 
            this.toolStripMenuItem_RemoveAll.Name = "toolStripMenuItem_RemoveAll";
            this.toolStripMenuItem_RemoveAll.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem_RemoveAll.Text = "Remove All";
            this.toolStripMenuItem_RemoveAll.Click += new System.EventHandler(this.toolStripMenuItem_RemoveAll_Click);
            // 
            // checkBox_UseMemoryStream
            // 
            this.checkBox_UseMemoryStream.AutoSize = true;
            this.checkBox_UseMemoryStream.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_UseMemoryStream.Location = new System.Drawing.Point(2, 366);
            this.checkBox_UseMemoryStream.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_UseMemoryStream.Name = "checkBox_UseMemoryStream";
            this.checkBox_UseMemoryStream.Size = new System.Drawing.Size(596, 17);
            this.checkBox_UseMemoryStream.TabIndex = 3;
            this.checkBox_UseMemoryStream.Text = "Save decompressed file in memory before finish decompression";
            this.toolTip_MainForm.SetToolTip(this.checkBox_UseMemoryStream, resources.GetString("checkBox_UseMemoryStream.ToolTip"));
            this.checkBox_UseMemoryStream.UseVisualStyleBackColor = true;
            // 
            // checkBox_Multithreaded
            // 
            this.checkBox_Multithreaded.AutoSize = true;
            this.checkBox_Multithreaded.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_Multithreaded.Location = new System.Drawing.Point(2, 345);
            this.checkBox_Multithreaded.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_Multithreaded.Name = "checkBox_Multithreaded";
            this.checkBox_Multithreaded.Size = new System.Drawing.Size(596, 17);
            this.checkBox_Multithreaded.TabIndex = 2;
            this.checkBox_Multithreaded.Text = "Decompress multiple blocks at the same time";
            this.toolTip_MainForm.SetToolTip(this.checkBox_Multithreaded, "May improve performance, but will increase cpu and ram usage.");
            this.checkBox_Multithreaded.UseVisualStyleBackColor = true;
            // 
            // checkBox_LoadWholeFileToMemory
            // 
            this.checkBox_LoadWholeFileToMemory.AutoSize = true;
            this.checkBox_LoadWholeFileToMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_LoadWholeFileToMemory.Location = new System.Drawing.Point(2, 324);
            this.checkBox_LoadWholeFileToMemory.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_LoadWholeFileToMemory.Name = "checkBox_LoadWholeFileToMemory";
            this.checkBox_LoadWholeFileToMemory.Size = new System.Drawing.Size(596, 17);
            this.checkBox_LoadWholeFileToMemory.TabIndex = 1;
            this.checkBox_LoadWholeFileToMemory.Text = "Load whole bsa file to memory";
            this.toolTip_MainForm.SetToolTip(this.checkBox_LoadWholeFileToMemory, resources.GetString("checkBox_LoadWholeFileToMemory.ToolTip"));
            this.checkBox_LoadWholeFileToMemory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_ResultFolder
            // 
            this.tableLayoutPanel_ResultFolder.ColumnCount = 4;
            this.tableLayoutPanel_ResultFolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_ResultFolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_ResultFolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_ResultFolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_ResultFolder.Controls.Add(this.label_Folder, 0, 0);
            this.tableLayoutPanel_ResultFolder.Controls.Add(this.textBox_Folder, 1, 0);
            this.tableLayoutPanel_ResultFolder.Controls.Add(this.button_SelectFolder, 2, 0);
            this.tableLayoutPanel_ResultFolder.Controls.Add(this.checkBox_SameAsOriginalFolder, 3, 0);
            this.tableLayoutPanel_ResultFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_ResultFolder.Location = new System.Drawing.Point(2, 296);
            this.tableLayoutPanel_ResultFolder.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel_ResultFolder.Name = "tableLayoutPanel_ResultFolder";
            this.tableLayoutPanel_ResultFolder.RowCount = 1;
            this.tableLayoutPanel_ResultFolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_ResultFolder.Size = new System.Drawing.Size(596, 24);
            this.tableLayoutPanel_ResultFolder.TabIndex = 5;
            // 
            // label_Folder
            // 
            this.label_Folder.AutoSize = true;
            this.label_Folder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Folder.Location = new System.Drawing.Point(2, 0);
            this.label_Folder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Folder.Name = "label_Folder";
            this.label_Folder.Size = new System.Drawing.Size(39, 24);
            this.label_Folder.TabIndex = 0;
            this.label_Folder.Text = "Folder:";
            this.label_Folder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_Folder
            // 
            this.textBox_Folder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Folder.Location = new System.Drawing.Point(45, 2);
            this.textBox_Folder.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Folder.Name = "textBox_Folder";
            this.textBox_Folder.Size = new System.Drawing.Size(376, 20);
            this.textBox_Folder.TabIndex = 1;
            // 
            // button_SelectFolder
            // 
            this.button_SelectFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_SelectFolder.Location = new System.Drawing.Point(425, 2);
            this.button_SelectFolder.Margin = new System.Windows.Forms.Padding(2);
            this.button_SelectFolder.Name = "button_SelectFolder";
            this.button_SelectFolder.Size = new System.Drawing.Size(56, 20);
            this.button_SelectFolder.TabIndex = 2;
            this.button_SelectFolder.Text = "...";
            this.button_SelectFolder.UseVisualStyleBackColor = true;
            this.button_SelectFolder.Click += new System.EventHandler(this.button_SelectFolder_Click);
            // 
            // checkBox_SameAsOriginalFolder
            // 
            this.checkBox_SameAsOriginalFolder.AutoSize = true;
            this.checkBox_SameAsOriginalFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_SameAsOriginalFolder.Location = new System.Drawing.Point(485, 2);
            this.checkBox_SameAsOriginalFolder.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_SameAsOriginalFolder.Name = "checkBox_SameAsOriginalFolder";
            this.checkBox_SameAsOriginalFolder.Size = new System.Drawing.Size(109, 20);
            this.checkBox_SameAsOriginalFolder.TabIndex = 3;
            this.checkBox_SameAsOriginalFolder.Text = "Same as original";
            this.toolTip_MainForm.SetToolTip(this.checkBox_SameAsOriginalFolder, "If this is checked, the uncompressed bsa files will replace the original ones.");
            this.checkBox_SameAsOriginalFolder.UseVisualStyleBackColor = true;
            this.checkBox_SameAsOriginalFolder.CheckedChanged += new System.EventHandler(this.checkBox_SameAsOriginalFolder_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label_BsaSplitDesc);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDown_BsaSplitSize);
            this.flowLayoutPanel1.Controls.Add(this.label_BsaSplitGB);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 387);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(596, 26);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // label_BsaSplitDesc
            // 
            this.label_BsaSplitDesc.Location = new System.Drawing.Point(2, 0);
            this.label_BsaSplitDesc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BsaSplitDesc.Name = "label_BsaSplitDesc";
            this.label_BsaSplitDesc.Size = new System.Drawing.Size(86, 18);
            this.label_BsaSplitDesc.TabIndex = 0;
            this.label_BsaSplitDesc.Text = "Split bsa files by ";
            this.label_BsaSplitDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDown_BsaSplitSize
            // 
            this.numericUpDown_BsaSplitSize.DecimalPlaces = 1;
            this.numericUpDown_BsaSplitSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_BsaSplitSize.Location = new System.Drawing.Point(92, 2);
            this.numericUpDown_BsaSplitSize.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_BsaSplitSize.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_BsaSplitSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_BsaSplitSize.Name = "numericUpDown_BsaSplitSize";
            this.numericUpDown_BsaSplitSize.Size = new System.Drawing.Size(90, 20);
            this.numericUpDown_BsaSplitSize.TabIndex = 1;
            this.toolTip_MainForm.SetToolTip(this.numericUpDown_BsaSplitSize, "because oblivion cannot handle bsa files bigger than 2GB, we have to split bsa fi" +
        "les, default value is 1.8 GB instead of 2 GB just in case, you can set it from 0" +
        ".1 GB to 2 GB if you like.");
            this.numericUpDown_BsaSplitSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // label_BsaSplitGB
            // 
            this.label_BsaSplitGB.Location = new System.Drawing.Point(186, 0);
            this.label_BsaSplitGB.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BsaSplitGB.Name = "label_BsaSplitGB";
            this.label_BsaSplitGB.Size = new System.Drawing.Size(52, 18);
            this.label_BsaSplitGB.TabIndex = 2;
            this.label_BsaSplitGB.Text = "GB";
            this.label_BsaSplitGB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(2, 449);
            this.button_Start.Margin = new System.Windows.Forms.Padding(2);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(56, 20);
            this.button_Start.TabIndex = 4;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.checkBox_DummyPluginGeneration);
            this.flowLayoutPanel2.Controls.Add(this.comboBox_CurrentGame);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 418);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(594, 26);
            this.flowLayoutPanel2.TabIndex = 8;
            // 
            // checkBox_DummyPluginGeneration
            // 
            this.checkBox_DummyPluginGeneration.AutoSize = true;
            this.checkBox_DummyPluginGeneration.Checked = true;
            this.checkBox_DummyPluginGeneration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DummyPluginGeneration.Location = new System.Drawing.Point(2, 2);
            this.checkBox_DummyPluginGeneration.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_DummyPluginGeneration.Name = "checkBox_DummyPluginGeneration";
            this.checkBox_DummyPluginGeneration.Size = new System.Drawing.Size(219, 17);
            this.checkBox_DummyPluginGeneration.TabIndex = 7;
            this.checkBox_DummyPluginGeneration.Text = "Generate dummy esp files for bsa loading";
            this.toolTip_MainForm.SetToolTip(this.checkBox_DummyPluginGeneration, "Generate dummy esp files for splited bsa files, otherwise you will have to add bs" +
        "a files to Oblivion.ini by yourself.");
            this.checkBox_DummyPluginGeneration.UseVisualStyleBackColor = true;
            this.checkBox_DummyPluginGeneration.CheckedChanged += new System.EventHandler(this.checkBox_DummyPluginGeneration_CheckedChanged);
            // 
            // comboBox_CurrentGame
            // 
            this.comboBox_CurrentGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CurrentGame.FormattingEnabled = true;
            this.comboBox_CurrentGame.Location = new System.Drawing.Point(226, 3);
            this.comboBox_CurrentGame.Name = "comboBox_CurrentGame";
            this.comboBox_CurrentGame.Size = new System.Drawing.Size(121, 21);
            this.comboBox_CurrentGame.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 473);
            this.Controls.Add(this.tableLayoutPanel_Main);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Oblivion BSA Uncompressor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.tableLayoutPanel_Main.ResumeLayout(false);
            this.tableLayoutPanel_Main.PerformLayout();
            this.contextMenuStrip_FileList.ResumeLayout(false);
            this.tableLayoutPanel_ResultFolder.ResumeLayout(false);
            this.tableLayoutPanel_ResultFolder.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_BsaSplitSize)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Main;
        private System.Windows.Forms.ListView listView_Files;
        private System.Windows.Forms.ColumnHeader fileNames;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.CheckBox checkBox_LoadWholeFileToMemory;
        private System.Windows.Forms.CheckBox checkBox_Multithreaded;
        private System.Windows.Forms.CheckBox checkBox_UseMemoryStream;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_ResultFolder;
        private System.Windows.Forms.Label label_Folder;
        private System.Windows.Forms.TextBox textBox_Folder;
        private System.Windows.Forms.Button button_SelectFolder;
        private System.Windows.Forms.CheckBox checkBox_SameAsOriginalFolder;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label_BsaSplitDesc;
        private System.Windows.Forms.NumericUpDown numericUpDown_BsaSplitSize;
        private System.Windows.Forms.Label label_BsaSplitGB;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_FileList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_RemoveSelected;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_RemoveAll;
        private System.Windows.Forms.ToolTip toolTip_MainForm;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.CheckBox checkBox_DummyPluginGeneration;
        private System.Windows.Forms.ComboBox comboBox_CurrentGame;
    }
}

