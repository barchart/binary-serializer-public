using System.Text;

namespace Barchart.BinarySerializer.Examples.Common;

public static class Helper
{
    public static string PrintBits(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }
}