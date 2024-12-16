#region Using Statements

using System.Linq.Expressions;
using System.Reflection;

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Entities.Keys;
using Barchart.BinarySerializer.Serializers;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Entities.Factories;

public class EntityManagerFactory
{
    #region Constructor(s)
    
    public EntityManagerFactory()
    {

    }

    #endregion

    #region Methods

    public EntityManager<TEntity> Make<TEntity>(Serializer<TEntity> serializer)  where TEntity : class, new()
    {
        return new EntityManager<TEntity>(serializer, MakeKeyExtractor<TEntity>());
    }
    
    public Func<TEntity, IEntityKey<TEntity>> MakeKeyExtractor<TEntity>() where TEntity : class, new()
    {
        Type entityType = typeof(TEntity);

        MemberInfo[] keyMembers = Reflection.GetMembersForType(entityType, true).ToArray();
        
        if (keyMembers.Length == 0)
        {
            throw new InvalidOperationException($"The [ {nameof(TEntity)} ] type has no properties or fields marked as keys with the [ { nameof(SerializeAttribute)} ]");
        }
        
        Type[] keyTypes = keyMembers.Select(Reflection.GetMemberType).ToArray();
        
        Expression[] keyAccessExpressions = new Expression[ keyMembers.Length ];
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
