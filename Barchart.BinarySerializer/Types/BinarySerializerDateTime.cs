#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) DateTime values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDateTime : IBinaryTypeSerializer<DateTime>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(long) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, DateTime value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            TimeSpan unixTimeSpan = value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixTime = (long)unixTimeSpan.TotalMilliseconds;

            dataBuffer.WriteBytes(BitConverter.GetBytes(unixTime));
        }

        /// <inheritdoc />
        public Attribute<DateTime> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            DateTime decodedValue = default;
            
            if (!valueIsMissing && !valueIsNull)
            {
                byte[] valueBytes = dataBuffer.ReadBytes(sizeof(long));
                long milliSeconds = BitConverter.ToInt64(valueBytes, 0);
                DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                decodedValue = epoch.AddMilliseconds(milliSeconds);
            }

            return new Attribute<DateTime>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(DateTime value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}