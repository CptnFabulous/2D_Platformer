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

    Vector2 movementValues;

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
        //rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        //Debug.Log("Updating, frame " + Time.frameCount);
        jumpTimer += Time.deltaTime;
        if (Input.GetButtonDown("Jump")/* && jumpTimer >= jumpCooldown && IsGrounded() == true*/)
        {
            jumpTimer = 0;
            Debug.Log("Jump on " + Time.frameCount);

            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpHeight);
        }

        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, 0);
        movementValues = movementInput * Time.fixedDeltaTime;

        

    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = rb.transform.position;
        rb.MovePosition(currentPosition + movementValues);
    }
}
