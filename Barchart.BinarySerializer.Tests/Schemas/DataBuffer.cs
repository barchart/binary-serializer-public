using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests.Schemas;

public class DataBufferTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
    
    #endregion
    
    #region Constructor(s)
    
    public DataBufferTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    #endregion

    #region Test Methods (WriteBit)
    
    [Fact]
    public void WriteBit_True_ModifiesBuffer()
    {
        var byteArray = new byte[1];
        var dataBuffer = new DataBuffer(byteArray);
        
        dataBuffer.WriteBit(true);
        
        Assert.True(GetBit(byteArray[0], 0));
    }
    
    [Fact]
    public void WriteBit_False_ModifiesBuffer()
    {
        var byteArray = new byte[1];
        var dataBuffer = new DataBuffer(byteArray);
        
        dataBuffer.WriteBit(false);
        
        Assert.False(GetBit(byteArray[0], 0));
    }
    
    [Theory]
    [InlineData(new[] { true, true })]
    [InlineData(new[] { false, false })]
    [InlineData(new[] { true, true, false, true })]
    [InlineData(new[] { false, false, true, false })]
    [InlineData(new[] { true, false, true, false, true, false, true, false })]
    [InlineData(new[] { false, true, false, true, false, true, false, true })]
    public void WriteBit_Multiple_ModifiesBuffer(bool[] bits)
    {
        var byteArray = new byte[1];
        var dataBuffer = new DataBuffer(byteArray);
        
        for (int i = 0; i < bits.Length; i++)
        {
            dataBuffer.WriteBit(bits[i]);
        }
        
        _testOutputHelper.WriteLine(PrintBits(byteArray[0]));
        
        for (int i = 0; i < bits.Length; i++)
        {
            Assert.Equal(GetBit(byteArray[0], i), bits[i]);
        }
    }
    
    #endregion
    
    #region Static Methods
    
    private static bool GetBit(byte b, int index)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Bit index cannot be less than zero.");
        }
        
        if (index > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Bit index cannot be greater than seven.");
        }

        return ((b >> (7 - index)) & 1) == 1;
    }

    private static string PrintBits(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    } 
    
    #endregion
}