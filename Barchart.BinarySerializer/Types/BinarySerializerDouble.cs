#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) double values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDouble : IBinaryTypeSerializer<double>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(double) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, double value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<double> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);

            if (valueIsMissing || valueIsNull)
            {
                return new Attribute<double>(valueIsMissing, default);
            }

            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(double));
            double decodedValue = BitConverter.ToDouble(valueBytes);
            return new Attribute<double>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(double value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}