#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerDateTimeTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    private readonly BinarySerializerDateTime _serializer;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerDateTimeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerDateTime();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Theory]
    [InlineData(2023, 1, 1, 12, 0, 0)]
    [InlineData(1, 1, 1, 0, 0, 0)]
    [InlineData(9999, 12, 31, 23, 59, 59)]
    [InlineData(2000, 2, 29, 15, 30, 45)]
    [InlineData(1970, 1, 1, 0, 0, 0)]
    public void Encode_Various_WritesExpectedBytes(int year, int month, int day, int hour, int minute, int second)
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
        
        DateTime value = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        _serializer.Encode(mock.Object, value);
            
        Assert.Empty(bitsWritten);
        Assert.Empty(byteWritten);
        
        long ticks = value.Ticks;
        byte[] expectedBytes = BitConverter.GetBytes(ticks);
        
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
    [InlineData(2023, 1, 1, 12, 0, 0)]
    [InlineData(1, 1, 1, 0, 0, 0)]
    [InlineData(9999, 12, 31, 23, 59, 59)]
    [InlineData(2000, 2, 29, 15, 30, 45)]
    [InlineData(1970, 1, 1, 0, 0, 0)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(int year, int month, int day, int hour, int minute, int second)
    {
        Mock<IDataBufferReader> mock = new();
        
        DateTime value = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        mock.Setup(m => m.ReadBytes(8)).Returns(BitConverter.GetBytes(value.Ticks));

        var deserialized = _serializer.Decode(mock.Object);
        
        Assert.Equal(value, deserialized);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(2023, 1, 1, 12, 0, 0, 2023, 1, 1, 12, 0, 0)]
    [InlineData(1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0)]
    [InlineData(9999, 12, 31, 23, 59, 59, 9999, 12, 31, 23, 59, 59)]
    [InlineData(2000, 2, 29, 15, 30, 45, 2000, 2, 29, 15, 30, 45)]
    [InlineData(1970, 1, 1, 0, 0, 0, 1970, 1, 1, 0, 0, 1)]
    public void GetEquals_Various_MatchesIEquatableOutput(int year1, int month1, int day1, int hour1, int minute1, int second1, 
        int year2, int month2, int day2, int hour2, int minute2, int second2)
    {
        DateTime date1 = new DateTime(year1, month1, day1, hour1, minute1, second1, DateTimeKind.Utc);
        DateTime date2 = new DateTime(year2, month2, day2, hour2, minute2, second2, DateTimeKind.Utc);
        
        var actual = _serializer.GetEquals(date1, date2);
        var expected = date1.Equals(date2);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}