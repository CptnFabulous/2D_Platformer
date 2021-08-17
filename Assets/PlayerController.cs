using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D collider;
    

    [Header("Movement")]
    public float movementSpeed = 8;
    Vector2 movementValues;


    [Header("Jumping")]
    public float jumpHeight = 22;
    public float jumpCooldown = 0.5f;
    float jumpTimer;

    public Vector2 groundBoxcastDimensions = new Vector2(1, 0.01f);
    public float groundedRaycastLength = 0.1f;
    public LayerMask groundingDetection = ~0;

    [Header("Crouching")]
    public float standHeight;
    public float crouchHeight;
    public float crouchMovementSpeedMultiplier;

    [Header("Physics")]
    public Vector2 velocityDecay = new Vector2(30, 30);
    Vector2 velocity;



    bool IsGrounded
    {
        get
        {
            RaycastHit2D[] results = Physics2D.BoxCastAll(transform.position, groundBoxcastDimensions, transform.rotation.z, -transform.up, groundedRaycastLength, groundingDetection);
            if (results.Length > 1 && results[0].collider.gameObject != this)
            {
                return true;
            }

            return false;
        }
    }

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        jumpTimer += Time.deltaTime;
        if (Input.GetButtonDown("Jump") && jumpTimer >= jumpCooldown && IsGrounded == true)
        {
            jumpTimer = 0;
            velocity.y += jumpHeight;
        }

        movementValues.x = Input.GetAxis("Horizontal") * movementSpeed;
    }

    void SetCrouch(bool isCrouching)
    {
        float newHeight = standHeight;
        float newSpeed = movementSpeed;
        if (isCrouching)
        {
            newHeight = crouchHeight;
            newSpeed = movementSpeed * crouchMovementSpeedMultiplier;
        }
        
        collider.size = new Vector2(collider.size.x, newHeight);
        collider.offset = new Vector2(collider.offset.x, newHeight / 2);
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition((Vector2)rb.transform.position + ((movementValues + velocity) * Time.fixedDeltaTime));
        
        if (velocity.magnitude > 0)
        {
            // Slowly reduce velocity magnitude
            velocity -= velocityDecay * Time.fixedDeltaTime;
            velocity.x = Mathf.Max(0, velocity.x);
            velocity.y = Mathf.Max(0, velocity.y);
        }
        
    }
}
