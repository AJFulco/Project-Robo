using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ConsoleButton : MonoBehaviour
{
    // Mandatory Rewired Nonsense
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

    private MenuManager menu = null;
    private LevelManager levelManager = null;
    [SerializeField] public Image cursor;
    private Collider2D collide = null;

    [SerializeField] string command;


    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("MenuCanvas").GetComponent<MenuManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        collide = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.Equals(cursor.transform.position))
        {
            Debug.Log("On button " + command);

            if (player.GetButtonDown("UISubmit"))
            {
                switch (command)
                {
                    case "new":
                        menu.PlayGame();
                        break;
                    case "load":
                        levelManager.LoadPlayer();
                        break;
                    case "options":
                        cursor.GetComponent<ConsoleCursor>().menuIndex = 0;
                        menu.LoadOptions();
                        break;
                    case "quit":
                        levelManager.SavePlayerAndQuit();
                        break;
                    case "resume":
                        menu.Resume();
                        break;
                    case "back":
                        cursor.GetComponent<ConsoleCursor>().menuIndex = 0;
                        menu.OptionsBack();
                        break;
                }
            }
        }
    }
}
