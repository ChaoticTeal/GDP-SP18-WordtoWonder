using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleText : IInventoryText
{
    public string DisplayText
    {
        get
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(TextColor) + ">" + BaseText + "</color>";
        }
    }

    public bool TextCollected { get; set; }
    public Color TextColor
    {
        get
        {
            return Color.blue;
        }
    }
    public string BaseText { get; set; }


    public void TextEffect()
    {
        TextCollected = true;
    }

    public PuzzleText(string text)
    {
        BaseText = text;
    }
}
