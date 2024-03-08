using Barchart.BinarySerializer.Types;
using System.Linq.Expressions;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    public static class SchemaFactory
    {
        private static readonly Dictionary<Type, object> allSerializers = new Dictionary<Type, object>();

        static SchemaFactory()
        {
            InitializeSerializers();
        }

        private static void InitializeSerializers()
        {
            allSerializers.Add(typeof(String), new BinarySerializerString());
            allSerializers.Add(typeof(Int32), new BinarySerializerInt32());
            allSerializers.Add(typeof(Int32?), new BinarySerializerInt32Nullable());
            allSerializers.Add(typeof(Int16), new BinarySerializerInt16());
            allSerializers.Add(typeof(Int16?), new BinarySerializerInt16Nullable());
            allSerializers.Add(typeof(Char), new BinarySerializerChar16());
            allSerializers.Add(typeof(Char?), new BinarySerializerChar16Nullable());
            allSerializers.Add(typeof(SByte), new BinarySerializerSInt8());
            allSerializers.Add(typeof(SByte?), new BinarySerializerSInt8Nullable());
            allSerializers.Add(typeof(Byte), new BinarySerializerInt8());
            allSerializers.Add(typeof(Byte?), new BinarySerializerInt8Nullable());
            allSerializers.Add(typeof(Boolean), new BinarySerializerBool8());
            allSerializers.Add(typeof(Boolean?), new BinarySerializerBool8Nullable());
            allSerializers.Add(typeof(Int64), new BinarySerializerInt64());
            allSerializers.Add(typeof(Int64?), new BinarySerializerInt64Nullable());
            allSerializers.Add(typeof(UInt16), new BinarySerializerUInt16());
            allSerializers.Add(typeof(UInt16?), new BinarySerializerUInt16Nullable());
            allSerializers.Add(typeof(UInt32), new BinarySerializerUInt32());
            allSerializers.Add(typeof(UInt32?), new BinarySerializerUInt32Nullable());
            allSerializers.Add(typeof(UInt64), new BinarySerializerUInt64());
            allSerializers.Add(typeof(UInt64?), new BinarySerializerUInt64Nullable());
            allSerializers.Add(typeof(Single), new BinarySerializerFloat());
            allSerializers.Add(typeof(Single?), new BinarySerializerFloatNullable());
            allSerializers.Add(typeof(Double), new BinarySerializerDouble());
            allSerializers.Add(typeof(Double?), new BinarySerializerDoubleNullable());
            allSerializers.Add(typeof(Decimal), new BinarySerializerDecimal());
            allSerializers.Add(typeof(Decimal?), new BinarySerializerDecimalNullable());
            allSerializers.Add(typeof(DateTime), new BinarySerializerDateTime());
            allSerializers.Add(typeof(DateTime?), new BinarySerializerDateTimeNullable());
            allSerializers.Add(typeof(DateOnly), new BinarySerializerDateOnly());
            allSerializers.Add(typeof(DateOnly?), new BinarySerializerDateOnlyNullable());
        }

        /// <summary>
        ///     Processes a class of generic type and collects data about fields and properties and their attributes
        /// </summary>
        /// <returns>Schema with all necessary data</returns>
        /// 
        public static Schema<T> GetSchema<T>() where T : new()
        {
            Type type = typeof(T);
            MemberInfo[] members = GetAllMembersForType(type);
            List<IMemberData<T>> memberDataList = new List<IMemberData<T>>();

            foreach (MemberInfo memberInfo in members)
            {
                IMemberData<T>? memberData = ProcessMemberInfo<T>(memberInfo);
                if(memberData != null) memberDataList.Add(memberData);
            }

            Schema<T> schema = new Schema<T>(memberDataList);

            return schema;
        } 

        private static IMemberData<T>? ProcessMemberInfo<T>(MemberInfo memberInfo) where T : new()
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

            if (IsReferenceType(memberType))
            {
                ISchema nestedSchema = GenerateSchemaInterface(memberType);
                IMemberData<T>? newMemberDataNestedClass = GenerateObjectMemberDataInterface<T>(nestedSchema, memberType, memberInfo);
                return newMemberDataNestedClass;
            }
            else
            {
                IMemberData<T>? newMemberData = GenerateMemberDataInterface<T>(memberType, memberInfo);
                return newMemberData;
            }
        }

        private static bool IsReferenceType(Type type)
        {
            return !type.IsValueType && type != typeof(string);
        }

        private static IBinaryTypeSerializer<V>? GetSerializer<V>()
        {
            object? serializer = null;
            allSerializers.TryGetValue(typeof(V), out serializer);

            return (IBinaryTypeSerializer<V>?) serializer;
        }

        private static MemberInfo[] GetAllMembersForType(Type type) {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            return type.FindMembers(MemberTypes.Property | MemberTypes.Field, bindingFlags, null, null);
        }

        private static bool GetIncludeAttributeValue(MemberInfo memberInfo)
        {
            var attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
            return attribute?.Include ?? false;
        }

        private static bool GetKeyAttributeValue(MemberInfo memberInfo)
        {
            var attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
            return attribute?.Key ?? false;
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

        public static IMemberData<T>? GenerateData<T, V> (MemberInfo memberInfo) 
        {
            bool include = GetIncludeAttributeValue(memberInfo);
            IBinaryTypeSerializer<V>? serializer = GetSerializer<V>();

            if (!include || serializer == null)
            {
                return null;
            }

            MemberData<T, V> newMemberData = new MemberData<T, V>(
                typeof(V),
                memberInfo.Name,
                include,
                GetKeyAttributeValue(memberInfo),
                memberInfo,
                GenerateGetter<T, V>(memberInfo),
                GenerateSetter<T, V>(memberInfo),
                serializer
                );
            return newMemberData;
        }

        public static IMemberData<T>? GenerateObjectData<T, V>(ISchema nestedSchema, MemberInfo memberInfo) where V : new()
        {
            bool include = GetIncludeAttributeValue(memberInfo);

            if (!include)
            {
                return null;
            }

            ObjectBinarySerializer<V> serializer = new ObjectBinarySerializer<V>((Schema<V>)nestedSchema);

            ObjectMemberData<T, V> newMemberData = new ObjectMemberData<T, V>(
                typeof(V),
                memberInfo.Name,
                include,
                GetKeyAttributeValue(memberInfo),
                memberInfo,
                GenerateGetter<T, V>(memberInfo),
                GenerateSetter<T, V>(memberInfo),
                serializer
                );

            return newMemberData;
        }

        public static IMemberData<T>? GenerateMemberDataInterface<T>(Type memberType, MemberInfo memberInfo)
        {
            var memberTypeExpr = Expression.Constant(memberType);
            var memberInfoExpr = Expression.Constant(memberInfo);
            var genericArgs = new Type[] { typeof(T), memberType };
            var generateDataMethod = typeof(SchemaFactory).GetMethod(nameof(GenerateData))!.MakeGenericMethod(genericArgs);
            var generateDataCallExpr = Expression.Call(null, generateDataMethod, memberInfoExpr);
            var lambdaExpr = Expression.Lambda<Func<IMemberData<T>?>>(generateDataCallExpr);
            var func = lambdaExpr.Compile();
            var memberData = func();

            return memberData;
        }

        public static IMemberData<T>? GenerateObjectMemberDataInterface<T>(ISchema nestedSchema,Type memberType, MemberInfo memberInfo)
        {
            var nestedSchemaExpr = Expression.Constant(nestedSchema);
            var memberTypeExpr = Expression.Constant(memberType);
            var memberInfoExpr = Expression.Constant(memberInfo);
            var genericArgs = new Type[] { typeof(T), memberType };
            var generateDataMethod = typeof(SchemaFactory).GetMethod(nameof(GenerateObjectData))!.MakeGenericMethod(genericArgs);
            var generateDataCallExpr = Expression.Call(null, generateDataMethod, nestedSchemaExpr, memberInfoExpr);
            var lambdaExpr = Expression.Lambda<Func<IMemberData<T>?>>(generateDataCallExpr);
            var func = lambdaExpr.Compile();
            var memberData = func();

            return memberData;
        }

        public static Action<T, V> GenerateSetter<T, V>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var value = Expression.Parameter(typeof(V), "value");
            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var assign = Expression.Assign(member, value);

            return Expression.Lambda<Action<T, V>>(assign, instance, value).Compile();
        }

        public static Func<T, V> GenerateGetter<T, V>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var member = Expression.MakeMemberAccess(instance, memberInfo);

            return Expression.Lambda<Func<T, V>>(member, instance).Compile();
        }
    }
}