﻿#region Using Statements

using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.Headers
{
    /// <summary>
    ///   A data structure used to store metadata regarding an attribute.
    /// </summary>
    public readonly struct Header
    {
        #region Properties
        
        /// <summary>
        ///     If true, the referenced attribute has been omitted from the binary
        ///     serialization. If true, that does not necessarily mean the attribute
        ///     has no value (it simply means the attribute's value was not serialized).
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
        public static void WriteToBuffer(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit(header.IsMissing);

            if (!header.IsMissing)
            {
                dataBuffer.WriteBit(header.IsNull);
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
        public static Header ReadFromBuffer(DataBuffer dataBuffer)
        {
            bool headerIsMissing = dataBuffer.ReadBit();
            bool headerIsNull = !headerIsMissing && dataBuffer.ReadBit();
            
            return new() { IsMissing = headerIsMissing, IsNull = headerIsNull };
        }
        
        /// <summary>
        ///     Checks if the header indicates that the value is missing or null.
        /// </summary>
        /// <returns>True if the value is missing or null, otherwise false.</returns>
        public bool IsValueMissingOrNull()
        {
            return IsMissing || IsNull;
        }
        
        #endregion
    }
}