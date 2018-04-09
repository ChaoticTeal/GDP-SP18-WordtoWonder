using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("What the NPC says without highlighted text.")]
    [SerializeField]
    string dialogueText_NoPuzzle;
    [Tooltip("What the NPC says with highlighted text.")]
    [SerializeField]
    string dialogueText_Puzzle;
    [Tooltip("Dialogue box prefab.")]
    [SerializeField]
    GameObject dialogueBox;
    [Tooltip("UI canvas.")]
    [SerializeField]
    Canvas canvas;
    [Tooltip("Value of puzzle-type text (if present).")]
    [SerializeField]
    string puzzleTextValue;

    // Private fields
    /// <summary>
    /// The active camera
    /// </summary>
    Camera camera;
    /// <summary>
    /// Position of the NPC in screen space
    /// </summary>
    Vector3 screenPosition;
    /// <summary>
    /// Puzzle-type text (if present).
    /// </summary>
    public PuzzleText puzzleText = null;

    // Properties
    /// <summary>
    /// Active dialogue box
    /// </summary>
    public GameObject ActiveDialogueBox { get; private set; }
    /// <summary>
    /// Is there unobtained text?
    /// </summary>
    public bool TextCollected
    {
        get
        {
            if (puzzleText != null)
                return puzzleText.TextCollected;
            return true;
        }
    }
    /// <summary>
    /// Text to use in dialogue box.
    /// </summary>
    string DialogueText
    {
        get
        {
            if (puzzleTextValue != "" && puzzleText.TextCollected == false)
                return string.Format(dialogueText_Puzzle, puzzleText.DisplayText);
            else
                return dialogueText_NoPuzzle;
        }
    }

	// Use this for initialization
	void Start () 
	{
        camera = FindObjectOfType<Camera>();
        if (puzzleTextValue != "")
            puzzleText = new PuzzleText(puzzleTextValue);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    /// <summary>
    /// Action taken when the player interacts with the NPC
    /// </summary>
    public void Interact()
    {
        // If there isn't a dialogue box, make one
        if (ActiveDialogueBox == null)
        {
            // Get position on screen to determine position of box
            screenPosition = camera.WorldToScreenPoint(transform.position);
            // Instantiate the dialogue box
            ActiveDialogueBox = Instantiate(dialogueBox, canvas.transform);
            // Set the text
            dialogueBox.GetComponentInChildren<Text>().text = DialogueText;
            // If the NPC is on the bottom half of the screen, put the box on the top
            if (screenPosition.y < camera.pixelHeight / 2)
                dialogueBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 25f, dialogueBox.GetComponent<RectTransform>().sizeDelta.y);
            // Otherwise, put it on the bottom
            else
                dialogueBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 25f, dialogueBox.GetComponent<RectTransform>().sizeDelta.y);
        }
        // If there is a dialogue box, get rid of it
        else
            Destroy(ActiveDialogueBox);
    }
}
