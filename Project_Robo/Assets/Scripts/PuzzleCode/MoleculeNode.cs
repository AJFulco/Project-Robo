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
    [SerializeField] public int numConnections;
    [SerializeField] public Color nodeColor = Color.red;
    [SerializeField] public string nodeType = "basic";
    [SerializeField] public Image thisNode;
    [SerializeField] public Image cursor;
    [SerializeField] public GameObject connectorPrefab;

    List<MoleculeNode> nearby = new List<MoleculeNode>();
    List<MoleculeNode> currentlyConnected = new List<MoleculeNode>();
    public int currentConnections = 0;

    public RectTransform rect = null;
    public PuzzleMaster master = null;
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

        master = GetComponentInParent<PuzzleMaster>();
        rect = GetComponent<RectTransform>();
        collide = GetComponent<Collider2D>();

        if (master != null)
            Debug.Log("Found it");
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
        HandleConnections();
    }

    void HandleMotion()
    {
        if (moving)
        {
            rect.transform.position = cursor.rectTransform.position;
        }
    }

    void HandleConnections()
    {
        if (!moving)
        {
            if (master != null)
                nearby = master.nearbyNodes(this, numConnections);

            if (nearby.Count > 0)
            {
                for (int i = 0; i < nearby.Count; i++)
                {
                    MoleculeNode currentNode = nearby[i];

                    if (!currentlyConnected.Contains(currentNode))
                    {
                        Instantiate<GameObject>(connectorPrefab);
                        currentlyConnected.Add(currentNode);
                    }
                }
            }
        }
    }
}
