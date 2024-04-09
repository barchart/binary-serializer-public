using Xunit;
using Xunit.Abstractions;

namespace Barchart.BinarySerializer.Tests
{
    public class MultiThreadSerializationUnitTests
	{
        private readonly ITestOutputHelper output;

        public MultiThreadSerializationUnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void MultiThreadSingleSerializationTest()
        {
            try
            {
                int numberOfThreads = Environment.ProcessorCount;
                SingleSerializationUnitTests testInstance = new(output);

                Parallel.For(0, numberOfThreads, index =>
                {
                    testInstance.SerializationTest();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Fact]
        public void MultiThreadClassPropertiesSingleSerializationTest()
        {
            try
            {
                int numberOfThreads = Environment.ProcessorCount;
                SingleSerializationUnitTests testInstance = new(output);

                Parallel.For(0, numberOfThreads, index =>
                {
                    testInstance.ClassPropertiesSerializationTest();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Fact]
        public void MultiThreadClassPropertiesDifferenceSerializationTest()
        {
            try
            {
                int numberOfThreads = Environment.ProcessorCount;
                DifferenceSerializationUnitTests testInstance = new(output);

                Parallel.For(0, numberOfThreads, index =>
                {
                    testInstance.ClassPropertiesDifferenceSerializationTest();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Fact]
        public void MultiThreadPropertyDifferenceSerializationTest()
        {
            try
            {
                int numberOfThreads = Environment.ProcessorCount;
                DifferenceSerializationUnitTests testInstance = new(output);

                Parallel.For(0, numberOfThreads, index =>
                {
                    testInstance.PropertyDifferenceSerializationTest();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}