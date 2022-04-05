namespace Tnt.KofaxCapture.TntCiReportingExport
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
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.OutputDirectoryGroupBox = new System.Windows.Forms.GroupBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.OutputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.OutputDirectoryLabel = new System.Windows.Forms.Label();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.DocClassLabel = new System.Windows.Forms.Label();
            this.BatchClassLabel = new System.Windows.Forms.Label();
            this.DocClassValueLabel = new System.Windows.Forms.Label();
            this.BatchClassValueLabel = new System.Windows.Forms.Label();
            this.OutputDirectoryFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.MainTableLayoutPanel.SuspendLayout();
            this.OutputDirectoryGroupBox.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            this.HeaderTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.OutputDirectoryGroupBox, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.ButtonsPanel, 0, 2);
            this.MainTableLayoutPanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(574, 181);
            this.MainTableLayoutPanel.TabIndex = 0;
            // 
            // OutputDirectoryGroupBox
            // 
            this.OutputDirectoryGroupBox.Controls.Add(this.BrowseButton);
            this.OutputDirectoryGroupBox.Controls.Add(this.OutputDirectoryTextBox);
            this.OutputDirectoryGroupBox.Controls.Add(this.OutputDirectoryLabel);
            this.OutputDirectoryGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputDirectoryGroupBox.Location = new System.Drawing.Point(7, 81);
            this.OutputDirectoryGroupBox.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.OutputDirectoryGroupBox.Name = "OutputDirectoryGroupBox";
            this.OutputDirectoryGroupBox.Size = new System.Drawing.Size(560, 44);
            this.OutputDirectoryGroupBox.TabIndex = 0;
            this.OutputDirectoryGroupBox.TabStop = false;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseButton.Location = new System.Drawing.Point(479, 11);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "#&Browse#";
            this.BrowseButton.UseVisualStyleBackColor = true;
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputDirectoryTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.OutputDirectoryTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(106, 13);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(367, 20);
            this.OutputDirectoryTextBox.TabIndex = 1;
            // 
            // OutputDirectoryLabel
            // 
            this.OutputDirectoryLabel.AutoSize = true;
            this.OutputDirectoryLabel.Location = new System.Drawing.Point(6, 16);
            this.OutputDirectoryLabel.Name = "OutputDirectoryLabel";
            this.OutputDirectoryLabel.Size = new System.Drawing.Size(101, 13);
            this.OutputDirectoryLabel.TabIndex = 0;
            this.OutputDirectoryLabel.Text = "#Output Directory:#";
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonsPanel.AutoSize = true;
            this.ButtonsPanel.Controls.Add(this.VersionLabel);
            this.ButtonsPanel.Controls.Add(this.ApplyButton);
            this.ButtonsPanel.Controls.Add(this.CloseButton);
            this.ButtonsPanel.Controls.Add(this.OkButton);
            this.ButtonsPanel.Location = new System.Drawing.Point(3, 147);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(568, 31);
            this.ButtonsPanel.TabIndex = 1;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.Enabled = false;
            this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ApplyButton.Location = new System.Drawing.Point(488, 4);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 2;
            this.ApplyButton.Text = "#&Apply#";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CloseButton.Location = new System.Drawing.Point(407, 4);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "#&Cancel#";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.OkButton.Location = new System.Drawing.Point(326, 4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "#&OK#";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.AutoSize = true;
            this.HeaderPanel.Controls.Add(this.HeaderTableLayoutPanel);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(3, 3);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(568, 72);
            this.HeaderPanel.TabIndex = 5;
            // 
            // HeaderTableLayoutPanel
            // 
            this.HeaderTableLayoutPanel.AutoSize = true;
            this.HeaderTableLayoutPanel.ColumnCount = 2;
            this.HeaderTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.HeaderTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.HeaderTableLayoutPanel.Controls.Add(this.NameLabel, 0, 2);
            this.HeaderTableLayoutPanel.Controls.Add(this.NameTextBox, 1, 2);
            this.HeaderTableLayoutPanel.Controls.Add(this.DocClassLabel, 0, 1);
            this.HeaderTableLayoutPanel.Controls.Add(this.BatchClassLabel, 0, 0);
            this.HeaderTableLayoutPanel.Controls.Add(this.DocClassValueLabel, 1, 1);
            this.HeaderTableLayoutPanel.Controls.Add(this.BatchClassValueLabel, 1, 0);
            this.HeaderTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderTableLayoutPanel.Name = "HeaderTableLayoutPanel";
            this.HeaderTableLayoutPanel.RowCount = 3;
            this.HeaderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.HeaderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.HeaderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.HeaderTableLayoutPanel.Size = new System.Drawing.Size(568, 72);
            this.HeaderTableLayoutPanel.TabIndex = 0;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(3, 46);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.NameLabel.Size = new System.Drawing.Size(57, 23);
            this.NameLabel.TabIndex = 4;
            this.NameLabel.Text = "#Name:#";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameTextBox.Location = new System.Drawing.Point(115, 49);
            this.NameTextBox.MaxLength = 32;
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(464, 20);
            this.NameTextBox.TabIndex = 5;
            // 
            // DocClassLabel
            // 
            this.DocClassLabel.AutoSize = true;
            this.DocClassLabel.Location = new System.Drawing.Point(3, 23);
            this.DocClassLabel.Name = "DocClassLabel";
            this.DocClassLabel.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.DocClassLabel.Size = new System.Drawing.Size(106, 23);
            this.DocClassLabel.TabIndex = 2;
            this.DocClassLabel.Text = "#Document Class:#";
            // 
            // BatchClassLabel
            // 
            this.BatchClassLabel.AutoSize = true;
            this.BatchClassLabel.Location = new System.Drawing.Point(3, 0);
            this.BatchClassLabel.Name = "BatchClassLabel";
            this.BatchClassLabel.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.BatchClassLabel.Size = new System.Drawing.Size(85, 23);
            this.BatchClassLabel.TabIndex = 0;
            this.BatchClassLabel.Text = "#Batch Class:#";
            // 
            // DocClassValueLabel
            // 
            this.DocClassValueLabel.AutoSize = true;
            this.DocClassValueLabel.Location = new System.Drawing.Point(115, 23);
            this.DocClassValueLabel.Name = "DocClassValueLabel";
            this.DocClassValueLabel.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.DocClassValueLabel.Size = new System.Drawing.Size(72, 23);
            this.DocClassValueLabel.TabIndex = 3;
            this.DocClassValueLabel.Text = "<Doc Class>";
            // 
            // BatchClassValueLabel
            // 
            this.BatchClassValueLabel.AutoSize = true;
            this.BatchClassValueLabel.Location = new System.Drawing.Point(115, 0);
            this.BatchClassValueLabel.Name = "BatchClassValueLabel";
            this.BatchClassValueLabel.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.BatchClassValueLabel.Size = new System.Drawing.Size(80, 23);
            this.BatchClassValueLabel.TabIndex = 1;
            this.BatchClassValueLabel.Text = "<Batch Class>";
            // 
            // VersionLabel
            // 
            this.VersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.VersionLabel.Location = new System.Drawing.Point(3, 9);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(88, 13);
            this.VersionLabel.TabIndex = 5;
            this.VersionLabel.Text = "#Version x.x.x.x#";
            // 
            // MainForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(574, 181);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(590, 220);
            this.Name = "MainForm";
            this.Text = "#Avatar Ops Admin A6 Export#";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.MainTableLayoutPanel.PerformLayout();
            this.OutputDirectoryGroupBox.ResumeLayout(false);
            this.OutputDirectoryGroupBox.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.HeaderTableLayoutPanel.ResumeLayout(false);
            this.HeaderTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.TableLayoutPanel HeaderTableLayoutPanel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label DocClassLabel;
        private System.Windows.Forms.Label BatchClassLabel;
        private System.Windows.Forms.Label DocClassValueLabel;
        private System.Windows.Forms.Label BatchClassValueLabel;
        private System.Windows.Forms.GroupBox OutputDirectoryGroupBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox OutputDirectoryTextBox;
        private System.Windows.Forms.Label OutputDirectoryLabel;
        private System.Windows.Forms.FolderBrowserDialog OutputDirectoryFolderBrowserDialog;
        private System.Windows.Forms.Label VersionLabel;
    }
}

