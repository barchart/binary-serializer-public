#region Using Statements

using Barchart.BinarySerializer.Buffers.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Buffers;

/// <summary>
///     A data buffer reader which uses a fixed-length byte array
///     for internal storage.
/// </summary>
public class DataBufferReader : IDataBufferReader
{
    #region Fields

    private readonly byte[] _byteArray;

    private int _positionByte;
    private int _positionBit;

    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Creates a new <see cref="DataBufferReader"/> instance.
    /// </summary>
    /// <param name="byteArray">
    ///     The internal storage mechanism for the class.
    /// </param>
    public DataBufferReader(byte[] byteArray)
    {
        _byteArray = byteArray;

        _positionByte = 0;
        _positionBit = 0;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public bool ReadBit()
    {
        if (CapacityWouldBeExceeded(0))
        {
            throw new InsufficientCapacityException(false);
        }

        int bit = (_byteArray[_positionByte] >> (7 - _positionBit)) & 1;

        AdvanceBit();

        return bit == 1;
    }

    /// <inheritdoc />
    public byte ReadByte()
    {
        if (CapacityWouldBeExceeded(_positionBit == 0 ? 0 : 1))
        {
            throw new InsufficientCapacityException(false);
        }

        return ReadByteUnchecked();
    }

    /// <inheritdoc />
    public byte[] ReadBytes(int size)
    {
        if (size == 0)
        {
            return Array.Empty<byte>();
        }
        
        if (CapacityWouldBeExceeded(_positionBit == 0 ? size - 1 : size))
        {
            throw new InsufficientCapacityException(false);
        }

        byte[] bytes = new byte[size];
        
        for (int i = 0; i < size; i++)
        {
            bytes[i] = ReadByteUnchecked();
        }
        
        return bytes;
    }

    private byte ReadByteUnchecked()
    {
        if (_positionBit == 0)
        {
            return _byteArray[_positionByte++];
        }

        byte byteFirst = _byteArray[_positionByte];
        byte byteSecond = _byteArray[++_positionByte];

        byte byteStart = (byte)(byteFirst << _positionBit);
        byte byteEnd = (byte)(byteSecond >> (8 - _positionBit));

        byte byteMerged = (byte)(byteStart | byteEnd);
        
        return byteMerged;
    }
    
    /// <inheritdoc />
    public IDisposable Bookmark()
    {
        return new DataBufferReaderBookmark(this, _positionByte, _positionBit);
    }

    private void AdvanceBit()
    {
        if (_positionBit == 7)
        {
            _positionBit = 0;
            _positionByte++;
        }
        else
        {
            _positionBit++;
        }
    }
    
    private bool CapacityWouldBeExceeded(int additionalBytes)
    {
        return _positionByte + additionalBytes >= _byteArray.Length;
    }
    
    #endregion
    
    #region Nested Class

    private class DataBufferReaderBookmark : IDisposable
    {
        #region Fields
        private readonly DataBufferReader _reader;
        
        private readonly int _positionByte;
        private readonly int _positionBit;

        private volatile int _disposed;

        #endregion
        
        #region Constructor(s)

        public DataBufferReaderBookmark(DataBufferReader reader, int positionByte, int positionBit)
        {
            _reader = reader;

            _positionByte = positionByte;
            _positionBit = positionBit;

            _disposed = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Restores the position of the <see cref="DataBufferReader"/> to the 
        ///     position at the time this bookmark was created. 
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }
            
            _reader._positionByte = _positionByte;
            _reader._positionBit = _positionBit;
        }

        #endregion
    }
    
    #endregion
}