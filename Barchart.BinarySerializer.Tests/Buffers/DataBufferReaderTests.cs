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
    public void ReadBit_Once_ReturnsFirstBitFromFirstByte()
    {
        byte[] byteArray = { 0b10101100, 0b11010010 };
        DataBufferReader dataBuffer = new(byteArray);

        bool firstBit = dataBuffer.ReadBit();

        Assert.True(firstBit);
    }

    [Fact]
    public void ReadBit_Twice_ReturnsSecondBitFromFirstByte()
    {
        byte[] byteArray = { 0b10101100, 0b11010010 };
        DataBufferReader dataBuffer = new(byteArray);

        bool firstBit = dataBuffer.ReadBit();
        bool secondBit = dataBuffer.ReadBit();

        Assert.True(firstBit);
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
    public void ReadBytes_SameAsArrayLength_ReturnsCompleteArray()
    {
        byte first;
        byte second;
        byte third;
        
        byte[] byteArray = { first = 250, second = 175, third = 100 };
        DataBufferReader dataBuffer = new(byteArray);

        var expectedBytes = new byte[] { first, second, third };
        var readBytes = dataBuffer.ReadBytes(3);

        Assert.Equal(expectedBytes, readBytes);
    }

    [Fact]
    public void ReadBytes_LessThanArrayLength_ReturnsPartialArray()
    {
        byte first;
        byte second;
        
        byte[] byteArray = { first = 250, second = 175, 100 };
        DataBufferReader dataBuffer = new(byteArray);

        var expectedBytes = new byte[] { first, second };
        var readBytes = dataBuffer.ReadBytes(2);

        Assert.Equal(expectedBytes, readBytes);
    }

    [Fact]
    public void ReadBytes_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = { 250, 175 };
        DataBufferReader dataBuffer = new(byteArray);

        Assert.Throws<InvalidOperationException>(() => dataBuffer.ReadBytes(byteArray.Length + 1));
    }

    #endregion
    
    #region Test Methods (Multiple - ReadBit + ReadByte)
    
    [Fact]
    public void Read_OneBitOneByte_ReturnsCorrectData()
    {
        byte[] byteArray = new byte[2] { 0b10000001, 0b10000001 };
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit = dataBuffer.ReadBit();
        byte readByte = dataBuffer.ReadByte();
        
        Assert.True(readBit);
        Assert.Equal(0b00000011, readByte);
    }
    
    [Fact]
    public void Read_TwoBitsOneByte_ReturnsCorrectData()
    {
        byte[] byteArray = new byte[2] { 0b10000001, 0b10000001 };
        DataBufferReader dataBuffer = new(byteArray);

        bool readBitOne = dataBuffer.ReadBit();
        bool readBitTwo = dataBuffer.ReadBit();
        byte readByte = dataBuffer.ReadByte();
        
        Assert.True(readBitOne);
        Assert.False(readBitTwo);
        
        Assert.Equal(0b00000110, readByte);
    }
    
    [Fact]
    public void Read_OneBitTwoBytes_ReturnsCorrectData()
    {
        byte[] byteArray = new byte[3] { 0b10000001, 0b10000011, 0b10000000 };
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit = dataBuffer.ReadBit();
        byte readByteOne = dataBuffer.ReadByte();
        byte readByteTwo = dataBuffer.ReadByte();
        
        Assert.True(readBit);
        
        Assert.Equal(0b00000011, readByteOne);
        Assert.Equal(0b00000111, readByteTwo);
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