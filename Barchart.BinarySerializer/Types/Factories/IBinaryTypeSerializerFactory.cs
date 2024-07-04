#region Using Statements

namespace Barchart.BinarySerializer.Types.Factories;

#endregion

public interface IBinaryTypeSerializerFactory
{
    IBinaryTypeSerializer<T> Make<T>();
} 