﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
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
                    imageBytes = Uncompress(dictLength, imageBytes).ToArray();

                Bitmap bmp = ConvertFromYUV(height, width, downsampled, imageBytes);
                Add(bmp);
            }

            onImageChanged?.Invoke(this, EventArgs.Empty);
            return false;
        }


        /// <summary>
        /// Saves the image to the filesystem.
        /// </summary>
        /// <param name="filepath">Full path to the file</param>
        /// <param name="compress"></param>
        /// <param name="downsample"></param>
        public void SaveImage(string filepath, bool compress, bool downsample)
        {
            var bitmap = ActiveBitmap;

            List<byte> bytes = new();

            bytes.AddRange(ConvertToYUV(bitmap, downsample));
            if (compress)
            {
                bytes = Compress(bytes);
            }
            else
            {
                byte[] dictLength = BitConverter.GetBytes(0);
                bytes.InsertRange(0, dictLength);
            }

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

        /// <summary>
        /// Converts a Bitmap object to a list of bytes, represented in YUV color space.
        /// </summary>
        /// <param name="bitmap">Bitmap of the image</param>
        /// <param name="downsample"></param>
        /// <returns>List of bytes, with first byte indicating if the image is downsampled or not.</returns>
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

                    bytes[y_index++] = getY(R, G, B);

                    if (!downsample)
                    {
                        bytes[u_index++] = getU(R, G, B);
                        bytes[v_index++] = getV(R, G, B);
                        continue;
                    }

                    if (i / 2 % 2 == 0)
                    {
                        if (j / 2 % 2 == 0)
                        {
                            bytes[u_index++] = getU(R, G, B);
                            bytes[v_index++] = getV(R, G, B);
                        }
                    }
                    else
                    {
                        if (j / 2 % 2 == 1)
                        {
                            bytes[u_index++] = getU(R, G, B);
                            bytes[v_index++] = getV(R, G, B);
                        }
                    }
                }
            }

            return bytes;
        }

        /// <summary>
        /// Compresses a list of bytes using the Huffman algorithm
        /// </summary>
        /// <param name="bytes">List of bytes to compress.</param>
        /// <param name="compress"></param>
        /// <returns>List of bytes, with first 4 bytes storing dictionary length in int32, then dictionary, and finally the compressed bytes.</returns>
        private List<byte> Compress(List<byte> bytes)
        {
            var dict = Huffman.GetHuffmanDict(bytes);
            byte[] dictBytes = Huffman.EncodeDictionary(dict);
            byte[] imageBytes = Huffman.EncodeImage(bytes, dict);

            List<byte> resultList = new();
            resultList.AddRange(BitConverter.GetBytes(dictBytes.Length));
            resultList.AddRange(dictBytes);
            resultList.AddRange(imageBytes);
            return resultList;
        }

        /// <summary>
        /// Undoes the Huffman compression.
        /// </summary>
        /// <param name="dictLen">Dictionary length. First <paramref name="dictLen"/> bytes are the dictionary bytes.</param>
        /// <param name="bytes"></param>
        /// <returns>List of bytes, the way they were before compression.</returns>
        private List<byte> Uncompress(int dictLen, byte[] bytes)
        {
            var dictBytes = new byte[dictLen];
            Array.Copy(bytes, 0, dictBytes, 0, dictLen);
            var dictionary = Huffman.DecodeDictionary(dictBytes);

            var imageBytes = new byte[bytes.Length - dictLen];
            Array.Copy(bytes, dictLen, imageBytes, 0, imageBytes.Length); //dictLen == index pocetka image bytes
            return Huffman.DecodeImage(imageBytes, dictionary);
        }

        /// <summary>
        /// Reads image yuv bytes and creates a bitmap.
        /// </summary>
        /// <param name="height">Image height</param>
        /// <param name="width">Image width</param>
        /// <param name="downsampled"></param>
        /// <param name="yuvBytes">Image bytes</param>
        /// <returns>Bitmap representing the converted image.</returns>
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
                            var next_u_index = (index + 2) / 2 + ((index + 2) % 4 == 0 ? 0 : 1);
                            if (next_u_index >= uv_length)
                                u = yuvBytes[u_start + (index - 2) / 2 + ((index - 2) % 4 == 0 ? 0 : 1)];
                            else
                                u = yuvBytes[u_start + next_u_index];

                            var next_v_index = (index + 2) / 2 + ((index + 2) % 4 == 0 ? 0 : 1);
                            if (next_v_index >= uv_length)
                                v = yuvBytes[v_start + (index - 2) / 2 + ((index - 2) % 4 == 0 ? 0 : 1)];
                            else
                                v = yuvBytes[v_start + next_v_index];
                            //kopiram iz sledeceg (gotta keep away from the index out of range exception)
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

                r = reduce(r);
                g = reduce(g);
                b = reduce(b);

                bitmap.SetPixel(j, i, Color.FromArgb((int)r, (int)g, (int)b));
            }

            return bitmap;
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Gets the Y value of a given RGB pixel
        /// </summary>
        /// <param name="R">Red pixel component</param>
        /// <param name="G">Green pixel component</param>
        /// <param name="B">Blue pixel component</param>
        /// <returns>Y value of the RGB pixel</returns>
        private byte getY(int R, int G, int B)
        {
            double y = 0.299 * R + 0.587 * G + 0.114 * B;
            return Convert.ToByte((int)reduce(y));
        }

        /// <summary>
        /// Gets the U value of a given RGB pixel
        /// </summary>
        /// <param name="R">Red pixel component</param>
        /// <param name="G">Green pixel component</param>
        /// <param name="B">Blue pixel component</param>
        /// <returns>U value of the RGB pixel</returns>
        private byte getU(int R, int G, int B)
        {
            double u = -0.14713 * R - 0.28886 * G + 0.436 * B + 128;
            return Convert.ToByte((int)reduce(u));
        }

        /// <summary>
        /// Gets the V value of a given RGB pixel
        /// </summary>
        /// <param name="R">Red pixel component</param>
        /// <param name="G">Green pixel component</param>
        /// <param name="B">Blue pixel component</param>
        /// <returns>V value of the RGB pixel</returns>
        private byte getV(int R, int G, int B)
        {
            double v = 0.615 * R - 0.51498 * G - 0.10001 * B + 128;
            return Convert.ToByte((int)reduce(v));
        }

        /// <summary>
        /// Reduces a double value between 0 and 255
        /// </summary>
        private double reduce(double value) => 
            value > 255 ? 255 : value < 0 ? 0 : value;
        #endregion
    }
}
