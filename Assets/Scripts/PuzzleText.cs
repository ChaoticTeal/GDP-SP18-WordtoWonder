using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleText : IInventoryText
{
    // SerializeFields
    [Tooltip("Index of the text within its category.")]
    [SerializeField]
    int textIndex;
    [Tooltip("Category of the text.\n0 for food.")]
    [SerializeField]
    int textType;
    [Tooltip("The actual text value.")]
    [SerializeField]
    string baseText_UseProperty;

    public static event Action<int, int> OnCollected;

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
    public string BaseText
    {
        get
        {
            return baseText_UseProperty;
        }
    }


    public void TextEffect()
    {
        TextCollected = true;
    }
}
