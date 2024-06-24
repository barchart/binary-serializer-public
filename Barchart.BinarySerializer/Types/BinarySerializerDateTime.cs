using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerDateTime : BinarySerializerNumeric<DateTime>
    {
        #region Properties

        public override int Size => sizeof(long);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, DateTime value)
        {
            TimeSpan unixTimeSpan = value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixTime = (long)unixTimeSpan.TotalMilliseconds;

            dataBuffer.WriteBytes(BitConverter.GetBytes(unixTime));
        }

        protected override DateTime DecodeBytes(byte[] bytes)
        {
            long ticksPerMillisecond = TimeSpan.TicksPerMillisecond;
            long milliSeconds = BitConverter.ToInt64(bytes, 0);
            DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddTicks(milliSeconds * ticksPerMillisecond);
        }

        #endregion
    }
}