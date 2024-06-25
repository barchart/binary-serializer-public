#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Attributes
{
    /// <summary>
    ///     Metadata regarding an attribute.
    /// </summary>
    public readonly struct Header
    {
        #region Properties
        
        /// <summary>
        ///     Indicates if the value of the attribute is included in serialized output (either
        ///     as a null flag in the header or as binary data following the header).
        /// </summary>
        public bool IsMissing { get; init; }
        
        /// <summary>
        ///     If true, the attribute's value is null.
        /// </summary>
        public bool IsNull { get; init; }
        
        #endregion
        
        #region Methods

        /// <summary>
        ///     Writes header data to a data buffer.
        /// </summary>
        /// <param name="dataBuffer">
        ///     The data buffer to write to.
        /// </param>
        /// <param name="header">
        ///     The header to write.
        /// </param>
        public static void WriteToBuffer(IDataBuffer dataBuffer, Header header)
        {
            WriteToBuffer(dataBuffer, header.IsMissing, header.IsNull);
        }
        
        /// <summary>
        ///     Writes header data to a data buffer.
        /// </summary>
        /// <param name="dataBuffer">
        ///     The data buffer to write to.
        /// </param>
        /// <param name="valueIsMissing">
        ///     Indicates if the value of the attribute is included in serialized output (either
        ///     as a null flag in the header or as binary data following the header).
        /// </param>
        /// <param name="valueIsNull">
        ///     Indicates if the value of the attribute is null.
        /// </param>
        public static void WriteToBuffer(IDataBuffer dataBuffer, bool valueIsMissing, bool valueIsNull)
        {
            dataBuffer.WriteBit(valueIsMissing);

            if (!valueIsMissing)
            {
                dataBuffer.WriteBit(valueIsNull);
            }
        }
        
        /// <summary>
        ///     Reads a header from the data buffer.
        /// </summary>
        /// <param name="dataBuffer">
        ///     The data buffer to read from.
        /// </param>
        /// <returns>
        ///     A header consisting of the next one (or two) bits from the data buffer.
        /// </returns>
        public static Header ReadFromBuffer(IDataBuffer dataBuffer)
        {
            bool headerIsMissing = dataBuffer.ReadBit();
            bool headerIsNull = !headerIsMissing && dataBuffer.ReadBit();
            
            return new() { IsMissing = headerIsMissing, IsNull = headerIsNull };
        }
        
        #endregion
    }
}