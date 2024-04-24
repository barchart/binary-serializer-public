using Barchart.BinarySerializer.Types;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Factory class for generating schemas for binary serialization.
    /// </summary>
    public static class SchemaFactory
    {
        private static readonly object _lock = new();
        private static readonly IDictionary<Type, object> allSerializers = new Dictionary<Type, object>();

        public static bool DefaultIncludeValue { get; set; } = false;

        static SchemaFactory()
        {
            InitializeSerializers();
        }

        private static void InitializeSerializers()
        {
            BinarySerializerString stringSerializer = new();
            BinarySerializerInt32 intSerializer = new();
            BinarySerializerInt16 shortSerializer = new();
            BinarySerializerChar16 charSerializer = new();
            BinarySerializerSInt8 sbyteSerializer = new();
            BinarySerializerInt8 byteSerializer = new();
            BinarySerializerBool8 boolSerializer = new();
            BinarySerializerInt64 longSerializer = new();
            BinarySerializerUInt16 ushortSerializer = new();
            BinarySerializerUInt32 uintSerializer = new();
            BinarySerializerUInt64 ulongSerializer = new();
            BinarySerializerFloat floatSerializer = new();
            BinarySerializerDouble doubleSerializer = new();
            BinarySerializerDecimal decimalSerializer = new();
            BinarySerializerDateTime dateTimeSerializer = new();
            BinarySerializerDateOnly dateOnlySerializer = new();

            allSerializers.Add(typeof(string), stringSerializer);
            allSerializers.Add(typeof(int), intSerializer);
            allSerializers.Add(typeof(short), shortSerializer);
            allSerializers.Add(typeof(char), charSerializer);
            allSerializers.Add(typeof(sbyte), sbyteSerializer);
            allSerializers.Add(typeof(byte), byteSerializer);
            allSerializers.Add(typeof(bool), boolSerializer);
            allSerializers.Add(typeof(long), longSerializer);
            allSerializers.Add(typeof(ushort), ushortSerializer);
            allSerializers.Add(typeof(uint), uintSerializer);
            allSerializers.Add(typeof(ulong), ulongSerializer);
            allSerializers.Add(typeof(float), floatSerializer);
            allSerializers.Add(typeof(double), doubleSerializer);
            allSerializers.Add(typeof(decimal), decimalSerializer);
            allSerializers.Add(typeof(DateTime), dateTimeSerializer);
            allSerializers.Add(typeof(DateOnly), dateOnlySerializer);

            allSerializers.Add(typeof(int?), new BinarySerializerNullable<int>(intSerializer));
            allSerializers.Add(typeof(short?), new BinarySerializerNullable<short>(shortSerializer));
            allSerializers.Add(typeof(char?), new BinarySerializerNullable<char>(charSerializer));
            allSerializers.Add(typeof(sbyte?), new BinarySerializerNullable<sbyte>(sbyteSerializer));
            allSerializers.Add(typeof(byte?), new BinarySerializerNullable<byte>(byteSerializer));
            allSerializers.Add(typeof(bool?), new BinarySerializerNullable<bool>(boolSerializer));
            allSerializers.Add(typeof(long?), new BinarySerializerNullable<long>(longSerializer));
            allSerializers.Add(typeof(ushort?), new BinarySerializerNullable<ushort>(ushortSerializer));
            allSerializers.Add(typeof(uint?), new BinarySerializerNullable<uint>(uintSerializer));
            allSerializers.Add(typeof(ulong?), new BinarySerializerNullable<ulong>(ulongSerializer));
            allSerializers.Add(typeof(float?), new BinarySerializerNullable<float>(floatSerializer));
            allSerializers.Add(typeof(double?), new BinarySerializerNullable<double>(doubleSerializer));
            allSerializers.Add(typeof(decimal?), new BinarySerializerNullable<decimal>(decimalSerializer));
            allSerializers.Add(typeof(DateTime?), new BinarySerializerNullable<DateTime>(dateTimeSerializer));
            allSerializers.Add(typeof(DateOnly?), new BinarySerializerNullable<DateOnly>(dateOnlySerializer));
        }

        /// <summary>
        /// Retrieves the schema for the specified type <typeparamref name="TContainer"/>.
        /// </summary>
        /// <typeparam name="TContainer">The type of object for which the schema is generated.</typeparam>
        /// <returns>The schema for the specified type.</returns>
        public static Schema<TContainer> GetSchema<TContainer>() where TContainer : new()
        {
            Type type = typeof(TContainer);
            MemberInfo[] members = GetAllMembersForType(type);
            List<IMemberData<TContainer>> memberDataList = new();

            foreach (MemberInfo memberInfo in members)
            {
                bool isMemberIncluded = GetIncludeAttributeValue(memberInfo);

                if (isMemberIncluded)
                {
                    IMemberData<TContainer>? memberData = ProcessMemberInfo<TContainer>(memberInfo);
                    if (memberData != null) memberDataList.Add(memberData);
                }
            }

            Schema<TContainer> schema = new(memberDataList);

            return schema;
        }

        /// <summary>
        /// Processes the provided member information to generate member data for the specified type <typeparamref name="TContainer"/>.
        /// </summary>
        /// <typeparam name="TContainer">The type of object.</typeparam>
        /// <param name="memberInfo">The member information to process.</param>
        /// <returns>The generated member data for the specified type <typeparamref name="TContainer"/> if successful; otherwise, <see langword="null"/>.</returns>
        public static IMemberData<TContainer>? ProcessMemberInfo<TContainer>(MemberInfo memberInfo) where TContainer : new()
        {
            Type memberType;

            if (memberInfo is FieldInfo fieldInfo)
            {
                memberType = fieldInfo.FieldType;
                if (fieldInfo.IsInitOnly) return null;
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                memberType = propertyInfo.PropertyType;
                if (!propertyInfo.CanWrite) return null;
            }
            else
            {
                return null;
            }

            if (IsMemberComplexType(memberType) || IsMemberListType(memberType))
            {
                return GenerateObjectMemberData<TContainer>(memberType, memberInfo);
            }
            else
            {
                return GenerateMemberData<TContainer>(memberType, memberInfo);
            }
        }

        /// <summary>
        /// Gets a serializer for a Complex type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="TMember"/>.</returns>
        public static ObjectBinarySerializer<TMember> GetObjectSerializer<TMember>() where TMember : new()
        {
            Schema<TMember> schema = GetSchema<TMember>();
            return new ObjectBinarySerializer<TMember>(schema);
        }

        /// <summary>
        /// Gets a serializer for a list type <typeparamref name="TList"/> containing elements of type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The type of elements in the list.</typeparam>
        /// <typeparam name="TList">The type of list for which to get the serializer.</typeparam>
        /// <returns>A serializer for the specified list type <typeparamref name="TList"/> containing elements of type <typeparamref name="TMember"/> if successful; otherwise, <see langword="null"/>.</returns>
        public static BinarySerializerIList<TList, TMember>? GetListSerializer<TMember, TList>() where TList : IList<TMember>, new()
        {
            IBinaryTypeSerializer<TMember>? serializer = GetSerializer<TMember>();
            if (serializer == null) return null;
            return new BinarySerializerIList<TList, TMember>(serializer);
        }

        /// <summary>
        /// Gets a serializer for an enum type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The enum type for which to get the serializer.</typeparam>
        /// <returns>A serializer for the specified enum type <typeparamref name="TMember"/> if successful; otherwise, <see langword="null"/>.</returns>
        /// <remarks>
        /// The method assumes that the underlying storage type for the enum is `int`, and retrieves a serializer for `int` type to serialize the enum values.
        /// </remarks>
        public static BinarySerializerEnum<TMember> GetEnumSerializer<TMember>() where TMember : struct, Enum
        {
            return new BinarySerializerEnum<TMember>((BinarySerializerInt32)GetSerializer<int>()!);
        }

        /// <summary>
        /// Gets a serializer for a nullable value type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The nullable value type for which to get the serializer.</typeparam>
        /// <returns>
        /// A serializer for the specified nullable value type <typeparamref name="TMember"/> if successful; otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// The method first attempts to retrieve a serializer for the nullable type <typeparamref name="TMember"/> from the <paramref name="allSerializers"/> dictionary.
        /// If a serializer is found, it is cast to the appropriate type and returned.
        /// If a serializer is not found, the method retrieves a serializer for the underlying non-nullable type <typeparamref name="TMember"/>.
        /// If the retrieval is successful, the method constructs and returns a new serializer for the nullable type <typeparamref name="TMember"/>.
        /// If the retrieval fails, the method returns <see langword="null"/>.
        /// </remarks>
        public static BinarySerializerNullable<TMember>? GetNullableSerializer<TMember>() where TMember : struct
        {
            if (allSerializers.TryGetValue(typeof(TMember?), out object? serializer))
            {
                return (BinarySerializerNullable<TMember>)serializer;
            }

            IBinaryTypeSerializer<TMember>? newSerializer = GetSerializer<TMember>();
            if (newSerializer == null) return null;

            BinarySerializerNullable<TMember> nullableSerializer = new(newSerializer);

            lock (_lock)
            {
                if (!allSerializers.ContainsKey(typeof(TMember?)))
                {
                    allSerializers.Add(typeof(TMember?), nullableSerializer);
                }
            }

            return nullableSerializer;
        }

        /// <summary>
        /// Gets a serializer for the specified type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The type for which to get the serializer.</typeparam>
        /// <returns>A serializer for the specified type <typeparamref name="TMember"/> if available; otherwise, <c>null</c>.</returns>
        public static IBinaryTypeSerializer<TMember>? GetSerializer<TMember>()
        {
            if (IsMemberComplexType(typeof(TMember)))
            {
                var genericArgs = new Type[] { typeof(TMember) };
                var generateSerializerForListElements = typeof(SchemaFactory).GetMethod(nameof(GetObjectSerializer))!.MakeGenericMethod(genericArgs);
                var generateSerializerCallExpr = Expression.Call(null, generateSerializerForListElements);
                var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<TMember>?>>(generateSerializerCallExpr);
                var func = lambdaExpr.Compile();
                var result = func();

                return result;
            }
            else if (IsMemberListType(typeof(TMember)))
            {
                Type elementsType = typeof(TMember).GetGenericArguments()[0];

                var genericArgs = new Type[] { elementsType, typeof(TMember)};
                var generateSerializerForListElements = typeof(SchemaFactory).GetMethod(nameof(GetListSerializer))!.MakeGenericMethod(genericArgs);
                var generateSerializerCallExpr = Expression.Call(null, generateSerializerForListElements);
                var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<TMember>?>>(generateSerializerCallExpr);
                var func = lambdaExpr.Compile();
                var result = func();

                return result;
            }
            else if (IsMemberEnumType(typeof(TMember)))
            {
                var genericArgs = new Type[] { typeof(TMember) };
                var generateSerializerForListElements = typeof(SchemaFactory).GetMethod(nameof(GetEnumSerializer))!.MakeGenericMethod(genericArgs);
                var generateSerializerCallExpr = Expression.Call(null, generateSerializerForListElements);
                var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<TMember>?>>(generateSerializerCallExpr);
                var func = lambdaExpr.Compile();
                var result = func();

                return result;
            }
            else if (IsNullableNumericType(typeof(TMember)))
            {
                Type? underlyingType = Nullable.GetUnderlyingType(typeof(TMember));

                if (underlyingType == null) return null;

                var genericArgs = new Type[] { underlyingType };
                var generateNullableSerializer = typeof(SchemaFactory).GetMethod(nameof(GetNullableSerializer))!.MakeGenericMethod(genericArgs);
                var generateSerializerCallExpr = Expression.Call(null, generateNullableSerializer);
                var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<TMember>?>>(generateSerializerCallExpr);
                var func = lambdaExpr.Compile();
                var result = func();

                return result;
            }
            else
            {
                if (allSerializers.TryGetValue(typeof(TMember), out object? serializer))
                {
                    return (IBinaryTypeSerializer<TMember>?)serializer;
                }
            }

            return null;
        }

        /// <summary>
        /// Generates member data for the specified member.
        /// </summary>
        /// <typeparam name="TContainer">The type of object.</typeparam>
        /// <typeparam name="TMember">The type of the member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The member data if successful; otherwise, <see langword="null"/>.</returns>
        public static IMemberData<TContainer>? GenerateData<TContainer, TMember> (MemberInfo memberInfo)
        {
            IBinaryTypeSerializer<TMember>? serializer = GetSerializer<TMember>();

            if (serializer == null)
            {
                return null;
            }

            MemberData<TContainer, TMember> newMemberData = new(
                typeof(TMember),
                memberInfo.Name,
                GetKeyAttributeValue(memberInfo),
                memberInfo,
                GenerateGetter<TContainer, TMember>(memberInfo),
                GenerateSetter<TContainer, TMember>(memberInfo),
                serializer
            );

            return newMemberData;
        }

        /// <summary>
        /// Generates object member data for the specified member.
        /// </summary>
        /// <typeparam name="TContainer">The type of object.</typeparam>
        /// <typeparam name="TMember">The type of the object member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The object member data if successful; otherwise, <see langword="null"/>.</returns>
        public static IMemberData<TContainer>? GenerateObjectData<TContainer, TMember>(MemberInfo memberInfo) where TMember : new()
        {
            IBinaryTypeSerializer<TMember>? serializer = GetSerializer<TMember>();

            if (serializer == null)
            {
                return null;
            }

            ObjectMemberData<TContainer, TMember> newMemberData = new(
                typeof(TMember),
                memberInfo.Name,
                GetKeyAttributeValue(memberInfo),
                memberInfo,
                GenerateGetter<TContainer, TMember>(memberInfo),
                GenerateSetter<TContainer, TMember>(memberInfo),
                (IBinaryTypeObjectSerializer<TMember>)serializer
            );

            return newMemberData;
        }

        /// <summary>
        /// Generates member data interface for the specified member type and information.
        /// </summary>
        /// <typeparam name="TContainer">The type of object.</typeparam>
        /// <param name="memberType">The type of the member.</param>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The member data interface for the specified member.</returns>
        public static IMemberData<TContainer>? GenerateMemberData<TContainer>(Type memberType, MemberInfo memberInfo)
        {
            var memberInfoExpr = Expression.Constant(memberInfo);
            var genericArgs = new Type[] { typeof(TContainer), memberType };
            var generateDataMethod = typeof(SchemaFactory).GetMethod(nameof(GenerateData))!.MakeGenericMethod(genericArgs);
            var generateDataCallExpr = Expression.Call(null, generateDataMethod, memberInfoExpr);
            var lambdaExpr = Expression.Lambda<Func<IMemberData<TContainer>?>>(generateDataCallExpr);
            var func = lambdaExpr.Compile();
            var memberData = func();

            return memberData;
        }

        /// <summary>
        /// Generates object member data interface for the specified member type and information.
        /// </summary>
        /// <typeparam name="TContainer">The type of object.</typeparam>
        /// <param name="memberType">The type of the member.</param>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The object member data interface for the specified member.</returns>
        public static IMemberData<TContainer>? GenerateObjectMemberData<TContainer>(Type memberType, MemberInfo memberInfo)
        {
            var memberInfoExpr = Expression.Constant(memberInfo);
            var genericArgs = new Type[] { typeof(TContainer), memberType };
            var generateDataMethod = typeof(SchemaFactory).GetMethod(nameof(GenerateObjectData))!.MakeGenericMethod(genericArgs);
            var generateDataCallExpr = Expression.Call(null, generateDataMethod, memberInfoExpr);
            var lambdaExpr = Expression.Lambda<Func<IMemberData<TContainer>?>>(generateDataCallExpr);
            var func = lambdaExpr.Compile();
            var memberData = func();

            return memberData;
        }

        /// <summary>
        /// Generates a setter function for the specified member.
        /// </summary>
        /// <typeparam name="TContainer">The type of object.</typeparam>
        /// <typeparam name="TMember">The type of the member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The setter function for the specified member.</returns>
        public static Action<TContainer, TMember>? GenerateSetter<TContainer, TMember>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(TContainer), "instance");
            var value = Expression.Parameter(typeof(TMember), "value");
            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var assign = Expression.Assign(member, value);

            return Expression.Lambda<Action<TContainer, TMember>>(assign, instance, value).Compile();
        }

        /// <summary>
        /// Generates a getter function for the specified member.
        /// </summary>
        /// <typeparam name="TContainer">The type of object.</typeparam>
        /// <typeparam name="TMember">The type of the member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The getter function for the specified member.</returns>
        public static Func<TContainer, TMember> GenerateGetter<TContainer, TMember>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(TContainer), "instance");
            var member = Expression.MakeMemberAccess(instance, memberInfo);

            return Expression.Lambda<Func<TContainer, TMember>>(member, instance).Compile();
        }

        /// <summary>
        /// Determines whether the specified type is a complex type, i.e., not a value type, 
        /// string, ByteString, Enum or List.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a complex type; otherwise, false.</returns>
        private static bool IsMemberComplexType(Type type)
        {
            Type? underLyingType = Nullable.GetUnderlyingType(type);
            var isValueType = type.IsValueType;
            var isStringType = type == typeof(string);
            var isEnumType = underLyingType == null ? type.IsEnum : underLyingType.IsEnum;
            var isListOrRepeatedFieldGenericType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

            return !isValueType && !isEnumType && !isStringType && !isListOrRepeatedFieldGenericType;
        }

        /// <summary>
        /// Determines whether the specified type is a List or RepeatedField type, 
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a list type; otherwise, false./returns>
        private static bool IsMemberListType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
        }

        /// <summary>
        /// Determines whether the specified type is a Enum type, 
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a list type; otherwise, false./returns>
        private static bool IsMemberEnumType(Type type)
        {
            return type.IsEnum;
        }

        private static bool IsNullableNumericType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static MemberInfo[] GetAllMembersForType(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            return type.FindMembers(MemberTypes.Property | MemberTypes.Field, bindingFlags, null, null);
        }

        private static bool GetKeyAttributeValue(MemberInfo memberInfo)
        {
            var attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
            return attribute?.Key ?? false;
        }

        private static bool GetIncludeAttributeValue(MemberInfo memberInfo)
        {
            Attribute? attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
            return attribute != null || DefaultIncludeValue;
        }
    }
}