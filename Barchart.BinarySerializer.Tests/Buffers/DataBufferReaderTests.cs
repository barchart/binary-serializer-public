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
        var byteArray = new byte[] { 0b10101100, 0b11010010 };
        var dataBuffer = new DataBufferReader(byteArray);

        bool firstBit = dataBuffer.ReadBit();

        Assert.True(firstBit);
    }

    [Fact]
    public void ReadBit_Twice_ReturnsSecondBitOfFirstByte()
    {
        var byteArray = new byte[] { 0b10101100, 0b11010010 };
        var dataBuffer = new DataBufferReader(byteArray);

        dataBuffer.ReadBit();
        bool secondBit = dataBuffer.ReadBit();

        Assert.False(secondBit);
    }

    [Fact]
    public void ReadBit_ExceedingArrayLength_ThrowsError()
    {
        var byteArray = new byte[] { 0b10101100 };
        var dataBuffer = new DataBufferReader(byteArray);

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

        var byteArray = new[] { first = 250 };
        var dataBuffer = new DataBufferReader(byteArray);

        byte readFirst = dataBuffer.ReadByte();

        Assert.Equal(first, readFirst);
    }

    [Fact]
    public void ReadByte_Twice_ReturnsSecondByte()
    {
        byte first;
        byte second;

        var byteArray = new[] { first = 250, second = 175 };
        var dataBuffer = new DataBufferReader(byteArray);

        byte readFirst = dataBuffer.ReadByte();
        byte readSecond = dataBuffer.ReadByte();

        Assert.Equal(first, readFirst);
        Assert.Equal(second, readSecond);
    }
    
    [Fact]
    public void ReadByte_ExceedingArrayLength_ThrowsError()
    {
        var byteArray = new[] { (byte)250, (byte)175 };
        var dataBuffer = new DataBufferReader(byteArray);

        dataBuffer.ReadByte();
        dataBuffer.ReadByte();
        
        Assert.Throws<InvalidOperationException>(() => dataBuffer.ReadByte());
    }

    #endregion

    #region Test Methods (ReadBytes)

    [Fact]
    public void ReadBytes_WithExactArrayLength_ReturnsCompleteArray()
    {
        var byteArray = new byte[] { 250, 175, 100 };
        var dataBuffer = new DataBufferReader(byteArray);

        var readBytes = dataBuffer.ReadBytes(byteArray.Length);

        Assert.Equal(byteArray, readBytes);
    }

    [Fact]
    public void ReadBytes_WithPartialLength_ReturnsPartialArray()
    {
        var byteArray = new byte[] { 250, 175, 100 };
        var dataBuffer = new DataBufferReader(byteArray);

        var expectedBytes = new byte[] { 250, 175 };
        var readBytes = dataBuffer.ReadBytes(2);

        Assert.Equal(expectedBytes, readBytes);
    }

    [Fact]
    public void ReadBytes_ExceedingArrayLength_ThrowsError()
    {
        var byteArray = new byte[] { 250, 175 };
        var dataBuffer = new DataBufferReader(byteArray);

        Assert.Throws<InvalidOperationException>(() => dataBuffer.ReadBytes(3));
    }

    #endregion
    
    #region Test Methods (Reset)
    
    [Fact]
    public void ReadByte_AfterReset_ReturnsFirstByte()
    {
        byte first;
        byte second;

        var byteArray = new[] { first = 250, second = 175 };
        var dataBuffer = new DataBufferReader(byteArray);

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