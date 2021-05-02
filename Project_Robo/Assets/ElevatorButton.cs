using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ElevatorButton : MonoBehaviour
{
    #region Rewired Stuff
    //Here, we establish what a name that we will use instead of "Input" 
    private Rewired.Player player;
    //The playerID lables which player you are, 0=P1, 1=P2, and so on.
    public int playerId = 0;
    //Stuff for rumble support
    //int motorIndex0 = 0;
    //int motorIndex1 = 1;
    #endregion

    #region Awake
    //Awake function is code that is executed before the Start or OnEnable functions. 
    private void Awake()
    {
        //THIS LINE IS CRUCIAL! IF IT IS NOT IN THE SCRIPT REWIRD WONT READ THE INPUTS FROM THE PROPER CONTROL INPUT!
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }
    #endregion
    [SerializeField] GameObject gamer;//this is the player, but that was already taken so i use gamer
    [SerializeField] bool firstFloor;
    [SerializeField] LevelManager levelManager;
    public bool canElevate = false;
    public PlayerMovement pM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canElevate) {
            if (levelManager.Cycle == 3) {
                if (player.GetButtonSinglePressHold("Interact")) 
                {
                    Debug.Log("Dubgg");
                    pM.TeleportPlayer(firstFloor);
                    firstFloor = !firstFloor;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            canElevate = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            canElevate = false;
        }
    }
}
