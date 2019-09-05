using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected Color _outlineColor;

    public bool Active { get; set; } = true;

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
        mat.SetColor("_OutlineColor", _outlineColor);
    }

    public void Unhighlight()
    {
        Material mat = GetComponent<MeshRenderer>().material;

        // Setting the outline to a black color will result in having an "invisible" outline
        // since we add the color to the rendered texture as a post effect and black = (0, 0, 0)
        mat.SetColor("_OutlineColor", Color.black);
    }
}
