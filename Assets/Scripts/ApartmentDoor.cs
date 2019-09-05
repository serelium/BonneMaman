using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentDoor : Interactable
{
    // I'm no longer using these states since I won't be animating the door anymore, but
    // I'll keep and change the enums in order to change between locked and unlocked states 
    enum DoorState {Locked, Unlocked};


    //Visible members
    [SerializeField] private GameObject _requiredToOpen; // Object required to unlock the door
    [SerializeField] private float yRotation = 1f;
    [SerializeField] private float swingAngle = 60.0f;
    [SerializeField] private float Duration = .50f;

    //Privte members
    private Player              _interactor;
    private Transform           _transform      = null;
    private DoorState           _doorState      = DoorState.Locked;
    private Rigidbody           _rigidbody;
    private Collider            _collider;
    private HingeJoint          _hingeJoint;
    private JointSpring         _hingeSpring; 

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
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        else
            _doorState = DoorState.Unlocked;
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        //_hingeJoint.spring. = 50f;
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        float time = 0.0f;
        float t = time / Duration;
        Quaternion target = Quaternion.Euler(90, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Duration);
        time += Time.deltaTime;
    }
}
