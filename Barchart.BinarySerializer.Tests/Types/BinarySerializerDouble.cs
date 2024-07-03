#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;
using System;
using Xunit;
using Moq;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerDoubleTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    private readonly BinarySerializerDouble _serializer;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerDoubleTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerDouble();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    [InlineData(double.Epsilon)]
    [InlineData(Math.PI)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(double.NaN)]
    public void Encode_Various_WritesExpectedBytes(double value)
    {
        Mock<IDataBufferWriter> mock = new();

        _serializer.Encode(mock.Object, value);
            
        byte[] expectedBytes = BitConverter.GetBytes(value);
        
        mock.Verify(m => m.WriteBytes(It.Is<byte[]>(b => b.SequenceEqual(expectedBytes))), Times.Once);
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    [InlineData(double.Epsilon)]
    [InlineData(Math.PI)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(double expected)
    {
        Mock<IDataBufferReader> mock = new();
        
        byte[] bytes = BitConverter.GetBytes(expected);
        
        mock.Setup(m => m.ReadBytes(8)).Returns(bytes);

        var actual = _serializer.Decode(mock.Object);
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Decode_PositiveInfinity_ReturnsPositiveInfinity()
    {
        Mock<IDataBufferReader> mock = new();
        mock.Setup(m => m.ReadBytes(8)).Returns(BitConverter.GetBytes(double.PositiveInfinity));
        var result = _serializer.Decode(mock.Object);
        Assert.Equal(double.PositiveInfinity, result);
    }

    [Fact]
    public void Decode_NegativeInfinity_ReturnsNegativeInfinity()
    {
        Mock<IDataBufferReader> mock = new();
        mock.Setup(m => m.ReadBytes(8)).Returns(BitConverter.GetBytes(double.NegativeInfinity));
        var result = _serializer.Decode(mock.Object);
        Assert.Equal(double.NegativeInfinity, result);
    }

    [Fact]
    public void Decode_NaN_ReturnsNaN()
    {
        Mock<IDataBufferReader> mock = new();
        mock.Setup(m => m.ReadBytes(8)).Returns(BitConverter.GetBytes(double.NaN));
        var result = _serializer.Decode(mock.Object);
        Assert.True(double.IsNaN(result));
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(-1, -1)]
    [InlineData(double.MaxValue, double.MaxValue)]
    [InlineData(double.MinValue, double.MinValue)]
    [InlineData(double.Epsilon, double.Epsilon)]
    [InlineData(Math.PI, Math.PI)]
    [InlineData(1, -1)]
    [InlineData(0.1, 0.2)]
    public void GetEquals_Various_MatchesIEquatableOutput(double a, double b)
    {
        var actual = _serializer.GetEquals(a, b);
        var expected = a.Equals(b);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}