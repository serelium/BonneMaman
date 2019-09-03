using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentDoor : Interactable
{
    //? e: PRE-REWRITE
    // I'm no longer using these states since I won't be animating the door anymore, but
    // I'll keep and change the enums in order to change between locked and unlocked states 
    enum DoorState {Open, Animating, Closed};
    

    //Public members
    public float                swingAngle      = 60.0f;
    public float                Duration        = .50f;
    public AnimationCurve       SwingCurve      = new AnimationCurve();


    //Privte members
    private Player              interactor;
    private Transform           _transform      = null;
    //! testing something
    private float yRotation = 1.0f;
    private Vector3             _openRot        = Vector3.zero;
    private Vector3             _closedRot      = new Vector3(0, 90, 0);
    private DoorState           _doorState      = DoorState.Closed;
    private Rigidbody           _rigidbody;
    private Collider            _collider;

    public override void Interact(Player interactor)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // Cached values for transform, original position, rigidbody, and collider
        _transform  = transform;
        _closedRot  = _transform.eulerAngles; 
        _rigidbody  = GetComponent<Rigidbody>();
        _collider   = GetComponent<Collider>();  
        
        // _openRot = _closedRot + (_transform.eulerAngles * swingAngle);
    }
    
    
    
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E)){

    //         StartCoroutine(AnimateDoor((_doorState == DoorState.Open) ? DoorState.Closed : DoorState.Open));
    //     }
    // }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        yRotation *= Input.GetAxis("Vertical");
        transform.eulerAngles = new Vector3(90, yRotation, 0);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, target, SwingCurve.Evaluate(t));
        time += Time.deltaTime;
    }

    // IEnumerator AnimateDoor( DoorState newstate){
    //     _doorState = DoorState.Animating;
    //     float time = 0.0f;

    //     Vector3 startPos = (newstate == DoorState.Open) ? _closedRot:_openRot;
    //     Vector3 endPos   = (newstate == DoorState.Open) ? _closedRot:_openRot;

    //     while (time <= Duration) 
    //     {
    //         // Calculate normalized time and evaluate the result on our animation curve.
    //         // The result of the curve evaluation is then used as the t value in the
    //         // Vector Lerp between the start and ending positions
    //         

    //         //! testing smoothly tilt a transform toward a target rotation for Z and X
    //         // float tiltAroundZ = Input.GetAxis("Horizontal") * swingAngle;
    //         // float tiltAroundX = Input.GetAxis("Vertical") * swingAngle;
            
    //         // Rotate the cube by converting the angles into a quaternion.
    //         Quaternion target = Quaternion.Euler(0, 0, 0);

    //         //transform.rotation = Quaternion.Slerp(transform.rotation, target, SwingCurve.Evaluate(t));
    //         yRotation += Input.GetAxis("Horizontal");
    //         transform.eulerAngles = new Vector3(10, yRotation, 0);

    //         //Accumulate time and yield until the next frame
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
        
    //     // Snap object to the end position (just to make sure)
    //     _transform.eulerAngles = endPos;

    //     //Assign new state to the door
    //     _doorState = newstate;
    // }

}
