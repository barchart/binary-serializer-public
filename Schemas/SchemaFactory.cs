using JerqAggregatorNew.Types;
using System.Linq.Expressions;
using System.Reflection;

namespace JerqAggregatorNew.Schemas
{
    public class BinarySerializeAttribute : Attribute
    {
        public bool Include { get; }
        public bool Key { get; }
        public BinarySerializeAttribute(bool include, bool key)
        {
            Include = include;
            Key = key;
        }
    }

    public static class SchemaFactory
    {

        private static readonly Dictionary<Type, ISerializer> allSerializers = new Dictionary<Type, ISerializer>();

        static SchemaFactory()
        {
            allSerializers.Add(typeof(String), new BinarySerializerString());
            allSerializers.Add(typeof(Int32), new BinarySerializerInt32());
            allSerializers.Add(typeof(Int32?), new BinarySerializerInt32());
            allSerializers.Add(typeof(Int16), new BinarySerializerInt16());
            allSerializers.Add(typeof(Int16?), new BinarySerializerInt16());
            allSerializers.Add(typeof(Char), new BinarySerializerChar16());
            allSerializers.Add(typeof(Char?), new BinarySerializerChar16());
            allSerializers.Add(typeof(SByte), new BinarySerializerSInt8());
            allSerializers.Add(typeof(SByte?), new BinarySerializerSInt8());
            allSerializers.Add(typeof(Byte), new BinarySerializerInt8());
            allSerializers.Add(typeof(Byte?), new BinarySerializerInt8());
            allSerializers.Add(typeof(Boolean), new BinarySerializerBool8());
            allSerializers.Add(typeof(Boolean?), new BinarySerializerBool8());
            allSerializers.Add(typeof(Int64), new BinarySerializerInt64());
            allSerializers.Add(typeof(Int64?), new BinarySerializerInt64());
            allSerializers.Add(typeof(UInt16), new BinarySerializerUInt16());
            allSerializers.Add(typeof(UInt16?), new BinarySerializerUInt16());
            allSerializers.Add(typeof(UInt32), new BinarySerializerUInt32());
            allSerializers.Add(typeof(UInt32?), new BinarySerializerUInt32());
            allSerializers.Add(typeof(UInt64), new BinarySerializerUInt64());
            allSerializers.Add(typeof(UInt64?), new BinarySerializerUInt64());
            allSerializers.Add(typeof(Single), new BinarySerializerFloat());
            allSerializers.Add(typeof(Single?), new BinarySerializerFloat());
            allSerializers.Add(typeof(Double), new BinarySerializerDouble());
            allSerializers.Add(typeof(Double?), new BinarySerializerDouble());
            allSerializers.Add(typeof(Decimal), new BinarySerializerDecimal());
            allSerializers.Add(typeof(Decimal?), new BinarySerializerDecimal());
            allSerializers.Add(typeof(DateTime), new BinarySerializerDateTime());
            allSerializers.Add(typeof(DateTime?), new BinarySerializerDateTime());
            allSerializers.Add(typeof(DateOnly), new BinarySerializerDateOnly());
            allSerializers.Add(typeof(DateOnly?), new BinarySerializerDateOnly());
        }

        /// <summary>
        ///     Processes a class of generic type and collects data about fields and properties and their attributes
        /// </summary>
        /// <returns>Schema with all necessary data</returns>
        /// 
        public static Schema<T> GetSchema<T>() where T : new()
        {
            Schema<T> schema = new Schema<T>();
            Type type = typeof(T);

            MemberInfo[] members = GetAllMembersForType(type);

            foreach (MemberInfo memberInfo in members)
            {
                MemberData<T>? member = ProcessMemberInfo<T>(memberInfo);
                if(member != null) schema.AddMemberData((MemberData<T>)member);
            }

            return schema;
        }

        private static MemberData<T>? ProcessMemberInfo<T>(MemberInfo memberInfo) where T : new()
        {
            Type memberType;

            if (memberInfo is FieldInfo)
            {
                memberType = ((FieldInfo)memberInfo).FieldType;
            }
            else
            {
                memberType = ((PropertyInfo)memberInfo).PropertyType;
            }

            BinarySerializeAttribute? attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));

            if (attribute == null) { return null; }

            bool include = attribute.Include;
            bool key = attribute.Key;

            ISerializer? serializer;
            allSerializers.TryGetValue(memberType, out serializer);

            if (IsClassMember(serializer, memberType))
            {
                ISchema nestedSchema = GenerateSchemaInterface(memberType);
                var getterNestedClass = GenerateGetter<T>(memberInfo);
                var setterNestedClass = GenerateSetter<T>(memberInfo);
                var objectbinarySerializer = new ObjectBinarySerializer(nestedSchema);

                MemberData<T> newMemberDataNestedClass = new MemberData<T>()
                {
                    Type = memberType,
                    Name = memberInfo.Name,
                    IsIncluded = include,
                    IsKeyAttribute = key,
                    BinarySerializer = objectbinarySerializer,
                    MemberInfo = memberInfo,
                };

                newMemberDataNestedClass.GetDelegate = getterNestedClass;
                newMemberDataNestedClass.SetDelegate = setterNestedClass;
                return newMemberDataNestedClass;
            }

            MemberData<T> newMemberData = new MemberData<T>()
            {
                Type = memberType,
                Name = memberInfo.Name,
                IsIncluded = include,
                IsKeyAttribute = key,
                BinarySerializer = serializer!,
                MemberInfo = memberInfo
            };

            var getter = GenerateGetter<T>(memberInfo);
            var setter = GenerateSetter<T>(memberInfo);
            newMemberData.GetDelegate = getter;
            newMemberData.SetDelegate = setter;
           
            return newMemberData;
        }

        private static bool IsClassMember(ISerializer? serializer, Type memberType)
        {
            return (serializer == null && memberType.IsClass);
        }

        private static MemberInfo[] GetAllMembersForType(Type type) {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            return type.FindMembers(MemberTypes.Property | MemberTypes.Field, bindingFlags, null, null);
        }

        public static ISchema GenerateSchemaInterface(Type type)
        {
            Type[] types = { type };
            MethodCallExpression methodCallExpression = Expression.Call(typeof(SchemaFactory), nameof(GetSchema), types, null);
            Expression<Func<ISchema>> lambdaExpression = Expression.Lambda<Func<ISchema>>(methodCallExpression);
            Func<ISchema> function = lambdaExpression.Compile();
            ISchema schemaInterface = function();
            return schemaInterface;
        }

        public static Action<T, object?> GenerateSetter<T>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var value = Expression.Parameter(typeof(object), "value");
            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var memberType = memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
                _ => throw new NotImplementedException(),
            };
            var convert = Expression.Convert(value, memberType);
            var assign = Expression.Assign(member, convert);

            return Expression.Lambda<Action<T, object?>>(assign, instance, value).Compile();
        }

        public static Func<T, object?> GenerateGetter<T>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var convert = Expression.Convert(member, typeof(object));

            return Expression.Lambda<Func<T, object?>>(convert, instance).Compile();
        }
    }
}