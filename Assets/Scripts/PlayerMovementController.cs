using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    /*
    Inspirations
    https://www.youtube.com/watch?v=vAZV5xO_AHU
    https://www.youtube.com/watch?v=3sWTzMsmdx8
    */

    [Header("Movement")]
    [Min(0)] public float movementSpeed = 5;

    [Header("Grounding")]
    public LayerMask terrainDetection = ~0;
    [Min(0)] public float groundingCheckLength = 1.1f;
    [Min(0)] public float wallCheckLength = 0.6f;
    [Range(0, 90)] public float groundAngle = 45;

    [Header("Jumping")]
    [Min(0)] public float jumpForce = 5;
    [Min(0)] public float jumpHoldTime = 0.5f;
    [Min(0)] public float jumpCooldown = 0.5f;
    [Min(0)] public float coyoteTime = 0.1f;
    [Min(0)] public float jumpBufferTime = 0.1f;
    [Min(0)] public float jumpApexTime = 0.1f;

    [Header("Gravity")]
    public float gravityScale = 2;
    [Min(0)] public float maxFallSpeed = 10;
    /*
    [Header("Wall-jumping")]
    public float wallJumpForce = 5;
    */

    Rigidbody2D rb;
    CapsuleCollider2D collider;
    Vector2 movementValues; // The current movement input

    public RaycastHit2D groundData { get; private set; } // The thing the player is currently standing on
    //public RaycastHit2D wallData { get; private set; } // Whatever wall the player is touching. Returns the left wall if somehow touching both sides
    float lastTimeGrounded;

    float lastTimeJumpAttempted;
    float lastTimeJumped;
    float lastTimeAscending;


    #region Properties
    /// <summary>
    /// Returns the player's velocity, but relative to their rotation. Also used to directly set velocity based on local values
    /// </summary>
    public Vector2 localVelocity
    {
        get => Quaternion.Inverse(transform.rotation) * rb.velocity;
        set => rb.velocity = transform.rotation * value;
    }
    /// <summary>
    /// Collider size in world space.
    /// </summary>
    public Vector2 worldColliderSize => collider.size * transform.lossyScale;
    public bool isGrounded
    {
        get
        {
            if (groundData.collider == null) return false;
            if (Vector2.Angle(-groundData.normal, Physics.gravity) > groundAngle) return false;
            return true;
        }
    }
    //public bool isTouchingWall => wallData.collider != null;
    #endregion

    #region Input functions
    public void OnMove(InputValue input)
    {
        movementValues = input.Get<Vector2>();
    }
    public void OnJump(InputValue input)
    {
        if (input.isPressed)
        {
            lastTimeJumpAttempted = Time.time;
        }
        else // If after jumping, player releases button before jumpPressTime
        {
            // Check time of released compared to pressed, and alter velocity accordingly
            float jumpHeldTimeProportion = (Time.time - lastTimeJumped) / jumpHoldTime; // If value is over 1, jump was held for full period of time
            if (jumpHeldTimeProportion < 1)
            {
                float forceToSubtract = jumpForce * (1 - jumpHeldTimeProportion);
                Vector2 v = localVelocity;
                v.y -= forceToSubtract;
                localVelocity = v;
            }
        }
    }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }
    private void FixedUpdate()
    {
        CheckGrounding();
        //CheckIfTouchingWall();
        CheckJump();
        CheckJumpApex();

        Vector2 newVelocity = localVelocity;

        /*
        float acceleratedMovement = Mathf.MoveTowards(localVelocity.x, movementValues.x * movementSpeed, acceleration * Time.fixedDeltaTime);
        localVelocity = new Vector2(acceleratedMovement, localVelocity.y);
        */
        newVelocity.x = movementValues.x * movementSpeed; // Set velocity to movement direction and speed

        newVelocity.y = Mathf.Clamp(newVelocity.y, -maxFallSpeed, Mathf.Infinity); // Clamp vertical velocity

        localVelocity = newVelocity; // Set vertical velocity
    }

    void CheckGrounding()
    {
        Vector2 box = worldColliderSize;
        box.y *= 0.1f;
        groundData = Physics2D.BoxCast(collider.bounds.center, box, transform.eulerAngles.z, -transform.up, groundingCheckLength, terrainDetection);

        if (isGrounded)
        {
            lastTimeGrounded = Time.time;
        }
    }
    /*
    void CheckIfTouchingWall()
    {
        Vector2 box = worldColliderSize;
        box.x *= 0.1f;
        RaycastHit2D leftWall = Physics2D.BoxCast(collider.bounds.center, box, transform.eulerAngles.z, -transform.right, wallCheckLength, terrainDetection);
        RaycastHit2D rightWall = Physics2D.BoxCast(collider.bounds.center, box, transform.eulerAngles.z, transform.right, wallCheckLength, terrainDetection);

        wallData = leftWall.collider != null ? leftWall : rightWall;
    }
    */
    void CheckJump()
    {
        if (Time.time - lastTimeJumpAttempted > jumpBufferTime) return; // Is the buffer still active since the player pressed the jump button?
        if (Time.time - lastTimeGrounded > coyoteTime) return; // Is the player grounded (or is coyote time still in effect?)
        if (Time.time - lastTimeJumped < jumpCooldown) return; // Has the cooldown elapsed?

        lastTimeJumped = Time.time; // Reset the jump-related variables

        // Jump!
        localVelocity = new Vector2(localVelocity.x, jumpForce);
    }
    void CheckJumpApex()
    {
        if (isGrounded)
        {
            // If the player is not airborne, set normal gravity scale and don't perform any other calculations
            rb.gravityScale = gravityScale;
            return;
        }

        if (localVelocity.y > 0) // if the player is ascending, log time. 
        {
            lastTimeAscending = Time.fixedTime;
        }
        else // When they aren't, the time is not updated and represents the last time they were.
        {
            bool inApexTime = Time.fixedTime - lastTimeAscending < jumpApexTime;
            rb.gravityScale = inApexTime ? 0 : gravityScale;
        }
    }

    /// <summary>
    /// EXPERIMENTAL AND UNTESTED: This function is meant to alter the player's velocity in such a way that it doesn't override other, greater forces acting on it. This is so if the player is knocked in a direction by a larger force, they cannot immediately negate it by making any movement.
    /// </summary>
    /// <param name="value"></param>
    void ModifyVelocity(Vector2 value)
    {
        Vector2 velocity = localVelocity;

        // Set the desired force for a direction only if that direction's magnitude is greater than the current value.
        // If it's already higher, then there's no need to move any faster
        if (Mathf.Abs(velocity.x) < Mathf.Abs(value.x))
        {
            velocity.x = value.x;
        }
        if (Mathf.Abs(velocity.y) < Mathf.Abs(value.y))
        {
            velocity.y = value.y;
        }

        localVelocity = velocity;
    }

}
