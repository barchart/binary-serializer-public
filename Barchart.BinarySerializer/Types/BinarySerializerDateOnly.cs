#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) DateOnly values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerDateOnly : IBinaryTypeSerializer<DateOnly>
    {
        #region Fields

        private readonly BinarySerializerInt _binarySerialzierInt;
        
        #endregion

        #region Constructors

        public BinarySerializerDecimal()
        {
            _binarySerialzierInt = new BinarySerializerInt();
        }
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, DateOnly value)
        {
            _binarySerialzierInt.Encode(dataBuffer, GetDaysSinceEpoch(value));
        }

        /// <inheritdoc />
        public DateOnly Decode(IDataBufferReader dataBuffer)
        {
            int daysSinceEpoch = _binarySerialzierInt.Decode(dataBuffer);
            
            return DateOnly.MinValue.AddDays(daysSinceEpoch);
        }

        /// <inheritdoc />
        public int GetLengthInBits(DateOnly value)
        {
            return _binarySerialzierInt.GetLengthInBits(GetDaysSinceEpoch(value));
        }

        private static int GetDaysSinceEpoch(DateOnly value)
        {
            return value.DayNumber - DateOnly.MinValue.DayNumber;
        }

        #endregion
    }
}