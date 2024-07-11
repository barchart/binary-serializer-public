using System.Text;
using Microsoft.Extensions.Primitives;

namespace Barchart.BinarySerializer.Examples.Common;

public static class Helper
{
    public static void WriteStep(ref int counter, string message)
    {
        Console.WriteLine($"{counter++}. {message}");
    }

    public static void WriteDetails(string message)
    {
        var builder = new StringBuilder();

        builder.AppendLine();
        builder.Append('\t');
        builder.Append(message);
        builder.AppendLine();
        
        Console.WriteLine(builder.ToString());
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
        
        Console.WriteLine(builder.ToString());
    }
    
    public static string PrintBits(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }
}