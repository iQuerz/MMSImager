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
        private class Node
        {
            public byte? Symbol { get; set; }
            public int Frequency { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }


        public static Dictionary<byte, BitArray> CreateHuffmanDictionary(byte[] data)
        {
            // Count the frequency of each symbol in the data
            Dictionary<byte, int> frequencyTable = new Dictionary<byte, int>();
            foreach (byte b in data)
            {
                if (!frequencyTable.ContainsKey(b))
                {
                    frequencyTable[b] = 0;
                }

                frequencyTable[b]++;
            }

            // Create a list of leaf nodes for each symbol
            List<Node> nodes = frequencyTable.Select(pair => new Node
            {
                Symbol = pair.Key,
                Frequency = pair.Value
            }).ToList();

            // Build the Huffman tree by merging nodes with the lowest frequency
            while (nodes.Count > 1)
            {
                // Sort the nodes by frequency
                nodes.Sort((a, b) => a.Frequency.CompareTo(b.Frequency));

                // Merge the two nodes with the lowest frequency
                Node left = nodes[0];
                Node right = nodes[1];
                Node parent = new Node
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };

                // Remove the merged nodes and add the parent node
                nodes.RemoveAt(0);
                nodes.RemoveAt(0);
                nodes.Add(parent);
            }

            // Traverse the Huffman tree to generate codes for each symbol
            Dictionary<byte, BitArray> huffmanDict = new Dictionary<byte, BitArray>();
            TraverseTree(nodes[0], new BitArray(0), huffmanDict);

            return huffmanDict;
        }

        private static void TraverseTree(Node node, BitArray code, Dictionary<byte, BitArray> huffmanDict)
        {
            if (node.Symbol != null)
            {
                huffmanDict[(byte)node.Symbol] = new BitArray(code);
            }
            else
            {
                BitArray leftCode = new BitArray(code);
                leftCode.Length++;
                leftCode[leftCode.Length - 1] = false;
                TraverseTree(node.Left, leftCode, huffmanDict);

                BitArray rightCode = new BitArray(code);
                rightCode.Length++;
                rightCode[rightCode.Length - 1] = true;
                TraverseTree(node.Right, rightCode, huffmanDict);
            }
        }
    }
}
