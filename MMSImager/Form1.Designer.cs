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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetButton = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPictureBox = new System.Windows.Forms.PictureBox();
            this.GammaButton = new System.Windows.Forms.Button();
            this.SmoothButton = new System.Windows.Forms.Button();
            this.EdgeDetectVerticalButton = new System.Windows.Forms.Button();
            this.TimeWarpButton = new System.Windows.Forms.Button();
            this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(876, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openButton,
            this.saveButton});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openButton
            // 
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(180, 22);
            this.openButton.Text = "Open";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(180, 22);
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetButton,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // resetButton
            // 
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(180, 22);
            this.resetButton.Text = "Reset";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // MainPictureBox
            // 
            this.MainPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPictureBox.Location = new System.Drawing.Point(12, 27);
            this.MainPictureBox.Name = "MainPictureBox";
            this.MainPictureBox.Size = new System.Drawing.Size(642, 642);
            this.MainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MainPictureBox.TabIndex = 1;
            this.MainPictureBox.TabStop = false;
            // 
            // GammaButton
            // 
            this.GammaButton.Location = new System.Drawing.Point(660, 79);
            this.GammaButton.Name = "GammaButton";
            this.GammaButton.Size = new System.Drawing.Size(204, 23);
            this.GammaButton.TabIndex = 2;
            this.GammaButton.Text = "Add Gamma";
            this.GammaButton.UseVisualStyleBackColor = true;
            this.GammaButton.Click += new System.EventHandler(this.GammaButton_Click);
            // 
            // SmoothButton
            // 
            this.SmoothButton.Location = new System.Drawing.Point(660, 108);
            this.SmoothButton.Name = "SmoothButton";
            this.SmoothButton.Size = new System.Drawing.Size(204, 23);
            this.SmoothButton.TabIndex = 3;
            this.SmoothButton.Text = "Add Smooth";
            this.SmoothButton.UseVisualStyleBackColor = true;
            this.SmoothButton.Click += new System.EventHandler(this.SmoothButton_Click);
            // 
            // EdgeDetectVerticalButton
            // 
            this.EdgeDetectVerticalButton.Location = new System.Drawing.Point(660, 137);
            this.EdgeDetectVerticalButton.Name = "EdgeDetectVerticalButton";
            this.EdgeDetectVerticalButton.Size = new System.Drawing.Size(204, 23);
            this.EdgeDetectVerticalButton.TabIndex = 4;
            this.EdgeDetectVerticalButton.Text = "Add Edge Detect Vertical";
            this.EdgeDetectVerticalButton.UseVisualStyleBackColor = true;
            this.EdgeDetectVerticalButton.Click += new System.EventHandler(this.EdgeDetectVerticalButton_Click);
            // 
            // TimeWarpButton
            // 
            this.TimeWarpButton.Location = new System.Drawing.Point(660, 166);
            this.TimeWarpButton.Name = "TimeWarpButton";
            this.TimeWarpButton.Size = new System.Drawing.Size(204, 23);
            this.TimeWarpButton.TabIndex = 5;
            this.TimeWarpButton.Text = "Add Time Warp";
            this.TimeWarpButton.UseVisualStyleBackColor = true;
            this.TimeWarpButton.Click += new System.EventHandler(this.TimeWarpButton_Click);
            // 
            // openImageDialog
            // 
            this.openImageDialog.DefaultExt = "jpg";
            this.openImageDialog.FileName = "openImageDialog";
            this.openImageDialog.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.png, *rif)|*.bmp;*.jpg;*.jpeg;*.png;*rif;";
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.FileName = "Image";
            this.saveImageDialog.Filter = "RIF Image|*.rif";
            this.saveImageDialog.Title = "SaveImage Rale Image";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 681);
            this.Controls.Add(this.TimeWarpButton);
            this.Controls.Add(this.EdgeDetectVerticalButton);
            this.Controls.Add(this.SmoothButton);
            this.Controls.Add(this.GammaButton);
            this.Controls.Add(this.MainPictureBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openButton;
        private ToolStripMenuItem saveButton;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem resetButton;
        private PictureBox MainPictureBox;
        private Button GammaButton;
        private Button SmoothButton;
        private Button EdgeDetectVerticalButton;
        private Button TimeWarpButton;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private OpenFileDialog openImageDialog;
        private SaveFileDialog saveImageDialog;
    }
}