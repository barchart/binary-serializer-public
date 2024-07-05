#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Tests.Buffers;

public class DataBufferReaderTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    #endregion

    #region Constructor(s)

    public DataBufferReaderTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    #endregion

    #region Test Methods (ReadBit)

    [Fact]
    public void ReadBit_Once_ReturnsFirstBitOfFirstByte()
    {
        byte[] byteArray = { 0b10101100, 0b11010010 };
        DataBufferReader dataBuffer = new(byteArray);

        bool firstBit = dataBuffer.ReadBit();

        Assert.True(firstBit);
    }

    [Fact]
    public void ReadBit_Twice_ReturnsSecondBitOfFirstByte()
    {
        byte[] byteArray = { 0b10101100, 0b11010010 };
        DataBufferReader dataBuffer = new(byteArray);

        dataBuffer.ReadBit();
        bool secondBit = dataBuffer.ReadBit();

        Assert.False(secondBit);
    }

    [Fact]
    public void ReadBit_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = { 0b10101100 };
        DataBufferReader dataBuffer = new(byteArray);

        for (int i = 0; i < 8; i++)
        {
            dataBuffer.ReadBit();
        }

        Assert.Throws<InvalidOperationException>(() => dataBuffer.ReadBit());
    }

    #endregion
    
    #region Test Methods (ReadByte)

    [Fact]
    public void ReadByte_Once_ReturnsFirstByte()
    {
        byte first;

        byte[] byteArray = { first = 250 };
        DataBufferReader dataBuffer = new(byteArray);

        byte readFirst = dataBuffer.ReadByte();

        Assert.Equal(first, readFirst);
    }

    [Fact]
    public void ReadByte_Twice_ReturnsSecondByte()
    {
        byte first;
        byte second;

        byte[] byteArray = { first = 250, second = 175 };
        DataBufferReader dataBuffer = new(byteArray);

        byte readFirst = dataBuffer.ReadByte();
        byte readSecond = dataBuffer.ReadByte();

        Assert.Equal(first, readFirst);
        Assert.Equal(second, readSecond);
    }
    
    [Fact]
    public void ReadByte_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = { 250, 75 };
        DataBufferReader dataBuffer = new(byteArray);

        dataBuffer.ReadByte();
        dataBuffer.ReadByte();
        
        Assert.Throws<InvalidOperationException>(() => dataBuffer.ReadByte());
    }

    #endregion

    #region Test Methods (ReadBytes)

    [Fact]
    public void ReadBytes_WithExactArrayLength_ReturnsCompleteArray()
    {
        byte[]  byteArray = { 250, 175, 100 };
        DataBufferReader dataBuffer = new(byteArray);

        var readBytes = dataBuffer.ReadBytes(byteArray.Length);

        Assert.Equal(byteArray, readBytes);
    }

    [Fact]
    public void ReadBytes_WithPartialLength_ReturnsPartialArray()
    {
        byte[] byteArray = { 250, 175, 100 };
        DataBufferReader dataBuffer = new(byteArray);

        var expectedBytes = new byte[] { 250, 175 };
        var readBytes = dataBuffer.ReadBytes(2);

        Assert.Equal(expectedBytes, readBytes);
    }

    [Fact]
    public void ReadBytes_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = { 250, 175 };
        DataBufferReader dataBuffer = new(byteArray);

        Assert.Throws<InvalidOperationException>(() => dataBuffer.ReadBytes(3));
    }

    #endregion
    
    #region Test Methods (Reset)
    
    [Fact]
    public void ReadByte_AfterReset_ReturnsFirstByte()
    {
        byte first;
        byte second;

        byte[] byteArray = { first = 250, second = 175 };
        DataBufferReader dataBuffer = new(byteArray);

        byte readFirst = dataBuffer.ReadByte();
        byte readSecond = dataBuffer.ReadByte();

        Assert.Equal(first, readFirst);
        Assert.Equal(second, readSecond);
        
        dataBuffer.Reset();
        
        byte readThird = dataBuffer.ReadByte();
        
        Assert.Equal(first, readThird);
    }
    
    #endregion
}