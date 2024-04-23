namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Stores header byte in a specified format based on missing/null bits and string length bits and the value of type <typeparamref name="TContainer"/>.
    /// </summary>
    /// <typeparam name="TContainer">The type of the value stored alongside the header.</typeparam>
    public struct HeaderWithValue<TContainer>
    {
        public Header Header { get; set; }

        public TContainer? Value { get; set; }

        public HeaderWithValue(Header header, TContainer? value)
        {
            Header = header;
            Value = value;
        }
    }
}