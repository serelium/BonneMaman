using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    //public Transform HoldItem;

    private bool isHolding = false;

    private Rigidbody rb;
    private Collider collider;
    private Player interactor;
    private Color previousColor;

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
            transform.parent = null;
            collider.isTrigger = false;
            interactor.IsHoldingItem = false;
            interactor = null;
            outlineColor = previousColor;
        }
    }

    public override void Interact(Player interactor)
    {
        StartCoroutine(Pickup(interactor));
    }

    private void OnTriggerEnter(Collider collider)
    {

    }

    private IEnumerator Pickup(Player interactor)
    {
        yield return new WaitForEndOfFrame();

        interactor.CanInteract = false;
        isHolding = true;
        transform.parent = interactor.HoldPoint.transform;
        rb.useGravity = false;
        collider.isTrigger = true;
        this.interactor = interactor;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        interactor.IsHoldingItem = true;
        Unhighlight();
    }
}
