namespace Barchart.BinarySerializer.Types
{
    public static class DateOnlyHelper
    {
        public static int GetSizeOfDateOnly()
        {
            return sizeof(int);
        }

        public static byte[] ConvertDateToByteArray(DateOnly value)
        {
            int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
            return BitConverter.GetBytes(daysSinceEpoch);
        }

        public static DateOnly ConvertBytesToDate(byte[] bytes)
        {
            int daysSinceEpoch = BitConverter.ToInt32(bytes);
            return DateOnly.MinValue.AddDays(daysSinceEpoch);
        }
    }

    public class BinarySerializerDateOnly : BinarySerializerNumeric<DateOnly>
    {
        public override int Size => DateOnlyHelper.GetSizeOfDateOnly();

        protected override byte[] ConvertToByteArray(DateOnly value)
        {
            return DateOnlyHelper.ConvertDateToByteArray(value);
        }

        protected override DateOnly DecodeBytes(byte[] bytes)
        {
            return DateOnlyHelper.ConvertBytesToDate(bytes);
        }
    }

    public class BinarySerializerDateOnlyNullable : BinarySerializerNullableNumeric<DateOnly>
    {
        public override int Size => DateOnlyHelper.GetSizeOfDateOnly();

        protected override byte[] ConvertToByteArray(DateOnly value)
        {
            return DateOnlyHelper.ConvertDateToByteArray(value);
        }

        protected override DateOnly DecodeBytes(byte[] bytes)
        {
            return DateOnlyHelper.ConvertBytesToDate(bytes);
        }
    }
}