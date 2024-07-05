#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerNullableTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerNullableTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    #endregion

    #region Test Methods (Encode)

    [Theory]
    [MemberData(nameof(GetEncodeTestData))]
    public void Encode_Value_WritesExpectedBitsAndBytes<T>(IBinaryTypeSerializer<T> innerSerializer, T? value) where T : struct
    {
        BinarySerializerNullable<T> serializer = new(innerSerializer);
        var mock = new Mock<IDataBufferWriter>();
        var bitsWritten = new List<bool>();
        var bytesWritten = new List<byte>();

        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(bytesWritten)));
        mock.Setup(m => m.WriteBytes(It.IsAny<byte[]>())).Callback<byte[]>(b => bytesWritten.AddRange(b));

        serializer.Encode(mock.Object, value);

        if (value.HasValue)
        {
            Assert.Single(bitsWritten);
            Assert.False(bitsWritten[0]);

            var expectedBytes = new List<byte>();
            var innerMock = new Mock<IDataBufferWriter>();
            innerMock.Setup(m => m.WriteByte(Capture.In(expectedBytes)));
            innerMock.Setup(m => m.WriteBytes(It.IsAny<byte[]>())).Callback<byte[]>(b => expectedBytes.AddRange(b));

            innerSerializer.Encode(innerMock.Object, value.Value);

            Assert.Equal(expectedBytes, bytesWritten);
        }
        else
        {
            Assert.Single(bitsWritten);
            Assert.True(bitsWritten[0]);
            Assert.Empty(bytesWritten);
        }
    }


    #endregion

    #region Nested Types
        
    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
        
    #endregion

    #region Test Data

    public static IEnumerable<object[]> GetEncodeTestData()
    {
        yield return new object[] { new BinarySerializerInt(), null! };
        yield return new object[] { new BinarySerializerInt(), (int?)42 };
        yield return new object[] { new BinarySerializerFloat(), null! };
        yield return new object[] { new BinarySerializerFloat(), (float?)3.14f };
        yield return new object[] { new BinarySerializerDouble(), null! };
        yield return new object[] { new BinarySerializerDouble(), (double?)2.71828 };
    }

    #endregion
}