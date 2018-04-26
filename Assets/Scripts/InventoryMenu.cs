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

    [SerializeField]
    Transform npcRange;
    /// <summary>
    /// Interactive layer
    /// </summary>
    [Tooltip("Interactive layer.")]
    [SerializeField]
    LayerMask interactible;
    /// <summary>
    /// Radius of interaction
    /// </summary>
    [Tooltip("Radius of interaction.")]
    [SerializeField]
    float interactionRangeRadius = .5f;
    [SerializeField]
    GameObject useButton;
    [SerializeField]
    PlayerMovement player;

    PuzzleText activeText;

    NPCInteraction activeNPC;
    bool checkPuzzle;
    private List<GameObject> menuItems;
    private string defaultDescriptionText;

    public bool canPause = true;

    public List<IInventoryText> PlayerInventory { get; private set; }

    bool IsVisible
    {
        get { return inventoryMenuPanel.activeSelf; }
    }

    public void UpdateDescriptionAreaText(string descriptionText, IInventoryText objectRepresented)
    {
        activeText = objectRepresented as PuzzleText;
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
        Interact();
    }

    // Interact
    void Interact()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(npcRange.position, interactionRangeRadius, interactible);
        if (colliders.Length > 0)
            foreach (Collider2D c in colliders)
            {
                if (c.gameObject.GetComponent<NPCInteraction>() != null)
                {
                    activeNPC = c.GetComponent<NPCInteraction>();
                    if (activeNPC.PuzzleSolved == 0)
                        checkPuzzle = true;
                    else
                        checkPuzzle = false;
                }
            }
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Cancel") && canPause)
        {
            if (IsVisible)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
            UpdatePlayer();
        }
    }

    private void ShowMenu()
    {
        UpdateDescriptionAreaText(defaultDescriptionText, null);
        GenerateMenuItems();
        inventoryMenuPanel.SetActive(true);
        useButton.SetActive(checkPuzzle);
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

    private void UpdatePlayer()
    {
        player.CanControl = !IsVisible;
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

    public void UseButton()
    {
        if (activeText.TextType == activeNPC.SolutionType)
        {
            if (activeText.TextIndex == activeNPC.SolutionType)
                activeNPC.PuzzleSolved = 2;
            else
                activeNPC.PuzzleSolved = 1;
            HideMenu();
            activeNPC.Interact();
            player.CanControl = false;
            PlayerInventory.Remove(activeText);
        }
        else
            descriptionAreaText.text = "That doesn't seem right.";
    }
}