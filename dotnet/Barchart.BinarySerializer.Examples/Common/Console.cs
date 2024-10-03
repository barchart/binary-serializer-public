#region Using Statements

using System.Text;

#endregion

namespace Barchart.BinarySerializer.Examples.Common;

/// <summary>
///     Provides custom console output methods for the binary serializer examples.
/// </summary>
public static class Console
{
    #region Methods

    /// <summary>
    ///     Writes a blank line to the console.
    /// </summary>
    public static void WriteLine()
    {
        System.Console.WriteLine();
    }
    
    /// <summary>
    ///     Writes the specified message followed by a line terminator to the console.
    /// </summary>
    /// <param name="message">
    ///     The message to write.
    /// </param>
    public static void WriteLine(string message)
    {
        System.Console.WriteLine(message);
    }
    
    /// <summary>
    ///     Writes a numbered step followed by the specified message to the console.
    /// </summary>
    /// <param name="counter">
    ///     The current step counter.
    /// </param>
    /// <param name="message">
    ///     The message to write.
    /// </param>
    public static void WriteStep(ref int counter, string message)
    {
        System.Console.WriteLine($"{counter++}. {message}");
    }

    /// <summary>
    ///     Writes detailed information to the console, optionally including line breaks before and after the message.
    /// </summary>
    /// <param name="message">
    ///     The detailed message to write.
    /// </param>
    /// <param name="lineBreakBefore">
    ///     Whether to include a line break before the message.
    /// </param>
    /// <param name="lineBreakAfter">
    ///     Whether to include a line break after the message.
    /// </param>
    public static void WriteDetails(string message, bool lineBreakBefore = true, bool lineBreakAfter = true)
    {
        StringBuilder builder = new();

        if (lineBreakBefore)
        {
            builder.AppendLine();
        }

        builder.Append('\t');
        builder.Append(message);

        if (lineBreakAfter)
        {
            builder.AppendLine();
        }

        System.Console.WriteLine(builder.ToString());
    }
    
    /// <summary>
    ///     Writes the binary representation of the specified byte array to the console.
    /// </summary>
    /// <param name="bytes">
    ///     The byte array to write.
    /// </param>
    public static void WriteDetails(byte[] bytes)
    {
        StringBuilder builder = new();
        
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

    #endregion
}