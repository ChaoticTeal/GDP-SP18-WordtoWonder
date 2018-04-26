using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Private fields
    /// <summary>
    /// The animator on the object
    /// </summary>
    Animator animator;
    /// <summary>
    /// Does the player have control?
    /// </summary>
    bool canControl_UseProperty;
    /// <summary>
    /// Has the player unlocked the ability to glide?
    /// </summary>
    bool canGlide;
    /// <summary>
    /// Is the player on the ground?
    /// </summary>
    bool isOnGround;
    /// <summary>
    /// Is the player trying to interact with a thing?
    /// </summary>
    bool shouldInteract;
    /// <summary>
    /// Should the player jump?
    /// </summary>
    bool shouldJump;
    /// <summary>
    /// Box collider 2D
    /// </summary>
    BoxCollider2D boxCol;
    /// <summary>
    /// Horizontal input check
    /// </summary>
    float horizontalInput;
    /// <summary>
    /// Rigidbody 2D
    /// </summary>
    Rigidbody2D rigidbody2D;
    /// <summary>
    /// Force added on jump
    /// </summary>
    Vector2 jumpForce;

    // SerializeFields
    /// <summary>
    /// Radius for ground detection circle
    /// </summary>
    [Tooltip("Radius of ground detection circle.")]
    [SerializeField]
    float groundDetectRadius = 0.25f;
    /// <summary>
    /// Radius of interaction
    /// </summary>
    [Tooltip("Radius of interaction.")]
    [SerializeField]
    float interactionRangeRadius = .5f;
    /// <summary>
    /// Strength of jump
    /// </summary>
    [Tooltip("Force added on jump.")]
    [SerializeField]
    float jumpStrength;
    /// <summary>
    /// Movement speed
    /// </summary>
    [Tooltip ("Movement speed.")]
    [SerializeField]
    float moveSpeed = 5.0f;
    [Tooltip("Inventory menu.")]
    [SerializeField]
    InventoryMenu inventoryMenu;
    /// <summary>
    /// Interactive layer
    /// </summary>
    [Tooltip("Interactive layer.")]
    [SerializeField]
    LayerMask interactible;
    /// <summary>
    /// Ground layer
    /// </summary>
    [Tooltip("Ground layer.")]
    [SerializeField]
    LayerMask whatCountsAsGround;
    /// <summary>
    /// Center of ground detection circle
    /// </summary>
    [Tooltip("Ground detection point.")]
    [SerializeField]
    Transform groundDetectPoint;
    /// <summary>
    /// Center of interaction circle
    /// </summary>
    [Tooltip("Center of interaction circle.")]
    [SerializeField]
    Transform interactPoint;
    [Tooltip("Gravity scale for gliding.")]
    [SerializeField]
    float glideGravity = .5f;
    [Tooltip("Base gravity scale.")]
    [SerializeField]
    float baseGravityScale = 2.5f;

    public bool CanControl
    {
        get { return canControl_UseProperty; }
        set { canControl_UseProperty = value; }
    }

    // Use this for initialization
    void Start ()
    {
        jumpForce = new Vector2(0, jumpStrength);
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = baseGravityScale;
        boxCol = GetComponent<BoxCollider2D>();
        CanControl = true;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetJumpInput();
        GetMoveInput();
        GetInteractInput();
        UpdateIsOnGround();
        Interact();
        if (!CanControl)
            GetText();
    }

    private void FixedUpdate()
    {
        if (CanControl)
        {
            Move();
            Jump();
        }
    }

    // Get text if it's available
    private void GetText()
    {
        if(shouldJump)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(interactPoint.position, interactionRangeRadius, interactible);
            if (colliders.Length > 0)
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.GetComponent<NPCInteraction>() != null)
                    {
                        if(!c.gameObject.GetComponent<NPCInteraction>().TextCollected)
                        {
                            c.gameObject.GetComponent<NPCInteraction>().puzzleText.TextEffect();
                            inventoryMenu.PlayerInventory.Add(c.gameObject.GetComponent<NPCInteraction>().puzzleText);
                        }
                    }
                }
        }
    }

    // Check interaction input
    private void GetInteractInput()
    {
        shouldInteract = Input.GetButtonDown("Interact");
        Debug.Log("Should interact: " + shouldInteract);
    }

    // Interact
    void Interact()
    {
        if(shouldInteract)
        {
            Debug.Log("Interact");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(interactPoint.position, interactionRangeRadius, interactible);
            if (colliders.Length > 0)
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.GetComponent<NPCInteraction>() != null)
                    {
                        c.gameObject.GetComponent<NPCInteraction>().Interact();
                        CanControl = !CanControl;
                    }
                }
        }
    }

    // Take jump input
    private void GetJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
            shouldJump = true;
        else
            shouldJump = false;
    }

    // Player jump logic
    private void Jump()
    {
        if (shouldJump)
        {
            if (isOnGround)
            {
                rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
                isOnGround = false;
                shouldJump = false;
            }
            else //if(canGlide)
            {
                if (rigidbody2D.gravityScale > glideGravity)
                {
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                    rigidbody2D.gravityScale = glideGravity;
                }
                else
                    rigidbody2D.gravityScale = baseGravityScale;

            }
        }
    }

    // Check movement input
    private void GetMoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // Player movement logic
    private void Move()
    {
        rigidbody2D.velocity = new Vector2(horizontalInput * moveSpeed, rigidbody2D.velocity.y);
        animator.SetFloat("speed", Mathf.Abs(horizontalInput));
    }

    // Checks for ground below and updates the isOnGround variable accordingly
    private void UpdateIsOnGround()
    {
        Collider2D[] groundObjects = Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatCountsAsGround);
        isOnGround = groundObjects.Length > 0;
        if (isOnGround)
            rigidbody2D.gravityScale = baseGravityScale;
        boxCol.enabled = true;
    }
}
