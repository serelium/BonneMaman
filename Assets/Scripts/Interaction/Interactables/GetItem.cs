using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : Interactable
{
    public float rayLength;
    private RaycastHit hit;
    private Ray ray;

    private GameObject futureCamera;

    public override void Interact(Player interactor)
    {
        //Destroy(hit.collider.gameObject);
        //GameObject.Find("Player").GetComponent<Player>().useTimeCamera = true;
        //futureCamera.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        futureCamera = GameObject.Find("FutureCamera");
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.tag == "CameraPickUp" && Input.GetKeyDown(KeyCode.E))
            {
                Destroy(hit.collider.gameObject);
                GameObject.Find("Player").GetComponent<Player>().useTimeCamera = true;
                futureCamera.SetActive(false);
            }

            if (hit.collider.gameObject.tag == "CameraFutureLense" && Input.GetKeyDown(KeyCode.E))
            {
                Destroy(hit.collider.gameObject);
                futureCamera.SetActive(true);
            }
        }
    }
}