using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float outlineWidth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Interact(Player interactor);

    public void Highlight()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat.SetFloat("_OutlineWidth", outlineWidth);
    }

    public void Unhighlight()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat.SetFloat("_OutlineWidth", 0);
    }
}
