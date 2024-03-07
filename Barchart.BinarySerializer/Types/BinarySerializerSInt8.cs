﻿namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerSInt8 : BinarySerializerNumeric<sbyte>
    {
        public override int Size => sizeof(sbyte);

        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return new byte[] { (byte)value };
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return (sbyte)bytes[0];
        }
    }

    public class BinarySerializerSInt8Nullable : BinarySerializerNullableNumeric<sbyte>
    {
        public override int Size => sizeof(sbyte);

        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return new byte[] { (byte)value };
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return (sbyte)bytes[0];
        }
    }
}