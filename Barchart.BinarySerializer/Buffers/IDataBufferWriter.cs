namespace Barchart.BinarySerializer.Buffers;

/// <summary>
///     A utility for writing binary data.
/// </summary>
public interface IDataBufferWriter
{
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
    ///     Thrown when remaining internal storage is less than the number of bytes to write.
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