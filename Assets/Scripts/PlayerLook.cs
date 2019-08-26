using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float minimumY = -60f;
    public float maximumY = 60f;

    private float mouseX = 0f;
    private float mouseY = 0f;

    public float sensitivity = 8f;

    [SerializeField] private Transform playerBody;

    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
    }

    //Lets you look around the world like a boss!
    void MouseMovement()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        //should clamp the camera when looking up or down at determined angles, but isn't working :S
        //mouseY = Mathf.Clamp(mouseY, minimumY, maximumY);

        //rotates the player with the camera so forward movement is always in the direction the camera is facing
        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);

        if(mouseX > 60)
        {
            //maybe this will work for clamping?
        }
    }

    private void LockCursor()
    {
        //Locks the mouse cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
}
