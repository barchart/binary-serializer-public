namespace Barchart.BinarySerializer.Tests.Common;

public static class Helpers
{
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
}