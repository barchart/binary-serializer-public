#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) DateOnly values to (and from) a binary data source.
/// </summary>
public class BinarySerializerDateOnly : IBinaryTypeSerializer<DateOnly>
{
    #region Fields

    private readonly BinarySerializerInt _binarySerializerInt;
    
    #endregion

    #region Constructors

    public BinarySerializerDateOnly()
    {
        _binarySerializerInt = new BinarySerializerInt();
    }
    
    #endregion

    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, DateOnly value)
    {
        _binarySerializerInt.Encode(buffer, GetDaysSinceEpoch(value));
    }

    /// <inheritdoc />
    public DateOnly Decode(IDataBufferReader buffer)
    {
        int daysSinceEpoch = _binarySerializerInt.Decode(buffer);
        
        return DateOnly.MinValue.AddDays(daysSinceEpoch);
    }
    
    /// <inheritdoc />
    public bool GetEquals(DateOnly a, DateOnly b)
    {
        return a.Equals(b);
    }
    
    private static int GetDaysSinceEpoch(DateOnly value)
    {
        return value.DayNumber - DateOnly.MinValue.DayNumber;
    }

    #endregion
}