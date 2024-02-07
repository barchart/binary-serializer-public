using Xunit;
using JerqAggregatorNew.Schemas;
using System.Xml.Linq;
using System.Diagnostics;
using Xunit.Abstractions;

namespace JerqAggregatorNew.Tests
{
    class Person
    {
         [BinarySerialize(include: true, key: false)]
         public int? IntNumber { get; set; }

         [BinarySerialize(include: true, key: true)]
         public bool? BoolNumber { get; set; }
        
         [BinarySerialize(include: true, key: false)]
         public double doubleNumber;

         [BinarySerialize(include: true, key: false)]
         public decimal DecimalNumber { get; set; }

         [BinarySerialize(include: true, key: false)]
         public string? StringName { get; set; }
 
         [BinarySerialize(include: true, key: false)]
         public DateTime? DateTimeDate { get; set; }
    }
    public class UnitTests
    {
        private readonly ITestOutputHelper output;

        public UnitTests(ITestOutputHelper output)
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
                    DecimalNumber = (decimal)1.5,
                    doubleNumber = (double)2.5,
                    BoolNumber = true,
                    DateTimeDate = roundedDateTime,
                    StringName = "Luka"
                };

                Schema<Person> personSchema = SchemaFactory.GetSchema<Person>();
                
                stopwatch.Start();

                byte[] serializedData = personSchema.Serialize(person);

                Person deserializedPerson = personSchema.Deserialize(serializedData);

                stopwatch.Stop();
                output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");

                Assert.Equal(person.IntNumber, deserializedPerson.IntNumber);
                Assert.Equal(person.BoolNumber, deserializedPerson.BoolNumber);
                Assert.Equal(person.DecimalNumber, deserializedPerson.DecimalNumber);
                Assert.Equal(person.doubleNumber, deserializedPerson.doubleNumber);
                Assert.Equal(person.StringName, deserializedPerson.StringName);
                Assert.Equal(person.DateTimeDate, deserializedPerson.DateTimeDate);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            } 
        }
    }
}
