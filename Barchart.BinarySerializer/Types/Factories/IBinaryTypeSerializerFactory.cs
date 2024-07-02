namespace Barchart.BinarySerializer.Types.Factories;

public interface IBinaryTypeSerializerFactory
{
    IBinaryTypeSerializer<T> Make<T>();
} 