#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Attributes
{
    /// <summary>
    ///     Metadata regarding an attribute.
    /// </summary>
    public static class Header
    {
        #region Constants

        public const int NUMBER_OF_BITS_IS_MISSING = 1;
        public const int NUMBER_OF_HEADER_BITS_NON_STRING = 2;
        
        #endregion
        
        #region Methods
        
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
        public static void WriteToBuffer(IDataBufferWriter dataBuffer, bool valueIsMissing, bool valueIsNull, int length = 0)
        {
            dataBuffer.WriteBit(valueIsMissing);

            if (!valueIsMissing)
            {
                dataBuffer.WriteBit(valueIsNull);
            }

            if (length > 0)
            {
                for (int i = 5; i >= 0; i--)
                {
                    bool bit = ((length >> i) & 1) == 1;
                    dataBuffer.WriteBit(bit);
                }
            }
        }

        /// <summary>
        ///     Reads a header from the data buffer (into out parameters)
        /// </summary>
        /// <param name="dataBuffer">
        ///     The data buffer to read from.
        /// </param>
        /// <param name="valueIsMissing">
        ///     Indicates if the value of the attribute is included in serialized output (either
        ///     as a null flag in the header or as binary data following the header).
        /// </param>
        /// <param name="valueIsNull">
        ///     Indicates if the value of the attribute is null.
        /// </param>
        public static void ReadFromBuffer(IDataBufferReader dataBuffer, out bool valueIsMissing, out bool valueIsNull)
        {
            valueIsMissing = dataBuffer.ReadBit();
            valueIsNull = !valueIsMissing && dataBuffer.ReadBit();
        }
        
        #endregion
    }
}