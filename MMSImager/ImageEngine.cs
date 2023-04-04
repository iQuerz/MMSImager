using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MMSImager
{
    internal class ImageEngine
    {
        public Bitmap ActiveBitmap => History[activeIndex];

        private List<Bitmap> History { get; set; }
        private int activeIndex { get; set; }

        public EventHandler onImageChanged;

        public ImageEngine()
        {
            History = new List<Bitmap>
            {
                new Bitmap(10, 10)
            };
            activeIndex = 0;
        }

        #region History Stack - App bitmaps
        public void Undo()
        {
            if (activeIndex > 0)
            {
                activeIndex--; ;
            }
            onImageChanged?.Invoke(this, EventArgs.Empty);
        }
        public void Redo()
        {
            if (History.Count > activeIndex + 1)
            {
                activeIndex++;
            }
            onImageChanged?.Invoke(this, EventArgs.Empty);
        }
        public void Add(Bitmap bmp)
        {
            History.Insert(++activeIndex, bmp);
            for (int i = activeIndex + 1; i < History.Count; i++)
            {
                History.RemoveAt(i);
            }
        }
        public void Clear()
        {
            History = new List<Bitmap>
            {
                new Bitmap(10, 10)
            };
            activeIndex = 0;
            onImageChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Filesystem
        public bool LoadImage(string filename)
        {
            if(!filename.EndsWith(".rif", StringComparison.OrdinalIgnoreCase))
            {//not rale image format
                Add(new Bitmap(filename));
            }
            else
            {//.rif

            }

            onImageChanged?.Invoke(this, EventArgs.Empty);
            return false;
        }

        public Bitmap CompressSave()
        {
            return null;
        }

        public bool Save(string filename)
        {
            return false;
        }
        #endregion

        #region Filters
        public void Gamma()
        {

            onImageChanged?.Invoke(this, EventArgs.Empty);
        }
        public void Smooth()
        {

            onImageChanged?.Invoke(this, EventArgs.Empty);
        }
        public void EdgeDetectVertical()
        {

            onImageChanged?.Invoke(this, EventArgs.Empty);
        }
        public void TimeWarp()
        {

            onImageChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

    }
}
