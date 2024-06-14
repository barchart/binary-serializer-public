using System.Diagnostics;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Barchart.BinarySerializer.Tests
{
    public class SingleSerializationUnitTests
    {
        private readonly ITestOutputHelper _output;

        public SingleSerializationUnitTests(ITestOutputHelper output)
        {
            LoggerWrapper.InitializeLogger();
            _output = output;
        }

        [Fact]
        public void SerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                // Date is in UTC format and only milliseconds needs to be transmitted to the receiver side
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

                Schema<Person> personSchema = SchemaFactory.GetSchema<Person>();

                stopwatch.Start();

                byte[] serializedData = personSchema.Serialize(person);
                Person deserializedPerson = personSchema.Deserialize(serializedData);

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(person, deserializedPerson);
            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                throw;
            }
        }

        [Fact]
        public void ClassPropertiesSerializationTest()
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

            Car car = new()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Car",
                PersonObjectInCar = person
            };

            Garage garage = new()
            {
                CarObject = null,
                PersonObject = person,
                StringNameGarage = "Garage",
                DecimalNumberGarage = 12.5m,
                DateTimeDateGarage = roundedDateTime,
                doubleNumberGarage = 10.5d
            };

            Schema<Garage> garageSchema = SchemaFactory.GetSchema<Garage>();

            stopwatch.Start();

            byte[] serializedData = garageSchema.Serialize(garage);
            Garage deserializedGarage = garageSchema.Deserialize(serializedData);

            stopwatch.Stop();
            _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

            Assert.Equal(garage, deserializedGarage);
        }

        [Fact]
        public void EnumAndListSerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                ListSpecs ListSpecs = new()
                {
                    enumField = SerializationOptions.Option1,
                    enumField2 = SerializationOptions.Option3
                };

                Schema<ListSpecs> ListSpecsSchema = SchemaFactory.GetSchema<ListSpecs>();
                stopwatch.Start();

                byte[] serializedData = ListSpecsSchema.Serialize(ListSpecs);
                ListSpecs deserializedListSpecs = ListSpecsSchema.Deserialize(serializedData);

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(ListSpecs.enumField, deserializedListSpecs.enumField);
                Assert.Equal(ListSpecs.list, deserializedListSpecs.list);

            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                throw;
            }
        }

        [Fact]
        public void ListAndStringSerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                Hotel hotel = new()
                {
                    RoomNumbers = new List<int> { 101, 102, 103 },
                    Data = "104"
                };

                Schema<Hotel> hotelSchema = SchemaFactory.GetSchema<Hotel>();
                stopwatch.Start();

                byte[] serializedData = hotelSchema.Serialize(hotel);
                Hotel deserializedHotel = hotelSchema.Deserialize(serializedData);

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(hotel, deserializedHotel);
            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                throw;
            }
        }

      

        [Fact]
        public void ListOfObjectsTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                ListOfObjectsSpecs1 example = new ListOfObjectsSpecs1
                    {
                        property1 = true,
                        property2 = SerializationOptions.Option2,
                        property3 = 3.52f,
                        example = new List<ListOfObjectsSpecs2>
                        {
                            new ListOfObjectsSpecs2
                            {
                                property1 = true,
                                property3 = 2.11f
                            },
                            new ListOfObjectsSpecs2
                            {
                                property1 = false,
                                property3 = 5.11f
                            }
                        }
                    };

                Schema<ListOfObjectsSpecs1> exampleSchema = SchemaFactory.GetSchema<ListOfObjectsSpecs1>();
                stopwatch.Start();

                byte[] serializedData = exampleSchema.Serialize(example);
                ListOfObjectsSpecs1 deserializedExample = exampleSchema.Deserialize(serializedData);

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(example, deserializedExample);
            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                throw;
            }
        }
    }
}