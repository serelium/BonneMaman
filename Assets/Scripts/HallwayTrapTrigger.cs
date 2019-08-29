using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// struct to hold a wall and direction
[System.Serializable]
public struct HallwayTrapPlane
{
    public GameObject gameObject;
    public Vector2 scrollVector;
}

public class HallwayTrapTrigger : MonoBehaviour
{

    // The walls that will be scrolling and which way they should scroll
    [SerializeField] private HallwayTrapPlane[] hallwayPlanes;

    public Vector3 stopDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().directionToStop = stopDirection;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if( other.tag == "Player" ){
            other.GetComponent<PlayerMovement>().directionToStop = Vector3.zero;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (HallwayTrapPlane plane in hallwayPlanes)
            {
                Material mat = plane.gameObject.GetComponent<Renderer>().material;
                mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + plane.scrollVector.x * Time.deltaTime, mat.mainTextureOffset.y + plane.scrollVector.y * Time.deltaTime);
            }
        }
    }
}
