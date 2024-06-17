namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerDateOnly : BinarySerializerNumeric<DateOnly>
    {
        #region Properties

        public override int Size => sizeof(int);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(DateOnly value)
        {
            int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
            return BitConverter.GetBytes(daysSinceEpoch);
        }

        protected override DateOnly DecodeBytes(byte[] bytes)
        {
            int daysSinceEpoch = BitConverter.ToInt32(bytes);
            return DateOnly.MinValue.AddDays(daysSinceEpoch);
        }

        #endregion
    }
}