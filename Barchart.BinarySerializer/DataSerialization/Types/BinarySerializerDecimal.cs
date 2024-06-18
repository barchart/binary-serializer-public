namespace Barchart.BinarySerializer.DataSerialization.Types
{
    public class BinarySerializerDecimal : BinarySerializerNumeric<decimal>
    {
        #region Properties

        public override int Size => sizeof(decimal);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(decimal value)
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);
            writer.Write(value);

            return stream.ToArray();
        }

        protected override decimal DecodeBytes(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            using BinaryReader reader = new(stream);

            return reader.ReadDecimal();
        }

        #endregion
    }
}