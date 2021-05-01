using System;
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
    /// 2 = Puzzle gameplay
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
    [Space]
    public GameObject[] cutSceneCams;

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

    [Space(2)]
    [Header("Sleeping Bays")]
    // Sleeping Bays
    [SerializeField] public List<GameObject> firstFlrSleepingBays;
    [SerializeField] public List<GameObject> secondFlrSleepingBays;

    [Space]
    public AudioSource doorOpenedFan = null;

    private bool cycleOneScene = false;

    public bool inMenu;
    public bool inCutScene;

    public bool isBusy()
    {
        return inMenu || inCutScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        IncrementCycle();
        IncrementDoorSwitch();
        //Skybox.ge Assets/Art/Skybox Stuff/Faces/skybox blue/cubemap.png
    }

    

    // Update is called once per frame
    void Update()
    {
        #region Player State Switch Statement
        switch (playerState)
        {
            case 0: //if this is 0 we are in the main menu code.
                //puzzleUI.SetActive(false);
                //player movment code 
                playerMovementScript.enabled = false;

                //the virtual cameras 
                virtualCams[0].SetActive(true);
                virtualCams[1].SetActive(false);
                break;
            case 1: //player is allowed overworld movement. 
                //puzzleUI.SetActive(true);
                //player movement code
                playerMovementScript.enabled = true;
                virtualCams[1].SetActive(true);
                virtualCams[0].SetActive(false);
                break;
            case 2: 
                break;
            case 3: 
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
                        if (CheckPuzzlesExist(0,1))
                        {
                            // If both tutorial puzzles are complete
                            if (CheckPuzzlesComplete(0,1))
                            {
                                Debug.Log("Both tutorial puzzles complete, door should be open");
                                if (cycleOneScene == false)
                                {
                                    StartCoroutine(DoorOpenCutScene(DoorSwitch - 1));
                                }

                            }
                        }
                        #endregion
                        break;
                    case 2: // Checking if all cycle 1 puzzles are complete
                        #region Remaining Puzzles Check
                        if (CheckPuzzlesExist(2,3))
                        {
                            // If all four tutorial puzzles are complete
                            if (CheckPuzzlesComplete(2,3))
                            {
                                // Set the sleeping bay trigger to active
                                //Debug.Log("All First Cycle puzzles complete, head to sleep");
                                ActivateSleepBays();
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                
                break;
            case 2: // The second game cycle
                switch (DoorSwitch)     // Opening doors in Cycle 1
                {
                    case 2: // Checking if puzzles 4,5 are complete
                        #region Opening lots of doors
                        //make sure they all exist
                        if (CheckPuzzlesExist(4, 5))
                        {
                            // If both tutorial puzzles are complete
                            if (CheckPuzzlesComplete(4, 5))
                            {

                                DoorList[DoorSwitch - 1].Open();
                                IncrementDoorSwitch();
                                DoorList[DoorSwitch - 1].Open();
                                IncrementDoorSwitch();
                                /*Debug.Log("Both puzzles complete, doors should be open");
                                if (cycleOneScene == false)
                                {
                                    StartCoroutine(DoorOpenCutScene(DoorSwitch - 1));//DoorSwitch is at 2
                                    StartCoroutine(DoorOpenCutScene(DoorSwitch - 1));//DoorSwitch is at 3
                                }*/

                            }
                        }
                        break;
                    case 4:
                        if (CheckPuzzlesExist(6, 6))
                        {
                            // If all four tutorial puzzles are complete
                            if (CheckPuzzlesComplete(6, 6))
                            {
                                DoorList[DoorSwitch - 1].Open();
                                IncrementDoorSwitch();
                            }
                        }
                        break;
                    case 5:
                        if (CheckPuzzlesExist(10, 10))
                        {
                            // If all four tutorial puzzles are complete
                            if (CheckPuzzlesComplete(10, 10))
                            {
                                DoorList[DoorSwitch - 1].Open();
                                IncrementDoorSwitch();
                                DoorList[DoorSwitch - 1].Open();
                                IncrementDoorSwitch();
                            }
                        }
                        break;
                        #endregion
                    case 7: // Checking if all cycle 2
                        //needs to be case four
                        #region Remaining Puzzles Check
                        if (CheckPuzzlesExist(7, 10))
                        {
                            // If all four tutorial puzzles are complete
                            if (CheckPuzzlesComplete(7, 10))
                            {
                                // Set the sleeping bay trigger to active
                                Debug.Log("All Second Cycle puzzles complete, head to sleep");
                                ActivateSleepBays();
                                IncrementDoorSwitch();//Doorswith++ but fancier
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                break;
            case 3:
                break;
        }
        #endregion

    }
    #region Connor adds a bunch of small functions to make busywork faster later.
    private void ActivateSleepBays()//sets all the colliders in the sleepbays to true once the player finishes all the puzzles in a cylce
    {
        GameObject.Find("RobotCellsOpen").GetComponent<BoxCollider>().enabled = true;   
        GameObject.Find("RobotCellsOpen (1)").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("RobotCellsOpen (2)").GetComponent<BoxCollider>().enabled = true;  
        GameObject.Find("RobotCellsOpen (3)").GetComponent<BoxCollider>().enabled = true;  
        GameObject.Find("RobotCellsOpen (4)").GetComponent<BoxCollider>().enabled = true;  
        GameObject.Find("RobotCellsOpen (5)").GetComponent<BoxCollider>().enabled = true;  
    }
    public void DeactivateSleepBays()//sets all the colliders in the sleepbays to true once the player finishes all the puzzles in a cylce
    {
        GameObject.Find("RobotCellsOpen").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("RobotCellsOpen (1)").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("RobotCellsOpen (2)").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("RobotCellsOpen (3)").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("RobotCellsOpen (4)").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("RobotCellsOpen (5)").GetComponent<BoxCollider>().enabled = false;
    }
    public void IncrementCycle()
    {
        if (Cycle < 3)
        {
            Cycle++;
        }
        else { Debug.Log("thats not supposed to happen! Too many cycles!"); }
    }
    private void IncrementDoorSwitch() 
    {
        if (DoorSwitch < 15) {DoorSwitch++;}
        else { Debug.Log("thats not supposed to happen! Too many doors!"); }
    }
    public Boolean CheckPuzzlesExist(int start, int end)//checks to make sure that the following inclusive puzzles are not Null
    {
        for (int i = start; i <= end; i++) {
            if (PuzzleUIScript.puzzles[i] == null) 
            {
                return false;
            }
        }
        return true;

    }
    public Boolean CheckPuzzlesComplete(int start, int end)//checks to make sure that the following inclusive puzzles are not Null
    {
        for (int i = start; i <= end; i++)
        {
            if (PuzzleUIScript.puzzles[i].isComplete == false)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Door Unlocked Cutscene
    IEnumerator DoorOpenCutScene(int doorNumb)//input the number of the current door switch - 1 so that the rest of the code works :)
    {
        inCutScene = true;
        //make player busy
        //playerMovementScript.enabled = false;

        //activate the camera for that cycle.
        cutSceneCams[doorNumb].SetActive(true);
        virtualCams[1].SetActive(false);

        yield return new WaitForSeconds(1.0f);
        //activate the door
        if (!DoorList[doorNumb].isOpen)
        {
            DoorList[doorNumb].Open();
            IncrementDoorSwitch();
        }

        //play music
        //doorOpenedFan.Play();
        yield return new WaitForSeconds(2.0f);//200 is wack, 2 for testing :/
        //toggle off camera
        cutSceneCams[doorNumb].SetActive(true);
        virtualCams[1].SetActive(false);
        yield return new WaitForSeconds(1.0f);
        //restore player movement
        playerMovementScript.enabled = true;
        inCutScene = false;
        cycleOneScene = true;
    }

    #endregion

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
        SaveSystem.SavePlayer(playerMovementScript, this);

    } // End of SavePlayer()

    public void SavePlayerAndQuit()
    {
        SaveSystem.SavePlayer(playerMovementScript, this);
        Application.Quit();
        Debug.Log("Saving player data and quiting the game...");
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
