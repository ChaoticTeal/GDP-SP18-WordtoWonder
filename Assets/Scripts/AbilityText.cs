using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityText : IInventoryText
{
    // SerializeFields
    [Tooltip("Index of the text within its category.")]
    [SerializeField]
    int textIndex_UseProperty;
    [Tooltip("Category of the text.\n0 for glide.")]
    [SerializeField]
    int textType_UseProperty;
    [Tooltip("The actual text value.")]
    [SerializeField]
    string baseText_UseProperty;
    [Tooltip("Description text.")]
    [SerializeField]
    string descriptionText_UseProperty;

    public string DisplayText
    {
        get
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(TextColor) + ">" + BaseText + "</color>";
        }
    }

    public string DescriptionText
    {
        get
        {
            return descriptionText_UseProperty;
        }
        set
        {
            descriptionText_UseProperty = value;
        }
    }

    public int TextIndex
    {
        get
        {
            return textIndex_UseProperty;
        }
    }

    public int TextType
    {
        get
        {
            return textType_UseProperty;
        }
    }

    public bool TextCollected { get; set; }
    public Color TextColor
    {
        get
        {
            return Color.green;
        }
    }
    public string BaseText
    {
        get
        {
            return baseText_UseProperty;
        }
        set
        {
            baseText_UseProperty = value;
        }
    }


    public void TextEffect()
    {
        TextCollected = true;
    }
}
