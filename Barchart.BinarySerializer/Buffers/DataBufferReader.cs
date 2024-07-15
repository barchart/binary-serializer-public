#region Using Directives

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
            throw new InsufficientCapacityException(true);
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
    public void Reset()
    {
        _positionByte = 0;
        _positionBit = 0;
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
}