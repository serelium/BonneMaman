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
    private Animator animator;

    public LayerMask groundLayer;

    private CapsuleCollider col;

    //variables to handle hallway trap
    [HideInInspector]
    public Vector3 directionToStop = new Vector3(0.0f, 0.0f, 0.0f);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        PlayerMove();
        Jump();
    }

    private void PlayerMove()
    {
        //Grabs the user's input on the vertical axis and horizontal axis.
        moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        Vector3 playerMovement = new Vector3(moveHorizontal, 0, moveVertical);

        // if we're trapped in a hallway, restrict movement in one direction
        if ( directionToStop != Vector3.zero )
        {
            Vector3 localDir = transform.InverseTransformVector(directionToStop);
            if (Vector3.Dot( playerMovement, localDir) > 0)
            {
                playerMovement = playerMovement - Vector3.Project(playerMovement, localDir);
            }
        }

        animator?.SetFloat("Speed", playerMovement.magnitude);
        transform.Translate(playerMovement);
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
