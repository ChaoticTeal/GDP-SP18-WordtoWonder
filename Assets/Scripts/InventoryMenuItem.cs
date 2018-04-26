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
        // Update the description area text!
        inventoryMenu.UpdateDescriptionAreaText(InventoryObjectRepresented.BaseText, InventoryObjectRepresented);
        if(audioSource != null)
            audioSource.Play();
    }
}