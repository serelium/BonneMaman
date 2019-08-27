using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform portalPlayer;
    public Transform portalCamera;
    

    private bool playerIsOverlapping = false;
    private bool playerIsPorting = false;


    void Update()
    {
        //if (playerIsOverlapping)
        //{
        //    Vector3 portalToPlayer = player.position - transform.position;
        //    float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

        //    if(dotProduct < 0f)
        //    {
           
        //        playerIsOverlapping = false;
        //    }

         
        //}

        if(playerIsPorting)
        {
            portalPlayer.position = portalCamera.position + new Vector3(0,-1.8f,0);


        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag =="Player")   
        {
            playerIsOverlapping = true;
            playerIsPorting = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag =="Player")
        {
            playerIsOverlapping = false;
        }
    }

    ////Let the rigidbody take control and detect collisions.
    //public void EnableRagdoll(Rigidbody rb)
    //{
    //    rb.isKinematic = false;
    //    rb.detectCollisions = true;
    //}

    //// Let animation control the rigidbody and ignore collisions.
    //void DisableRagdoll(Rigidbody rb)
    //{
    //    rb.isKinematic = true;
    //    rb.detectCollisions = false;
    //}
}
