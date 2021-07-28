using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    


    public float movementSpeed = 8;
    Vector2 movementValues;


    [Header("Jumping")]
    public float jumpHeight = 22;
    public float jumpCooldown = 0.5f;
    float jumpTimer;

    public Vector2 groundBoxcastDimensions = new Vector2(1, 0.01f);
    public float groundedRaycastLength = 0.1f;
    public LayerMask groundingDetection = ~0;

    
    public Vector2 velocityDecay = new Vector2(30, 30);
    Vector2 velocity;



    bool IsGrounded()
    {
        RaycastHit2D[] results = Physics2D.BoxCastAll(transform.position, groundBoxcastDimensions, transform.rotation.z, -transform.up, groundedRaycastLength, groundingDetection);
        if (results.Length > 1 && results[0].collider.gameObject != this)
        {
            return true;
        }

        return false;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        jumpTimer += Time.deltaTime;
        if (Input.GetButtonDown("Jump") && jumpTimer >= jumpCooldown && IsGrounded() == true)
        {
            jumpTimer = 0;
            velocity.y += jumpHeight;
        }

        movementValues.x = Input.GetAxis("Horizontal") * movementSpeed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition((Vector2)rb.transform.position + ((movementValues + velocity) * Time.fixedDeltaTime));
        
        if (velocity.magnitude > 0)
        {
            velocity -= velocityDecay * Time.fixedDeltaTime;
            velocity.x = Mathf.Max(0, velocity.x);
            velocity.y = Mathf.Max(0, velocity.y);
        }
        
    }
}
