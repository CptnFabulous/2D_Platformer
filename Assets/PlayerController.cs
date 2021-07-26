using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    public float movementSpeed = 5;



    [Header("Jumping")]
    public float jumpHeight = 5;
    public float jumpCooldown = 1;
    float jumpTimer;

    public float groundedRaycastLength = 0.01f;

    

    
    public Vector2 velocityDecay = new Vector2(0.2f, 0.2f);
    Vector2 movementValues;
    Vector2 velocity;



    bool IsGrounded()
    {
        Debug.Log("checking");
        if (Physics.Raycast(transform.position, -transform.up, groundedRaycastLength, ~0))
        {
            Debug.Log("Is grounded");
            return true;
        }

        Debug.Log("Is not grounded");
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
        if (Input.GetButtonDown("Jump") && jumpTimer >= jumpCooldown/* && IsGrounded() == true*/)
        {
            jumpTimer = 0;
            Debug.Log("Jump on " + Time.frameCount);

            rb.AddForce(transform.up * jumpHeight);

            velocity.y += jumpHeight;
        }

        movementValues.x = Input.GetAxis("Horizontal") * movementSpeed;
    }

    private void FixedUpdate()
    {
        //Debug.Log(movementValues + ", " + velocity);
        rb.MovePosition((Vector2)rb.transform.position + ((movementValues + velocity) * Time.fixedDeltaTime));
        
        if (velocity.magnitude > 0)
        {
            velocity -= velocityDecay * Time.fixedDeltaTime;
            velocity.x = Mathf.Max(0, velocity.x);
            velocity.y = Mathf.Max(0, velocity.y);
        }
        
    }
}
