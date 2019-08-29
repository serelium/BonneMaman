using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    Animator doorAnim;
    public GameObject door;

    private void Awake() {
        doorAnim = door.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other) {
        doorAnim.SetBool("isOpen", true);
    }

    private void OnTriggerExit(Collider other) {
        doorAnim.SetBool("isOpen", false);
    }

}
