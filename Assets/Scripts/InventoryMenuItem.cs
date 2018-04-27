using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenuItem : MonoBehaviour 
{
    private InventoryMenu inventoryMenu;
    private AudioSource audioSource;

    public IInventoryText InventoryObjectRepresented { get; set; }

    void Start () 
	{
        inventoryMenu = FindObjectOfType<InventoryMenu>();
        audioSource = GetComponent<AudioSource>();
	}

    public void OnValueChanged()
    {
        string descriptionText;
        if (InventoryObjectRepresented is PuzzleText)
            descriptionText = ((PuzzleText)InventoryObjectRepresented).DescriptionText;
        else if (InventoryObjectRepresented is AbilityText)
            descriptionText = ((AbilityText)InventoryObjectRepresented).DescriptionText;
        else
            descriptionText = InventoryObjectRepresented.BaseText;
        // Update the description area text!
        
        inventoryMenu.UpdateDescriptionAreaText(descriptionText, InventoryObjectRepresented, inventoryMenu.ActiveMenu);
        if(audioSource != null)
            audioSource.Play();
    }
}