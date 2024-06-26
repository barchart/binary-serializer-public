#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) DateOnly values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDateOnly : IBinaryTypeSerializer<DateOnly>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(int) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, DateOnly value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
            dataBuffer.WriteBytes(BitConverter.GetBytes(daysSinceEpoch));
        }

        /// <inheritdoc />
        public Attribute<DateOnly> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(int));
            int daysSinceEpoch = BitConverter.ToInt32(valueBytes);
            DateOnly decodedValue = DateOnly.MinValue.AddDays(daysSinceEpoch);
            
            return new Attribute<DateOnly>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(DateOnly value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}