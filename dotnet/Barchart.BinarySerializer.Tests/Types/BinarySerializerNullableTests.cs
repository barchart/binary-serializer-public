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
        Mock<IDataBufferWriter> mock = new();
        List<bool> bitsWritten = [];
        List<byte> bytesWritten = [];

        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(bytesWritten)));
        mock.Setup(m => m.WriteBytes(It.IsAny<byte[]>())).Callback<byte[]>(b => bytesWritten.AddRange(b));

        serializer.Encode(mock.Object, value);

        if (value.HasValue)
        {
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

    #region Test Methods (Decode)

    [Theory]
    [MemberData(nameof(GetDecodeTestData))]
    public void Decode_Value_ReadsExpectedBitsAndBytes<T>(IBinaryTypeSerializer<T> innerSerializer, T? expectedValue) where T : struct
    {
        BinarySerializerNullable<T> serializer = new(innerSerializer);
        Mock<IDataBufferReader> mock = new();
        
        List<bool> bitsToRead = [];
        byte byteToRead = new();
        List<byte> bytesToRead = [];

        if (expectedValue.HasValue)
        {
            bitsToRead.Add(false);

            List<byte> buffer = [];
            Mock<IDataBufferWriter> bufferWriterMock = new();

            bufferWriterMock.Setup(m => m.WriteBit(It.IsAny<bool>())).Callback<bool>(b => bitsToRead.Add(b));
            bufferWriterMock.Setup(m => m.WriteByte(It.IsAny<byte>())).Callback<byte>(b => byteToRead = b);
            bufferWriterMock.Setup(m => m.WriteBytes(It.IsAny<byte[]>())).Callback<byte[]>(b => buffer.AddRange(b));

            innerSerializer.Encode(bufferWriterMock.Object, expectedValue.Value);
            bytesToRead.AddRange(buffer);
        }
        else
        {
            bitsToRead.Add(true);
        }

        mock.Setup(m => m.ReadBit()).Returns(() =>
        {
            bool bit = bitsToRead.Count > 0 && bitsToRead[0];
            bitsToRead.RemoveAt(0);
            return bit;
        });

        mock.Setup(m => m.ReadByte()).Returns(() => byteToRead);

        mock.Setup(m => m.ReadBytes(It.IsAny<int>())).Returns<int>(count => 
        {
            byte[] result = bytesToRead.Take(count).ToArray();
            bytesToRead.RemoveRange(0, Math.Min(count, bytesToRead.Count));
            return result;
        });

        T? decodedValue = serializer.Decode(mock.Object);

        Assert.Equal(expectedValue, decodedValue);
    }
    
    #endregion

    #region Test Methods (GetEquals)

    [Theory]
    [MemberData(nameof(GetEqualsTestData))]
    public void Equals_GivenValues_ExpectCorrectResult<T>(IBinaryTypeSerializer<T> innerSerializer, T? value1, T? value2) where T : struct
    {
        BinarySerializerNullable<T> serializer = new(innerSerializer);
        
        bool expected = value1.Equals(value2);
        bool actual = serializer.GetEquals(value1, value2);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion

    #region Nested Types

    private enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
        
    #endregion

    #region Test Data

    public static IEnumerable<object[]> GetEncodeTestData()
    {
        yield return [new BinarySerializerBool(), null!];
        yield return [new BinarySerializerBool(), (bool?)true];
        
        yield return [new BinarySerializerByte(), null!];
        yield return [new BinarySerializerByte(), (byte?)255];
        
        yield return [new BinarySerializerChar(), null!];
        yield return [new BinarySerializerChar(), (char?)'A'];
        
        yield return [new BinarySerializerDateOnly(), null!];
        yield return [new BinarySerializerDateOnly(), (DateOnly?) new DateOnly(2000, 2, 29)];
        
        yield return [new BinarySerializerDateTime(), null!];
        yield return [new BinarySerializerDateTime(), (DateTime?) new DateTime(2023, 1, 1, 12, 0, 0)];
        
        yield return [new BinarySerializerDecimal(), null!];
        yield return [new BinarySerializerDecimal(), (decimal?)123.45m];
        
        yield return [new BinarySerializerDouble(), null!];
        yield return [new BinarySerializerDouble(), (double?)2.71828];
        
        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), null!];
        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), (TestEnum?)TestEnum.Value2];
        
        yield return [new BinarySerializerFloat(), null!];
        yield return [new BinarySerializerFloat(), (float?)3.14f];
        
        yield return [new BinarySerializerInt(), null!];
        yield return [new BinarySerializerInt(), (int?)42];
    
        yield return [new BinarySerializerLong(), null!];
        yield return [new BinarySerializerLong(), (long?)123456789L];
       
        yield return [new BinarySerializerSByte(), null!];
        yield return [new BinarySerializerSByte(), (sbyte?)-128];
        
        yield return [new BinarySerializerShort(), null!];
        yield return [new BinarySerializerShort(), (short?)12345];
        
        yield return [new BinarySerializerUInt(), null!];
        yield return [new BinarySerializerUInt(), (uint?)uint.MaxValue];

        yield return [new BinarySerializerULong(), null!];
        yield return [new BinarySerializerULong(), (ulong?)ulong.MaxValue];
      
        yield return [new BinarySerializerUShort(), null!];
        yield return [new BinarySerializerUShort(), (ushort?)ushort.MaxValue];
    }

    public static IEnumerable<object[]> GetDecodeTestData()
    {
        yield return [new BinarySerializerBool(), null!];
        yield return [new BinarySerializerBool(), (bool?)true];
        
        yield return [new BinarySerializerByte(), null!];
        yield return [new BinarySerializerByte(), (byte?)255];
        
        yield return [new BinarySerializerChar(), null!];
        yield return [new BinarySerializerChar(), (char?)'A'];
        
        yield return [new BinarySerializerDateOnly(), null!];
        yield return [new BinarySerializerDateOnly(), (DateOnly?) new DateOnly(2000, 2, 29)];
        
        yield return [new BinarySerializerDateTime(), null!];
        yield return [new BinarySerializerDateTime(), (DateTime?) new DateTime(2023, 1, 1, 12, 0, 0)];
        
        yield return [new BinarySerializerDecimal(), null!];
        yield return [new BinarySerializerDecimal(), (decimal?)123.45m];
        
        yield return [new BinarySerializerDouble(), null!];
        yield return [new BinarySerializerDouble(), (double?)2.71828];
        
        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), null!];
        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), (TestEnum?)TestEnum.Value2];
        
        yield return [new BinarySerializerFloat(), null!];
        yield return [new BinarySerializerFloat(), (float?)3.14f];
        
        yield return [new BinarySerializerInt(), null!];
        yield return [new BinarySerializerInt(), (int?)42];
    
        yield return [new BinarySerializerLong(), null!];
        yield return [new BinarySerializerLong(), (long?)123456789L];
       
        yield return [new BinarySerializerSByte(), null!];
        yield return [new BinarySerializerSByte(), (sbyte?)-128];
        
        yield return [new BinarySerializerShort(), null!];
        yield return [new BinarySerializerShort(), (short?)12345];
        
        yield return [new BinarySerializerUInt(), null!];
        yield return [new BinarySerializerUInt(), (uint?)uint.MaxValue];

        yield return [new BinarySerializerULong(), null!];
        yield return [new BinarySerializerULong(), (ulong?)ulong.MaxValue];
      
        yield return [new BinarySerializerUShort(), null!];
        yield return [new BinarySerializerUShort(), (ushort?)ushort.MaxValue];
    }

    public static IEnumerable<object[]> GetEqualsTestData()
    {
        yield return [new BinarySerializerBool(), (bool?)true, (bool?)true];
        yield return [new BinarySerializerBool(), null!, null!];
        yield return [new BinarySerializerBool(), (bool?)true, null!];
        yield return [new BinarySerializerBool(), null!, (bool?)true];

        yield return [new BinarySerializerByte(), (byte?)255, (byte?)255];
        yield return [new BinarySerializerByte(), null!, null!];
        yield return [new BinarySerializerByte(), (byte?)255, null!];
        yield return [new BinarySerializerByte(), null!, (byte?)255];

        yield return [new BinarySerializerChar(), (char?)'A', (char?)'A'];
        yield return [new BinarySerializerChar(), null!, null!];
        yield return [new BinarySerializerChar(), (char?)'A', null!];
        yield return [new BinarySerializerChar(), null!, (char?)'A'];

        yield return [new BinarySerializerDateOnly(), (DateOnly?)DateOnly.FromDateTime(DateTime.Now), (DateOnly?)DateOnly.FromDateTime(DateTime.Now)
        ];
        yield return [new BinarySerializerDateOnly(), null!, null!];
        yield return [new BinarySerializerDateOnly(), (DateOnly?)DateOnly.FromDateTime(DateTime.Now), null!];
        yield return [new BinarySerializerDateOnly(), null!, (DateOnly?)DateOnly.FromDateTime(DateTime.Now)];

        yield return [new BinarySerializerDateTime(), (DateTime?)DateTime.Now, (DateTime?)DateTime.Now];
        yield return [new BinarySerializerDateTime(), null!, null!];
        yield return [new BinarySerializerDateTime(), (DateTime?)DateTime.Now, null!];
        yield return [new BinarySerializerDateTime(), null!, (DateTime?)new DateTime(2023, 1, 1, 12, 0, 0)];

        yield return [new BinarySerializerDecimal(), (decimal?)123.45m, (decimal?)123.45m];
        yield return [new BinarySerializerDecimal(), null!, null!];
        yield return [new BinarySerializerDecimal(), (decimal?)123.45m, null!];
        yield return [new BinarySerializerDecimal(), null!, (decimal?)123.45m];

        yield return [new BinarySerializerDouble(), (double?)2.71828, (double?)2.71828];
        yield return [new BinarySerializerDouble(), null!, null!];
        yield return [new BinarySerializerDouble(), (double?)2.71828, null!];
        yield return [new BinarySerializerDouble(), null!, (double?)2.71828];

        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), (TestEnum?)TestEnum.Value2, (TestEnum?)TestEnum.Value2
        ];
        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), null!, null!];
        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), (TestEnum?)TestEnum.Value2, null!];
        yield return [new BinarySerializerEnum<TestEnum>(new BinarySerializerInt()), null!, (TestEnum?)TestEnum.Value2];

        yield return [new BinarySerializerFloat(), (float?)3.14f, (float?)3.14f];

        yield return [new BinarySerializerLong(), (long?)123456789L, (long?)123456789L];
        yield return [new BinarySerializerLong(), null!, null!];
        yield return [new BinarySerializerLong(), (long?)123456789L, null!];
        yield return [new BinarySerializerLong(), null!, (long?)123456789L];

        yield return [new BinarySerializerSByte(), (sbyte?)-128, (sbyte?)-128];
        yield return [new BinarySerializerSByte(), null!, null!];
        yield return [new BinarySerializerSByte(), (sbyte?)-128, null!];
        yield return [new BinarySerializerSByte(), null!, (sbyte?)-128];

        yield return [new BinarySerializerShort(), (short?)12345, (short?)12345];
        yield return [new BinarySerializerShort(), null!, null!];
        yield return [new BinarySerializerShort(), (short?)12345, null!];
        yield return [new BinarySerializerShort(), null!, (short?)12345];

        yield return [new BinarySerializerUInt(), (uint?)uint.MaxValue, (uint?)uint.MaxValue];
        yield return [new BinarySerializerUInt(), null!, null!];
        yield return [new BinarySerializerUInt(), (uint?)uint.MaxValue, null!];
        yield return [new BinarySerializerUInt(), null!, (uint?)uint.MaxValue];

        yield return [new BinarySerializerULong(), (ulong?)ulong.MaxValue, (ulong?)ulong.MaxValue];
        yield return [new BinarySerializerULong(), null!, null!];
        yield return [new BinarySerializerULong(), (ulong?)ulong.MaxValue, null!];
        yield return [new BinarySerializerULong(), null!, (ulong?)ulong.MaxValue];

        yield return [new BinarySerializerUShort(), (ushort?)ushort.MaxValue, (ushort?)ushort.MaxValue];
        yield return [new BinarySerializerUShort(), null!, null!];
        yield return [new BinarySerializerUShort(), (ushort?)ushort.MaxValue, null!];
        yield return [new BinarySerializerUShort(), null!, (ushort?)ushort.MaxValue];
    }

    #endregion
}