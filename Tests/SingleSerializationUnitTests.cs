﻿using System.Diagnostics;
using Barchart.BinarySerializer.Schemas;
using Xunit;
using Xunit.Abstractions;

namespace Barchart.BinarySerializer.Tests
{
    public class SingleSerializationUnitTests
    {
        private readonly ITestOutputHelper output;

        public SingleSerializationUnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();

                // Date is in UTC format and only milliseconds needs to be transmitted to the receiver side
                DateTime now = DateTime.UtcNow;
                long ticks = now.Ticks;
                long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
                DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);

                Person person = new Person()
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
                output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(person, deserializedPerson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Fact]
        public void TestClassProperties()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Person person = new Person()
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

            Car car = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Car",
                PersonObjectInCar = person
            };

            Garage garage = new Garage()
            {
                CarObject = car,
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
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

            Assert.Equal(garage, deserializedGarage);
        }
    }
}