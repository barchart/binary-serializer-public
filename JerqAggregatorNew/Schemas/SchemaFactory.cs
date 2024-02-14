using JerqAggregatorNew.Types;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace JerqAggregatorNew.Schemas
{
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

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            MemberInfo[] members = type.FindMembers(MemberTypes.Property | MemberTypes.Field, bindingFlags, null, null);

            foreach (MemberInfo memberInfo in members)
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
                if (attribute == null) { continue; }

                bool include = attribute.Include;
                bool key = attribute.Key;

                ISerializer? serializer;
                allSerializers.TryGetValue(memberType, out serializer);

                if (serializer == null) { continue; }

                MemberData<T> newMemberData = new MemberData<T>()
                {
                    Type = memberType,
                    Name = memberInfo.Name,
                    IsIncluded = include,
                    IsKeyAttribute = key,
                    BinarySerializer = serializer,
                    MemberInfo = memberInfo
                };

                if (memberInfo is FieldInfo)
                {
                    var getter = GenerateGetter<T>(memberInfo);
                    var setter = GenerateSetter<T>(memberInfo);
                    newMemberData.GetDelegate = getter;
                    newMemberData.SetDelegate = setter;
                }
                else
                {
                    var getter = GenerateGetter<T>(memberInfo);
                    var setter = GenerateSetter<T>(memberInfo);
                    newMemberData.GetDelegate = getter;
                    newMemberData.SetDelegate = setter;
                }

                schema.AddMemberData(newMemberData);
            }

            return schema;
        }
        public static Action<T, object> GenerateSetter<T>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var value = Expression.Parameter(typeof(object), "value");
            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var memberType = memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
            };
            var convert = Expression.Convert(value, memberType);
            var assign = Expression.Assign(member, convert);

            return Expression.Lambda<Action<T, object>>(assign, instance, value).Compile();
        }

        public static Func<T, object> GenerateGetter<T>(MemberInfo memberInfo)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var member = Expression.MakeMemberAccess(instance, memberInfo);
            var convert = Expression.Convert(member, typeof(object));

            return Expression.Lambda<Func<T, object>>(convert, instance).Compile();
        }
    }
}