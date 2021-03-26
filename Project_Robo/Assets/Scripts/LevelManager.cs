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
    public GameObject mainMenu = null;
    public GameObject pauseMenu = null;
    public GameObject puzzleUI = null; //guess what this is?
    public PlayerMovement thePlayer = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameState) {
            case 0:
                mainMenu.SetActive(true);
                pauseMenu.SetActive(false);
                puzzleUI.SetActive(false);
                //player movment code 
                break;
            case 1:
                mainMenu.SetActive(true);
                pauseMenu.SetActive(false);
                puzzleUI.SetActive(false);
                //player movement code
                break;
            case 2:
                mainMenu.SetActive(true);
                pauseMenu.SetActive(false);
                puzzleUI.SetActive(false);
                //player movement code
                break;
            case 3:
                mainMenu.SetActive(true);
                pauseMenu.SetActive(false);
                puzzleUI.SetActive(false);
                //player movement code
                break;
        }
    }
}
