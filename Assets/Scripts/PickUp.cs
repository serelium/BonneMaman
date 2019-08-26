﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //public Transform HoldItem;

    float throwForce = 500;
    Vector3 objectPos;
    float distance;

    public bool canHold = true;
    public bool isHolding = false;

    public GameObject item;
    public GameObject tempParent;

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);

        if(distance >= 1f)
        {
            isHolding = false;
        }

        //Check if isHolding is true or not
        if (isHolding == true)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            this.transform.parent = GameObject.Find("HoldItem").transform;

            if (Input.GetMouseButtonDown(1))
            {
                //throws whatever object is being held
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }
        }

        else
        {
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;
        }
    }

    void OnMouseDown()
    {
        if (distance <= 1f)
        {
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
        }
    }

    void OnMouseUp()
    {
        isHolding = false;
    }

    /*public void PickUpItem()
    {
        //Turns off gravity of held item
        GetComponent<Rigidbody>().useGravity = false;
        //Repositions object to being carried
        this.transform.position = HoldItem.position;
        this.transform.parent = GameObject.Find("HoldItem").transform;
    }

    public void PutDownItem()
    {
        //Puts gravity back on the held item once dropped
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
    }*/
}