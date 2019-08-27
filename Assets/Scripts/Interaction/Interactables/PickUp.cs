using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    //public Transform HoldItem;

    float throwForce = 500;
    Vector3 objectPos;
    float distance;

    public bool canHold = true;
    public bool isHolding = false;

    private Rigidbody rb;
    private Collider collider;
    private Player interactor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding && Input.GetKeyDown(KeyCode.E))
        {
            interactor.CanInteract = true;
            isHolding = false;
            rb.useGravity = true;
            //rb.isKinematic = false;
            transform.parent = null;
            interactor = null;
            collider.isTrigger = false;
        }
    }

    public override void Interact(Player interactor)
    {
        StartCoroutine(Pickup(interactor));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(isHolding && interactor)
        {
            interactor.RigidBody.velocity = Vector3.zero;
            interactor.RigidBody.angularVelocity = Vector3.zero;
        }
    }

    private IEnumerator Pickup(Player interactor)
    {
        yield return new WaitForEndOfFrame();

        interactor.CanInteract = false;
        isHolding = true;
        transform.parent = interactor.HoldPoint.transform;
        rb.useGravity = false;
        //rb.isKinematic = true;
        collider.isTrigger = true;
        this.interactor = interactor;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
