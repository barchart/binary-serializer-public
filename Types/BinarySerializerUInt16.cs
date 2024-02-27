﻿namespace JerqAggregatorNew.Types
{
    public class BinarySerializerUInt16 : BinarySerializerNumeric<ushort>
    {
        public override int Size => sizeof(ushort);

        protected override byte[] ConvertToByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(ushort? value)
        {
            return Size + sizeof(byte);
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }
    }
}