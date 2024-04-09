namespace Barchart.BinarySerializer.Types
{
    public static class DateTimeHelper
    {
        public static int GetSizeOfDateTime()
        {
            return sizeof(long);
        }

        public static byte[] ConvertDateTimeToByteArray(DateTime value)
        {
            TimeSpan unixTimeSpan = value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixTime = (long)unixTimeSpan.TotalMilliseconds;
            return BitConverter.GetBytes(unixTime);
        }

        public static DateTime ConvertBytesToDateTime(byte[] bytes)
        {
            long ticksPerMillisecond = TimeSpan.TicksPerMillisecond;
            long milliSeconds = BitConverter.ToInt64(bytes, 0);
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddTicks(milliSeconds * ticksPerMillisecond);
        }
    }

    public class BinarySerializerDateTime : BinarySerializerNumeric<DateTime>
    {
        public override int Size => DateTimeHelper.GetSizeOfDateTime();

        protected override byte[] ConvertToByteArray(DateTime value)
        {
            return DateTimeHelper.ConvertDateTimeToByteArray(value);
        }

        protected override DateTime DecodeBytes(byte[] bytes)
        {
            return DateTimeHelper.ConvertBytesToDateTime(bytes);
        }
    }

    public class BinarySerializerDateTimeNullable : BinarySerializerNullableNumeric<DateTime>
    {
        public override int Size => DateTimeHelper.GetSizeOfDateTime();

        protected override byte[] ConvertToByteArray(DateTime value)
        {
            return DateTimeHelper.ConvertDateTimeToByteArray(value);
        }

        protected override DateTime DecodeBytes(byte[] bytes)
        {
            return DateTimeHelper.ConvertBytesToDateTime(bytes);
        }
    }
}