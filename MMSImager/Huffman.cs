using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSImager
{
    internal class Huffman
    {
        #region Generation
        public static Dictionary<byte, BitArray> GetHuffmanDict(List<byte> bytes)
        {
            var frequency = new Dictionary<byte, int>();
            foreach (var b in bytes)
            {
                if (!frequency.ContainsKey(b))
                {
                    frequency[b] = 0;
                }
                frequency[b]++;
            }

            var heap = new PriorityQueue<HuffmanNode, int>();
            foreach (var pair in frequency)
            {
                heap.Enqueue(new HuffmanNode() { Byte = pair.Key, Frequency = pair.Value }, pair.Value);
            }

            while (heap.Count > 1)
            {
                var left = heap.Dequeue();
                var right = heap.Dequeue();
                heap.Enqueue(new HuffmanNode() { Left = left, Right = right, Frequency = left.Frequency + right.Frequency }, left.Frequency + right.Frequency);
            }

            var root = heap.Dequeue();
            var dictionary = new Dictionary<byte, BitArray>();
            Traverse(root, new BitArray(0), dictionary);

            return dictionary;
        }

        private static void Traverse(HuffmanNode node, BitArray bits, Dictionary<byte, BitArray> dictionary)
        {
            if (node.Left == null && node.Right == null)
            {
                dictionary[node.Byte] = bits;
                return;
            }

            if (node.Left != null)
            {
                var leftBits = new BitArray(bits);
                leftBits.Length++;
                leftBits[leftBits.Length - 1] = false;
                Traverse(node.Left, leftBits, dictionary);
            }

            if (node.Right != null)
            {
                var rightBits = new BitArray(bits);
                rightBits.Length++;
                rightBits[rightBits.Length - 1] = true;
                Traverse(node.Right, rightBits, dictionary);
            }
        }

        private class HuffmanNode
        {
            public byte Byte { get; set; }
            public int Frequency { get; set; }
            public HuffmanNode? Left { get; set; }
            public HuffmanNode? Right { get; set; }
        }

        #endregion

        #region Encoding Image
        public static byte[] EncodeImage(List<byte> imageBytes, Dictionary<byte, BitArray> dictionary)
        {
            var encodedBitlist = new List<bool>();
            foreach (var imageByte in imageBytes)
            {
                var encodedBits = dictionary[imageByte];
                encodedBitlist.AddRange(encodedBits.Cast<bool>());
            }

            var encodedBitArray = new BitArray(encodedBitlist.ToArray());
            int byteCount = (encodedBitArray.Length + 7) / 8;
            byte[] byteArray = new byte[byteCount];
            encodedBitArray.CopyTo(byteArray, 0);
            return byteArray;
        }

        public static List<byte> DecodeImage(byte[] encodedBytes, Dictionary<byte, BitArray> dictionary)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var encodedBitArray = new BitArray(encodedBytes);
            var decodedBytes = new List<byte>();
            var bitsBuffer = new List<bool>();

            Dictionary<RaleBitArray, byte> efficientDict = dictionary.ToDictionary(pair => new RaleBitArray(pair.Value), pair => pair.Key);

            for (int i = 0; i < encodedBitArray.Length; i++)
            {
                bitsBuffer.Add(encodedBitArray[i]);
                var bitArrayToCompare = new BitArray(bitsBuffer.ToArray());

                var decodedPair = dictionary.FirstOrDefault(x => SameBits(x.Value, bitArrayToCompare));
                if (decodedPair.Value is not null)
                {
                    decodedBytes.Add(decodedPair.Key);
                    bitsBuffer.Clear();
                }
            }

            stopwatch.Stop();
            return decodedBytes;
        }

        #endregion

        #region Encoding Dictionary
        public static byte[] EncodeDictionary(Dictionary<byte, BitArray> dictionary)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(dictionary.Count);
            foreach (var pair in dictionary)
            {
                writer.Write(pair.Key);

                int byteCount = (pair.Value.Length + 7) / 8;
                byte[] bytes = new byte[byteCount];
                pair.Value.CopyTo(bytes, 0);

                writer.Write(pair.Value.Length);
                writer.Write(bytes);
            }

            return stream.ToArray();
        }
        public static Dictionary<byte, BitArray> DecodeDictionary(byte[] bytes)
        {
            var dictionary = new Dictionary<byte, BitArray>();
            var stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);

            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var key = reader.ReadByte();
                var length = reader.ReadInt32();
                var bitArrayBytes = reader.ReadBytes((length + 7) / 8);
                var tempBitArray = new BitArray(bitArrayBytes);
                var bitArray = new BitArray(length);
                for (int j = 0; j < length; j++)
                    bitArray[j] = tempBitArray[j];
                dictionary.Add(key, bitArray);
            }

            return dictionary;
        }
        #endregion

        #region Helpers
        private static bool SameBits(BitArray first, BitArray second)
        {
            if (first.Length != second.Length)
                return false;

            // Convert the arrays to int[]s
            int[] firstInts = new int[(int)Math.Ceiling((decimal)first.Count / 32)];
            first.CopyTo(firstInts, 0);
            int[] secondInts = new int[(int)Math.Ceiling((decimal)second.Count / 32)];
            second.CopyTo(secondInts, 0);

            // Look for differences
            bool areDifferent = false;
            for (int i = 0; i < firstInts.Length && !areDifferent; i++)
                areDifferent = firstInts[i] != secondInts[i];

            return !areDifferent;
        }
        #endregion
    }
}
