using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float rayLength = 20.0f;
    private RaycastHit hit;
    public LayerMask layerMask;

    public Material highlightMaterial;
    private Material savedMaterial;
    private GameObject curObject;

    private PickUp pickUpRef;

    // Update is called once per frame
    void Update()
    {
        //Simply shoots out a raycast that's visible in the Scene view
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.blue);

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, layerMask))
        {
            //let's us run a function that interacts with the object being hit by the raycast in some way
            if(Input.GetKeyDown(KeyCode.E) && curObject != null)
            {
                ObjectInteraction(curObject);
            }

            //highlights any material deemed interactive when faced by the player
            if (curObject == null)
            {
                curObject = hit.collider.gameObject;
                savedMaterial = curObject.GetComponent<MeshRenderer>().material;
                curObject.GetComponent<MeshRenderer>().material = highlightMaterial;
            }
        }

        else
        {
            if(curObject != null)
            {
                NullifyCurObject();
            }
        }
    }

    void NullifyCurObject()
    {
        curObject.GetComponent<MeshRenderer>().material = savedMaterial;
        curObject = null;
    }

    void ObjectInteraction(GameObject objectFromRaycast)
    {
       if(objectFromRaycast.tag == "Key")
        {
            //??
        }
    }
}
