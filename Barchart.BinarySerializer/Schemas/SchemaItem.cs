#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents metadata about a member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">
    ///     The type which contains the field (or property).
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The type of the field (or property).
    /// </typeparam>
    public class SchemaItem<TContainer, TValue> where TContainer : new()
    {
        #region Fields

        private readonly string _name;
        
        private readonly bool _key;

        private readonly Func<TContainer, TValue> _getter;
        private readonly Action<TContainer, TValue> _setter;

        private readonly IBinaryTypeSerializer<TValue> _serializer;
        
        #endregion

        #region Constructor(s)

        public SchemaItem(string name, bool key, Func<TContainer, TValue> getter, Action<TContainer, TValue> setter, IBinaryTypeSerializer<TValue> serializer)
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
        
        public void Encode(IDataBufferWriter buffer, TContainer source) 
        {
            _serializer.Encode(buffer, _getter(source));
        }
        
        public void Decode(IDataBufferReader buffer, TContainer target)
        {
            _setter(target, _serializer.Decode(buffer));
        }

        public bool GetEquals(TContainer a, TContainer b)
        {
            return _serializer.GetEquals(_getter(a), _getter(b));
        }
        
        #endregion
    }
}