namespace Barchart.BinarySerializer.Examples.Scripts;

public interface IScript
{
    Script Script { get; }
    
    void Execute();
}