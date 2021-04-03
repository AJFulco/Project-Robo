using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// 0 = Main menu
    /// 1 = Normal gameplay
    /// 2 = Puzzel gameplay
    /// 3 = Paused game
    /// We can build on top of this more as the we add more stuff. 
    /// </summary>
    public int GameState = 0; //this is an int that will be used to keep track of what stat the game is in. 

    [Space(2)]
    [Header("References")]
    //the parts of the game that we will be manipulating with this script. 
    public GameObject puzzleUI = null; //guess what this is?
    public PlayerMovement thePlayer = null;

    [Space(2)]
    [Header("Camera Stuff")]
    public GameObject[] virtualCams; //the collection of virtual cameras. Uses the game state int to change. 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (GameState)
        {
            case 0: //if this is 0 we are in the main menu code.
                puzzleUI.SetActive(false);
                //player movment code 
                thePlayer.enabled = false;

                //the virtual cameras 
                virtualCams[0].SetActive(true);
                virtualCams[1].SetActive(false);
                break;
            case 1: //player is allowed overworld movement. 
                puzzleUI.SetActive(false);
                //player movement code
                thePlayer.enabled = true;
                virtualCams[1].SetActive(true);
                virtualCams[0].SetActive(false);
                break;
            case 2: //pause menue is active
                puzzleUI.SetActive(false);
                //player movement code
                break;
            case 3: //puzzle Ui is active. 
                puzzleUI.SetActive(false);
                //player movement code
                break;
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(thePlayer);

    } // End of SavePlayer()

    public void LoadPlayer()
    {
        PlayerData playerData = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = playerData.position[0];
        position.y = playerData.position[1];
        position.z = playerData.position[2];

        // Set the player so they start in the nearest sleeping bay
        //transform.position = position;

    } // End of LoadPlayer()
}
