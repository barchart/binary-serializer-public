#region Using Statements

using Barchart.BinarySerializer.Buffers.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Buffers;

/// <summary>
///     A utility for reading binary data.
/// </summary>
public interface IDataBufferReader
{
    #region Properties
    
    /// <summary>
    ///     Gets the total number of bytes read from the buffer.
    /// </summary>
    /// <returns>
    ///     The total number of bytes read.
    /// </returns>
    int BytesRead { get; }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    ///     Reads a single bit from the internal storage.
    /// </summary>
    /// <returns>
    ///     The next bit from the internal storage.
    /// </returns>
    /// <exception cref="InsufficientCapacityException">
    ///     Thrown when the internal storage has been read completely.
    /// </exception>
    bool ReadBit();
    
    /// <summary>
    ///     Reads a byte from the internal storage.
    /// </summary>
    /// <returns>
    ///     The next byte from the internal storage.
    /// </returns>
    /// <exception cref="InsufficientCapacityException">
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
    /// <exception cref="InsufficientCapacityException">
    ///     Thrown when the internal storage has less remaining space than the <paramref name="size"/> requested.
    /// </exception>
    byte[] ReadBytes(int size);
    
    /// <summary>
    ///     Records the current read position of the internal storage.
    /// </summary>
    /// <returns>
    ///     An <see cref="IDisposable"/> that causes the read position
    ///     of the internal storage to be reset.
    /// </returns>
    IDisposable Bookmark();
    
    #endregion
}