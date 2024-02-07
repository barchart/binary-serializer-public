

using JerqAggregatorNew.Types;
using System.Reflection;

namespace JerqAggregatorNew.Schemas
{
    public static class SchemaFactory
    {
     
        private static readonly Dictionary<Type, ISerializer> allSerializers = new Dictionary<Type, ISerializer>();
        static SchemaFactory()
        {
            allSerializers.Add(typeof(String), new BinarySerializerString());

            allSerializers.Add(typeof(DateTime?), new BinarySerializerNumeric<DateTime>());
            allSerializers.Add(typeof(DateTime), allSerializers[typeof(DateTime?)]);
            allSerializers.Add(typeof(Decimal?), new BinarySerializerNumeric<decimal>());
            allSerializers.Add(typeof(Decimal), allSerializers[typeof(Decimal?)]);
            allSerializers.Add(typeof(Boolean?), new BinarySerializerNumeric<bool>());
            allSerializers.Add(typeof(Boolean), allSerializers[typeof(Boolean?)]);
            allSerializers.Add(typeof(Int32?), new BinarySerializerNumeric<int>());
            allSerializers.Add(typeof(Int32), allSerializers[typeof(Int32?)]);
            allSerializers.Add(typeof(Int16?), new BinarySerializerNumeric<short>());
            allSerializers.Add(typeof(Int16), allSerializers[typeof(Int16?)]);
            allSerializers.Add(typeof(SByte?), new BinarySerializerNumeric<sbyte>());
            allSerializers.Add(typeof(SByte), allSerializers[typeof(SByte?)]);
            allSerializers.Add(typeof(Byte?), new BinarySerializerNumeric<byte>());
            allSerializers.Add(typeof(Byte), allSerializers[typeof(Byte?)]);
            allSerializers.Add(typeof(Int64?), new BinarySerializerNumeric<long>());
            allSerializers.Add(typeof(Int64), allSerializers[typeof(Int64?)]);
            allSerializers.Add(typeof(UInt16?), new BinarySerializerNumeric<ushort>());
            allSerializers.Add(typeof(UInt16), allSerializers[typeof(UInt16?)]);
            allSerializers.Add(typeof(UInt32?), new BinarySerializerNumeric<uint>());
            allSerializers.Add(typeof(UInt32), allSerializers[typeof(UInt32?)]);
            allSerializers.Add(typeof(UInt64?), new BinarySerializerNumeric<ulong>());
            allSerializers.Add(typeof(UInt64), allSerializers[typeof(UInt64?)]);
            allSerializers.Add(typeof(Single?), new BinarySerializerNumeric<float>());
            allSerializers.Add(typeof(Single), allSerializers[typeof(Single?)]);
            allSerializers.Add(typeof(Double?), new BinarySerializerNumeric<double>());
            allSerializers.Add(typeof(Double), allSerializers[typeof(Double?)]);
            allSerializers.Add(typeof(Char?), new BinarySerializerNumeric<char>());
            allSerializers.Add(typeof(Char), allSerializers[typeof(Char?)]);
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

                if(memberInfo is FieldInfo)
                {
                    memberType = ((FieldInfo) memberInfo).FieldType;
                }
                else
                {
                    memberType = ((PropertyInfo)memberInfo).PropertyType;
                }

                BinarySerializeAttribute? attribute = (BinarySerializeAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(BinarySerializeAttribute));
                if(attribute == null) { continue; }

                bool include = attribute.Include;
                bool key = attribute.Key;

                ISerializer? serializer;
                allSerializers.TryGetValue(memberType, out serializer);

                if (serializer == null) { continue; }    
    
                MemberData newMemberData = new MemberData()
                {
                    Type = memberType,
                    Name = memberInfo.Name,
                    IsIncluded = include,
                    IsKeyAttribute = key,
                    BinarySerializer = serializer,
                    MemberInfo = memberInfo
                };
                schema.AddMemberData(newMemberData);
            }
         
            return schema;
        }
    }
}
