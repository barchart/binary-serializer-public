namespace Barchart.BinarySerializer.DataSerialization.Types
{
    public class BinarySerializerDouble : BinarySerializerNumeric<double>
    {
        #region Properties
        
        public override int Size => sizeof(double);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override double DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes);
        }
        
        #endregion
    }
}