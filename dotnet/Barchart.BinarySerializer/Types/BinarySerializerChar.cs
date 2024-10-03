#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) characters to (and from) a binary data source.
/// </summary>
public class BinarySerializerChar : IBinaryTypeSerializer<char>
{
    #region Constants

    private const int ENCODED_LENGTH_IN_BYTES = sizeof(char);
    
    #endregion

    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, char value) 
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public char Decode(IDataBufferReader reader)
    {
        return BitConverter.ToChar(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }
    
    /// <inheritdoc />
    public bool GetEquals(char a, char b)
    {
        return a.Equals(b);
    }

    #endregion
}