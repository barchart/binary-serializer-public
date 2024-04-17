using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;
using Google.Protobuf;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Barchart.BinarySerializer.Tests
{
    public class DifferenceSerializationUnitTests
	{
        private readonly ITestOutputHelper _output;

        public DifferenceSerializationUnitTests(ITestOutputHelper output)
        {
            LoggerWrapper.InitializeLogger();
            _output = output;
        }

        [Fact]
        public void PropertyDifferenceSerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new();
                DateTime now = DateTime.UtcNow;
                long ticks = now.Ticks;
                long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
                DateTime roundedDateTime = new(roundedTicks, DateTimeKind.Utc);

                Car carOld = new()
                {
                    DecimalNumber = 1.5m,
                    doubleNumber = 2.5,
                    DateTimeDate = roundedDateTime,
                    StringName = "Luka",
                    Byte = 1,
                    SByte = 2
                };

                Car carNew = new Car()
                {
                    DecimalNumber = 1.5m,
                    doubleNumber = 2.5,
                    DateTimeDate = roundedDateTime,
                    StringName = "Luka2",
                    Byte = 2,
                    SByte = 2
                };

                Schema<Car> carSchema = SchemaFactory.GetSchema<Car>();

                stopwatch.Start();

                for (int i = 0; i < 1000; i++)
                {
                    byte[] serializedDataDifference = carSchema.Serialize(carOld, carNew);
                    Car deserializedCarDifference = carSchema.Deserialize(serializedDataDifference, carOld);
                    Assert.Equal(carNew, deserializedCarDifference);
                }

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                throw;
            }
        }

        [Fact]
        public void ClassPropertiesDifferenceSerializationTest()
        {
            Stopwatch stopwatch = new();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new(roundedTicks, DateTimeKind.Utc);

            Person person = new()
            {
                IntNumber = 1,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

                IntNumber2 = 1,
                DecimalNumber2 = 1.5m,
                doubleNumber2 = 2.5,
                BoolNumber2 = true,
                DateTimeDate2 = roundedDateTime,
                StringName2 = "Luka",

                IntNumber3 = 1,
                DecimalNumber3 = 1.5m,
                doubleNumber3 = 2.5,
                BoolNumber3 = true,
                DateTimeDate3 = roundedDateTime,
                StringName3 = "Luka",

                IntNumber4 = 1,
                DecimalNumber4 = 1.5m,
                doubleNumber4 = 2.5,
                BoolNumber4 = true,
                DateTimeDate4 = roundedDateTime,
                StringName4 = "Luka",

                IntNumber5 = 1,
                DecimalNumber5 = 1.5m,
                doubleNumber5 = 2.5,
                BoolNumber5 = true,
                DateTimeDate5 = roundedDateTime,
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 14)
            };

            Person person2 = new()
            {
                IntNumber = 4,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

                IntNumber2 = 1,
                DecimalNumber2 = 1.5m,
                doubleNumber2 = 2.5,
                BoolNumber2 = true,
                DateTimeDate2 = roundedDateTime,
                StringName2 = "Luka",

                IntNumber3 = 1,
                DecimalNumber3 = 1.5m,
                doubleNumber3 = 2.5,
                BoolNumber3 = true,
                DateTimeDate3 = roundedDateTime,
                StringName3 = "Luka",

                IntNumber4 = 1,
                DecimalNumber4 = 1.5m,
                doubleNumber4 = 2.5,
                BoolNumber4 = true,
                DateTimeDate4 = roundedDateTime,
                StringName4 = "Luka22",

                IntNumber5 = 1,
                DecimalNumber5 = 1.5m,
                doubleNumber5 = 10.5,
                BoolNumber5 = false,
                DateTimeDate5 = roundedDateTime,
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 15)
            };

            Person person3 = new()
            {
                IntNumber = 1,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

                IntNumber2 = 1,
                DecimalNumber2 = 1.5m,
                doubleNumber2 = 2.5,
                BoolNumber2 = true,
                DateTimeDate2 = roundedDateTime,
                StringName2 = "Luka",

                IntNumber3 = 1,
                DecimalNumber3 = 1.5m,
                doubleNumber3 = 2.5,
                BoolNumber3 = true,
                DateTimeDate3 = roundedDateTime,
                StringName3 = "Luka",

                IntNumber4 = 1,
                DecimalNumber4 = 1.5m,
                doubleNumber4 = 2.5,
                BoolNumber4 = true,
                DateTimeDate4 = roundedDateTime,
                StringName4 = "Luka",

                IntNumber5 = 1,
                DecimalNumber5 = 1.5m,
                doubleNumber5 = 2.5,
                BoolNumber5 = true,
                DateTimeDate5 = roundedDateTime,
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 14)
            };

            Person person4 = new()
            {
                IntNumber = 1,
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                BoolNumber = true,
                DateTimeDate = roundedDateTime,
                StringName = null,

                IntNumber2 = 1,
                DecimalNumber2 = 1.5m,
                doubleNumber2 = 2.5,
                BoolNumber2 = true,
                DateTimeDate2 = roundedDateTime,
                StringName2 = "Luka",

                IntNumber3 = 1,
                DecimalNumber3 = 1.5m,
                doubleNumber3 = 2.5,
                BoolNumber3 = true,
                DateTimeDate3 = roundedDateTime,
                StringName3 = "Luka",

                IntNumber4 = 1,
                DecimalNumber4 = 1.5m,
                doubleNumber4 = 2.5,
                BoolNumber4 = true,
                DateTimeDate4 = roundedDateTime,
                StringName4 = "Luka",

                IntNumber5 = 1,
                DecimalNumber5 = 1.5m,
                doubleNumber5 = 2.5,
                BoolNumber5 = true,
                DateTimeDate5 = roundedDateTime,
                StringName5 = "Luka",

                DateOnly = new DateOnly(2022, 2, 14)
            };

            Car car = new()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Car",
                PersonObjectInCar = person3
            };

            Car car2 = new()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Car",
                PersonObjectInCar = person4
            };

            Garage garage = new()
            {
                CarObject = car,
                PersonObject = person,
                StringNameGarage = "Garage",
                DecimalNumberGarage = 12.5m,
                DateTimeDateGarage = roundedDateTime,
                doubleNumberGarage = 10.5d
            };

            Garage garage2 = new()
            {
                CarObject = car2,
                PersonObject = person2,
                StringNameGarage = "Garage",
                DecimalNumberGarage = 122.5m,
                DateTimeDateGarage = roundedDateTime,
                doubleNumberGarage = 10.5d,
            };

            Schema<Garage> garageSchema = SchemaFactory.GetSchema<Garage>();

            stopwatch.Start();

            byte[] serializedDataDifference = garageSchema.Serialize(garage, garage2);
            Garage deserializedGarageDifference = garageSchema.Deserialize(serializedDataDifference, garage);

            stopwatch.Stop();
            _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

            Assert.Equal(garage2, deserializedGarageDifference);
        }

        [Fact]
        public void ListAndByteStringDifferenceSerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                Hotel hotel1 = new()
                {
                    RoomNumbers = new List<int> { 101, 105, 103 },
                    Data = ByteString.CopyFromUtf8("104")
                };

                Hotel hotel2 = new()
                {
                    RoomNumbers = new List<int> { 101, 102, 103 },
                    Data = ByteString.CopyFromUtf8("105")
                };

                Schema<Hotel> hotelSchema = SchemaFactory.GetSchema<Hotel>();

                stopwatch.Start();

                byte[] serializedData = hotelSchema.Serialize(hotel1, hotel2);
                Hotel deserializedHotel = hotelSchema.Deserialize(serializedData, hotel1);

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(hotel2, deserializedHotel);
            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                throw;
            }
        }
    }
}