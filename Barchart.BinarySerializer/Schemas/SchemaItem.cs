#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Information regarding a single piece of data that can be serialized
    ///     to a binary storage (or deserialized from binary data storage).
    /// </summary>
    /// <typeparam name="TSource">
    ///     The type which contains the data to be serialized. In other words,
    ///     this is the source of data being serialized (or the assignment
    ///     target of data being deserialized).
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The type of data being serialized (which is read from the source
    ///     object) or deserialized (which assigned to the source object).
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
        
        /// <summary>
        ///     The name of the member (property, field, etc) which data to serialize
        ///     is read from (or deserialized data is assigned to). This name is used
        ///     for display purposes only.
        /// </summary>
        public string Name => _name;

        /// <summary>
        ///     Indicates if the member (property, field, etc) participates in the
        ///     key relationship (as a single key or part of a composite key).
        /// </summary>
        public bool Key => _key;
        
        #endregion

        #region Methods
        
        /// <summary>
        ///     Reads data from the source object and writes it to the binary data storage.
        /// </summary>
        /// <param name="buffer">
        ///     Writable binary data storage.
        /// </param>
        /// <param name="source">
        ///     The object to read data from.
        /// </param>
        public void Encode(IDataBufferWriter buffer, TSource source) 
        {
            _serializer.Encode(buffer, _getter(source));
        }
        
        /// <summary>
        ///     Reads dat from the binary data storage and assigns it to the source (target)
        ///     object.
        /// </summary>
        /// <param name="buffer">
        ///     Writable binary data storage.
        /// </param>
        /// <param name="target">
        ///     The object to assign data to.
        /// </param>
        public void Decode(IDataBufferReader buffer, TSource target)
        {
            _setter(target, _serializer.Decode(buffer));
        }

        /// <summary>
        ///     Indicates whether two values are equal. Presence of this
        ///     method prevents the need for boxing since the type parameter
        ///     is not constrained to <see cref="IEquatable{T}" />.
        /// </summary>
        /// <param name="a">
        ///     The first value.
        /// </param>
        /// <param name="b">
        ///     The second value.
        /// </param>
        /// <returns>
        ///     True if the two values are equal; otherwise false.
        /// </returns>
        public bool GetEquals(TSource a, TSource b)
        {
            return _serializer.GetEquals(_getter(a), _getter(b));
        }
        
        #endregion
    }
}