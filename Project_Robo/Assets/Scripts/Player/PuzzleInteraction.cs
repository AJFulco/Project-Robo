using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PuzzleInteraction : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas").GetComponent<PuzzleUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("ShowPuzzle"))
        {
            UI.togglePuzzle1();
        }

        if (player.GetButtonDown("Exit"))
        {
            UI.exitPuzzle();
        }
    }


}
