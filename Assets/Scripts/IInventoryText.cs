using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryText
{
    /// <summary>
    /// The full string of text to add to the dialogue
    /// </summary>
    string DisplayText { get; }

    /// <summary>
    /// Has the text been collected?
    /// </summary>
    bool TextCollected { get; set; }

    /// <summary>
    /// Color with which to highlight text
    /// </summary>
    Color TextColor { get; }
    
    /// <summary>
    /// The text to use for the word
    /// </summary>
    string BaseText { get; set; }

    /// <summary>
    /// The action taken when the text is collected
    /// </summary>
    void TextEffect();
}
