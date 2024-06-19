namespace Barchart.BinarySerializer.DataSerialization.Headers
{
    /// <summary>
    ///     Stores header byte in a specified format based on missing/null bits and string length bits and the value of type <typeparamref name="TMember"/>.
    /// </summary>
    /// <typeparam name="TMember">The type of the value stored alongside the header.</typeparam>
    public readonly struct HeaderWithValue<TMember>
    {
        #region Properties

        public Header Header { get; }

        public TMember? Value { get; }

        #endregion
        
        #region Constructor(s)

        public HeaderWithValue(Header header, TMember? value)
        {
            Header = header;
            Value = value;
        }

        #endregion
    }
}