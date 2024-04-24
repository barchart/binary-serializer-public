namespace Barchart.SerializationData
{
    public interface IMemberConverter<T, V>
    {
        public V Convert(T obj);

        public T Invert(V obj);
    }
}
