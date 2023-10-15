using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] int breakableGems;  // Serialized for debugging purposes
    
    // Cached reference
    SceneLoader sceneLoader;
    
    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void CountGems()
    {
        breakableGems++;
    }

    public void GemDestroyed()
    {
        breakableGems--;
        if (breakableGems <= 0)
        {
            sceneLoader.LoadNextScene();
        }
    }
}
