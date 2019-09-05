using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{
    [SerializeField] private float maxZoom = 5;
    private bool _examining;
    private Vector3 _originalRotation;
    private Vector3 _originalPosition;
    private float _zoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!_examining)
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

        float zoom = Input.GetAxis("Mouse ScrollWheel");

        if (zoom > 0 && _zoom <= 0 || zoom < 0 && _zoom >= maxZoom)
            return;

        _zoom -= zoom;

        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.position += dir.normalized * zoom;

        Debug.Log(zoom);
    }

    public void StartExamining()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _examining = true;
        _originalRotation = transform.eulerAngles;
        _originalPosition = transform.position;
        _zoom = 0;
    }

    public void StopExamining()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _examining = false;
        transform.eulerAngles = _originalRotation;
        transform.position = _originalPosition;
        _zoom = 0;
    }
}
