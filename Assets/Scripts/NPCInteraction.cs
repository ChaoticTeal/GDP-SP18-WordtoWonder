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
    [Tooltip("Dialogue box prefab.")]
    [SerializeField]
    GameObject dialogueBox;
    [Tooltip("UI canvas.")]
    [SerializeField]
    Canvas canvas;

    [Header("Puzzle elements")]
    [Tooltip("Ability fragment given as a reward (if any.)")]
    [SerializeField]
    public AbilityText abilityReward;
    [Tooltip("Type of word which works as a solution.\n0 for food.")]
    [SerializeField]
    int solutionType_UseProperty;
    [Tooltip("Index of 'best' solution with a unique response/reward.")]
    [SerializeField]
    int bestSolutionIndex_UseProperty;
    [Tooltip("The puzzle-type text, if present.")]
    [SerializeField]
    public PuzzleText puzzleText;
    [Tooltip("Best solution text.")]
    [SerializeField]
    string dialogueText_BestSolution;
    [Tooltip("Problem text.")]
    [SerializeField]
    string dialogueText_Problem;
    [Tooltip("Problem solved text.")]
    [SerializeField]
    string dialogueText_ProblemSolved;
    [Tooltip("What the NPC says with highlighted text.")]
    [SerializeField]
    string dialogueText_Puzzle;


    // Private fields
    /// <summary>
    /// The active camera
    /// </summary>
    Camera camera;
    /// <summary>
    /// Position of the NPC in screen space
    /// </summary>
    Vector3 screenPosition;

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
            if (dialogueText_Problem != "")
            {
                switch(PuzzleSolved)
                {
                    case 0:
                        return dialogueText_Problem;
                    case 1:
                        return dialogueText_ProblemSolved;
                    case 2:
                        return dialogueText_BestSolution;
                    default:
                        return dialogueText_Problem;
                }
            }
            else if (puzzleText.BaseText != "" && !TextCollected)
                return string.Format(dialogueText_Puzzle, puzzleText.DisplayText);
            else
                return dialogueText_NoPuzzle;
        }
    }
    /// <summary>
    /// Is there an unsolved puzzle?
    /// </summary>
    public int PuzzleSolved { get; set; }
    /// <summary>
    /// Solution type
    /// </summary>
    public int SolutionType
    {
        get { return solutionType_UseProperty; }
    }

    /// <summary>
    /// Best solution index
    /// </summary>
    public int SolutionIndex
    {
        get { return bestSolutionIndex_UseProperty; }
    }


	// Use this for initialization
	void Start () 
	{
        camera = FindObjectOfType<Camera>();
        if (puzzleText.BaseText == "")
            puzzleText = null;
        if (abilityReward.BaseText == "")
            abilityReward = null;
        if (dialogueText_Problem == "")
            PuzzleSolved = 1;
        else
            PuzzleSolved = 0;
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
            ActiveDialogueBox.GetComponentInChildren<Text>().text = DialogueText;
            // If the NPC is on the bottom half of the screen, put the box on the top
            if (screenPosition.y < camera.pixelHeight / 2)
                ActiveDialogueBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 25f, dialogueBox.GetComponent<RectTransform>().sizeDelta.y);
            // Otherwise, put it on the bottom
            else
                ActiveDialogueBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 25f, dialogueBox.GetComponent<RectTransform>().sizeDelta.y);
        }
        // If there is a dialogue box, get rid of it
        else
            Destroy(ActiveDialogueBox);
    }
}
