#region Using Statements

using Barchart.BinarySerializer.Buffers;

using Barchart.BinarySerializer.Tests.Common;

#endregion

namespace Barchart.BinarySerializer.Tests.Buffers;

public class DataBufferWriterTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    #endregion

    #region Constructor(s)

    public DataBufferWriterTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    #endregion

    #region Test Methods (WriteBit)

    [Fact]
    public void WriteBit_True_ModifiesBuffer()
    {
        byte[] byteArray = new byte[1];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);

        Assert.True(GetBit(byteArray[0], 0));
    }

    [Fact]
    public void WriteBit_False_ModifiesBuffer()
    {
        byte[] byteArray = new byte[1];
        DataBufferWriter dataBuffer = new(byteArray);

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
        byte[] byteArray = new byte[1];
        DataBufferWriter dataBuffer = new(byteArray);

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

    #region Test Methods (WriteByte)

    [Fact]
    public void WriteByte_OneByte_ModifiesBuffer()
    {
        byte[] byteArray = new byte[1];
        DataBufferWriter dataBuffer = new(byteArray);
        
        byte valueToWrite = 0xAC;

        dataBuffer.WriteByte(valueToWrite);

        Assert.Equal(valueToWrite, byteArray[0]);
    }

    [Fact]
    public void WriteByte_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = new byte[1];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteByte(0xFF);

        Assert.Throws<InvalidOperationException>(() => dataBuffer.WriteByte(0xFF));
    }

    #endregion

    #region Test Methods (WriteBytes)

    [Fact]
    public void WriteBytes_ThreeBytes_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);
        
        byte[] valuesToWrite = { 0xAC, 0xBD, 0xCE };
        
        dataBuffer.WriteBytes(valuesToWrite.ToArray());

        Assert.Equal(valuesToWrite, byteArray);
    }

    [Fact]
    public void WriteBytes_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);
        
        byte[] valuesToWrite = { 0xAC, 0xBD, 0xCE };

        Assert.Throws<InvalidOperationException>(() => dataBuffer.WriteBytes(valuesToWrite));
    }

    #endregion

    #region Test Methods (Multiple - WriteBit + WriteByte)
    
    [Fact]
    public void Write_OneBitOneByte_ModifiesBuffer()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(false);
        dataBuffer.WriteByte(0b11111111);
        
        Assert.Equal(0b01111111, byteArray[0]);
        Assert.Equal(0b10000000, byteArray[1]);
    }
    
    [Fact]
    public void Write_TwoBitsOneByte_ModifiesBuffer()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(false);
        dataBuffer.WriteBit(false);
        dataBuffer.WriteByte(0b11111111);
        
        Assert.Equal(0b00111111, byteArray[0]);
        Assert.Equal(0b11000000, byteArray[1]);
    }
    
    [Fact]
    public void Write_OneBitTwoBytes_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(false);
        dataBuffer.WriteByte(0b11111111);
        dataBuffer.WriteByte(0b11111111);
        
        Assert.Equal(0b01111111, byteArray[0]);
        Assert.Equal(0b11111111, byteArray[1]);
        Assert.Equal(0b10000000, byteArray[2]);
    }
    
    [Fact]
    public void Write_OneBitOneByteOneBitOneByte_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(false);
        dataBuffer.WriteByte(0b11111111);
        dataBuffer.WriteBit(false);
        dataBuffer.WriteByte(0b11111111);
        
        Assert.Equal(0b01111111, byteArray[0]);
        Assert.Equal(0b10111111, byteArray[1]);
        Assert.Equal(0b11000000, byteArray[2]);
    }
    
    #endregion

    #region Test Methods (Multiple - WriteBit + WriteBytes)

    [Fact]
    public void WriteBitAndWriteBytes_Combination_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);
        dataBuffer.WriteBytes(new byte[] { 0xAC, 0xBD });

        Assert.Equal(0b11010110, byteArray[0]);
        Assert.Equal(0b01011110, byteArray[1]);
        Assert.Equal(0b10000000, byteArray[2]);
    }

    [Fact]
    public void WriteBytesAndWriteBit_Combination_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBytes(new byte[] { 0xAC, 0xBD });
        dataBuffer.WriteBit(true);

        Assert.Equal(0xAC, byteArray[0]);
        Assert.Equal(0xBD, byteArray[1]);
        Assert.Equal(0b10000000, byteArray[2]);
    }

    [Fact]
    public void WriteMultipleBytes_Sequentially_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBytes(new byte[] { 0xAC });
        dataBuffer.WriteBytes(new byte[] { 0xBD, 0xCE });

        Assert.Equal(0xAC, byteArray[0]);
        Assert.Equal(0xBD, byteArray[1]);
        Assert.Equal(0xCE, byteArray[2]);
        Assert.Equal(0x00, byteArray[3]);
    }

    [Fact]
    public void WriteComplexCombination_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);
        dataBuffer.WriteByte(0xFF);
        dataBuffer.WriteBytes(new byte[] { 0xAC, 0xBD });
        dataBuffer.WriteBit(false);

        Assert.Equal(0b11111111, byteArray[0]);
        Assert.Equal(0b11010110, byteArray[1]);
        Assert.Equal(0b01011110, byteArray[2]);
        Assert.Equal(0b10000000, byteArray[3]);
    }

    #endregion
    
    #region Test Methods (ToBytes)

    [Fact]
    public void ToBytes_NoDataHasBeenWritten_ReturnsEmptyArray()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        var bytes = dataBuffer.ToBytes();

        Assert.Empty(bytes);
    }

    [Fact]
    public void ToBytes_OneBitHasBeenWritten_ReturnsOneByteArray()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        for (int i = 0; i < 1; i++)
        {
            dataBuffer.WriteBit(i % 2 == 0);
        }

        var bytes = dataBuffer.ToBytes();

        Assert.Single(bytes);
    }

    [Fact]
    public void ToBytes_EightBitsHaveBeenWritten_ReturnsOneByteArray()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        for (int i = 0; i < 8; i++)
        {
            dataBuffer.WriteBit(i % 2 == 0);
        }

        var bytes = dataBuffer.ToBytes();

        Assert.Single(bytes);
    }

    [Fact]
    public void ToBytes_TwelveBitsHaveBeenWritten_ReturnsTwoBytesArray()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        for (int i = 0; i < 12; i++)
        {
            dataBuffer.WriteBit(i % 2 == 0);
        }

        var bytes = dataBuffer.ToBytes();

        Assert.Equal(2, bytes.Length);
    }

    [Fact]
    public void ToBytes_SixteenBitsHaveBeenWritten_ReturnsTwoBytesArray()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        for (int i = 0; i < 16; i++)
        {
            dataBuffer.WriteBit(i % 2 == 0);
        }

        var bytes = dataBuffer.ToBytes();

        Assert.Equal(2, bytes.Length);
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