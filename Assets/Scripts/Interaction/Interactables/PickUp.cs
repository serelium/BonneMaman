using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    private bool isHolding = false;
    private Rigidbody rb;
    private Collider collider;
    private Player holder;

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
            holder.CanInteract = true;
            isHolding = false;
            rb.useGravity = true;
            rb.isKinematic = false;
            transform.parent = null;
            holder = null;
        }
    }

    public override void Interact(Player interactor)
    {
        StartCoroutine(Pickup(interactor));
    }

    private IEnumerator Pickup(Player holder)
    {
        yield return new WaitForEndOfFrame();

        holder.CanInteract = false;
        isHolding = true;
        transform.parent = holder.HoldPoint.transform;
        rb.useGravity = false;
        rb.isKinematic = true;
        this.holder = holder;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
