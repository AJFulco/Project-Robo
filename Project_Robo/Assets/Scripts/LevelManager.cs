using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    

    [Header("Tracking Ints")]
    /// <summary>
    /// 0 = Main menu
    /// 1 = Normal gameplay
    /// 2 = Puzzel gameplay
    /// 3 = Paused game
    /// We can build on top of this more as the we add more stuff. 
    /// </summary>
    public int progressState = 0;
    public int playerState = 0; //this is an int that will be used to keep track of what stat the game is in.
    public int Cycle = 0; //What is the playthrough number for for the player [Changes the skybox basically]
    public int DoorSwitch = 0;


    [Space(2)]
    [Header("References")]
    //the parts of the game that we will be manipulating with this script. 
    public GameObject puzzleUI = null; //guess what this is?
    public PlayerMovement playerMovementScript = null;
    [SerializeField] public GameObject thePlayer;
    public MenuManager menuManagerScript;


    [Space(2)]
    [Header("Camera Stuff")]
    public GameObject[] virtualCams; //the collection of virtual cameras. Uses the game state int to change. 

    [Space(2)]
    [Header("Puzzle Progress Tracking")]
    //All of the scripts we pull from
    public int[] puzzleList; //the number of puzzles is set to 15 because I (A.J.) said so. (^-^)/ 
    public int puzzleID; //the ID number of the puzzle. (is changed by the puzzle script)
    [SerializeField] public PuzzleUI PuzzleUIScript;
    public PuzzleMaster PuzzleMasterScript;//need for the boolean isComplete


    [Space(2)]
    [Header("Door")]
    // Door stuff
    [SerializeField] public List<DoorBehavior> DoorList;

    [Space(2)]
    [Header("Skybox/CubeMap Stuff")]
    //Changing the skybox / CubeMaps
    [SerializeField] public ArrayList CubeMapArrayList = new ArrayList();


    // Start is called before the first frame update
    void Start()
    {
        Cycle++;
        DoorSwitch++;
        //Skybox.ge Assets/Art/Skybox Stuff/Faces/skybox blue/cubemap.png
    }

    // Update is called once per frame
    void Update()
    {
        #region Player State Switch Statement
        switch (playerState)
        {
            case 0: //if this is 0 we are in the main menu code.
                puzzleUI.SetActive(false);
                //player movment code 
                playerMovementScript.enabled = false;

                //the virtual cameras 
                virtualCams[0].SetActive(true);
                virtualCams[1].SetActive(false);
                break;
            case 1: //player is allowed overworld movement. 
                puzzleUI.SetActive(true);
                //player movement code
                playerMovementScript.enabled = true;
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
        #endregion

        #region Cycle Switch Statement
        switch (Cycle)
        {
            case 1: //The first cycle of the game!!!
                switch (DoorSwitch)     // Opening doors in Cycle 1
                {
                    case 1: // Checking if door 1 is open
                        #region Open Door 1 Checks
                        //make sure they all exist
                        if (PuzzleUIScript.puzzles[0] != null &&
                            PuzzleUIScript.puzzles[1] != null &&
                            PuzzleUIScript.puzzles[2] != null &&
                            PuzzleUIScript.puzzles[3] != null)
                        {
                            // If all four tutorial puzzles are complete
                            if (PuzzleUIScript.puzzles[0].isComplete &&
                                PuzzleUIScript.puzzles[1].isComplete &&
                                PuzzleUIScript.puzzles[2].isComplete &&
                                PuzzleUIScript.puzzles[3].isComplete)
                            {
                                Debug.Log("All tutorial puzzles complete, door should open");
                                //open the first door
                                if (!DoorList[0].isOpen)
                                {
                                    DoorList[0].Open();
                                    DoorSwitch++;
                                }

                            }
                        }
                        #endregion
                        break;
                    case 2: // Checking if all cycle 1 puzzles are complete
                        #region Remaining Puzzles Check
                        if (PuzzleUIScript.puzzles[4] != null)
                        {
                            // If all four tutorial puzzles are complete
                            if (PuzzleUIScript.puzzles[4].isComplete)
                            {
                                // Set the sleeping bay trigger to active
                                GameObject.Find("RobotCellsOpen (2)").GetComponent<BoxCollider>().enabled = true;   // This is the sleeping bay that Bitbot starts off in
                            }
                        }
                        #endregion
                        break;
                }
                break;
            case 2: // The second game cycle
                break;
            case 3:
                break;
        }
        #endregion

    }

    #region Update Finished Puzzles
    public void UpdateFinishedPuzzles(Text taskText)
    {
        //set the value of this puzzle (whichever one matches the puzzleID) to be "1" which equals true;
        puzzleList[puzzleID] = 1;

        taskText.color = Color.green;
    }
    #endregion

    #region Save System
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(playerMovementScript);

    } // End of SavePlayer()

    public void SavePlayerAndQuit()
    {
        SaveSystem.SavePlayer(playerMovementScript);
        Application.Quit();
    }

    public void LoadPlayer()
    {
        Debug.Log("Trying to load the player data");
        PlayerData playerData = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = playerData.position[0];
        position.y = playerData.position[1];
        position.z = playerData.position[2];

        // Set the player so they start in the nearest sleeping bay
        //transform.position = position;
        thePlayer.transform.position = position;
        menuManagerScript.PlayGame();

    } // End of LoadPlayer()

    public void NewGameStart()
    {

        Cycle++; //add a value to the number of cycles.
        //Skybox.ge Assets/Art/Skybox Stuff/Faces/skybox blue/cubemap.png

        //Reset all world events and conditions. 

    }
    #endregion

}
