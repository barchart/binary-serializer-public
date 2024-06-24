namespace Barchart.BinarySerializer.Buffers;

/// <summary>
///     A utility for writing (and reading) binary data to (and from) a byte array.
/// </summary>
public interface IDataBuffer
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
    
    /// <summary>
    ///     Writes a single bit to the internal storage.
    /// </summary>
    /// <param name="value">
    ///     Value of the bit to write.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when internal storage is full.
    /// </exception>
    void WriteBit(bool value);
    
    /// <summary>
    ///     Writes a byte to the internal storage.
    /// </summary>
    /// <param name="value">
    ///     Value of the byte to write.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when remaining internal storage is less than one byte.
    /// </exception>
    void WriteByte(byte value);
    
    /// <summary>
    ///     Writes an array of bytes to the internal storage.
    /// </summary>
    /// <param name="value">
    ///     Value of the bytes to write.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when remaining internal storage is than the number of bytes to write.
    /// </exception>
    void WriteBytes(byte[] value);

    /// <summary>
    ///     Generates a copy of the internal storage, as a byte array, containing
    ///     the data that has been written to the buffer.
    /// </summary>
    /// <returns>
    ///     A byte array containing the data up to the current offset.
    /// </returns>
    byte[] ToBytes();
}