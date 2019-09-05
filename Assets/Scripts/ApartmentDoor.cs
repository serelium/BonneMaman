using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentDoor : Interactable
{
    // I'm no longer using these states since I won't be animating the door anymore, but
    // I'll keep and change the enums in order to change between locked and unlocked states 
    enum DoorState {Locked, Unlocked};
    

    //Visible members
    [SerializeField] private float yRotation = 1f;
    [SerializeField] private float swingAngle = 60.0f;
    [SerializeField] private float Duration = .50f;

    //Privte members
    private Player              _interactor;
    private Transform           _transform      = null;
    private DoorState           _doorState      = DoorState.Unlocked;
    private Rigidbody           _rigidbody;
    private Collider            _collider;
    private HingeJoint          _hingeJoint;
    private JointSpring         _hingeSpring; 
    public override void Interact(Player interactor)
    {
        
        // if (hasKey)  { Doorstate.Unlocked; }
        //
        //StartCoroutine(DoorCheck(interactor));    
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

    IEnumerator DoorCheck (DoorState newState){
        //? E:  1. Make the door highlight as an interactable object
        //?     2. Allow the physics to happen IF you have the key
        yield return null;
        _doorState = newState;
    }

}
