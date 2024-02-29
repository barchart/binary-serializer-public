using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    public class PerformanceUnitTests
	{
        private readonly ITestOutputHelper output;

        public PerformanceUnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestDirect()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                carOld.DecimalNumber = 22.5m;
                var x = carOld.DecimalNumber;
            }
            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestReflection()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            Type carType = typeof(Car);
            PropertyInfo? decimalNumberProperty = carType.GetProperty("DecimalNumber");
            FieldInfo? doubleNumberProperty = carType.GetField("doubleNumber");

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                decimalNumberProperty?.SetValue(carOld, 22.5m);
                double? a = (double?)doubleNumberProperty?.GetValue(carOld);
            }
            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestLambda()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            var getter = (Car carold) => carold.DecimalNumber;
            var setter = (Car carold, decimal value) => carold.DecimalNumber = value;

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                getter(carOld);
                setter(carOld, 22.5m);
            }
            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestDelegate()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Luka",
            };

            var carType = carOld.GetType();
            var getMethod = carType.GetProperty("DecimalNumber")!.GetGetMethod()!.CreateDelegate<Func<Car, decimal>>()!;
            var setMethod = carType.GetProperty("DecimalNumber")!.GetSetMethod()!.CreateDelegate<Action<Car, decimal>>()!;

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                setMethod(carOld, 22.5m);
                var x = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }

        [Fact]
        public void TestDelegateGetterSetter()
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            long roundedTicks = (ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond;
            DateTime roundedDateTime = new DateTime(roundedTicks, DateTimeKind.Utc);
            stopwatch.Start();

            Car carOld = new Car()
            {
                DecimalNumber = 1.5m,
                doubleNumber = 2.5d,
                DateTimeDate = roundedDateTime,
                StringName = "Luka"
            };

            var carType = carOld.GetType();
            FieldInfo? fieldInfo = carType.GetField("doubleNumber");
            PropertyInfo? propertyInfo = carType.GetProperty("DecimalNumber");

            if (fieldInfo == null || propertyInfo == null)
            {
                Assert.Fail();
                return;
            }

            Func<Car, object?> getMethod = SchemaFactory.GenerateGetter<Car>(fieldInfo);
            Action<Car, object?> setMethod = SchemaFactory.GenerateSetter<Car>(propertyInfo);

            for (long i = 0; i < int.MaxValue / 2; i++)
            {
                setMethod(carOld, 22.5m);
                var x = getMethod(carOld);
            }

            stopwatch.Stop();
            output.WriteLine($"Time elapsed: {stopwatch.ElapsedTicks} ticks");
        }
    }
}

