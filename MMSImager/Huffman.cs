using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSImager
{
    internal class Huffman
    {
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

            var heap = new PriorityQueue<HuffmanNode, HuffmanNodeComparer>();
            foreach (var pair in frequency)
            {
                heap.Enqueue(new HuffmanNode() { Byte = pair.Key, Frequency = pair.Value }, new HuffmanNodeComparer());
            }

            while (heap.Count > 1)
            {
                var left = heap.Dequeue();
                var right = heap.Dequeue();
                heap.Enqueue(new HuffmanNode() { Left = left, Right = right, Frequency = left.Frequency + right.Frequency }, new HuffmanNodeComparer());
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

        public static byte[] EncodeImage(List<byte> bytes, Dictionary<byte, BitArray> dictionary)
        {
            var bitList = new List<bool>();
            foreach (var b in bytes)
            {
                var bits = dictionary[b];
                bitList.AddRange(bits.Cast<bool>());
            }

            var bitArray = new BitArray(bitList.ToArray());
            int byteCount = (bitArray.Length + 7) / 8;
            byte[] byteArray = new byte[byteCount];
            bitArray.CopyTo(byteArray, 0);
            return byteArray;
        }
        public static List<byte> DecodeImage(byte[] byteArray, Dictionary<byte, BitArray> dictionary)
        {
            var bitArray = new BitArray(byteArray);
            var bytes = new List<byte>();
            var bits = new List<bool>();
            for (int i = 0; i < bitArray.Length; i++)
            {
                bits.Add(bitArray[i]);
                if (dictionary.ContainsValue(new BitArray(bits.ToArray())))
                {
                    bytes.Add(dictionary.FirstOrDefault(x => x.Value.Cast<bool>().SequenceEqual(bits)).Key);
                    bits.Clear();
                }
            }
            return bytes;
        }

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

                writer.Write(byteCount);
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
                var bitArrayBytes = reader.ReadBytes(length);
                var bitArray = new BitArray(bitArrayBytes);
                dictionary.Add(key, bitArray);
            }

            return dictionary;
        }

        private class HuffmanNode
        {
            public byte Byte { get; set; }
            public int Frequency { get; set; }
            public HuffmanNode Left { get; set; }
            public HuffmanNode Right { get; set; }
        }

        private class HuffmanNodeComparer : IComparer<HuffmanNode>
        {
            public int Compare(HuffmanNode x, HuffmanNode y)
            {
                return x.Frequency - y.Frequency;
            }
        }
    }
}
