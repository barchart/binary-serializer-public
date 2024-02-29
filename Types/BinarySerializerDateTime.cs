namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerDateTime : BinarySerializerNumeric<DateTime>
    {
        public override int Size => sizeof(long);

        protected override byte[] ConvertToByteArray(DateTime value)
        {
            TimeSpan unixTimeSpan = value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixTime = (long)unixTimeSpan.TotalMilliseconds;
            return BitConverter.GetBytes(unixTime);
        }

        public override int GetLengthInBits(DateTime? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override DateTime DecodeBytes(byte[] bytes)
        {
            long ticksPerMillisecond = TimeSpan.TicksPerMillisecond;
            long milliSeconds = BitConverter.ToInt64(bytes, 0);
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddTicks(milliSeconds * ticksPerMillisecond);
        }
    }
}