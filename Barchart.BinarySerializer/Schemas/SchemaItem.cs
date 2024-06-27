#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents metadata about a member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The type which contains the field (or property) being serialized. In other words,
    ///     this is the source of data being serialized (or the assignment target of data
    ///     being deserialized).
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The type of data being serialized (which is read from the source object) or
    ///     deserialized (which assigned to the source object).
    /// </typeparam>
    public class SchemaItem<TSource, TValue> where TSource : new()
    {
        #region Fields

        private readonly string _name;
        
        private readonly bool _key;

        private readonly Func<TSource, TValue> _getter;
        private readonly Action<TSource, TValue> _setter;

        private readonly IBinaryTypeSerializer<TValue> _serializer;
        
        #endregion

        #region Constructor(s)

        public SchemaItem(string name, bool key, Func<TSource, TValue> getter, Action<TSource, TValue> setter, IBinaryTypeSerializer<TValue> serializer)
        {
            _name = name;
            _key = key;

            _getter = getter;
            _setter = setter;
            
            _serializer = serializer;
        }

        #endregion
        
        #region Properties
        
        public string Name => _name;

        public bool Key => _key;
        
        #endregion

        #region Methods
        
        public void Encode(IDataBufferWriter buffer, TSource source) 
        {
            _serializer.Encode(buffer, _getter(source));
        }
        
        public void Decode(IDataBufferReader buffer, TSource target)
        {
            _setter(target, _serializer.Decode(buffer));
        }

        public bool GetEquals(TSource a, TSource b)
        {
            return _serializer.GetEquals(_getter(a), _getter(b));
        }
        
        #endregion
    }
}