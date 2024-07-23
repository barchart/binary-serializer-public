#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

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
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(double.NaN)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(double expected)
    {
        Mock<IDataBufferReader> mock = new();
        
        byte[] bytes = BitConverter.GetBytes(expected);
        
        mock.Setup(m => m.ReadBytes(8)).Returns(bytes);

        double actual = _serializer.Decode(mock.Object);
        
        Assert.Equal(expected, actual);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(new[] { 0d, 0 })]
    [InlineData(new[] { 1d, 1 })]
    [InlineData(new[] { -1d, -1 })]
    [InlineData(new[] { double.MaxValue, double.MaxValue })]
    [InlineData(new[] { double.MinValue, double.MinValue })]
    [InlineData(new[] { double.Epsilon, double.Epsilon })]
    [InlineData(new[] { Math.PI, Math.PI })]
    [InlineData(new[] { 1d, -1 })]
    [InlineData(new[] { 0.1, 0.2 })]
    public void GetEquals_Various_MatchesIEquatableOutput(double[] doubles)
    {
        bool actual = _serializer.GetEquals(doubles[0], doubles[1]);
        bool expected = doubles[0].Equals(doubles[1]);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}