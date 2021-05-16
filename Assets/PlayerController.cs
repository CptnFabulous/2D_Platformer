using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    public float movementSpeed = 5;
    public float jumpHeight = 5;

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
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, 0);
        movementValues = movementInput * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump on " + Time.frameCount);
            rb.AddForce(new Vector2(0, jumpHeight));
        }

    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = rb.transform.position;
        rb.MovePosition(currentPosition + movementValues);
    }
}
