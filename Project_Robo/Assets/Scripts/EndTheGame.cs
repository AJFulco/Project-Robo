using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndTheGame : MonoBehaviour
{
    /*
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;
    */

    // Use this for initialization
    void Start()
    {
        /*
        myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
        scenePaths = myLoadedAssetBundle.GetAllScenePaths();
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        // If the player presses E/Interact while within the box collider trigger, they will be sent to the credits scene.
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(1);
        }
    }
}
