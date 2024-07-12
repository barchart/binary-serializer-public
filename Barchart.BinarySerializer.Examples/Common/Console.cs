using System.Text;

namespace Barchart.BinarySerializer.Examples.Common;

public static class Console
{
    public static void WriteLine()
    {
        System.Console.WriteLine();
    }
    
    public static void WriteLine(string message)
    {
        System.Console.WriteLine(message);
    }
    
    public static void WriteStep(ref int counter, string message)
    {
        System.Console.WriteLine($"{counter++}. {message}");
    }

    public static void WriteDetails(string message)
    {
        var builder = new StringBuilder();

        builder.AppendLine();
        builder.Append('\t');
        builder.Append(message);
        builder.AppendLine();
        
        System.Console.WriteLine(builder.ToString());
    }
    
    public static void WriteDetails(byte[] bytes)
    {
        var builder = new StringBuilder();
        
        if (bytes.Length == 0)
        {
            builder.AppendLine(); 
            builder.Append('\t');
            builder.Append("[Empty Byte Array]");
        }
        else
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 4 == 0)
                {
                    builder.AppendLine();
                    builder.Append('\t');
                }
            
                builder.Append(Helper.PrintBits(bytes[i]));
                builder.Append(' ');
            }
        }
        
        builder.AppendLine();
        
        System.Console.WriteLine(builder.ToString());
    }
}