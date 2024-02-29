namespace Barchart.BinarySerializer.Types
{
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

