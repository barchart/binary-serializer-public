namespace JerqAggregatorNew.Types
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

        public override int GetLengthInBytes(DateTime? value)
        {
            return Size;
        }

        protected override DateTime DecodeBytes(byte[] bytes, int offset)
        {
            long ticksPerMillisecond = TimeSpan.TicksPerMillisecond;
            long milliSeconds = BitConverter.ToInt64(bytes, 0);
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddTicks(milliSeconds * ticksPerMillisecond);
        }
    }
}