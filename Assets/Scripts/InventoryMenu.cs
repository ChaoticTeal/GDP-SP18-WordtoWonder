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
    private GameObject abilityPanel;

    [SerializeField]
    private GameObject menuItemPrefab;

    [SerializeField]
    Transform inventoryItemListPanel;

    [SerializeField]
    Transform abilityListPanel;

    [SerializeField]
    Text descriptionAreaText;

    [SerializeField]
    Text abilityDescriptionAreaText;

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
    [Tooltip("List of the total pieces required for each ability type to gain the ability.")]
    [SerializeField]
    List<int> abilityTotals;
    [Tooltip("Final word used for each ability.")]
    [SerializeField]
    List<string> abilityNames;
    [Tooltip("Description of each ability.")]
    [SerializeField]
    List<string> abilityDescriptions;
    [Tooltip("Word collect popup.")]
    [SerializeField]
    GameObject announcementBox;

    PuzzleText activeText;

    NPCInteraction activeNPC;
    bool checkPuzzle;
    private List<GameObject> menuItems;
    private string defaultDescriptionText;

    bool canPause_UseProperty = true;

    public bool CanPause
    {
        get
        {
            return canPause_UseProperty;
        }
        set
        {
            canPause_UseProperty = value;
        }
    }

    public static event Action<int> UnlockAbility;

    public List<IInventoryText> PlayerInventory { get; private set; }
    public List<AbilityText> AbilityInventory { get; private set; }

    bool IsVisible
    {
        get
        {
            if(inventoryMenuPanel.activeSelf || abilityPanel.activeSelf)
                return true;
            return false;
        }
    }

    public int ActiveMenu
    {
        get
        {
            if (inventoryMenuPanel.activeSelf)
                return 0;
            else if (abilityPanel.activeSelf)
                return 1;
            else
                return -1;
        }
    }

    public void UpdateDescriptionAreaText(string descriptionText, IInventoryText objectRepresented, int type)
    {
        switch (type)
        {
            case 0:
                activeText = objectRepresented as PuzzleText;
                descriptionAreaText.text = descriptionText;
                break;
            case 1:
                abilityDescriptionAreaText.text = descriptionText;
                break;
        }
    }

    // Use Awake for initialization
    // Have to use Awake here because it happens before Start.
    // Since other objects need to read PlayerInventory in Start when they initialize,
    // If this hasn't happened yet, inventoryMenu will be null when they try to read!
    private void Awake()
    {
        defaultDescriptionText = descriptionAreaText.text;
        PlayerInventory = new List<IInventoryText>();
        AbilityInventory = new List<AbilityText>();
        menuItems = new List<GameObject>();
        HideMenu();
    }
    // Update is called once per frame
    void Update () 
	{
        HandleInput();
        if (!canPause_UseProperty)
            HideMenu();
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
        if (Input.GetButtonDown("Cancel") && CanPause)
        {
            if (IsVisible)
            {
                HideMenu();
            }
            else
            {
                ShowMenu(0);
            }
            UpdatePlayer();
        }
    }

    private void ShowMenu(int type)
    {
        UpdateDescriptionAreaText(defaultDescriptionText, null, type);
        GenerateMenuItems(type);
        switch (type)
        {
            case 0:
                inventoryMenuPanel.SetActive(true);
                useButton.SetActive(checkPuzzle);
                break;
            case 1:
                abilityPanel.SetActive(true);
                break;
        }
        
    }

    private void GenerateMenuItems(int type)
    {
        switch (type)
        {
            case 0:
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
                break;
            case 1:
                foreach (AbilityText item in AbilityInventory)
                {
                    GameObject newMenuItem = Instantiate(menuItemPrefab, abilityListPanel) as GameObject;

                    // Set the toggle group so only one item at a time can be selected
                    newMenuItem.GetComponent<Toggle>().group = abilityListPanel.GetComponent<ToggleGroup>();

                    // Set the toggle label name text (it's on a child gameobject of the toggle)
                    newMenuItem.GetComponentInChildren<Text>().text = item.DisplayText;

                    // Tell the menu item what object it is representing
                    newMenuItem.GetComponent<InventoryMenuItem>().InventoryObjectRepresented = item;

                    menuItems.Add(newMenuItem);
                }
                break;
        }
    }

    private void UpdatePlayer()
    {
        player.CanControl = !IsVisible;
    }

    private void HideMenu()
    {
        if (inventoryMenuPanel.activeSelf)
            inventoryMenuPanel.SetActive(false);
        else if (abilityPanel.activeSelf)
            abilityPanel.SetActive(false);
        DestroyInventoryMenuItems();
    }

    private void DestroyInventoryMenuItems()
    {
        foreach (var item in menuItems)
        {
            Destroy(item);
        }
    }

    public void AddAbilityText(AbilityText abilityText)
    {
        GameObject popup = Instantiate(announcementBox, gameObject.GetComponentInParent<Transform>());
        int newType = abilityText.TextType;
        int totalOfType = 0;
        bool abilityUpdated = false;
        AbilityInventory.Add(abilityText);
        foreach (AbilityText a in AbilityInventory)
        {
            if (a.TextType == newType)
                totalOfType++;
        }
        if (totalOfType >= abilityTotals[newType])
        {
            if (UnlockAbility != null)
                UnlockAbility.Invoke(newType);
            for (int i = 0; i < AbilityInventory.Count; i++)
            {
                if (AbilityInventory[i].TextType == newType)
                {
                    if (!abilityUpdated)
                    {
                        AbilityInventory[i].BaseText = abilityNames[newType];
                        AbilityInventory[i].DescriptionText = abilityDescriptions[newType];
                        abilityUpdated = true;
                        popup.GetComponentInChildren<Text>().text = "You have unlocked " + AbilityInventory[i].DisplayText;
                    }
                    else
                    {
                        AbilityInventory.Remove(AbilityInventory[i]);
                        i--;
                    }

                }
            }
        }
        else
            popup.GetComponentInChildren<Text>().text = "You got " + abilityText.DisplayText;
    }

    public void AddInventoryText(IInventoryText inventoryText)
    {
        PlayerInventory.Add(inventoryText);
        GameObject popup = Instantiate(announcementBox, gameObject.GetComponentInParent<Transform>());
        popup.GetComponentInChildren<Text>().text = "You got " + inventoryText.DisplayText;
    }

    public void UseButton()
    {
        if (activeText != null)
        {
            if (activeText.TextType == activeNPC.SolutionType)
            {
                if (activeText.TextIndex == activeNPC.SolutionIndex)
                    activeNPC.PuzzleSolved = 2;
                else
                    activeNPC.PuzzleSolved = 1;
                HideMenu();
                activeNPC.Interact();
                if (activeNPC.abilityReward != null)
                {
                    activeNPC.abilityReward.TextEffect();
                    AddAbilityText(activeNPC.abilityReward);
                }
                player.CanControl = false;
                PlayerInventory.Remove(activeText);
            }
            else
                descriptionAreaText.text = "That doesn't seem right.";
        }
    }

    public void SwitchPanels(int type)
    {
        HideMenu();
        switch (type)
        {
            case 0:
                ShowMenu(1);
                break;
            case 1:
                ShowMenu(0);
                break;
        }
    }
}