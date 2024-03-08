namespace Barchart.BinarySerializer.Types
{
    public struct HeaderWithValue<T>
    {
        public Header Header { get; set; }

        public T? Value { get; set; }

        public HeaderWithValue(Header header, T? value = default)
        {
            Header = header;
            Value = value;
        }
    }
}