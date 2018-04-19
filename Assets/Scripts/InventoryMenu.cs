using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour 
{
    [SerializeField]
    private GameObject inventoryMenuPanel;

    [SerializeField]
    private GameObject menuItemPrefab;

    [SerializeField]
    Transform inventoryItemListPanel;

    [SerializeField]
    Text descriptionAreaText;

    private List<GameObject> menuItems;
    private string defaultDescriptionText;

    public bool canPause = true;

    public List<IInventoryText> PlayerInventory { get; private set; }

    bool IsVisible
    {
        get { return inventoryMenuPanel.activeSelf; }
    }

    public void UpdateDescriptionAreaText(string descriptionText)
    {
        descriptionAreaText.text = descriptionText;
    }

    // Use Awake for initialization
    // Have to use Awake here because it happens before Start.
    // Since other objects need to read PlayerInventory in Start when they initialize,
    // If this hasn't happened yet, inventoryMenu will be null when they try to read!
    private void Awake()
    {
        defaultDescriptionText = descriptionAreaText.text;
        PlayerInventory = new List<IInventoryText>();
        menuItems = new List<GameObject>();
        HideMenu();
        UpdateCursor();
    }
    // Update is called once per frame
    void Update () 
	{
        HandleInput();
        if (!canPause)
            HideMenu();
        // It seems if you don't do this every frame, the cursor is not locked properly...
        UpdateCursor();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Cancel") && canPause)
        {
            Debug.Log("Pause");
            if (IsVisible)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }            
            UpdateFirstPersonController();
        }
    }

    private void ShowMenu()
    {
        UpdateDescriptionAreaText(defaultDescriptionText);
        GenerateMenuItems();
        inventoryMenuPanel.SetActive(true);
    }

    private void GenerateMenuItems()
    {
        foreach (IInventoryText item in PlayerInventory)
        {
            GameObject newMenuItem = Instantiate(menuItemPrefab, inventoryItemListPanel) as GameObject;

            // Set the toggle group so only one item at a time can be selected
            newMenuItem.GetComponent<Toggle>().group = inventoryItemListPanel.GetComponent<ToggleGroup>();

            // Set the toggle label name text (it's on a child gameobject of the toggle)
            newMenuItem.GetComponentInChildren<Text>().text = item.DisplayText;

            // Tell the menu item what object it is representing
            newMenuItem.GetComponent<InventoryMenuItem>().InventoryObjectRepresented = item;

            menuItems.Add(newMenuItem);
        }
    }

    private void UpdateFirstPersonController()
    {

    }

    private void HideMenu()
    {
        inventoryMenuPanel.SetActive(false);
        DestroyInventoryMenuItems();
    }

    private void DestroyInventoryMenuItems()
    {
        foreach (var item in menuItems)
        {
            Destroy(item);
        }
    }

    private void UpdateCursor()
    {
        if (IsVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}