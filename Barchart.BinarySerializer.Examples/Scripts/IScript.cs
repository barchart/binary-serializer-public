namespace Barchart.BinarySerializer.Examples.Modes;

public interface IScript
{
    Script Script { get; }
    
    void Execute();
}