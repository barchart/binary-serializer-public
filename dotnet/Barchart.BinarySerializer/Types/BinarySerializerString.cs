#region Using Statements

using System.Text;

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types.Exceptions;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) string values to (and from) a binary data source.
/// </summary>
public class BinarySerializerString : IBinaryTypeSerializer<string?>
{
    #region Constants

    private const int MAXIMUM_STRING_LENGTH_IN_BYTES = ushort.MaxValue;

    #endregion

    #region Fields
    
    private static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;
    private readonly Encoding _encoding;

    private readonly BinarySerializerUShort _binarySerializerUShort;

    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Initializes a new instance of the <see cref="BinarySerializerString"/> class using the default encoding.
    /// </summary>
    public BinarySerializerString() : this(DEFAULT_ENCODING)
    {

    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BinarySerializerString"/> class using the specified encoding.
    /// </summary>
    /// <param name="encoding">
    ///     The encoding to use when reading and writing strings.
    /// </param>
    public BinarySerializerString(Encoding encoding)
    {
        _encoding = encoding;

        _binarySerializerUShort = new BinarySerializerUShort();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    /// <exception cref="InvalidStringLengthException">
    ///     Thrown when the serialized string would require more than <see cref="MAXIMUM_STRING_LENGTH_IN_BYTES"/> bytes.
    /// </exception>
    public void Encode(IDataBufferWriter writer, string? value)
    {
        Serialization.WriteNullFlag(writer, value == null);
        
        if (value == null)
        {
            return;
        }

        byte[] bytes = _encoding.GetBytes(value);

        if (bytes.Length > MAXIMUM_STRING_LENGTH_IN_BYTES)
        {
            throw new InvalidStringLengthException(bytes.Length, MAXIMUM_STRING_LENGTH_IN_BYTES);
        }

        _binarySerializerUShort.Encode(writer, Convert.ToUInt16(bytes.Length));

        writer.WriteBytes(bytes);
    }

    /// <inheritdoc />
    public string? Decode(IDataBufferReader reader)
    {
        if (Serialization.ReadNullFlag(reader))
        {
            return null;
        }

        return _encoding.GetString(reader.ReadBytes(_binarySerializerUShort.Decode(reader)));
    }

    /// <inheritdoc />
    public bool GetEquals(string? a, string? b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        return a.Equals(b);
    }

    #endregion
}