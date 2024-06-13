﻿namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Representing additional byte for a header for every property/field.
    ///     7_ 6_ 5_ 4_ 3_ 2_ 1_ 0_ (byte structure)
    ///     7 (Missing bit) 
    ///     6 (Null bit)
    ///     5-0 (bits for string length 0 max string length is 2^6 - 63)
    /// </summary>
    public struct Header
    {
        public bool IsMissing { get; set; }
        public bool IsNull { get; set; }

        private byte? _stringLength;

        public byte? StringLength
        {
            get
            {
                return _stringLength;
            }

            set
            {
                if (value.HasValue && value > 63)
                {
                    throw new Exception("Length needs to be under 64");
                }

                _stringLength = value;
            }
        }
    }
}