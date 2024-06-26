﻿#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Attributes
{
    /// <summary>
    ///     Metadata regarding an attribute.
    /// </summary>
    public class Header
    {
        #region Fields

        public const int NumberOfBitsIsMissing = 1;
        public const int NumberOfHeaderBitsNonString = 2;
        public const int NumberOfHeaderBitsString = 8;
        
        #endregion

        #region Constructor(s)

        private Header()
        {

        }
        
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
        public static void WriteToBuffer(IDataBufferWriter dataBuffer, bool valueIsMissing, bool valueIsNull, byte length = 0)
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