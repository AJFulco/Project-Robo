using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;


// This Script handles the cursor follow code, and some level of interaction with molecule nodes
public class CursorFollow : MonoBehaviour
{
    // Mandatory Rewired Stuff
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

    // Script communication
    public PuzzleUI UI = null;
    public RectTransform rect = null;
    public Collider2D collide = null;
    public int currentNode = 2;

    public bool carrying = false;

    // Start is called before the first frame update
    void Start()
    {
        // Establish Script Communication
        UI = GameObject.Find("PuzzleReadyCanvas").GetComponent<PuzzleUI>();
        rect = this.GetComponent<RectTransform>();
        collide = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // mouseMode determines what puzzle control scheme is currently in use
        bool mouseMode = true;

        // Check Rewired to see what controller is currently being used
        if (player.controllers.GetLastActiveController() == player.controllers.GetLastActiveController<Keyboard>() ||
            player.controllers.GetLastActiveController() == player.controllers.GetLastActiveController<Mouse>())
        {
            mouseMode = true;
        }
        else
        {
            mouseMode = false;
        }

        // While in mouseMode controls
        if (mouseMode)
        {
            // Cursor is always located at the player's mouse
            Vector2 mousePosition = new Vector2();

            mousePosition.x = Input.mousePosition.x;
            mousePosition.y = Input.mousePosition.y;

            rect.position = mousePosition;
        }
        else // While in Controller mode controls
        {
            // Get a list of every node in the current puzzleMaster
            List<MoleculeNode> nodes = UI.puzzles[UI.currentPuzzle].GetNodes();
            
            // When the player presses NextNode
            if (player.GetButtonDown("NextNode"))
            {
                // Make sure the current node is no longer moving if it had been picked up beforehand
                if (nodes[currentNode].moving)
                    nodes[currentNode].moving = false;

                // Loop around to the first node after the goalNodes if the player has reached the end of the list
                // Assumes the first two nodes in the puzzle are goal nodes
                if (currentNode + 1 >= nodes.Count)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (nodes[i].nodeType != "goalBasic")
                        {
                            currentNode = i;
                            break;
                        }
                    }
                }
                else
                    currentNode++;
            }

            // When the player pushes PrevNode
            if (player.GetButtonDown("PrevNode"))
            {
                // Make sure the current node is no longer moving if it had been picked up beforehand
                if (nodes[currentNode].moving)
                    nodes[currentNode].moving = false;

                // Once the player has reached a goal node, loop around to the last node in the list
                // Assumes the first two nodes in the puzzle are goal nodes
                if (nodes[currentNode - 1].nodeType == "goalBasic")
                {
                    currentNode = nodes.Count - 1;
                }
                else
                    currentNode--;
            }

            // If the current node is moving, move it based on the input coming from the left Joystick, then set the cursor to the node's position
            if (nodes[currentNode].moving)
            {
                nodes[currentNode].rect.position = new Vector2(nodes[currentNode].rect.position.x + 3 * player.GetAxis("Horizontal"),
                                                                nodes[currentNode].rect.position.y + 3 * player.GetAxis("Vertical"));
            }

            rect.position = nodes[currentNode].rect.position;
        }
    }
}
