namespace Barchart.BinarySerializer.Buffers;

public interface IDataBuffer
{
    bool ReadBit();
    byte ReadByte();
    byte[] ReadBytes(int size);
    
    void WriteBit(bool value);
    void WriteByte(byte value);
    void WriteBytes(byte[] value);

    byte[] ToBytes();
}