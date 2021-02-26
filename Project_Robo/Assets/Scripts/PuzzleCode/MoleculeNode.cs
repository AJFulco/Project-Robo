using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Rewired;

public class MoleculeNode : MonoBehaviour
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

    public bool moving = false;
    [SerializeField] private bool canMove;
    [SerializeField] public Color nodeColor = Color.red;
    [SerializeField] public string nodeType = "basic";
    [SerializeField] public Image thisNode;
    [SerializeField] public Image cursor;
    public RectTransform rect = null;
    public CursorFollow follow = null;
    public Collider2D collide = null;

    // Start is called before the first frame update
    void Start()
    {
        if (thisNode != null)
        {
            thisNode.color = nodeColor;
        }

        if (cursor != null)
            follow = cursor.GetComponent<CursorFollow>();

        rect = GetComponent<RectTransform>();
        collide = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && collide.IsTouching(cursor.GetComponent<Collider2D>()))
        {
            if (!follow.carrying)
            {
                if (player.GetButtonDown("Action")) {

                    moving = true;
                    follow.carrying = true;
                }
            }
            else
            {
                if (player.GetButtonDown("Action"))
                {

                    moving = false;
                    follow.carrying = false;
                }
            }
        }


        HandleMotion();
    }

    void HandleMotion()
    {
        if (moving)
        {
            rect.transform.position = cursor.rectTransform.position;
        }
    }
}
