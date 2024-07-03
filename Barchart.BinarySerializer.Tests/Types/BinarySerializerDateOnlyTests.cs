#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerDateOnlyTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    private readonly BinarySerializerDateOnly _serializer;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerDateOnlyTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerDateOnly();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(1, 1, 1)]
    [InlineData(9999, 12, 31)]
    [InlineData(2000, 2, 29)]
    [InlineData(1970, 1, 1)]
    public void Encode_Various_WritesExpectedBytes(int year, int month, int day)
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
        
        DateOnly value = new(year, month, day);
        _serializer.Encode(mock.Object, value);
            
        Assert.Empty(bitsWritten);
        Assert.Empty(byteWritten);
        
        int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
        byte[] expectedBytes = BitConverter.GetBytes(daysSinceEpoch);
        
        Assert.Single(bytesWritten);
        Assert.Equal(expectedBytes.Length, bytesWritten[0].Length);
        
        for (int i = 0; i < expectedBytes.Length; i++)
        {
            var expectedByte = expectedBytes[i];
            var actualByte = bytesWritten[0][i];
            
            Assert.Equal(expectedByte, actualByte);
        }
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(1, 1, 1)]
    [InlineData(9999, 12, 31)]
    [InlineData(2000, 2, 29)]
    [InlineData(1970, 1, 1)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(int year, int month, int day)
    {
        Mock<IDataBufferReader> mock = new();
        
        DateOnly value = new(year, month, day);
        int daysSinceEpoch = value.DayNumber - DateOnly.MinValue.DayNumber;
        mock.Setup(m => m.ReadBytes(4)).Returns(BitConverter.GetBytes(daysSinceEpoch));

        var deserialized = _serializer.Decode(mock.Object);
        
        Assert.Equal(value, deserialized);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(2023, 1, 1, 2023, 1, 1)]
    [InlineData(2023, 1, 1, 2023, 1, 2)]
    [InlineData(1, 1, 1, 9999, 12, 31)]
    [InlineData(2000, 2, 29, 2000, 2, 29)]
    [InlineData(1970, 1, 1, 1970, 1, 1)]
    public void GetEquals_Various_MatchesIEquatableOutput(int year1, int month1, int day1, int year2, int month2, int day2)
    {
        DateOnly date1 = new(year1, month1, day1);
        DateOnly date2 = new(year2, month2, day2);
        
        var actual = _serializer.GetEquals(date1, date2);
        var expected = date1.Equals(date2);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}