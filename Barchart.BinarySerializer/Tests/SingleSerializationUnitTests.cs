﻿using System.Diagnostics;
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

        class TestClass
        {
            [BinarySerialize(key: false)]
            public TestEnum? enumField;

            [BinarySerialize(key: false)]
            public TestEnum? enumField2;

            [BinarySerialize(key: false)]
            public List<int> list = new() { 2 , 3 };
        }

        enum TestEnum
        {
            Option1,
            Option2,
            Option3
        }

        [Fact]
        public void EnumAndListSerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                TestClass testClass = new()
                {
                    enumField = TestEnum.Option1,
                    enumField2 = TestEnum.Option3
                };

                Schema<TestClass> testClassSchema = SchemaFactory.GetSchema<TestClass>();
                stopwatch.Start();

                byte[] serializedData = testClassSchema.Serialize(testClass);
                TestClass deserializedTestClass = testClassSchema.Deserialize(serializedData);

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(testClass.enumField, deserializedTestClass.enumField);
                Assert.Equal(testClass.list, deserializedTestClass.list);

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


        class ExampleClass1 {
            public bool property1;
            public TestEnum property2;
            public float? property3;
            public List<ExampleClass2>? example;
        }

        class ExampleClass2 {
            public bool property1;
            public float? property3;
        }

        [Fact]
        public void ListOfObjectsTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                ExampleClass1 example = new ExampleClass1
                    {
                        property1 = true,
                        property2 = TestEnum.Option2,
                        property3 = 3.52f,
                        example = new List<ExampleClass2>
                        {
                            new ExampleClass2
                            {
                                property1 = true,
                                property3 = 2.11f
                            },
                            new ExampleClass2
                            {
                                property1 = false,
                                property3 = 5.11f
                            }
                        }
                    };

                Schema<ExampleClass1> exampleSchema = SchemaFactory.GetSchema<ExampleClass1>();
                stopwatch.Start();

                byte[] serializedData = exampleSchema.Serialize(example);
                ExampleClass1 deserializedExample = exampleSchema.Deserialize(serializedData);

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