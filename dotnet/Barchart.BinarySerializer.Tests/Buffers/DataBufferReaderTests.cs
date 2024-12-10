#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Exceptions;

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
        byte[] byteArray = [0b10101100, 0b11010010];
        DataBufferReader dataBuffer = new(byteArray);

        bool firstBit = dataBuffer.ReadBit();

        Assert.True(firstBit);
    }

    [Fact]
    public void ReadBit_Twice_ReturnsSecondBitFromFirstByte()
    {
        byte[] byteArray = [0b10101100, 0b11010010];
        DataBufferReader dataBuffer = new(byteArray);

        bool firstBit = dataBuffer.ReadBit();
        bool secondBit = dataBuffer.ReadBit();

        Assert.True(firstBit);
        Assert.False(secondBit);
    }

    [Fact]
    public void ReadBit_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = [0b10101100];
        DataBufferReader dataBuffer = new(byteArray);

        for (int i = 0; i < 8; i++)
        {
            dataBuffer.ReadBit();
        }

        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.ReadBit());
    }

    #endregion
    
    #region Test Methods (ReadByte)

    [Fact]
    public void ReadByte_Once_ReturnsFirstByte()
    {
        byte first;

        byte[] byteArray = [first = 250];
        DataBufferReader dataBuffer = new(byteArray);

        byte readFirst = dataBuffer.ReadByte();

        Assert.Equal(first, readFirst);
    }

    [Fact]
    public void ReadByte_Twice_ReturnsSecondByte()
    {
        byte first;
        byte second;

        byte[] byteArray = [first = 250, second = 175];
        DataBufferReader dataBuffer = new(byteArray);

        byte readFirst = dataBuffer.ReadByte();
        byte readSecond = dataBuffer.ReadByte();

        Assert.Equal(first, readFirst);
        Assert.Equal(second, readSecond);
    }
    
    [Fact]
    public void ReadByte_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = [250, 75];
        DataBufferReader dataBuffer = new(byteArray);

        dataBuffer.ReadByte();
        dataBuffer.ReadByte();
        
        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.ReadByte());
    }

    #endregion

    #region Test Methods (ReadBytes)

    [Fact]
    public void ReadBytes_SameAsArrayLength_ReturnsCompleteArray()
    {
        byte first;
        byte second;
        byte third;
        
        byte[] byteArray = [first = 250, second = 175, third = 100];
        DataBufferReader dataBuffer = new(byteArray);

        var expectedBytes = new[] { first, second, third };
        var readBytes = dataBuffer.ReadBytes(3);

        Assert.Equal(expectedBytes, readBytes);
    }

    [Fact]
    public void ReadBytes_LessThanArrayLength_ReturnsPartialArray()
    {
        byte first;
        byte second;
        
        byte[] byteArray = [first = 250, second = 175, 100];
        DataBufferReader dataBuffer = new(byteArray);

        var expectedBytes = new[] { first, second };
        var readBytes = dataBuffer.ReadBytes(2);

        Assert.Equal(expectedBytes, readBytes);
    }

    [Fact]
    public void ReadBytes_ExceedingArrayLength_ThrowsError()
    {
        byte[] byteArray = [250, 175];
        DataBufferReader dataBuffer = new(byteArray);

        Assert.Throws<InsufficientCapacityException>(() => dataBuffer.ReadBytes(byteArray.Length + 1));
    }

    #endregion
    
    #region Test Methods (Multiple - ReadBit + ReadByte)
    
    [Fact]
    public void Read_OneBitOneByte_ReturnsCorrectData()
    {
        byte[] byteArray = [0b01111111, 0b10000000];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit = dataBuffer.ReadBit();
        byte readByte = dataBuffer.ReadByte();

        Assert.False(readBit);
        Assert.Equal(0b11111111, readByte);
    }

    [Fact]
    public void Read_TwoBitsOneByte_ReturnsCorrectData()
    {
        byte[] byteArray = [0b00111111, 0b11000000];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBitOne = dataBuffer.ReadBit();
        bool readBitTwo = dataBuffer.ReadBit();
        byte readByte = dataBuffer.ReadByte();

        Assert.False(readBitOne);
        Assert.False(readBitTwo);
        Assert.Equal(0b11111111, readByte);
    }

    [Fact]
    public void Read_OneBitTwoBytes_ReturnsCorrectData()
    {
        byte[] byteArray = [0b01111111, 0b11111111, 0b10000000];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit = dataBuffer.ReadBit();
        byte readByteOne = dataBuffer.ReadByte();
        byte readByteTwo = dataBuffer.ReadByte();

        Assert.False(readBit);
        Assert.Equal(0b11111111, readByteOne);
        Assert.Equal(0b11111111, readByteTwo);
    }

    [Fact]
    public void Read_OneBitOneByteOneBitOneByte_ReturnsCorrectData()
    {
        byte[] byteArray = [0b01111111, 0b10111111, 0b11000000];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBitOne = dataBuffer.ReadBit();
        byte readByteOne = dataBuffer.ReadByte();
        bool readBitTwo = dataBuffer.ReadBit();
        byte readByteTwo = dataBuffer.ReadByte();

        Assert.False(readBitOne);
        Assert.Equal(0b11111111, readByteOne);
        Assert.False(readBitTwo);
        Assert.Equal(0b11111111, readByteTwo);
    }

    [Fact]
    public void Read_OneByteOneBitOneByte_ReturnsCorrectData()
    {
        byte[] byteArray = [0b11111111, 0b01111111, 0b10000000];
        DataBufferReader dataBuffer = new(byteArray);

        byte readByteOne = dataBuffer.ReadByte();
        bool readBit = dataBuffer.ReadBit();
        byte readByteTwo = dataBuffer.ReadByte();

        Assert.Equal(0b11111111, readByteOne);
        Assert.False(readBit);
        Assert.Equal(0b11111111, readByteTwo);
    }
    
    #endregion
    
    #region Test Methods (Multiple - ReadBit + ReadBytes)
    
    [Fact]
    public void Read_OneBitOneByteArray_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10000001, 0b10000000];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit = dataBuffer.ReadBit();
        byte[] readBytes = dataBuffer.ReadBytes(1);
        
        Assert.True(readBit);
        
        Assert.Single(readBytes);
        Assert.Equal(0b00000011, readBytes[0]);
    }
    
    [Fact]
    public void Read_OneBitTwoByteArray_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10000001, 0b10000011, 0b11111111];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit = dataBuffer.ReadBit();
        byte[] readBytes = dataBuffer.ReadBytes(2);
        
        Assert.True(readBit);
        
        Assert.Equal(2, readBytes.Length);
        Assert.Equal(0b00000011, readBytes[0]);
        Assert.Equal(0b00000111, readBytes[1]);
    }
    
    [Fact]
    public void Read_OneBitThreeByteArray_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10000001, 0b10000011, 0b11111111, 0b00000000];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit = dataBuffer.ReadBit();
        byte[] readBytes = dataBuffer.ReadBytes(3);
        
        Assert.True(readBit);
        
        Assert.Equal(3, readBytes.Length);
        Assert.Equal(0b00000011, readBytes[0]);
        Assert.Equal(0b00000111, readBytes[1]);
        Assert.Equal(0b11111110, readBytes[2]);
    }

    [Fact]
    public void Read_OneByteArrayOneBit_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10000001, 0b10000011, 0b11111111];
        DataBufferReader dataBuffer = new(byteArray);

        byte[] readBytes = dataBuffer.ReadBytes(2);
        bool readBit = dataBuffer.ReadBit();

        Assert.Equal(2, readBytes.Length);
        Assert.Equal(0b10000001, readBytes[0]);
        Assert.Equal(0b10000011, readBytes[1]);
        Assert.True(readBit);
    }

    [Fact]
    public void Read_TwoByteArrayOneBit_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10000001, 0b10000011, 0b11111111, 0b00000000];
        DataBufferReader dataBuffer = new(byteArray);

        byte[] readBytes = dataBuffer.ReadBytes(3);
        bool readBit = dataBuffer.ReadBit();

        Assert.Equal(3, readBytes.Length);
        Assert.Equal(0b10000001, readBytes[0]);
        Assert.Equal(0b10000011, readBytes[1]);
        Assert.Equal(0b11111111, readBytes[2]);
        Assert.False(readBit);
    }

    #endregion
    
    #region Test Methods (Multiple - ReadByte + ReadBytes)

    [Fact]
    public void Read_OneByteTwoByteArray_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10000001, 0b10000011, 0b11111111];
        DataBufferReader dataBuffer = new(byteArray);

        byte readByte = dataBuffer.ReadByte();
        byte[] readBytes = dataBuffer.ReadBytes(2);

        Assert.Equal(0b10000001, readByte);
        Assert.Equal(2, readBytes.Length);
        Assert.Equal(0b10000011, readBytes[0]);
        Assert.Equal(0b11111111, readBytes[1]);
    }

    [Fact]
    public void Read_TwoByteArrayOneByte_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10111101, 0b11001110, 0b10101100, 0b00000000];
        DataBufferReader dataBuffer = new(byteArray);

        byte[] readBytes = dataBuffer.ReadBytes(2);
        byte readByte = dataBuffer.ReadByte();

        Assert.Equal(new byte[] { 0b10111101, 0b11001110 }, readBytes);
        Assert.Equal(0xAC, readByte);
    }

    [Fact]
    public void Read_TwoBytesOneByteArray_ReturnsCorrectData()
    {
        byte[] byteArray = [0b10000001, 0b10000011, 0b11111111, 0b00000000];
        DataBufferReader dataBuffer = new(byteArray);

        byte readByteOne = dataBuffer.ReadByte();
        byte readByteTwo = dataBuffer.ReadByte();
        byte[] readBytes = dataBuffer.ReadBytes(2);

        Assert.Equal(0b10000001, readByteOne);
        Assert.Equal(0b10000011, readByteTwo);
        Assert.Equal(2, readBytes.Length);
        Assert.Equal(0b11111111, readBytes[0]);
        Assert.Equal(0b00000000, readBytes[1]);
    }

    #endregion
    
    #region Test Methods (Multiple - ReadBit + ReadByte + ReadBytes)

    [Fact]
    public void Read_OneBitOneByteTwoByteArrayOneBit_ReturnsCorrectData()
    {
        byte[] byteArray = [0b11111111, 0b11010110, 0b01011110, 0b10000000];
        DataBufferReader dataBuffer = new(byteArray);

        bool readBit1 = dataBuffer.ReadBit();
        byte readByte = dataBuffer.ReadByte();
        byte[] readBytes = dataBuffer.ReadBytes(2);
        bool readBit2 = dataBuffer.ReadBit();

        Assert.True(readBit1);
        Assert.Equal(0b11111111, readByte);
        Assert.Equal(new byte[] { 0b10101100, 0b10111101 }, readBytes);
        Assert.False(readBit2);
    }
   
    #endregion
    
    #region Test Methods (BytesRead)

    [Fact]
    public void BytesRead_NewDataBufferReader_ReturnsZero()
    {
        byte[] byteArray = new byte[2];
        DataBufferReader dataBuffer = new(byteArray);

        Assert.Equal(0, dataBuffer.BytesRead);
    } 
    
    [Fact]
    public void BytesRead_ReadBit_ReturnsOne()
    {
        byte[] byteArray = new byte[2];
        DataBufferReader dataBuffer = new(byteArray);
        
        dataBuffer.ReadBit();
        
        Assert.Equal(1, dataBuffer.BytesRead);
    } 
    
    [Fact]
    public void BytesRead_ReadByte_ReturnsOne()
    {
        byte[] byteArray = new byte[2];
        DataBufferReader dataBuffer = new(byteArray);
        
        dataBuffer.ReadByte();
        
        Assert.Equal(1, dataBuffer.BytesRead);
    } 
    
    [Fact]
    public void BytesRead_ReadByteReadBit_ReturnsTwo()
    {
        byte[] byteArray = new byte[2];
        DataBufferReader dataBuffer = new(byteArray);
        
        dataBuffer.ReadByte();
        dataBuffer.ReadBit();
        
        Assert.Equal(2, dataBuffer.BytesRead);
    } 
    
    [Fact]
    public void BytesRead_BookmarkReadBit_ReturnsZero()
    {
        byte[] byteArray = new byte[2];
        DataBufferReader dataBuffer = new(byteArray);
        
        using (dataBuffer.Bookmark())
        {
            dataBuffer.ReadBit();
        }
        
        Assert.Equal(0, dataBuffer.BytesRead);
    } 
    
    [Fact]
    public void BytesRead_BookmarkReadByte_ReturnsZero()
    {
        byte[] byteArray = new byte[2];
        DataBufferReader dataBuffer = new(byteArray);
        
        using (dataBuffer.Bookmark())
        {
            dataBuffer.ReadByte();
        }
        
        Assert.Equal(0, dataBuffer.BytesRead);
    } 

    #endregion
    
    #region Test Methods (Bookmark)
    
    [Fact]
    public void ReadByte_AfterBookmarkDisposal_ReturnsSameByte()
    {
        byte first;
        byte second;
        byte third;

        byte[] byteArray = [first = 250, second = 175, third = 100];
        DataBufferReader dataBuffer = new(byteArray);
 
        Assert.Equal(first, dataBuffer.ReadByte());
        
        using (dataBuffer.Bookmark())
        {
            Assert.Equal(second, dataBuffer.ReadByte());
        }

        Assert.Equal(second, dataBuffer.ReadByte());
        Assert.Equal(third, dataBuffer.ReadByte());
    }
    
    #endregion
}