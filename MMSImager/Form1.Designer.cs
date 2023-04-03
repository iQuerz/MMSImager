namespace MMSImager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openButton = new ToolStripMenuItem();
            saveButton = new ToolStripMenuItem();
            compressButton = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            resetButton = new ToolStripMenuItem();
            MainPictureBox = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainPictureBox).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(995, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openButton, saveButton, compressButton });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openButton
            // 
            openButton.Name = "openButton";
            openButton.Size = new Size(177, 22);
            openButton.Text = "Open";
            // 
            // saveButton
            // 
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(177, 22);
            saveButton.Text = "Save";
            // 
            // compressButton
            // 
            compressButton.Name = "compressButton";
            compressButton.Size = new Size(177, 22);
            compressButton.Text = "Compress and Save";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { resetButton });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // resetButton
            // 
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(134, 22);
            resetButton.Text = "Reset filters";
            // 
            // MainPictureBox
            // 
            MainPictureBox.Location = new Point(12, 27);
            MainPictureBox.Name = "MainPictureBox";
            MainPictureBox.Size = new Size(642, 642);
            MainPictureBox.TabIndex = 1;
            MainPictureBox.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(995, 681);
            Controls.Add(MainPictureBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "MMS Imager";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MainPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openButton;
        private ToolStripMenuItem saveButton;
        private ToolStripMenuItem compressButton;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem resetButton;
        private PictureBox MainPictureBox;
    }
}