using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;


// This Script handles the cursor follow code, and some level of interaction with molecule nodes
public class CursorFollow : MonoBehaviour
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

    public PuzzleUI UI = null;
    public RectTransform rect = null;
    public Collider2D collide = null;
    public int currentNode = 2;

    public bool carrying = false;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("PuzzleReadyCanvas").GetComponent<PuzzleUI>();
        rect = this.GetComponent<RectTransform>();
        collide = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool mouseMode = true;

        if (player.controllers.GetLastActiveController() == player.controllers.GetLastActiveController<Keyboard>() ||
            player.controllers.GetLastActiveController() == player.controllers.GetLastActiveController<Mouse>())
        {
            mouseMode = true;
        }
        else
        {
            mouseMode = false;
        }


        if (mouseMode)
        {
            Vector2 mousePosition = new Vector2();

            mousePosition.x = Input.mousePosition.x;
            mousePosition.y = Input.mousePosition.y;

            rect.position = mousePosition;
        }
        else
        {
            List<MoleculeNode> nodes = UI.puzzles[UI.currentPuzzle].GetNodes();
            
            if (player.GetButtonDown("NextNode"))
            {
                if (nodes[currentNode].moving)
                    nodes[currentNode].moving = false;

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

            if (player.GetButtonDown("PrevNode"))
            {
                if (nodes[currentNode].moving)
                    nodes[currentNode].moving = false;

                if (nodes[currentNode - 1].nodeType == "goalBasic")
                {
                    currentNode = nodes.Count - 1;
                }
                else
                    currentNode--;
            }

            if (nodes[currentNode].moving)
            {
                nodes[currentNode].rect.position = new Vector2(nodes[currentNode].rect.position.x + 3 * player.GetAxis("Horizontal"),
                                                                nodes[currentNode].rect.position.y + 3 * player.GetAxis("Vertical"));
            }

            rect.position = nodes[currentNode].rect.position;
        }
    }
}
