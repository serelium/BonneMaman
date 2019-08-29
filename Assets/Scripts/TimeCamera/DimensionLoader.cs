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

    private void OnTriggerEnter(Collider other) {
        foreach (string scene in scenesToLoad)
        {
            //* quick, impermanent unload scene test */
            SceneManager.UnloadSceneAsync(scene, 0);
            return;
        }
    }
}
