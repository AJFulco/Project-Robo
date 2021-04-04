using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*tracks the player progression for all the following
    * Doors 
    * Puzzles
    * Cycles
    * Soon to be more..
    */
    //All of the scripts we pull from
    public PuzzleUI PuzzleUIScript;
    public PuzzleMaster PuzzleMasterScript;//need for the boolean isComplete
    [SerializeField] public List<DoorBehavior> DoorList;
    //Variables
    public int Cycle = 0;
    [SerializeField] public ArrayList CubeMapArrayList = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        Cycle++;
        //Skybox.ge Assets/Art/Skybox Stuff/Faces/skybox blue/cubemap.png
    }

    // Update is called once per frame
    void Update()
    {
        //Cycle switch Statement
        switch (Cycle)
        {
            case 1://The first cycle of the game!!!
                //if (PuzzleUIScript.puzzles[0]! == null &&
                //    PuzzleUIScript.puzzles[1]! == null &&
                //    PuzzleUIScript.puzzles[2]! == null &&
                //    PuzzleUIScript.puzzles[3]! == null){
                //    //make sure they all exist
                //    if (PuzzleUIScript.puzzles[0].isComplete &&
                //        PuzzleUIScript.puzzles[1].isComplete &&
                //        PuzzleUIScript.puzzles[2].isComplete &&
                //        PuzzleUIScript.puzzles[3].isComplete) {
                //        //open the first door
                //        DoorList[0].Open();
                //    }
                //}
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
