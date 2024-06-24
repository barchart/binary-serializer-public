using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerDateOnly : BinarySerializerNumeric<DateOnly>
    {
        #region Properties

        public override int Size => sizeof(int);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, DateOnly value)
        {
            int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
            dataBuffer.WriteBytes(BitConverter.GetBytes(daysSinceEpoch));
        }

        protected override DateOnly DecodeBytes(byte[] bytes)
        {
            int daysSinceEpoch = BitConverter.ToInt32(bytes);
            return DateOnly.MinValue.AddDays(daysSinceEpoch);
        }

        #endregion
    }
}