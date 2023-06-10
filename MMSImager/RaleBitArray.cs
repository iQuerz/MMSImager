using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSImager
{
    public class RaleBitArray
    {
        public int Count => _bits.Count;
        public List<bool> Bits => _bits;

        private List<bool> _bits;

        public RaleBitArray()
        {
            _bits = new();
        }
        public RaleBitArray(List<bool> bits)
        {
            _bits = new();
            _bits.AddRange(bits);
        }
        public RaleBitArray(BitArray bits)
        {
            _bits = new();
            foreach(bool bit in bits)
            {
                _bits.Add(bit);
            }
        }

        public bool this [int index]
        {
            get { return _bits[index]; }
            set { _bits[index] = value; }
        }

        public static bool operator ==(RaleBitArray obj1, RaleBitArray obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
                return false;

            return obj1.Equals(obj2);
        }
        public static bool operator !=(RaleBitArray obj1, RaleBitArray obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var bits = (RaleBitArray)obj;

            if (_bits.Count != bits.Count)
                return false;

            for (int i = 0; i < _bits.Count; i++)
                if (_bits[i] != bits[i])
                    return false;

            return true;
        }
    }
}
