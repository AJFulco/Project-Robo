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
    private AudioSource audioComponent;
    [SerializeField] AudioClip ascend;
    [SerializeField] AudioClip descend;
    [SerializeField] GameObject lift;

    // Start is called before the first frame update
    void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canElevate)
        {
            if (levelManager.Cycle == 3)
            {
                if (player.GetButtonSinglePressHold("Interact"))
                {
                    Debug.Log("Dubgg");
                    if (firstFloor && ascend != null) audioComponent.PlayOneShot(ascend);
                    else if (descend != null) audioComponent.PlayOneShot(descend);
                    ElevatePlayer(firstFloor);
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
    public void ElevatePlayer(bool firstFloor)
    {
        if (firstFloor)
        {
            lift.GetComponent<Animator>().SetBool("firstFloor", true);
            Debug.Log("To 2nd Floor");
        }
        else
        {
            lift.GetComponent<Animator>().SetBool("firstFloor", false);
            Debug.Log("To 1st Floor");
        }
    }
}
