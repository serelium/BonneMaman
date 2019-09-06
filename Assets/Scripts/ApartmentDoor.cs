using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentDoor : Interactable
{
    //e: enum states to switch between when player has required obj
    enum DoorState {Locked, Unlocked};


    //Visible members
    [SerializeField] 
    private GameObject  _requiredToOpen; // Object required to unlock the door
    [SerializeField] 
    private float       _yRotation      = 1f;
    [SerializeField] 
    private float       _swingAngle     = 60.0f;
    [SerializeField] 
    private float       _Duration       = .50f;

    //Privte members
    private Player      _interactor;
    private Transform   _transform      = null;
    private DoorState   _doorState      = DoorState.Locked;
    private Rigidbody   _rigidbody;
    private Collider    _collider;
    // hinge members
    private HingeJoint  _hingeJoint;
    private JointSpring _hingeSpring;
    // player : door bool
    private bool        _holdingDoor;

    public override void Interact(Player interactor)
    {
        // Unfreeze position and rotation of door when the player interacts with it having the right object to open it
        if (FindObjectOfType<Player>().HeldObject == _requiredToOpen)
        {
            _doorState = DoorState.Unlocked;
            _rigidbody.constraints = RigidbodyConstraints.None;
            Active = false; // Disable future interaction
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Cached values for transform, original position, rigidbody, and collider
        _transform  = transform;
        _rigidbody  = GetComponent<Rigidbody>();
        _collider   = GetComponent<Collider>();  

        // hinge values
        _hingeJoint = GetComponent<HingeJoint>();
        _hingeSpring = _hingeJoint.spring;

        if (_requiredToOpen)
            // door is locked
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;  
        else
            _doorState = DoorState.Unlocked;
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        // attempt to force extra control for the player while they're inside the collider
        // trigger
        Cursor.visible = true;

        float rotX = Input.GetAxis("Mouse X") * 200 * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * 200 * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX, Space.World);
        transform.Rotate(transform.right, rotY, Space.World);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)){   
            StartCoroutine(Grab());
        }
    }

    // void OnTriggerExit(Collider other)
    // {
    //     float time = 0.0f;
    //     float t = time / _Duration;
    //     Quaternion target = Quaternion.Euler(90, 0, 0);
    //     transform.rotation = Quaternion.Slerp(transform.rotation, target, _Duration);
    //     time += Time.deltaTime;
    // }

    private IEnumerator Grab(){
        _holdingDoor = true;
        yield return null;
     }
    
    // private IEnumerator StopGrab(){
    //     _holdingDoor = false;

    // }


}
