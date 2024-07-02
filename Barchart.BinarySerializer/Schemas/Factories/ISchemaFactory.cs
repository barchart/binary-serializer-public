namespace Barchart.BinarySerializer.Schemas;

public interface ISchemaFactory
{
    ISchema<TEntity> Make<TEntity>() where TEntity: new();
}