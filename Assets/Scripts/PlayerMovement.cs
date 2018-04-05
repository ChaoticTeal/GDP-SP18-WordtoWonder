using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Private fields
    /// <summary>
    /// Does the player have control?
    /// </summary>
    bool canControl;
    /// <summary>
    /// Is the player on the ground?
    /// </summary>
    bool isOnGround;
    /// <summary>
    /// Should the player jump?
    /// </summary>
    bool shouldJump;
    /// <summary>
    /// Horizontal input check
    /// </summary>
    float horizontalInput;
    /// <summary>
    /// Box collider 2D
    /// </summary>
    BoxCollider2D boxCol;
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
    /// Movement speed
    /// </summary>
    [Tooltip ("Movement speed.")]
    [SerializeField]
    float moveSpeed = 5.0f;
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
    /// Strength of jump
    /// </summary>
    [Tooltip("Force added on jump.")]
    [SerializeField]
    float jumpStrength;

    // Use this for initialization
    void Start ()
    {
        jumpForce = new Vector2(0, jumpStrength);
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        canControl = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetJumpInput();
        GetMoveInput();
        UpdateIsOnGround();
    }

    private void GetMoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (canControl)
        {
            Move();
            Jump();
        }
    }

    // Take jump input
    private void GetJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isOnGround)
            shouldJump = true;
        else
            shouldJump = false;
    }

    // Player jump logic
    private void Jump()
    {
        if (shouldJump)
        {
            rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            shouldJump = false;
        }
    }

    // Player movement logic
    private void Move()
    {
        rigidbody2D.velocity = new Vector2(horizontalInput * moveSpeed, rigidbody2D.velocity.y);
    }

    // Checks for ground below and updates the isOnGround variable accordingly
    private void UpdateIsOnGround()
    {
        Collider2D[] groundObjects = Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatCountsAsGround);
        isOnGround = groundObjects.Length > 0;
        boxCol.enabled = true;
    }
}
