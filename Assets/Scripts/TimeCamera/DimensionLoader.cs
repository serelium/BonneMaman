using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DimensionLoader : MonoBehaviour
{
    [SerializeField] private List<string> scenesToLoad;

    private void Awake()
    {
        foreach(string scene in scenesToLoad)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
