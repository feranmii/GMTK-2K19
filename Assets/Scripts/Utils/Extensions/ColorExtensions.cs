using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions
{
    public static Color ConvertRGBAToColor(string targetColor)
    {
        Color retVal;

        ColorUtility.TryParseHtmlString(targetColor, out retVal);
        return retVal;
    }
}
