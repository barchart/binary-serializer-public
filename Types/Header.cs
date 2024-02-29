namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     one more byte for header for every property/field
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
        /// <summary>
        ///     returns header byte in specified format based on missing/null bits and string length bits
        /// </summary>
        public byte HeaderInformation
        {
            get
            {
                return (byte)((IsMissing ? 0x80 : 0x00) | (IsNull ? 0x40 : 0x00) | ((StringLength ?? 0) & 0x3F));
            }

            set
            {
                IsMissing = (value & 0x80) != 0;
                IsNull = (value & 0x40) != 0;
                StringLength = (byte)(value & 0x3F);
            }
        }
    }

    public struct HeaderWithValue
    {
        public Header Header { get; set; }

        public object? Value { get; set; }

        public HeaderWithValue(Header header, object? value = null)
        {
            Header = header;
            Value = value;
        }
    }
}