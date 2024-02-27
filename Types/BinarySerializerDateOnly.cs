﻿namespace JerqAggregatorNew.Types
{
    public class BinarySerializerDateOnly : BinarySerializerNumeric<DateOnly>
    {
        public override int Size => sizeof(int);

        protected override byte[] ConvertToByteArray(DateOnly value)
        {
            int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
            return BitConverter.GetBytes(daysSinceEpoch);
        }

        public override int GetLengthInBytes(DateOnly? value)
        {
            return Size + sizeof(byte);
        }

        protected override DateOnly DecodeBytes(byte[] bytes)
        {
            int daysSinceEpoch = BitConverter.ToInt32(bytes);
            return DateOnly.MinValue.AddDays(daysSinceEpoch);
        }
    }
}
