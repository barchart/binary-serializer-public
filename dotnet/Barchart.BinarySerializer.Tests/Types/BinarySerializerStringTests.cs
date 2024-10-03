#region Using Statements

using System.Text;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerStringTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
           
    private readonly BinarySerializerString _serializer;

    #endregion

    #region Constructor(s)

    public BinarySerializerStringTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        _serializer = new BinarySerializerString();
    }

    #endregion

    #region Test Methods (Encode)

    [Fact]
    public void Encode_Null_WritesExpectedBytes()
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
        
        _serializer.Encode(mock.Object, null!);

        Assert.Single(bitsWritten);
        Assert.Empty(byteWritten);
        Assert.Empty(bytesWritten);
        
        Assert.True(bitsWritten[0]);
    }
    
    [Theory]
    [InlineData("Testing Encoding & Decoding methods")]
    [InlineData("")]
    [InlineData("Binary Serialization")]
    public void Encode_VariousStringsToUTF8_WritesExpectedBytes(string value)
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
        
        _serializer.Encode(mock.Object, value);

        Assert.Single(bitsWritten);
        Assert.Empty(byteWritten);
        Assert.Equal(2, bytesWritten.Count);
        
        Assert.False(bitsWritten[0]);
        
        byte[] expectedLengthBytes = BitConverter.GetBytes((ushort)value.Length);
        byte[] actualLengthBytes = bytesWritten[0];
        
        Assert.Equal(expectedLengthBytes, actualLengthBytes);
         
        byte[] expectedContentBytes = Encoding.UTF8.GetBytes(value);
        byte[] actualContentBytes = bytesWritten[1];
        
        Assert.Equal(expectedContentBytes, actualContentBytes);
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData("Testing Encoding & Decoding methods")]
    [InlineData("")]
    [InlineData("Binary Serialization")]
    public void Decode_UTF8EncodedStrings_ReturnsExpectedValue(string value)
    {
        Mock<IDataBufferReader> mock = new();
        
        byte[] serializedLengthBytes = BitConverter.GetBytes((ushort)value.Length);
        byte[] serializedContentBytes = Encoding.UTF8.GetBytes(value);
        
        mock.Setup(m => m.ReadBit()).Returns(false);
        
        mock.Setup(m => m.ReadBytes(2)).Returns(serializedLengthBytes);
        mock.Setup(m => m.ReadBytes(value.Length)).Returns(serializedContentBytes);

        string deserialized = _serializer.Decode(mock.Object);

        Assert.Equal(value, deserialized);
    }
    
    [Fact]
    public void Decode_Null_ReturnsExpectedValue()
    {
        Mock<IDataBufferReader> mock = new();
        
        mock.Setup(m => m.ReadBit()).Returns(true);

        string deserialized = _serializer.Decode(mock.Object);

        Assert.Null(deserialized);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Theory]
    [InlineData(new object[] { new[] { "Test", "Test" }})]
    [InlineData(new object[] { new[] { "String", "string" }})]
    [InlineData(new object[] { new[] { "", "" }})]
    [InlineData(new object[] { new[] { "Binary", "Serialization" }})]
    [InlineData(new object[] { new[] { "a", null }})]
    [InlineData(new object[] { new[] { null, "b" }})]
    [InlineData(new object[] { new[] { null, null, "hack" }})]
    public void GetEquals_Various_MatchesIEquatableOutput(string[] strings)
    {
        string a = strings[0];
        string b = strings[1];
        
        bool actual = _serializer.GetEquals(a, b);

        bool expected;
        
        if (a != null)
        {
            expected = a.Equals(b);
        } 
        else if (b != null)
        {
            expected = b.Equals(a);
        }
        else
        {
            expected = true;
        }
        
        Assert.Equal(expected, actual);
    }

    #endregion
}