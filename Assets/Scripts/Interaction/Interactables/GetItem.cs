using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : Interactable
{
    public AudioSource audioSource;
    public AudioClip cameraSound;
    public AudioClip futureLensSound;

    public float rayLength;
    private RaycastHit hit;
    private Ray ray;

    private GameObject futureCamera;
    private GameObject cameraPickUp;

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
        cameraPickUp = GameObject.Find("CameraPickUp");
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.tag == "CameraPickUp" && Input.GetKeyDown(KeyCode.E))
            {
                //destroys the camera after a set delay of time (delay needed for particles to play)
                Destroy(hit.collider.gameObject, .8f);
                //plays a sound upon pickup
                cameraPickUp.GetComponent<ParticleSystem>().Play();
                //allows use of the time camera (needs to be toggled off in the inspector before pickup to work)
                GameObject.Find("Player").GetComponent<Player>().useTimeCamera = true;
                //hides the time camera item upon pickup, giving the particles time to play
                GameObject.Find("CameraModel").SetActive(false);
                //turns off the view for the future camera, though you can still swap to the dimension physically. needs tweaking
                futureCamera.SetActive(false);
                //plays a sound clip upon pick up of the camera
                AudioManager.Instance.Play(gameObject, "Item_PickUp");
            }

            if (hit.collider.gameObject.tag == "FutureLensePickUp" && Input.GetKeyDown(KeyCode.E))
            {
                Destroy(hit.collider.gameObject);
                futureCamera.SetActive(true);
            }
        }
    }
}