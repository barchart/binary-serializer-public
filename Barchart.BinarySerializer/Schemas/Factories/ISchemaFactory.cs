namespace Barchart.BinarySerializer.Schemas.Factories;

public interface ISchemaFactory
{
    ISchema<TEntity> Make<TEntity>() where TEntity: new();
}