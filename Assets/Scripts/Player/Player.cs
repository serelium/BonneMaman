using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float interactRayLength = 1f;
    [SerializeField] private TimeCamera timeCamera;
    [SerializeField] private Transform holdPoint;
    public bool useTimeCamera;

    public Transform HoldPoint => holdPoint;
    public TimeCamera TimeCamera => timeCamera;
    public Rigidbody RigidBody { get; private set; }
    public GameObject HeldObject => HoldPoint.childCount > 0 ? HoldPoint.GetChild(0).gameObject : null;

    public bool IsHoldingItem { get; set; }
    public bool IsExamining { get; private set; }
    public bool TimeCameraActive => TimeCamera.Active;
    public bool CanInteract { get; set; }
    public bool CanSwitchDimenstion { get; private set; }

    private Interactable currentInteractable;

    private void Start()
    {
        CanInteract = true;
        RigidBody = GetComponent<Rigidbody>();

        if (TimeCamera)
        {
            gameObject.layer = TimeCamera.CurrentLayer;
            CanSwitchDimenstion = true;
            TimeCamera.ChangingDimension += (() => CanSwitchDimenstion = false);
            TimeCamera.ChangedDimension += (() => CanSwitchDimenstion = true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsExamining)
        {
            HandleInteraction();
            HandleTimeCameraInput();
        }

        if(IsHoldingItem)
        {
            HandleInpsecting();
        }
    }

    private void HandleInpsecting()
    {
        Examine examineObject = HeldObject.GetComponent<Examine>();

        if (!examineObject)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!IsExamining)
            {
                IsExamining = true;
                examineObject.StartExamining();

                // Disable movment and camera control during examination of object
                GetComponent<PlayerMovement>().enabled = false;
                Camera.main.GetComponent<PlayerLook>().enabled = false;
            }

            else
            {
                IsExamining = false;
                examineObject.StopExamining();
                GetComponent<PlayerMovement>().enabled = true;
                Camera.main.GetComponent<PlayerLook>().enabled = true;
            }
        }
    }

    private void HandleTimeCameraInput()
    {
        if (!useTimeCamera)
            return;

        if (Input.GetKey(KeyCode.Alpha1))
        {
            TimeCamera.SwapCamera(CameraType.Past);
        }

        else if (Input.GetKey(KeyCode.Alpha2))
        {
            TimeCamera.SwapCamera(CameraType.Present);
        }

        else if (Input.GetKey(KeyCode.Alpha3))
        {
            TimeCamera.SwapCamera(CameraType.Future);
        }

        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (CanSwitchDimenstion)
            {
                TimeCamera.ChangeDimension();

                gameObject.layer = TimeCamera.CurrentLayer;

                if (HeldObject)
                {
                    HeldObject.gameObject.layer = TimeCamera.CurrentLayer;
                    LayerUtils.SetChildLayerRecursivly(HeldObject.gameObject.transform, TimeCamera.CurrentLayer);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            TimeCamera.SetActive(!TimeCameraActive);
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            TimeCamera.SetZoom(true);
        }

        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            TimeCamera.SetZoom(false);
        }
    }

    private void HandleInteraction()
    {
        if (!CanInteract)
            return;

        //Simply shoots out a raycast that's visible in the Scene view
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * interactRayLength, Color.blue);
        RaycastHit hit;

        LayerMask layerMask = TimeCamera.CurrentLayerMask;

        if (!useTimeCamera)
            layerMask = ~0;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRayLength, layerMask))
        {
            Interactable interactable = (Interactable)hit.collider.gameObject.GetComponent(typeof(Interactable));

            if (interactable != null)
            {
                if (currentInteractable && currentInteractable != interactable)
                    currentInteractable.Unhighlight();

                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    currentInteractable.Highlight();
                }
            }

            else if (currentInteractable)
            {
                currentInteractable.Unhighlight();
                currentInteractable = null;
            }
        }

        else if (currentInteractable)
        {
            currentInteractable.Unhighlight();
            currentInteractable = null;
        }

        if (CanInteract && Input.GetKeyDown(KeyCode.E) && currentInteractable)
        {
            currentInteractable.Interact(this);
        }
    }

    public void ActivateTimeCamera()
    {
        TimeCamera.SetActive(true);
    }

    public void DeactivateTimeCamera()
    {
        TimeCamera.SetActive(false);
    }
}
