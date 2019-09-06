using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearOnLight : MonoBehaviour
{
    [SerializeField] Light _light;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_light.intensity > 0)
        {
            GetComponent<Renderer>().enabled = true;
        }

        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
