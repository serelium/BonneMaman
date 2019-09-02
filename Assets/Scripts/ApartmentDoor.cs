using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentDoor : Interactable
{
    enum DoorState {Open, Animating, Closed};
    

    //Public members
    public float                swingAngle      = 60.0f;
    public float                Duration        = .50f;
    public AnimationCurve       SwingCurve      = new AnimationCurve();


    //Privte members
    private Player              interactor;
    private Transform           _transform      = null;
    
    private Vector3             _openRot        = Vector3.zero;
    private Vector3             _closedRot      = Vector3.zero;
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
        
        _openRot = _closedRot + (_transform.eulerAngles * swingAngle);
    }
    
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){

            StartCoroutine(AnimateDoor((_doorState == DoorState.Open) ? DoorState.Closed : DoorState.Open));
        }
    }

    IEnumerator AnimateDoor( DoorState newstate){
        _doorState = DoorState.Animating;
        float time = 0.0f;

        Vector3 startPos = (newstate == DoorState.Open) ? _closedRot:_openRot;
        Vector3 endPos   = (newstate == DoorState.Open) ? _closedRot : _openRot;

        while (time <= Duration) 
        {
            // Calculate normalized time and evaluate the result on our animation curve.
            // The result of the curve evaluation is then used as the t value in the
            // Vector Lerp between the start and ending positions
            float t = time/Duration;

            //Smoothly tilt a transform toward a target rotation
            float tiltAroundZ = Input.GetAxis("Horizontal") * swingAngle;
            float tiltAroundX = Input.GetAxis("Vertical") * swingAngle;

            //!_transform.position = Vector3.Lerp(startPos, endPos, SwingCurve.Evaluate(t));
            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(0, tiltAroundZ, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, target, SwingCurve.Evaluate(t));
            

            //Accumulate time and yield until the next frame
            time += Time.deltaTime;
            yield return null;
        }
        
        // Snap object to the end position (just to make sure)
        _transform.eulerAngles = endPos;

        //Assign new state to the door
        _doorState = newstate;
    }

}
