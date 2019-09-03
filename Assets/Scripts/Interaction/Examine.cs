using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{
    private bool examining;
    private Vector3 originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!examining)
            return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            float rotX = Input.GetAxis("Mouse X") * 200 * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * 200 * Mathf.Deg2Rad;

            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(transform.right, rotY, Space.World);
        }
    }

    public void StartExamining()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        examining = true;
        originalRotation = transform.eulerAngles;
    }

    public void StopExamining()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        examining = false;
        transform.eulerAngles = originalRotation;
    }
}
