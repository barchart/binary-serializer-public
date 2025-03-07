#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Exceptions;

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
    
    [Fact]
    public void WriteBit_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = new byte[1];
        DataBufferWriter dataBuffer = new(byteArray);

        for (int i = 0; i < 8; i++)
        {
            dataBuffer.WriteBit(true);
        }

        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.WriteBit(false));
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

        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.WriteByte(0xFF));
    }

    #endregion

    #region Test Methods (WriteBytes)

    [Fact]
    public void WriteBytes_ThreeBytes_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);
        
        byte[] valuesToWrite = [0xAC, 0xBD, 0xCE];
        
        dataBuffer.WriteBytes(valuesToWrite.ToArray());

        Assert.Equal(valuesToWrite, byteArray);
    }

    [Fact]
    public void WriteBytes_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);
        
        byte[] valuesToWrite = [0xAC, 0xBD, 0xCE];

        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.WriteBytes(valuesToWrite));
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

    [Fact]
    public void Write_OneByteOneBitOneByte_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteByte(0b11111111);
        dataBuffer.WriteBit(false);
        dataBuffer.WriteByte(0b11111111);
        
        Assert.Equal(0b11111111, byteArray[0]);
        Assert.Equal(0b01111111, byteArray[1]);
        Assert.Equal(0b10000000, byteArray[2]);
    }
    
    #endregion

    #region Test Methods (Multiple - WriteBit + WriteBytes)

    [Fact]
    public void Write_OneBitOneByteArray_ModifiesBuffer()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);
        dataBuffer.WriteBytes([0b00000011]);

        Assert.Equal(0b10000001, byteArray[0]);
        Assert.Equal(0b10000000, byteArray[1]);
    }

    [Fact]
    public void Write_OneBitTwoByteArray_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);
        dataBuffer.WriteBytes([0b00000011, 0b00000111]);

        Assert.Equal(0b10000001, byteArray[0]);
        Assert.Equal(0b10000011, byteArray[1]);
        Assert.Equal(0b10000000, byteArray[2]);
    }

    [Fact]
    public void Write_OneBitThreeByteArray_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);
        dataBuffer.WriteBytes([0b00000011, 0b00000111, 0b11111110]);

        Assert.Equal(0b10000001, byteArray[0]);
        Assert.Equal(0b10000011, byteArray[1]);
        Assert.Equal(0b11111111, byteArray[2]);
        Assert.Equal(0b00000000, byteArray[3]);
    }

    [Fact]
    public void Write_OneByteArrayOneBit_ModifiesBuffer()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBytes([0b10000001, 0b10000011]);
        dataBuffer.WriteBit(true);

        Assert.Equal(0b10000001, byteArray[0]);
        Assert.Equal(0b10000011, byteArray[1]);
        Assert.Equal(0b10000000, byteArray[2]);
    }

    [Fact]
    public void Write_TwoByteArrayOneBit_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBytes([0b10000001, 0b10000011, 0b11111111]);
        dataBuffer.WriteBit(false);

        Assert.Equal(0b10000001, byteArray[0]);
        Assert.Equal(0b10000011, byteArray[1]);
        Assert.Equal(0b11111111, byteArray[2]);
        Assert.Equal(0b00000000, byteArray[3]);
    }
    
    [Fact]
    public void Write_OneBitTwoByteArray_ThrowsError()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);
        
        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.WriteBytes([0b00000011, 0b00000111]));
    }
    
    [Fact]
    public void Write_OneBitThreeByteArray_ThrowsError()
    {
        byte[] byteArray = new byte[3];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);

        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.WriteBytes([0b00000011, 0b00000111, 0b11111110]));
    }

    #endregion
    
    #region Test Methods (Multiple - WriteByte + WriteBytes)

    [Fact]
    public void Write_OneByteTwoByteArray_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteByte(0b10000001);
        dataBuffer.WriteBytes([0b10000011, 0b11111111]);

        Assert.Equal(0b10000001, byteArray[0]);
        Assert.Equal(0b10000011, byteArray[1]);
        Assert.Equal(0b11111111, byteArray[2]);
        Assert.Equal(0b00000000, byteArray[3]);
    }

    [Fact]
    public void Write_TwoByteArrayOneByte_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBytes([0xBD, 0xCE]);
        dataBuffer.WriteByte(0xAC);

        Assert.Equal(0xBD, byteArray[0]);
        Assert.Equal(0xCE, byteArray[1]);
        Assert.Equal(0xAC, byteArray[2]);
        Assert.Equal(0x00, byteArray[3]);
    }

    [Fact]
    public void Write_TwoBytesOneByteArray_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteByte(0b10000001);
        dataBuffer.WriteByte(0b10000011);
        dataBuffer.WriteBytes([0b11111111, 0b00000000]);

        Assert.Equal(0b10000001, byteArray[0]);
        Assert.Equal(0b10000011, byteArray[1]);
        Assert.Equal(0b11111111, byteArray[2]);
        Assert.Equal(0b00000000, byteArray[3]);
    }

    #endregion

    #region Test Methods (Multiple - WriteBit + WriteByte + WriteBytes)

    [Fact]
    public void Write_OneBitOneByteTwoByteArrayOneBit_ModifiesBuffer()
    {
        byte[] byteArray = new byte[4];
        DataBufferWriter dataBuffer = new(byteArray);

        dataBuffer.WriteBit(true);
        dataBuffer.WriteByte(0b11111111);
        dataBuffer.WriteBytes([0b10101100, 0b10111101]);
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

    #region Test Methods (BytesWritten)

    [Fact]
    public void BytesWritten_NewDataBufferWriter_ReturnsZero()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);

        Assert.Equal(0, dataBuffer.BytesWritten);
    } 
    
    [Fact]
    public void BytesWritten_WriteBit_ReturnsOne()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(true);
        
        Assert.Equal(1, dataBuffer.BytesWritten);
    } 
    
    [Fact]
    public void BytesWritten_WriteByte_ReturnsOne()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteByte(0b00000000);
        
        Assert.Equal(1, dataBuffer.BytesWritten);
    } 
    
    [Fact]
    public void BytesWritten_WriteByteWriteBit_ReturnsTwo()
    {
        byte[] byteArray = new byte[2];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteByte(0b00000000);
        dataBuffer.WriteBit(true);
        
        Assert.Equal(2, dataBuffer.BytesWritten);
    } 

    #endregion
    
    #region Test Methods (Bug: Populated Array)
    
    [Fact]
    public void Write_PopulatedArrayOneBitOneByte_ModifiesBuffer()
    {
        byte[] byteArray = [0b11111111, 0b11111111];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(false);
        dataBuffer.WriteByte(0b10101010);
        
        Assert.Equal(0b01010101, byteArray[0]);
        Assert.Equal(0b00000000, byteArray[1]);
        
        byteArray = [0b11111111, 0b11111111];
        dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(true);
        dataBuffer.WriteByte(0b01010101);
        
        Assert.Equal(0b10101010, byteArray[0]);
        Assert.Equal(0b10000000, byteArray[1]);
    }
    
    [Fact]
    public void Write_PopulatedArrayTwoBitsOneByte_ModifiesBuffer()
    {
        byte[] byteArray = [0b11111111, 0b11111111];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(false);
        dataBuffer.WriteBit(true);
        dataBuffer.WriteByte(0b01010101);
        
        _testOutputHelper.WriteLine(PrintBits(byteArray[0]));
        _testOutputHelper.WriteLine(PrintBits(byteArray[1]));
        
        Assert.Equal(0b01010101, byteArray[0]);
        Assert.Equal(0b01000000, byteArray[1]);
    }
    
    [Fact]
    public void Write_PopulatedArrayThreeBitsTwoBytes_ModifiesBuffer()
    {
        byte[] byteArray = [0b11111111, 0b11111111, 0b11111111];
        DataBufferWriter dataBuffer = new(byteArray);
        
        dataBuffer.WriteBit(false);
        dataBuffer.WriteBit(true);
        dataBuffer.WriteBit(false);
        
        dataBuffer.WriteBytes([0b10101010, 0b10101010]);
        
        Assert.Equal(0b01010101, byteArray[0]);
        Assert.Equal(0b01010101, byteArray[1]);
        Assert.Equal(0b01000000, byteArray[2]);
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