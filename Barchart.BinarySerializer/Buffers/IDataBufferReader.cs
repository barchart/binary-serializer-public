namespace Barchart.BinarySerializer.Buffers;

/// <summary>
///     A utility for reading binary data from a byte array.
/// </summary>
public interface IDataBufferReader
{
    /// <summary>
    ///     Reads a single bit from the internal storage.
    /// </summary>
    /// <returns>
    ///     The next bit from the internal storage.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the internal storage has been read completely.
    /// </exception>
    bool ReadBit();
    
    /// <summary>
    ///     Reads a byte from the internal storage.
    /// </summary>
    /// <returns>
    ///     The next byte from the internal storage.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the internal storage has less than one byte remaining.
    /// </exception>
    byte ReadByte();
    
    /// <summary>
    ///     Reads multiple bytes from the internal storage.
    /// </summary>
    /// <param name="size">
    ///     The number of bytes to read from the internal storage.
    /// </param>
    /// <returns>
    ///     A byte array, with length of <paramref name="size"/>, with data from the internal storage.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the internal storage has less remaining space than the <paramref name="size"/> requested.
    /// </exception>
    byte[] ReadBytes(int size);
}