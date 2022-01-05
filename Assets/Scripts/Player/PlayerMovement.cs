using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof (Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidbody { get; private set; }
    BoxCollider2D collider;
    

    [Header("Movement")]
    public float movementSpeed = 8;
    Vector2 movementValues;


    [Header("Jumping")]
    public float jumpForce = 350;
    public float jumpCooldown = 0.5f;
    float jumpTimer;

    public Vector2 groundBoxcastDimensions = new Vector2(1, 0.01f);
    public float groundedRaycastLength = 0.1f;
    public LayerMask groundingDetection = ~0;

    [Header("Crouching")]
    public float standHeight;
    public float crouchHeight;
    public float crouchMovementSpeedMultiplier;




    public bool IsGrounded
    {
        get
        {
            RaycastHit2D[] groundingResults = Physics2D.BoxCastAll(transform.position, groundBoxcastDimensions, transform.rotation.z, -transform.up, groundedRaycastLength, groundingDetection);
            if (groundingResults.Length > 1 && groundingResults[0].collider.gameObject != this)
            {
                groundingData = groundingResults[0];
                //ebug.Log("Grounded");
                return true;
            }
            groundingData = new RaycastHit2D();
            //Debug.Log("Airborne");
            return false;
        }
    }
    RaycastHit2D groundingData;
    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
            //velocity.y += jumpHeight;
            rigidbody.AddForce(transform.up * jumpForce);
        }

        movementValues = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, 0);
        /*
        if (IsGrounded)
        {
            float normalAngle = Vector2.SignedAngle(transform.up, groundingData.normal);
            movementValues = Quaternion.Euler(0, 0, -normalAngle) * movementValues;
            //Vector2 direction = 
        }
        */
        Debug.DrawRay(transform.position, movementValues, Color.green);
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
        //rigidbody.MovePosition(rigidbody.position + (movementValues * Time.fixedDeltaTime));
        //Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        //AddForceWithinLimits2d(rigidbody, moveDirection, movementSpeed * 500, movementSpeed);

        /*
        if (movementValues.magnitude > 0)
        {
            Vector2 position2D = rigidbody.position;
            Vector2 deltaMovement = (rigidbody.velocity + movementValues) * Time.fixedDeltaTime;
            rigidbody.MovePosition(position2D + deltaMovement);
        }
        */


        Vector2 currentVelocity = rigidbody.velocity;
        if (movementValues.x > 0)
        {
            currentVelocity.x = Mathf.Max(currentVelocity.x, movementValues.x);
        }
        else if (movementValues.x < 0)
        {
            currentVelocity.x = Mathf.Min(currentVelocity.x, movementValues.x);
        }
        if (movementValues.y > 0)
        {
            currentVelocity.y = Mathf.Max(currentVelocity.y, movementValues.y);
        }
        else if (movementValues.y < 0)
        {
            currentVelocity.y = Mathf.Min(currentVelocity.y, movementValues.y);
        }
        rigidbody.velocity = currentVelocity;
    }
    /*
    public static void AddForceWithinLimits2d(Rigidbody2D rb, Vector2 direction, float force, float maxVelocity)
    {
        float clampedAcceleration = Mathf.Clamp(force, 0f, Mathf​.Max(0, maxVelocity - rb.velocity.magnitude));
        rb.velocity += direction.normalized * clampedAcceleration;
    }
    */
}


