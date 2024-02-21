using System;

namespace JerqAggregatorNew.Types
{
    public class BinarySerializerDateOnly : BinarySerializerNumeric<DateOnly>
    {
        public override int Size => sizeof(int);

        public override int GetLengthInBytes(DateOnly? value)
        {
            return Size;
        }

        protected override byte[] ConvertToByteArray(DateOnly value)
        {
            int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
            return BitConverter.GetBytes(daysSinceEpoch);
        }

        protected override DateOnly DecodeBytes(byte[] bytes, int offset)
        {
            int daysSinceEpoch = BitConverter.ToInt32(bytes);
            return DateOnly.MinValue.AddDays(daysSinceEpoch);
        }
    }
}
