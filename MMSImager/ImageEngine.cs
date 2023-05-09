using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MMSImager
{
    internal class ImageEngine
    {
        public Bitmap? ActiveBitmap => History.Count > 0 ? History[activeIndex] : null;

        private List<Bitmap> History { get; set; }
        private int activeIndex { get; set; }

        public EventHandler onImageChanged;

        public ImageEngine()
        {
            History = new List<Bitmap>()
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
        public bool LoadImage(string filepath)
        {
            if(!filepath.EndsWith(".rif", StringComparison.OrdinalIgnoreCase))
            {//not rale image format
                Add(new Bitmap(filepath));
            }
            else
            {//.rif
                byte[] bytes = File.ReadAllBytes(filepath); //limitacija - max velicina je 2gb... valjda nema tolke slike :D
                using var stream = new MemoryStream(bytes);
                using var reader = new BinaryReader(stream);

                var height = reader.ReadInt32();
                var width = reader.ReadInt32();
                var downsampled = reader.ReadBoolean();
                var dictLength = reader.ReadInt32();

                var imageBytes = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

                if(dictLength != 0) //compressed
                {
                    
                }
                else //uncompressed
                {
                    Bitmap bmp = ConvertFromYUV(height, width, downsampled, imageBytes);
                    Add(bmp);
                }
            }

            onImageChanged?.Invoke(this, EventArgs.Empty);
            return false;
        }

        public void Save(string filepath, bool compress, bool downsample)
        {
            var bitmap = ActiveBitmap;

            List<byte> bytes = new();

            bytes.AddRange(ConvertToYUV(bitmap, downsample));
            bytes.AddRange(Compress(bytes, compress));

            bytes.InsertRange(0, BitConverter.GetBytes(downsample));
            bytes.InsertRange(0, BitConverter.GetBytes(bitmap.Width));
            bytes.InsertRange(0, BitConverter.GetBytes(bitmap.Height));

            File.WriteAllBytes(filepath, bytes.ToArray());
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

        #region Logic
        private byte[] ConvertToYUV(Bitmap bitmap, bool downsample)
        {
            int bytesLength = bitmap.Width * bitmap.Height;
            bytesLength *= downsample ? 2 : 3;
            byte[] bytes = new byte[bytesLength];

            int y_index = 0;
            int u_index = bitmap.Width * bitmap.Height;
            int v_index = u_index + ((bytesLength - u_index) / 2);

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;

                    double y = 0.299*R + 0.587*G + 0.114*B;
                    bytes[y_index++] = Convert.ToByte((int)y);

                    if (!downsample)
                    {
                        double u = -0.14713*R - 0.28886*G + 0.436*B + 128;
                        bytes[u_index++] = Convert.ToByte((int)u);
                        double v = 0.615*R - 0.51498*G - 0.10001*B + 128;
                        bytes[v_index++] = Convert.ToByte((int)v);
                        continue;
                    }

                    if (i / 2 % 2 == 0)
                    {
                        if (j / 2 % 2 == 0)
                        {
                            double u = -0.14713 * R - 0.28886 * G + 0.436 * B + 128;
                            bytes[u_index++] = Convert.ToByte((int)u);
                            double v = 0.615 * R - 0.51498 * G - 0.10001 * B + 128;
                            bytes[v_index++] = Convert.ToByte((int)v);
                        }
                    }
                    else
                    {
                        if (j / 2 % 2 == 1)
                        {
                            double u = -0.14713 * R - 0.28886 * G + 0.436 * B + 128;
                            bytes[u_index++] = Convert.ToByte((int)u);
                            double v = 0.615 * R - 0.51498 * G - 0.10001 * B + 128;
                            bytes[v_index++] = Convert.ToByte((int)v);
                        }
                    }
                }
            }

            return bytes;
        }
        private List<byte> Compress(List<byte> bytes, bool compress)
        {
            if (!compress)
            {
                byte[] dictLength = BitConverter.GetBytes(0);
                bytes.InsertRange(0, dictLength);
                return bytes;
            }

            var dict = Huffman.GetHuffmanDict(bytes);
            byte[] dictBytes = Huffman.EncodeDictionary(dict);
            byte[] imageBytes = Huffman.EncodeImage(bytes, dict);

            List<byte> resultList = new();
            resultList.AddRange(BitConverter.GetBytes(dictBytes.Length));
            resultList.AddRange(dictBytes);
            resultList.AddRange(imageBytes);
            return bytes;
        }
        private List<byte> Uncompress(byte[] bytes, Dictionary<byte, BitArray> dict)
        {

        }

        private Bitmap ConvertFromYUV(int height, int width, bool downsampled, byte[] yuvBytes)
        {
            Bitmap bitmap = new Bitmap(width, height);
            int y_length = width * height;
            int uv_length = width * height;

            if(downsampled)
                uv_length /= 2;

            int y_start = 0;
            int u_start = y_length;
            int v_start = y_length + uv_length;

            for(int index = 0; index < y_length; index++)
            {
                int j = index % width;
                int i = index / width;

                byte y = yuvBytes[y_start + index];
                byte u;
                byte v;

                if (!downsampled)
                {
                    u = yuvBytes[u_start + index];
                    v = yuvBytes[v_start + index];
                }

                else
                {
                    if (i / 2 % 2 == 0)
                    {
                        if (j / 2 % 2 == 0)
                        {
                            u = yuvBytes[u_start + index / 2 + (index % 4 == 0 ? 0 : 1)];
                            v = yuvBytes[v_start + index / 2 + (index % 4 == 0 ? 0 : 1)];
                        }
                        else
                        {
                            u = yuvBytes[u_start + (index-2) / 2 + ((index - 2) % 4 == 0 ? 0 : 1)];
                            v = yuvBytes[v_start + (index-2) / 2 + ((index - 2) % 4 == 0 ? 0 : 1)];
                            //kopiram iz proslog
                        }
                    }
                    else
                    {
                        if (j / 2 % 2 == 0)
                        {
                            u = yuvBytes[u_start + (index + 2) / 2 + ((index + 2) % 4 == 0 ? 0 : 1)];
                            v = yuvBytes[v_start + (index + 2) / 2 + ((index + 2) % 4 == 0 ? 0 : 1)];
                            //kopiram iz sledeceg
                        }
                        else
                        {
                            u = yuvBytes[u_start + index / 2 + (index % 4 == 0 ? 0 : 1)];
                            v = yuvBytes[v_start + index / 2 + (index % 4 == 0 ? 0 : 1)];
                        }
                    }
                }

                double yd = y;
                double ud = u - 128;
                double vd = v - 128;

                double r = yd + 1.4075 * vd;
                double g = yd - 0.3455 * ud - (0.7169 * vd);
                double b = yd + 1.7790 * ud;

                r = Math.Max(0, Math.Min(255, r));
                g = Math.Max(0, Math.Min(255, g));
                b = Math.Max(0, Math.Min(255, b));

                bitmap.SetPixel(j, i, Color.FromArgb((int)r, (int)g, (int)b));
            }

            return bitmap;
        }
        #endregion
    }
}
