#region Using Statements

using System.Linq.Expressions;
using System.Reflection;

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Entities.Keys;
using Barchart.BinarySerializer.Serializers;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Entities.Factories;

/// <summary>
///     A factory class responsible for creating instances of <see cref="EntityManager{TEntity}"/>.
/// </summary>
public class EntityManagerFactory
{
    #region Constructor(s)

    public EntityManagerFactory()
    {
        
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Creates an instance of <see cref="EntityManager{TEntity}"/> for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of the entity.
    /// </typeparam>
    /// <param name="serializer">
    ///     The serializer used for serializing and deserializing entities.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="EntityManager{TEntity}"/> configured for the specified entity type.
    /// </returns>
    public EntityManager<TEntity> Make<TEntity>(Serializer<TEntity> serializer) where TEntity : class, new()
    {
        return new EntityManager<TEntity>(serializer, MakeKeyExtractor<TEntity>());
    }

    /// <summary>
    ///     Generates a function that extracts the key of an entity based on its members marked with the <see cref="SerializeAttribute"/>.
    /// </summary>
    /// <typeparam name="TEntity">
    ///    The type of the entity.
    /// </typeparam>
    /// <returns>
    ///     A function that takes an entity of type <typeparamref name="TEntity"/> and returns an instance of <see cref="IEntityKey{TEntity}"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the entity type does not have any properties or fields marked as keys with the <see cref="SerializeAttribute"/>.
    /// </exception>
    public Func<TEntity, IEntityKey<TEntity>> MakeKeyExtractor<TEntity>() where TEntity : class, new()
    {
        Type entityType = typeof(TEntity);

        MemberInfo[] keyMembers = Reflection.GetMembersForType(entityType, true).ToArray();

        if (keyMembers.Length == 0)
        {
            throw new InvalidOperationException($"The [ {nameof(TEntity)} ] type has no properties or fields marked as keys with the [ {nameof(SerializeAttribute)} ]");
        }

        Type[] keyTypes = keyMembers.Select(Reflection.GetMemberType).ToArray();

        Expression[] keyAccessExpressions = new Expression[keyMembers.Length];
        ParameterExpression typeParameterExpression = Expression.Parameter(entityType);

        for (int i = 0; i < keyMembers.Length; i++)
        {
            keyAccessExpressions[i] = Reflection.GetMemberAccessExpression(keyMembers[i], typeParameterExpression);
        }

        Expression constructorExpression = Expression.New(Reflection.GetTupleConstructor(keyTypes), keyAccessExpressions);
        Func<TEntity, Object> tupleFunction = Expression.Lambda<Func<TEntity, Object>>(constructorExpression, typeParameterExpression).Compile();

        return entity => new EntityKey<TEntity>(tupleFunction(entity));
    }

    #endregion
}