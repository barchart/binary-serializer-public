#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) DateTime values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDateTime : IBinaryTypeSerializer<DateTime>
    {
        #region Fields
        
        private readonly BinarySerializerLong _binarySerializerLong;
        
        #endregion

        #region Constructors

        public BinarySerializerDateTime()
        {
            _binarySerializerLong = new BinarySerializerLong();
        }
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, DateTime value)
        {
            _binarySerializerLong.Encode(dataBuffer, GetMillisecondsSinceEpoch(value));
        }

        /// <inheritdoc />
        public DateTime Decode(IDataBufferReader dataBuffer)
        {
            long millisecondsSinceEpoch = _binarySerializerLong.Decode(dataBuffer);
            
            return DateTime.UnixEpoch.AddMilliseconds(millisecondsSinceEpoch);
        }

        /// <inheritdoc />
        public int GetLengthInBits(DateTime value)
        {
            return _binarySerializerLong.GetLengthInBits(GetMillisecondsSinceEpoch(value));
        }
        
        /// <inheritdoc />
        public bool GetEquals(DateTime a, DateTime b)
        {
            return a.Equals(b);
        }

        private static long GetMillisecondsSinceEpoch(DateTime value)
        {
            TimeSpan timeSpan = value - DateTime.UnixEpoch;
            
           return Convert.ToInt64(timeSpan.TotalMilliseconds);
        }
        
        #endregion
    }
}