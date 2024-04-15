using System.Diagnostics;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;
using Google.Protobuf;
using Org.Openfeed;
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
            _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

            Assert.Equal(garage, deserializedGarage);
        }

        [Fact]
        public void ListAndByteStringSerializationTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                Hotel hotel = new()
                {
                    RoomNumbers = new List<int> { 101, 102, 103 },
                    Data = ByteString.CopyFromUtf8("104")
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
        public void MarketSnapshotTest()
        {
            try
            {
                Stopwatch stopwatch = new();

                MarketSnapshot marketSnapshot = new()
                {
                    MarketId = 5389879102616877808,
                    TransactionTime = 1713192945516374835,
                    MarketSequence = 1754155,
                    TradeDate = 20240415,
                    TotalChunks = 1,
                    CurrentChunk = 1,
                    Symbol = "AAPL",
                    PriceDenominator = 0,
                    Service = Service.Delayed,
                    InstrumentStatus = new InstrumentStatus
                    {
                        TradingStatus = InstrumentTradingStatus.Open,
                        TradeDate = 20240415
                    },
                    Bbo = new BestBidOffer
                    {
                        BidPrice = 1749400,
                        BidQuantity = 2,
                        BidOriginator = ByteString.CopyFromUtf8("UA == "),
                        OfferPrice = 1749500,
                        OfferQuantity = 3,
                        OfferOriginator = ByteString.CopyFromUtf8("UQ=="),
                        QuoteCondition = ByteString.CopyFromUtf8("Ug==")
                    }
                };

                Schema<MarketSnapshot> marketSnapshotSchema = SchemaFactory.GetSchema<MarketSnapshot>();

                stopwatch.Start();

                byte[] serializedData = marketSnapshotSchema.Serialize(marketSnapshot);
                MarketSnapshot deserializedMarletSnapshot = marketSnapshotSchema.Deserialize(serializedData);

                stopwatch.Stop();
                _output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(marketSnapshot, deserializedMarletSnapshot);
            }
            catch (Exception ex)
            {
                LoggerWrapper.LogError(ex.Message);
                throw;
            }
        }
    }
}