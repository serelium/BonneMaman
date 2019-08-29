using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float rayLength = 20.0f;
    public LayerMask layerMask;

    private RaycastHit hit;
    private Interactable currentInteractable;
    public Rigidbody RigidBody { get; private set; }

    bool active;

    private void Start()
    {
        active = true;
        RigidBody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        //Simply shoots out a raycast that's visible in the Scene view
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.blue);

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, layerMask))
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

        if (active && Input.GetKeyDown(KeyCode.E) && currentInteractable)
        {
            //currentInteractable.Interact(this);
        }
    }
    
    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }
}
