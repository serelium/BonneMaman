using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveVertical;
    private float moveHorizontal;
    public float speed = 6.0f;

    public float jumpForce = 4.0f;
    public float gravity = 1.0f;

    private Rigidbody rb;

    public LayerMask groundLayer;

    private CapsuleCollider col;

    // temp variable to handle hallway trap; better implementation needed
    [HideInInspector]
    public bool stop = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        PlayerMove();
        Jump();
    }

    private void PlayerMove()
    {
        if (!stop)
        {
            //Grabs the user's input on the vertical axis and horizontal axis.
            moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            transform.Translate(moveHorizontal, 0, moveVertical);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //Lets you adjust the gravity after jumping so you fall faster. Probably not needed.
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (gravity - 1) * Time.deltaTime;
        }
    }

    //Checks to see if player is grounded or not so you can't jump in the air infinitely.
    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .3f, groundLayer);
    }
}
