namespace Barchart.BinarySerializer.Examples.Common;

/// <summary>
///     Provides helper methods for common operations related to binary serialization.
/// </summary>
public static class Helper
{
    #region Methods

    /// <summary>
    ///     Converts a byte to its binary string representation.
    /// </summary>
    /// <param name="b">
    ///     The byte to convert.
    /// </param>
    /// <returns>
    ///     A string representing the binary format of the byte.
    /// </returns>
    public static string PrintBits(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }

    #endregion
}