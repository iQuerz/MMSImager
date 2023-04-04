
namespace MMSImager
{
    public partial class Form1 : Form
    {
        ImageEngine _engine { get; set; }
        public Form1()
        {
            InitializeComponent();
            _engine = new ImageEngine();
            _engine.onImageChanged += Image_Changed;
        }

        #region Filters
        private void Image_Changed(object sender, EventArgs e)
        {
            MainPictureBox.Image = _engine.ActiveBitmap;
        }

        private void GammaButton_Click(object sender, EventArgs e)
        {
            _engine.Gamma();
        }

        private void SmoothButton_Click(object sender, EventArgs e)
        {
            _engine.Smooth();
        }

        private void EdgeDetectVerticalButton_Click(object sender, EventArgs e)
        {
            _engine.EdgeDetectVertical();
        }

        private void TimeWarpButton_Click(object sender, EventArgs e)
        {
            _engine.TimeWarp();
        }
        #endregion

        private void openButton_Click(object sender, EventArgs e)
        {
            var result = openImageDialog.ShowDialog();

            if (result == DialogResult.OK)
            { 
                _engine.LoadImage(openImageDialog.FileName);
            }
        }

        #region Edit menu
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _engine.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _engine.Redo();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            _engine.Clear();
        }
        #endregion

    }
}