#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;
using System.IO;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) decimal values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDecimal : IBinaryTypeSerializer<decimal>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(decimal) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, decimal value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);
            writer.Write(value);

            dataBuffer.WriteBytes(stream.ToArray());
        }

        /// <inheritdoc />
        public Attribute<decimal> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(decimal));
            
            using MemoryStream stream = new(valueBytes);
            using BinaryReader reader = new(stream);
            decimal decodedValue = reader.ReadDecimal();

            return new Attribute<decimal>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(decimal value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}