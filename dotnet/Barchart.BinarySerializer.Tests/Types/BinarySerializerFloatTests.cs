#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerFloatTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    private readonly BinarySerializerFloat _serializer;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerFloatTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerFloat();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Theory]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(float.Epsilon)]
    [InlineData((float)Math.PI)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public void Encode_Various_WritesExpectedBytes(float value)
    {
        Mock<IDataBufferWriter> mock = new();

        _serializer.Encode(mock.Object, value);
            
        byte[] expectedBytes = BitConverter.GetBytes(value);
        
        mock.Verify(m => m.WriteBytes(It.Is<byte[]>(b => b.SequenceEqual(expectedBytes))), Times.Once);
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(float.Epsilon)]
    [InlineData((float)Math.PI)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(float expected)
    {
        Mock<IDataBufferReader> mock = new();
        
        byte[] bytes = BitConverter.GetBytes(expected);
        
        mock.Setup(m => m.ReadBytes(4)).Returns(bytes);

        float actual = _serializer.Decode(mock.Object);
        
        Assert.Equal(expected, actual);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(new[] { 0f, 0f })]
    [InlineData(new[] { 1f, 1f } )]
    [InlineData(new[] { -1f, -1f } )]
    [InlineData(new[] { float.MaxValue, float.MaxValue })]
    [InlineData(new[] { float.MinValue, float.MinValue })]
    [InlineData(new[] { float.Epsilon, float.Epsilon })]
    [InlineData(new[] { (float)Math.PI, (float)Math.PI })]
    [InlineData(new[] { 1f, -1f })]
    [InlineData(new[] { 0.1f, 0.2f })]
    public void GetEquals_Various_MatchesIEquatableOutput(float[] floats)
    {
        bool actual = _serializer.GetEquals(floats[0], floats[1]);
        bool expected = floats[0].Equals(floats[1]);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}