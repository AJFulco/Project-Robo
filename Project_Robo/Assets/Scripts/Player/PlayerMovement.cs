using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NEED THIS TO BE ABLE TO USE ANY KIND OF REWIRD FEATURES
using Rewired; 

public class PlayerMovement : MonoBehaviour
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
    private void Awake() {
        
        //THIS LINE IS CRUCIAL! IF IT IS NOT IN THE SCRIPT REWIRD WONT READ THE INPUTS FROM THE PROPER CONTROL INPUT!
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        //Example of Rewired code
        if (player.GetButtonDown("Action")) //in the "" you write the name of the action you labled in the Rewired Input Manager
        {
            //notice how we used "player" instead of "Input"
            return;
        }

        if (player.GetButtonDown("ShowPuzzle"))
        {
            
        }
    }
}
