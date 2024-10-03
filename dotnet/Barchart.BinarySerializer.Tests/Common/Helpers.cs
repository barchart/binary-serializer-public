namespace Barchart.BinarySerializer.Tests.Common;

/// <summary>
///     Provides utility methods for handling byte arrays and bit operations in the context of binary serialization tests.
/// </summary>
public static class Helpers
{
    #region Methods

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