namespace Barchart.BinarySerializer.Buffers;

/// <summary>
///     A data buffer writer which uses a fixed-length byte array
///     for internal storage.
/// </summary>
public class DataBufferWriter : IDataBufferWriter
{
    #region Fields

    private readonly byte[] _byteArray;

    private int _positionByte;
    private int _positionBit;

    private const byte TRUE = 1;

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
        _byteArray = byteArray;

        _positionByte = 0;
        _positionBit = 0;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void WriteBit(bool value)
    {
        if (IsBufferFull())
        {
            throw new InvalidOperationException("Unable to write bit. The buffer is currently positioned at the end of the internal byte array.");
        }

        if (value)
        {
            byte byteCurrent;

            if (_positionBit == 0)
            {
                byteCurrent = 0;
            }
            else
            {
                byteCurrent = _byteArray[_positionByte];
            }

            byte byteMask = (byte)(TRUE << (7 - _positionBit));

            _byteArray[_positionByte] = (byte)(byteCurrent | byteMask);
        }

        AdvanceBit();
    }

    /// <inheritdoc />
    public void WriteByte(byte value)
    {
        if (IsBufferFull())
        {
            throw new InvalidOperationException("Buffer is full.");
        }

        if (_positionBit == 0)
        {
            _byteArray[_positionByte] = value;
            _positionByte++;
        }
        else
        {
            int firstPartLength = 8 - _positionBit;
            byte firstPart = (byte)(value >> _positionBit);
            _byteArray[_positionByte] |= firstPart;

            if (_positionByte + 1 < _byteArray.Length)
            {
                byte secondPart = (byte)(value << firstPartLength);
                _byteArray[_positionByte + 1] = secondPart;
            }

            _positionByte++;
        }
    }

    /// <inheritdoc />
    public void WriteBytes(byte[] value)
    {
        if (value.Length == 0) return;

        if (value.Length > _byteArray.Length)
        {
            throw new InvalidOperationException("Unable to write bytes. Request exceeds available buffer size.");
        }

        if (_positionBit != 0)
        {
            byte modifiedFirstByte = (byte)(value[0] >> _positionBit);

            _byteArray[_positionByte] |= modifiedFirstByte;
            _positionByte++;
            
            if (value.Length == 1)
            {
                byte nextBytePart = (byte)(value[0] << (8 - _positionBit));

                if (_positionByte < _byteArray.Length)
                {
                    _byteArray[_positionByte] |= nextBytePart;
                }
            }
            else if (value.Length > 1)
            {
                for (int i = 1; i < value.Length; i++)
                {
                    byte nextBytePart = (byte)(value[i - 1] << (8 - _positionBit));
                    byte currentBytePart = (byte)(value[i] >> _positionBit);

                    _byteArray[_positionByte] = (byte)(nextBytePart | currentBytePart);
                    _positionByte++;
                }

                byte lastBytePart = (byte)(value[value.Length - 1] << (8 - _positionBit));

                if (_positionByte < _byteArray.Length)
                {
                    _byteArray[_positionByte] |= lastBytePart;
                }
            }
        }
        else
        {
            for (int i = 0; i < value.Length; i++)
            {
                _byteArray[_positionByte] = value[i];
                _positionByte++;
            }
        }
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

    private bool IsBufferFull()
    {
        return _positionByte >= _byteArray.Length;
    }

    #endregion
}