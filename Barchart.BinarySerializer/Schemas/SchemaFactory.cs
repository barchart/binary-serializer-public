#region Using Statements

using System.Linq.Expressions;
using System.Reflection;
using Barchart.BinarySerializer.SerializationUtilities.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Factory class for generating schemas for binary serialization.
    /// </summary>
    public static class SchemaFactory
    {
        #region Fields
        
        private static readonly IDictionary<Type, object> _serializers;
        
        #endregion
        
        #region Constructor(s)
        
        static SchemaFactory()
        {
            _serializers = new Dictionary<Type, object>();
            
            BinarySerializerString stringSerializer = new();
            BinarySerializerInt intSerializer = new();
            BinarySerializerShort shortSerializer = new();
            BinarySerializerChar charSerializer = new();
            BinarySerializerSbyte sbyteSerializer = new();
            BinarySerializerByte byteSerializer = new();
            BinarySerializerBool boolSerializer = new();
            BinarySerializerLong longSerializer = new();
            BinarySerializerUshort ushortSerializer = new();
            BinarySerializerUint uintSerializer = new();
            BinarySerializerUlong ulongSerializer = new();
            BinarySerializerFloat floatSerializer = new();
            BinarySerializerDouble doubleSerializer = new();
            BinarySerializerDecimal decimalSerializer = new();
            BinarySerializerDateTime dateTimeSerializer = new();
            BinarySerializerDateOnly dateOnlySerializer = new();

            _serializers.Add(typeof(string), stringSerializer);
            _serializers.Add(typeof(int), intSerializer);
            _serializers.Add(typeof(short), shortSerializer);
            _serializers.Add(typeof(char), charSerializer);
            _serializers.Add(typeof(sbyte), sbyteSerializer);
            _serializers.Add(typeof(byte), byteSerializer);
            _serializers.Add(typeof(bool), boolSerializer);
            _serializers.Add(typeof(long), longSerializer);
            _serializers.Add(typeof(ushort), ushortSerializer);
            _serializers.Add(typeof(uint), uintSerializer);
            _serializers.Add(typeof(ulong), ulongSerializer);
            _serializers.Add(typeof(float), floatSerializer);
            _serializers.Add(typeof(double), doubleSerializer);
            _serializers.Add(typeof(decimal), decimalSerializer);
            _serializers.Add(typeof(DateTime), dateTimeSerializer);
            _serializers.Add(typeof(DateOnly), dateOnlySerializer);

            _serializers.Add(typeof(int?), new BinarySerializerNullable<int>(intSerializer));
            _serializers.Add(typeof(short?), new BinarySerializerNullable<short>(shortSerializer));
            _serializers.Add(typeof(char?), new BinarySerializerNullable<char>(charSerializer));
            _serializers.Add(typeof(sbyte?), new BinarySerializerNullable<sbyte>(sbyteSerializer));
            _serializers.Add(typeof(byte?), new BinarySerializerNullable<byte>(byteSerializer));
            _serializers.Add(typeof(bool?), new BinarySerializerNullable<bool>(boolSerializer));
            _serializers.Add(typeof(long?), new BinarySerializerNullable<long>(longSerializer));
            _serializers.Add(typeof(ushort?), new BinarySerializerNullable<ushort>(ushortSerializer));
            _serializers.Add(typeof(uint?), new BinarySerializerNullable<uint>(uintSerializer));
            _serializers.Add(typeof(ulong?), new BinarySerializerNullable<ulong>(ulongSerializer));
            _serializers.Add(typeof(float?), new BinarySerializerNullable<float>(floatSerializer));
            _serializers.Add(typeof(double?), new BinarySerializerNullable<double>(doubleSerializer));
            _serializers.Add(typeof(decimal?), new BinarySerializerNullable<decimal>(decimalSerializer));
            _serializers.Add(typeof(DateTime?), new BinarySerializerNullable<DateTime>(dateTimeSerializer));
            _serializers.Add(typeof(DateOnly?), new BinarySerializerNullable<DateOnly>(dateOnlySerializer));
        }
        
        #endregion

        #region Methods
        
        /// <summary>
        ///     Retrieves the schema for the specified type <typeparamref name="TContainer"/>.
        /// </summary>
        /// <typeparam name="TContainer">The type of object for which the schema is generated.</typeparam>
        /// <returns>The schema for the specified type.</returns>
        public static Schema<TContainer> GetSchema<TContainer>() where TContainer : new()
        {
            Type type = typeof(TContainer);
            MemberInfo[] members = GetAllMembersForType(type);
            List<IMemberData<TContainer>> memberDataContainer = new();

            foreach (MemberInfo memberInfo in members)
            {
                bool isMemberIncluded = GetIncludeAttributeValue(memberInfo);

                if (isMemberIncluded)
                {
                    IMemberData<TContainer>? memberData = ProcessMemberInfo<TContainer>(memberInfo);
                    
                    if (memberData != null)
                    {
                        memberDataContainer.Add(memberData);
                    }
                }
            }

            Schema<TContainer> schema = new(memberDataContainer);

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
                
                if (fieldInfo.IsInitOnly)
                {
                    return null;
                }
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                memberType = propertyInfo.PropertyType;

                if (!propertyInfo.CanWrite)
                {
                    return null;
                }
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
        ///     Gets a serializer for a Complex type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="TMember"/>.</returns>
        public static ObjectBinarySerializer<TMember> GetObjectSerializer<TMember>() where TMember : new()
        {
            Schema<TMember> schema = GetSchema<TMember>();
            
            return new ObjectBinarySerializer<TMember>(schema);
        }

        /// <summary>
        ///     Gets a serializer for a list type <typeparamref name="TList"/> containing elements of type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The type of elements in the list.</typeparam>
        /// <typeparam name="TList">The type of list for which to get the serializer.</typeparam>
        /// <returns>A serializer for the specified list type <typeparamref name="TList"/> containing elements of type <typeparamref name="TMember"/> if successful; otherwise, <see langword="null"/>.</returns>
        public static BinarySerializerIList<TList, TMember>? GetListSerializer<TMember, TList>() where TList : IList<TMember>, new()
        {
            IBinaryTypeSerializer<TMember>? serializer = GetSerializer<TMember>();
            
            if (serializer == null)
            {
                return null;
            }
            
            return new BinarySerializerIList<TList, TMember>(serializer);
        }

        /// <summary>
        ///     Gets a serializer for an enum type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The enum type for which to get the serializer.</typeparam>
        /// <returns>A serializer for the specified enum type <typeparamref name="TMember"/> if successful; otherwise, <see langword="null"/>.</returns>
        /// <remarks>
        ///     The method assumes that the underlying storage type for the enum is `int`, and retrieves a serializer for `int` type to serialize the enum values.
        /// </remarks>
        public static BinarySerializerEnum<TMember> GetEnumSerializer<TMember>() where TMember : struct, Enum
        {
            return new BinarySerializerEnum<TMember>((BinarySerializerInt)GetSerializer<int>()!);
        }

        /// <summary>
        ///     Gets a serializer for a nullable value type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The nullable value type for which to get the serializer.</typeparam>
        /// <returns>
        ///     A serializer for the specified nullable value type <typeparamref name="TMember"/> if successful; otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        ///     The method first attempts to retrieve a serializer for the nullable type <typeparamref name="TMember"/> from the <paramref name="_serializers"/> dictionary.
        ///     If a serializer is found, it is cast to the appropriate type and returned.
        ///     If a serializer is not found, the method retrieves a serializer for the underlying non-nullable type <typeparamref name="TMember"/>.
        ///     If the retrieval is successful, the method constructs and returns a new serializer for the nullable type <typeparamref name="TMember"/>.
        ///     If the retrieval fails, the method returns <see langword="null"/>.
        /// </remarks>
        public static BinarySerializerNullable<TMember>? GetNullableSerializer<TMember>() where TMember : struct
        {
            if (_serializers.TryGetValue(typeof(TMember?), out object? serializer))
            {
                return (BinarySerializerNullable<TMember>)serializer;
            }

            IBinaryTypeSerializer<TMember>? newSerializer = GetSerializer<TMember>();
            
            if (newSerializer == null)
            {
                return null;
            }

            BinarySerializerNullable<TMember> nullableSerializer = new(newSerializer);

            lock (_serializers)
            {
                if (!_serializers.ContainsKey(typeof(TMember?)))
                {
                    _serializers.Add(typeof(TMember?), nullableSerializer);
                }
            }

            return nullableSerializer;
        }

        /// <summary>
        ///     Gets a serializer for the specified type <typeparamref name="TMember"/>.
        /// </summary>
        /// <typeparam name="TMember">The type for which to get the serializer.</typeparam>
        /// <returns>A serializer for the specified type <typeparamref name="TMember"/> if available; otherwise, <c>null</c>.</returns>
        public static IBinaryTypeSerializer<TMember>? GetSerializer<TMember>()
        {
            if (IsMemberComplexType(typeof(TMember)))
            {
                return GenerateSerializer<TMember>(nameof(GetObjectSerializer), null);
            }
            
            if (IsMemberListType(typeof(TMember)))
            {
                Type elementsType = typeof(TMember).GetGenericArguments()[0];
                
                return GenerateSerializer<TMember>(nameof(GetListSerializer), elementsType);
            }
            
            if (IsMemberEnumType(typeof(TMember)))
            {
                return GenerateSerializer<TMember>(nameof(GetEnumSerializer), null);
            }
            
            if (IsNullableNumericType(typeof(TMember)))
            {
                Type? underlyingType = Nullable.GetUnderlyingType(typeof(TMember));
                
                if (underlyingType == null)
                {
                    return null;
                }

                return GenerateSerializer<TMember>(nameof(GetNullableSerializer), underlyingType);
            }

            if (_serializers.TryGetValue(typeof(TMember), out object? serializer))
            {
                return (IBinaryTypeSerializer<TMember>?)serializer;
            }
            
            return null;
        }

        /// <summary>
        ///     Generates member data for the specified member.
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
                GenerateSetter<TContainer, TMember?>(memberInfo),
                serializer
            );

            return newMemberData;
        }

        /// <summary>
        ///     Generates object member data for the specified member.
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
                GenerateSetter<TContainer, TMember?>(memberInfo),
                (IBinaryTypeObjectSerializer<TMember>)serializer
            );

            return newMemberData;
        }

        /// <summary>
        ///     Generates member data interface for the specified member type and information.
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
        ///     Generates object member data interface for the specified member type and information.
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
        ///     Generates a setter function for the specified member.
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
        ///     Generates a getter function for the specified member.
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
        /// string, Enum or List.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a complex type; otherwise, false.</returns>
        private static bool IsMemberComplexType(Type type)
        {
            Type? underLyingType = Nullable.GetUnderlyingType(type);
            var isValueType = type.IsValueType;
            var isStringType = type == typeof(string);
            var isEnumType = underLyingType == null ? type.IsEnum : underLyingType.IsEnum;
            var isListOrGenericType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

            return !isValueType && !isEnumType && !isStringType && !isListOrGenericType;
        }

        /// <summary>
        ///     Determines whether the specified type is a List type, 
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a list type; otherwise, false.</returns>
        private static bool IsMemberListType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
        }

        /// <summary>
        ///     Determines whether the specified type is a Enum type, 
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a list type; otherwise, false.</returns>
        private static bool IsMemberEnumType(Type type)
        {
            return type.IsEnum;
        }

        private static IBinaryTypeSerializer<TMember>? GenerateSerializer<TMember>(string methodName, Type? type)
        {
            Type[]? genericArgs = null;

            genericArgs = methodName switch
            {
                nameof(GetObjectSerializer) or nameof(GetEnumSerializer) => new[] { typeof(TMember) },
                nameof(GetListSerializer) => new[] { type!, typeof(TMember) },
                nameof(GetNullableSerializer) => new[] { type! },
                _ => Array.Empty<Type>(),
            };
            
            var generateSerializerMethod = typeof(SchemaFactory).GetMethod(methodName)?.MakeGenericMethod(genericArgs);

            if (generateSerializerMethod == null)
            {
                return null;
            }
            
            var generateSerializerCallExpr = Expression.Call(null, generateSerializerMethod);
            var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<TMember>?>>(generateSerializerCallExpr);
            var func = lambdaExpr.Compile();
            var result = func();

            return result;
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
            BinarySerializeAttribute? attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
            
            if (attribute == null)
            {
                return true;
            }
            
            return attribute.Include;
        }
        
        #endregion
    }
}