namespace Barchart.BinarySerializer.Tests.Common;

/// <summary>
///     Provides utility methods for handling byte arrays and bit operations in the context of binary serialization tests.
/// </summary>
public static class Helpers
{
    #region Methods

    /// <summary>
    ///     Converts an array of boolean values to a byte array.
    ///     Each boolean value represents a bit, where `true` corresponds to a bit set to `1` and `false` to a bit set to `0`.
    ///     The bits are packed into bytes starting from the most significant bit (leftmost) to the least significant bit (rightmost).
    /// </summary>
    /// <param name="bits">
    ///     An array of boolean values representing the bits to be converted.
    /// </param>
    /// <returns>
    ///     A byte array containing the packed bit values.
    /// </returns>
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

    /// <summary>
    ///     Combines the first four byte arrays from a list into a single byte array.
    ///     The method concatenates these byte arrays in the order they appear in the list.
    ///     It throws an exception if the list does not contain at least four byte arrays.
    /// </summary>
    /// <param name="byteArrays">
    ///     A list containing byte arrays to be combined.
    /// </param>
    /// <returns>
    ///     A byte array that is the result of concatenating the first four byte arrays from the list.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the list contains fewer than four byte arrays.
    /// </exception>
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

    #endregion
}