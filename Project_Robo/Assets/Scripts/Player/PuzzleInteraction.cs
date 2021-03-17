using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


/* 
 * Do we even need this script anymore? I could really just move the exitPuzzle command to PuzzleUI and it should work just the same. 
 * Everything that was here was commented out for a while, so it clearly wasn't needed.
*/

public class PuzzleInteraction : MonoBehaviour
{
    // Mandatory Rewired Stuff
    #region Rewired Stuff
    //Here, we establish what a name that we will use instead of "Input" 
    private Rewired.Player player;
    //The playerID lables which player you are, 0=P1, 1=P2, and so on.
    public int playerId = 0;
    //Stuff for rumble support
    int motorIndex0 = 0;
    int motorIndex1 = 1;
    #endregion

    #region Awake
    //Awake function is code that is executed before the Start or OnEnable functions. 
    private void Awake()
    {

        //THIS LINE IS CRUCIAL! IF IT IS NOT IN THE SCRIPT REWIRD WONT READ THE INPUTS FROM THE PROPER CONTROL INPUT!
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }
    #endregion


    // CHRIS CODE
    private PuzzleUI UI = null;

    //public PlayerMovement playerMovement;    // Reference to PlayerMovement

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("PuzzleReadyCanvas").GetComponent<PuzzleUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // The player can press escape to leave a puzzle
        if (player.GetButtonDown("Exit"))
        {
            UI.exitPuzzle();
            //playerMovement.isInAMenu = false;   // Added to re-enable the player movement
        }
    }


}
