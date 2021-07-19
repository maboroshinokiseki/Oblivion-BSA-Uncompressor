
namespace OblivionBSAUncompressor
{
    partial class ProgressForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_TotalProgress = new System.Windows.Forms.Label();
            this.label_CurrentProgress = new System.Windows.Forms.Label();
            this.progressBar_File = new System.Windows.Forms.ProgressBar();
            this.progressBar_Current = new System.Windows.Forms.ProgressBar();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.timer_GUIUpdater = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label_TotalProgress, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_CurrentProgress, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBar_File, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.progressBar_Current, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.button_Cancel, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(622, 209);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label_TotalProgress
            // 
            this.label_TotalProgress.AutoSize = true;
            this.label_TotalProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_TotalProgress.Location = new System.Drawing.Point(3, 0);
            this.label_TotalProgress.Name = "label_TotalProgress";
            this.label_TotalProgress.Size = new System.Drawing.Size(616, 40);
            this.label_TotalProgress.TabIndex = 0;
            this.label_TotalProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_CurrentProgress
            // 
            this.label_CurrentProgress.AutoSize = true;
            this.label_CurrentProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_CurrentProgress.Location = new System.Drawing.Point(3, 80);
            this.label_CurrentProgress.Name = "label_CurrentProgress";
            this.label_CurrentProgress.Size = new System.Drawing.Size(616, 40);
            this.label_CurrentProgress.TabIndex = 1;
            this.label_CurrentProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar_File
            // 
            this.progressBar_File.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar_File.Location = new System.Drawing.Point(3, 43);
            this.progressBar_File.Name = "progressBar_File";
            this.progressBar_File.Size = new System.Drawing.Size(616, 34);
            this.progressBar_File.TabIndex = 2;
            // 
            // progressBar_Current
            // 
            this.progressBar_Current.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar_Current.Location = new System.Drawing.Point(3, 123);
            this.progressBar_Current.Name = "progressBar_Current";
            this.progressBar_Current.Size = new System.Drawing.Size(616, 34);
            this.progressBar_Current.TabIndex = 3;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_Cancel.Location = new System.Drawing.Point(273, 170);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 29);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // timer_GUIUpdater
            // 
            this.timer_GUIUpdater.Interval = 10;
            this.timer_GUIUpdater.Tick += new System.EventHandler(this.timer_GUIUpdater_Tick);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 209);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "ProgressForm";
            this.Text = "ProgressForm";
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_TotalProgress;
        private System.Windows.Forms.Label label_CurrentProgress;
        private System.Windows.Forms.ProgressBar progressBar_File;
        private System.Windows.Forms.ProgressBar progressBar_Current;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Timer timer_GUIUpdater;
    }
}