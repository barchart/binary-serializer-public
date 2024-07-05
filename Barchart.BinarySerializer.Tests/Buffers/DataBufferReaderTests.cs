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