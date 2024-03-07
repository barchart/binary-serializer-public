﻿namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerDouble : BinarySerializerNumeric<double>
    {
        public override int Size => sizeof(double);

        protected override byte[] ConvertToByteArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override double DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes);
        }
    }

    public class BinarySerializerDoubleNullable : BinarySerializerNullableNumeric<double>
    {
        public override int Size => sizeof(double);

        protected override byte[] ConvertToByteArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override double DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes);
        }
    }
}