#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     An interface that describes how to read a data point, usually
///     from an instance property, serialize the data point, and then
///     write the serialized data to binary data storage (and vice versa).
/// </summary>
/// <typeparam name="TEntity">
///     The source of the data point (when serializing) and the assignment
///     target (when deserializing).
/// </typeparam>
public interface ISchemaItem<TEntity> where TEntity: new()
{
    #region Properties

    /// <summary>
    ///     Indicates if the data point is the primary key of the
    ///     source object (or a member of a composite key).
    /// </summary>
    bool Key { get; }
    
    /// <summary>
    ///     The name of the member (property, field, etc) from which data is
    ///     read (or to which deserialized data). This name is used for
    ///     display purposes only.
    /// </summary>
    string Name { get; }

    #endregion
    
    #region Methods

    /// <summary>
    ///     Reads data from the source object, serializes that data to binary,
    ///     and writes the serialized data to the binary data storage.
    /// </summary>
    /// <param name="source">
    ///     The object to read data from.
    /// </param>
    /// <param name="writer">
    ///     Writable binary data storage.
    /// </param>
    void Encode(TEntity source, IDataBufferWriter writer);
    
    /// <summary>
    ///     Reads data from the binary data storage, deserializes that data,
    ///     and assigns the deserialized data to the source (target) object.
    /// </summary>
    /// <param name="target">
    ///     The object to assign data to.
    /// </param>
    /// <param name="reader">
    ///     Writable binary data storage.
    /// </param>
    /// <param name="existing">
    ///     Indicates if the <paramref name="target" /> is an existing object.
    /// </param>
    void Decode(TEntity target, IDataBufferReader reader, bool existing = false);
    
    /// <summary>
    ///     Indicates whether two data points are equal.
    /// </summary>
    /// <param name="a">
    ///     The first data point.
    /// </param>
    /// <param name="b">
    ///     The second data point.
    /// </param>
    /// <returns>
    ///     True if the two data points are equal; otherwise false.
    /// </returns>
    bool GetEquals(TEntity a, TEntity b);

    #endregion
}