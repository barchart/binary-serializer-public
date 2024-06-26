#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) DateOnly values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDateOnly : IBinaryTypeSerializer<DateOnly>
    {
        #region Fields

        private readonly BinarySerializerInt _binarySerializerInt;
        
        #endregion

        #region Constructors

        public BinarySerializerDateOnly()
        {
            _binarySerializerInt = new BinarySerializerInt();
        }
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, DateOnly value)
        {
            _binarySerializerInt.Encode(dataBuffer, GetDaysSinceEpoch(value));
        }

        /// <inheritdoc />
        public DateOnly Decode(IDataBufferReader dataBuffer)
        {
            int daysSinceEpoch = _binarySerializerInt.Decode(dataBuffer);
            
            return DateOnly.MinValue.AddDays(daysSinceEpoch);
        }

        /// <inheritdoc />
        public int GetLengthInBits(DateOnly value)
        {
            return _binarySerializerInt.GetLengthInBits(GetDaysSinceEpoch(value));
        }

        private static int GetDaysSinceEpoch(DateOnly value)
        {
            return value.DayNumber - DateOnly.MinValue.DayNumber;
        }

        #endregion
    }
}