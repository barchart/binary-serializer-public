namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Stores header byte in a specified format based on missing/null bits and string length bits and the value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value stored alongside the header.</typeparam>
    public struct HeaderWithValue<T>
    {
        public Header Header { get; set; }

        public T Value { get; set; }

        public HeaderWithValue(Header header, T value)
        {
            Header = header;
            Value = value;
        }
    }
}