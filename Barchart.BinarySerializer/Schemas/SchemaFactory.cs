﻿using Barchart.BinarySerializer.Types;
using Google.Protobuf;
using Google.Protobuf.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Factory class for generating schemas for binary serialization.
    /// </summary>
    public static class SchemaFactory
    {
        private static readonly IDictionary<Type, object> allSerializers = new Dictionary<Type, object>();
        public static bool DefaultIncludeValue { get; set; } = true;

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
            BinarySerializerByteString byteStringSerializer = new();

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
            allSerializers.Add(typeof(ByteString), byteStringSerializer);

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
        /// Retrieves the schema for the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object for which the schema is generated.</typeparam>
        /// <returns>The schema for the specified type.</returns>
        public static Schema<T> GetSchema<T>() where T : new()
        {
            Type type = typeof(T);
            MemberInfo[] members = GetAllMembersForType(type);
            List<IMemberData<T>> memberDataList = new();

            foreach (MemberInfo memberInfo in members)
            {
                IMemberData<T>? memberData = ProcessMemberInfo<T>(memberInfo);
                if(memberData != null) memberDataList.Add(memberData);
            }

            Schema<T> schema = new(memberDataList);

            return schema;
        } 

        public static IMemberData<T>? ProcessMemberInfo<T>(MemberInfo memberInfo) where T : new()
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

            if (IsMemberComplexType(memberType))
            {
                ISchema nestedSchema = GenerateSchemaInterface(memberType);
                IMemberData<T>? newMemberDataNestedClass = GenerateObjectMemberData<T>(nestedSchema, memberType, memberInfo);
                return newMemberDataNestedClass;
            }
            else if (IsMemberListOrRepeatedFieldType(memberType))
            {
                IMemberData<T>? newMemberDataList = GenerateListMemberData<T>(memberType, memberInfo);
                return newMemberDataList;
            }
            else
            {
                IMemberData<T>? newMemberData = GenerateMemberData<T>(memberType, memberInfo);
                return newMemberData;
            }
        }

        /// <summary>
        /// Gets a serializer for a Complex type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="T"/>.</returns>
        public static ObjectBinarySerializer<T> GetObjectSerializer<T>(Schema<T> schema) where T: new()
        {
            return new ObjectBinarySerializer<T>(schema);
        }

        /// <summary>
        /// Gets a serializer for a list of elements of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="T"/>.</returns>
        public static BinarySerializerIList<List<T>, T>? GetListSerializer<T>()
        {
            IBinaryTypeSerializer<T>? serializer = GetSerializer<T>();
            if (serializer == null) return null;
            return new BinarySerializerIList<List<T>, T>(serializer);
        }

        /// <summary>
        /// Gets a serializer for a list of elements of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="T"/>.</returns>
        public static BinarySerializerIList<List<T>, T>? GetListObjectSerializer<T>() where T: new()
        {
            Schema<T> schema = (Schema<T>)GenerateSchemaInterface(typeof(T));
            IBinaryTypeSerializer<T>? serializer = GetObjectSerializer(schema);

            if (serializer == null) return null;
            return new BinarySerializerIList<List<T>, T>(serializer);
        }


        /// <summary>
        /// Gets a serializer for a RepeatedField type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="T"/>.</returns>
        public static BinarySerializerIList<RepeatedField<T>, T>? GetRepeatedFieldSerializer<T>()
        {
            IBinaryTypeSerializer<T>? serializer = GetSerializer<T>();
            if (serializer == null) return null;
            return new BinarySerializerIList<RepeatedField<T>, T>(serializer);
        }

        /// <summary>
        /// Gets a serializer for a RepeatedField type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="T"/>.</returns>
        public static BinarySerializerIList<RepeatedField<T>, T>? GetRepeatedFieldObjectSerializer<T>() where T: new()
        {
            Schema<T> schema = (Schema<T>)GenerateSchemaInterface(typeof(T));
            IBinaryTypeSerializer<T>? serializer = GetObjectSerializer(schema);

            if (serializer == null) return null;
            return new BinarySerializerIList<RepeatedField<T>, T>(serializer);
        }

        /// <summary>
        /// Gets a serializer for a Enum type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="T"/>.</returns>
        public static BinarySerializerEnum<T> GetEnumSerializer<T>() where T : struct, Enum
        {
            return new BinarySerializerEnum<T>((BinarySerializerInt32)GetSerializer<int>()!);
        }

        /// <summary>
        /// Gets a serializer for a Enum type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A serializer for a list of elements of type <typeparamref name="T"/>.</returns>
        public static BinarySerializerNullable<T>? GetNullableSerializer<T>() where T : struct
        {
            if (allSerializers.TryGetValue(typeof(T?), out object? serializer))
            {
                return (BinarySerializerNullable<T>) serializer;
            }

            IBinaryTypeSerializer<T>? newSerializer = GetSerializer<T>();
            if (newSerializer == null) return null;
            allSerializers.Add(typeof(T?), newSerializer);

            return new BinarySerializerNullable<T>(newSerializer);
        }


        /// <summary>
        /// Gets a serializer for the specified type <typeparamref name="V"/>.
        /// </summary>
        /// <typeparam name="V">The type for which to get the serializer.</typeparam>
        /// <returns>A serializer for the specified type <typeparamref name="V"/> if available; otherwise, <c>null</c>.</returns>
        public static IBinaryTypeSerializer<V>? GetSerializer<V>()
        {
            if (IsMemberListType(typeof(V)))
            {
                Type elementsType = typeof(V).GetGenericArguments()[0];

                if (IsMemberComplexType(elementsType))
                {
                    var genericArgs = new Type[] { elementsType };
                    var generateSerializerForListElements = typeof(SchemaFactory).GetMethod(nameof(GetListObjectSerializer))!.MakeGenericMethod(genericArgs);
                    var generateSerializerCallExpr = Expression.Call(null, generateSerializerForListElements);
                    var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<V>?>>(generateSerializerCallExpr);
                    var func = lambdaExpr.Compile();
                    var result = func();

                    return result;
                }
                else
                {
                    if (allSerializers.TryGetValue(elementsType, out object? listElementsSerializer))
                    {
                        var genericArgs = new Type[] { elementsType };
                        var generateSerializerForListElements = typeof(SchemaFactory).GetMethod(nameof(GetListSerializer))!.MakeGenericMethod(genericArgs);
                        var generateSerializerCallExpr = Expression.Call(null, generateSerializerForListElements);
                        var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<V>?>>(generateSerializerCallExpr);
                        var func = lambdaExpr.Compile();
                        var result = func();

                        return result;
                    }
                }
            }
            else if (IsMemberRepeatedFieldType(typeof(V)))
            {
                Type elementsType = typeof(V).GetGenericArguments()[0];

                if (IsMemberComplexType(elementsType))
                {
                    var genericArgs = new Type[] { elementsType };
                    var generateSerializerForRepeatedFieldElements = typeof(SchemaFactory).GetMethod(nameof(GetRepeatedFieldObjectSerializer))!.MakeGenericMethod(genericArgs);
                    var generateSerializerCallExpr = Expression.Call(null, generateSerializerForRepeatedFieldElements);
                    var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<V>?>>(generateSerializerCallExpr);
                    var func = lambdaExpr.Compile();
                    var result = func();

                    return result;
                }
                else
                {
                    if (allSerializers.TryGetValue(elementsType, out object? repeatedFieldElementsSerializer))
                    {
                        var genericArgs = new Type[] { elementsType };
                        var generateSerializerForRepeatedFieldElements = typeof(SchemaFactory).GetMethod(nameof(GetRepeatedFieldSerializer))!.MakeGenericMethod(genericArgs);
                        var generateSerializerCallExpr = Expression.Call(null, generateSerializerForRepeatedFieldElements);
                        var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<V>?>>(generateSerializerCallExpr);
                        var func = lambdaExpr.Compile();
                        var result = func();

                        return result;
                    }
                }
            }
            else if (IsMemberEnumType(typeof(V)))
            {
                var genericArgs = new Type[] { typeof(V) };
                var generateSerializerForListElements = typeof(SchemaFactory).GetMethod(nameof(GetEnumSerializer))!.MakeGenericMethod(genericArgs);
                var generateSerializerCallExpr = Expression.Call(null, generateSerializerForListElements);
                var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<V>?>>(generateSerializerCallExpr);
                var func = lambdaExpr.Compile();
                var result = func();

                return result;
            }
            else if (IsNullableNumericType(typeof(V)))
            {
                Type? underlyingType = Nullable.GetUnderlyingType(typeof(V));

                if (underlyingType == null) return null;

                if (allSerializers.TryGetValue(underlyingType, out object? nullableFieldElementsSerializer))
                {
                    //var arguments = Expression.Constant(nullableFieldElementsSerializer);

                    var genericArgs = new Type[] { underlyingType };
                    var generateNullableSerializer = typeof(SchemaFactory).GetMethod(nameof(GetNullableSerializer))!.MakeGenericMethod(genericArgs);
                    var generateSerializerCallExpr = Expression.Call(null, generateNullableSerializer);
                    var lambdaExpr = Expression.Lambda<Func<IBinaryTypeSerializer<V>?>>(generateSerializerCallExpr);
                    var func = lambdaExpr.Compile();
                    var result = func();

                    return result;
                }
            }
            else
            {
                if (allSerializers.TryGetValue(typeof(V), out object? serializer))
                {
                    return (IBinaryTypeSerializer<V>?)serializer;
                }
            }

            return null;
        }

        /// <summary>
        /// Generates a schema interface for the specified type.
        /// </summary>
        /// <param name="type">The type for which to generate the schema interface.</param>
        /// <returns>The schema interface for the specified type.</returns>
        public static ISchema GenerateSchemaInterface(Type type)
        {
            Type[] types = { type };
            MethodCallExpression methodCallExpression = Expression.Call(typeof(SchemaFactory), nameof(GetSchema), types, null);
            Expression<Func<ISchema>> lambdaExpression = Expression.Lambda<Func<ISchema>>(methodCallExpression);
            Func<ISchema> function = lambdaExpression.Compile();
            ISchema schemaInterface = function();

            return schemaInterface;
        }

        /// <summary>
        /// Generates member data for the specified member.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <typeparam name="V">The type of the member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The member data if successful; otherwise, <see langword="null"/>.</returns>
        public static IMemberData<T>? GenerateData<T, V> (MemberInfo memberInfo)
        {
            bool include = GetIncludeAttributeValue(memberInfo);

            if (!include)
            {
                return null;
            }

            IBinaryTypeSerializer<V>? serializer = GetSerializer<V>();

            if (serializer == null)
            {
                return null;
            }

            MemberData<T, V> newMemberData = new(
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

        /// <summary>
        /// Generates object member data for the specified member.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <typeparam name="V">The type of the object member.</typeparam>
        /// <param name="nestedSchema">The nested schema for the member object.</param>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The object member data if successful; otherwise, <see langword="null"/>.</returns>
        public static IMemberData<T>? GenerateObjectData<T, V>(ISchema nestedSchema, MemberInfo memberInfo) where V : new()
        {
            bool include = GetIncludeAttributeValue(memberInfo);

            if (!include)
            {
                return null;
            }

            ObjectBinarySerializer<V> serializer = new((Schema<V>)nestedSchema);

            ObjectMemberData<T, V> newMemberData = new(
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

        /// <summary>
        /// Generates object member data for the specified member.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <typeparam name="V">The type of the object member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The object member data if successful; otherwise, <see langword="null"/>.</returns>
        public static IMemberData<T>? GenerateListData<T, V>(MemberInfo memberInfo) where V : new()
        {
            bool include = GetIncludeAttributeValue(memberInfo);

            if (!include)
            {
                return null;
            }
            
            IBinaryTypeSerializer<V>? serializer = GetSerializer<V>();

            if(serializer == null)
            {
                return null;
            }

            ObjectMemberData<T, V> newMemberData = new(
                typeof(V),
                memberInfo.Name,
                include,
                GetKeyAttributeValue(memberInfo),
                memberInfo,
                GenerateGetter<T, V>(memberInfo),
                GenerateRepeatedFieldSetter<T, V>(memberInfo),
                (IBinaryTypeObjectSerializer<V>)serializer
            );

            return newMemberData;
        }

        /// <summary>
        /// Generates member data interface for the specified member type and information.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="memberType">The type of the member.</param>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The member data interface for the specified member.</returns>
        public static IMemberData<T>? GenerateMemberData<T>(Type memberType, MemberInfo memberInfo)
        {
            var memberInfoExpr = Expression.Constant(memberInfo);
            var genericArgs = new Type[] { typeof(T), memberType };
            var generateDataMethod = typeof(SchemaFactory).GetMethod(nameof(GenerateData))!.MakeGenericMethod(genericArgs);
            var generateDataCallExpr = Expression.Call(null, generateDataMethod, memberInfoExpr);
            var lambdaExpr = Expression.Lambda<Func<IMemberData<T>?>>(generateDataCallExpr);
            var func = lambdaExpr.Compile();
            var memberData = func();

            return memberData;
        }

        /// <summary>
        /// Generates object member data interface for the specified member type and information.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="nestedSchema">The nested schema for the object member.</param>
        /// <param name="memberType">The type of the member.</param>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The object member data interface for the specified member.</returns>
        public static IMemberData<T>? GenerateObjectMemberData<T>(ISchema nestedSchema,Type memberType, MemberInfo memberInfo)
        {
            var nestedSchemaExpr = Expression.Constant(nestedSchema);
            var memberInfoExpr = Expression.Constant(memberInfo);
            var genericArgs = new Type[] { typeof(T), memberType };
            var generateDataMethod = typeof(SchemaFactory).GetMethod(nameof(GenerateObjectData))!.MakeGenericMethod(genericArgs);
            var generateDataCallExpr = Expression.Call(null, generateDataMethod, nestedSchemaExpr, memberInfoExpr);
            var lambdaExpr = Expression.Lambda<Func<IMemberData<T>?>>(generateDataCallExpr);
            var func = lambdaExpr.Compile();
            var memberData = func();

            return memberData;
        }

        /// <summary>
        /// Generates object member data interface for the specified member type and information.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="memberType">The type of the member.</param>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The object member data interface for the specified member.</returns>
        public static IMemberData<T>? GenerateListMemberData<T>(Type memberType, MemberInfo memberInfo)
        {
            var memberInfoExpr = Expression.Constant(memberInfo);
            var genericArgs = new Type[] { typeof(T), memberType };
            var generateDataMethod = typeof(SchemaFactory).GetMethod(nameof(GenerateListData))!.MakeGenericMethod(genericArgs);
            var generateDataCallExpr = Expression.Call(null, generateDataMethod, memberInfoExpr);
            var lambdaExpr = Expression.Lambda<Func<IMemberData<T>?>>(generateDataCallExpr);
            var func = lambdaExpr.Compile();
            var memberData = func();

            return memberData;
        }

        /// <summary>
        /// Generates a setter function for the specified member.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <typeparam name="V">The type of the member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The setter function for the specified member.</returns>
        public static Action<T, V> GenerateSetter<T, V>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var value = Expression.Parameter(typeof(V), "value");
            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var assign = Expression.Assign(member, value);

            return Expression.Lambda<Action<T, V>>(assign, instance, value).Compile();
        }

        /// <summary>
        /// Generates a setter function for the repeated field member.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <typeparam name="V">The type of the member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The setter function for the specified member.</returns>
        public static Action<T, V> GenerateRepeatedFieldSetter<T, V>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var value = Expression.Parameter(typeof(V), "value");

            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var addRangeMethod = typeof(V).GetMethod("AddRange")!;
            var clearMethod = typeof(V).GetMethod("Clear")!;

            var clearCall = Expression.Call(member, clearMethod);
            var addRangeCall = Expression.Call(member, addRangeMethod, value);
            var block = Expression.Block(clearCall, addRangeCall);

            return Expression.Lambda<Action<T, V>>(block, instance, value).Compile();
        }

        /// <summary>
        /// Generates a getter function for the specified member.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <typeparam name="V">The type of the member.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <returns>The getter function for the specified member.</returns>
        public static Func<T, V> GenerateGetter<T, V>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var member = Expression.MakeMemberAccess(instance, memberInfo);

            return Expression.Lambda<Func<T, V>>(member, instance).Compile();
        }

        private static Type GetMemberType(MemberInfo memberInfo)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo)memberInfo).FieldType,
                MemberTypes.Property => ((PropertyInfo)memberInfo).PropertyType,
                _ => throw new ArgumentException("Unknown member type: " + memberInfo.MemberType),
            };
        }

        /// <summary>
        /// Determines whether the specified type is a complex type, i.e., not a value type, 
        /// string, ByteString, Enum or List.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a complex type; otherwise, false.</returns>
        private static bool IsMemberComplexType(Type type)
        {
            var isNotValueType = !type.IsValueType;
            var isNotStringType = type != typeof(string);
            var isNotByteStringType = type != typeof(ByteString);
            var isNotEnumType = !type.IsEnum;
            var isListOrRepeatedFieldGenericType = type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(RepeatedField<>) || type.GetGenericTypeDefinition() == typeof(List<>));

            return isNotValueType && isNotEnumType && isNotStringType && isNotByteStringType && !isListOrRepeatedFieldGenericType;
        }

        /// <summary>
        /// Determines whether the specified type is a List or RepeatedField type, 
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a list type; otherwise, false./returns>
        private static bool IsMemberListOrRepeatedFieldType(Type type)
        {
            return IsMemberListType(type) || IsMemberRepeatedFieldType(type);
        }

        /// <summary>
        /// Determines whether the specified type is a List type, 
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

        /// <summary>
        /// Determines whether the specified type is a RepeatedField type, 
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type is a list type; otherwise, false./returns>
        private static bool IsMemberRepeatedFieldType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(RepeatedField<>));
        }

        private static MemberInfo[] GetAllMembersForType(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            return type.FindMembers(MemberTypes.Property | MemberTypes.Field, bindingFlags, null, null);
        }

        private static bool GetIncludeAttributeValue(MemberInfo memberInfo)
        {
            var attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
            return attribute?.Include ?? DefaultIncludeValue;
        }

        private static bool GetKeyAttributeValue(MemberInfo memberInfo)
        {
            var attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
            return attribute?.Key ?? false;
        }
    }
}