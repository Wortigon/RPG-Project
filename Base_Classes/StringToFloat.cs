using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringToFloat
{
    private static readonly NumberFormatInfo NumberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };

    public static float Convert(string input)
    {
        float output;
        if (!float.TryParse(input, NumberStyles.Float, NumberFormatInfo, out output))
        {
            UnityEngine.Debug.LogError($"Failed to convert '{input}' to float.");
        }
        return output;
    }
}
