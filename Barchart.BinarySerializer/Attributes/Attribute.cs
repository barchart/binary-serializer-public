namespace Barchart.BinarySerializer.Attributes
{
    /// <summary>
    ///     Stores header byte in a specified format based on missing/null bits and string length bits and the value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value stored alongside the header.</typeparam>
    public readonly struct Attribute<T>
    {
        #region Properties

        public bool IsValueMissing { get; }

        public T Value { get; }

        #endregion
        
        #region Constructor(s)

        public Attribute(bool isValueMissing, T value)
        {
            IsValueMissing = isValueMissing;
            Value = value;
        }

        #endregion
    }
}