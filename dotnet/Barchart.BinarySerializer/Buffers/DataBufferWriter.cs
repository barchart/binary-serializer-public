#region Using Statements

using Barchart.BinarySerializer.Buffers.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Buffers;

/// <summary>
///     A data buffer writer which uses a fixed-length byte array
///     for internal storage.
/// </summary>
public class DataBufferWriter : IDataBufferWriter
{
    #region Constants

    private const byte TRUE = 1;

    #endregion

    #region Fields
    
    private readonly byte[] _byteArray;

    private int _positionByte;
    private int _positionBit;

    #endregion

    #region Properties
    
    /// <inheritdoc />
    public bool IsAtRootNestingLevel { get; set; }
    
    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Creates a new <see cref="DataBufferWriter"/> instance.
    /// </summary>
    /// <param name="byteArray">
    ///     The internal storage mechanism for the class. Write operations will mutate this array.
    /// </param>
    public DataBufferWriter(byte[] byteArray)
    {
        IsAtRootNestingLevel = true;
        
        _byteArray = byteArray;

        _positionByte = 0;
        _positionBit = 0;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void WriteBit(bool value)
    {
        if (CapacityWouldBeExceeded(0))
        {
            throw new InsufficientCapacityException(true);
        }

        if (_positionBit == 0)
        {
            _byteArray[_positionByte] = 0;
        }

        if (value)
        {
            byte byteCurrent = _byteArray[_positionByte];
            byte byteMask = (byte)(TRUE << (7 - _positionBit));

            _byteArray[_positionByte] = (byte)(byteCurrent | byteMask);
        }

        AdvanceBit();
    }

    /// <inheritdoc />
    public void WriteByte(byte value)
    {
        if (CapacityWouldBeExceeded(_positionBit == 0 ? 0 : 1))
        {
            throw new InsufficientCapacityException(true);
        }

        if (_positionBit == 0)
        {
            _byteArray[_positionByte++] = value;

            return;
        }

        byte byteExisting = _byteArray[_positionByte];
        
        byte byteFirstMask = (byte)(value >> _positionBit);
        byte byteFirst = (byte)(byteExisting | byteFirstMask);

        _byteArray[_positionByte] = byteFirst;

        int bitsAppendedToFirstByte = 8 - _positionBit;
        
        byte byteSecond = (byte)(value << bitsAppendedToFirstByte);

        _byteArray[++_positionByte] = byteSecond;
    }

    /// <inheritdoc />
    public void WriteBytes(byte[] value)
    {
        if (value.Length == 0)
        {
            return;
        }
        
        if (value.Length == 1)
        {
            WriteByte(value[0]);

            return;
        }

        if (CapacityWouldBeExceeded(_positionBit == 0 ? value.Length - 1 : value.Length))
        {
            throw new InsufficientCapacityException(true);
        }

        if (_positionBit == 0)
        {
            for (int i = 0; i < value.Length; i++)
            {
                _byteArray[_positionByte++] = value[i];
            }

            return;
        }
        
        byte byteFirst = _byteArray[_positionByte];
        byte byteFirstMask = (byte)(value[0] >> _positionBit);

        _byteArray[_positionByte++] = (byte)(byteFirst | byteFirstMask);

        int bitsAppendedToFirstByte = 8 - _positionBit;
        
        for (int i = 1; i < value.Length; i++)
        {
            byte byteStart = (byte)(value[i - 1] << bitsAppendedToFirstByte);
            byte byteEnd = (byte)(value[i] >> _positionBit);

            byte byteMerged = (byte)(byteStart | byteEnd);
            
            _byteArray[_positionByte++] = byteMerged;
        }

        byte byteLast = (byte)(value[^1] << bitsAppendedToFirstByte);

        _byteArray[_positionByte] = byteLast;
    }

    /// <inheritdoc />
    public byte[] ToBytes()
    {
        int byteCount = _positionByte + (_positionBit == 0 ? 0 : 1);

        return _byteArray.Take(byteCount).ToArray();
    }

    /// <inheritdoc />
    public int BytesWritten()
    {
        return _positionBit == 0 ? _positionByte : _positionByte + 1;
    }

    /// <inheritdoc />
    public IDisposable Bookmark()
    {
        return new DataBufferWriterBookmark(this, _positionByte, _positionBit);
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

    private class DataBufferWriterBookmark : IDisposable
    {
        #region Fields

        private readonly DataBufferWriter _writer;
        
        private readonly int _positionByte;
        private readonly int _positionBit;

        private volatile int _disposed;

        #endregion
        
        #region Constructor(s)

        public DataBufferWriterBookmark(DataBufferWriter writer, int positionByte, int positionBit)
        {
            _writer = writer;

            _positionByte = positionByte;
            _positionBit = positionBit;

            _disposed = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Restores the position of the <see cref="DataBufferWriter"/> to the 
        ///     position at the time this bookmark was created.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }
            
            _writer._positionByte = _positionByte;
            _writer._positionBit = _positionBit;
        }

        #endregion
    }
    
    #endregion
}