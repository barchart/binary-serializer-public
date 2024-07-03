namespace Barchart.BinarySerializer.Tests.Common;

public static class Helpers
{
    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
        }

    public static byte[] ConvertToBytes(bool[] bits)
    {
        int byteCount = (int)Math.Ceiling(bits.Length / 8.0); 
        byte[] bytes = new byte[byteCount];

        int byteIndex = 0;
        int bitIndex = 7;

        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i])
            {
                bytes[byteIndex] |= (byte)(1 << bitIndex);
            }

            bitIndex--;
            if (bitIndex < 0)
            {
                byteIndex++;
                bitIndex = 7;
            }
        }

        return bytes;
    }

    public static byte[] CombineFourByteArrays(List<byte[]> byteArrays)
    {
        if (byteArrays == null || byteArrays.Count < 4)
        {
            throw new ArgumentException("The list must contain at least four byte arrays.");
        }

        int totalLength = 0;
        for (int i = 0; i < 4; i++)
        {
            totalLength += byteArrays[i].Length;
        }

        byte[] combinedArray = new byte[totalLength];
        int offset = 0;

        for (int i = 0; i < 4; i++)
        {
            Buffer.BlockCopy(byteArrays[i], 0, combinedArray, offset, byteArrays[i].Length);
            offset += byteArrays[i].Length;
        }

        return combinedArray;
    }
}