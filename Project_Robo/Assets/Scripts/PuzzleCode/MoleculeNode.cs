using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Rewired;

public class MoleculeNode : MonoBehaviour
{
    // Mandatory Rewired Nonsense
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

    // Boolean control variables
    public bool moving = false;
    public bool satisfied = false;
    public bool ruleMet = false;

    // Variables to be set in the inspector
    [SerializeField] private bool canMove;
    [SerializeField] public int numConnections;
    [SerializeField] public Color nodeColor = Color.red;
    [SerializeField] public string nodeType = "basic";
    [SerializeField] public Image thisNode;
    [SerializeField] public Image cursor;
    [SerializeField] public Sprite triangle;
    [SerializeField] public Sprite spikes;
    [SerializeField] public GameObject nearbyDetector;
    [SerializeField] public Connector connectorPrefab;
    [SerializeField] private AudioClip nodeConnect;

    // Connection Variables
    public List<Connector> connectorList = new List<Connector>();
    public List<MoleculeNode> currentlyConnected = new List<MoleculeNode>();
    List<MoleculeNode> nearby = new List<MoleculeNode>();
    public int currentConnections = 0;

    // Component and Script Communication Variables
    public RectTransform rect = null;
    public PuzzleMaster master = null;
    public CursorFollow follow = null;
    public Collider2D collide = null;

    // Constant for Color checking
    private static float delta = 0.001f;

    // Start is called before the first frame update
    // Basically just Script communication and initial setup from inspector variables
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

        if (nodeType == "basic" || nodeType == "goalBasic")
        {
            ruleMet = true;
        }

        if (numConnections == 3 && triangle != null)
        {
            thisNode.sprite = triangle;
        }

        if (nodeType == "spiked")
        {
            thisNode.sprite = spikes;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement code, only matters for nodes that can actually move
        // Only activates when touching the cursor
        if (canMove && collide.IsTouching(cursor.GetComponent<Collider2D>()))
        {
            // follow is the cursor, if it doesn't already have a node attached, pick this node up when "Action" is pressed
            // Otherwise, put put this node down if it was being carried
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

        // Safety code, current connections must be equal to the number of elements in the currentlyConnected list
        currentConnections = currentlyConnected.Count;

        // Method calls to divide work
        HandleMotion();
        HandleConnections();

        // Basic and goalBasic nodes do not have special rules
        if (nodeType != "basic" && nodeType != "goalBasic")
            HandleRules();


        satisfied = isSatisfied();
    }

    // This code was originally larger, but motion was simplified by a lot. I'm going to leave it separate for now in case it has to change later
    void HandleMotion()
    {
        if (moving)
            rect.transform.position = cursor.rectTransform.position;
    }

    // Connection were probably the most difficult part of the MoleculeNode's code, and admittedly their rotation still didn't turn out the way I wanted it to
    void HandleConnections()
    {
        // Can't connect if the node is moving
        if (!moving)
        {
            // Gather nearby nodes from the PuzzleMaster this node is a part of
            if (master != null && nearbyDetector != null)
                nearby = master.nearbyNodes(this);

            // Only run if there are other nodes nearby
            if (nearby.Count > 0)
            {
                // loop through the list of nearby nodes
                for (int i = 0; i < nearby.Count; i++)
                {
                    MoleculeNode currentNode = nearby[i];

                    // If this nearby node has already been connected to, don't connect to it again
                    // The node can only connect to new nodes when it hasn't reached its max numConnections, and the other node hasn't reached its max numConnections
                    if (!currentlyConnected.Contains(currentNode) && currentConnections < numConnections && currentNode.currentConnections < currentNode.numConnections)
                    {

                        Vector3 connectorPos = new Vector3((transform.position.x + nearby[i].transform.position.x) / 2, (transform.position.y + nearby[i].transform.position.y) / 2, 10);
                        Connector connector = Instantiate(connectorPrefab, connectorPos, transform.rotation, transform);

                        // Assign connector variables
                        connector.node1 = this;
                        connector.node2 = nearby[i];

                        // Rotate it to face this node (doesn't really work well, rotation is something that needs polish)
                        connector.transform.LookAt(transform);

                        // Add connections to their respecitive lists
                        connectorList.Add(connector);
                        currentlyConnected.Add(currentNode);

                        // Connector code to finish connecting to the other node (add itself to that node's lists)
                        connector.finishConnection();

                        // increment connections
                        currentConnections++;

                        // Play connection sound
                        if (nodeConnect != null)
                        {
                            AudioSource.PlayClipAtPoint(nodeConnect, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
                        }
                    }

                }

                // Remove nodes that you are no longer connected to (nodes that have moved)
                for (int i = 0; i < currentlyConnected.Count; i++)
                {
                    // Check if a node in the connections is no longer nearby
                    if (!nearby.Contains(currentlyConnected[i]))
                    {
                        // Loop through that nodes connections 
                        for (int j = 0; j < connectorList.Count; j++)
                        {
                            if (connectorList[j].node2 == currentlyConnected[i])
                            {
                                //Connector temp = connectorList[j];
                                Destroy(connectorList[j].gameObject);
                                connectorList.RemoveAt(j);
                                
                            }
                        }

                        // Remove the connector from this node's list
                        currentlyConnected.RemoveAt(i);
                    }
                }
            }
        }
        else // If the node is moving, remove all of its connections
        {
            for (int i = 0; i < connectorList.Count; i++)
            {
                Connector temp = connectorList[i];
                connectorList.RemoveAt(i);
                Destroy(temp.gameObject);
                //Debug.Log("Current Connections: " + currentConnections);
            }

            for (int i = 0; i < currentlyConnected.Count; i++)
            {
                currentlyConnected[i].currentlyConnected.Remove(this);
                currentlyConnected.RemoveAt(i);
            }
        }
    }

    // Handles the special rules that certain nodes may have (currently just spiked nodes)
    void HandleRules()
    {
        // Check what kind of node this is
        if (nodeType == "spiked")
        {
            int sameColor = 0;

            // Only check other colors if there is at least one node connected to this one
            if (currentlyConnected.Count > 0)
            {
                // Check the color of all nearby nodes and compare it to this node's color, incrementing sameColor if they are the same
                for (int i = 0; i < currentlyConnected.Count; i++)
                {
                    if (Mathf.Abs(currentlyConnected[i].nodeColor.linear.r - nodeColor.linear.r) <= delta &&
                        Mathf.Abs(currentlyConnected[i].nodeColor.linear.g - nodeColor.linear.g) <= delta &&
                        Mathf.Abs(currentlyConnected[i].nodeColor.linear.b - nodeColor.linear.b) <= delta)
                        sameColor++;
                } 
            }

            //Debug.Log("sameColor: " + sameColor);

            // Spiked nodes rule is only met if there is exactly one node of the same color connected to it
            if (sameColor == 1)
                ruleMet = true;
            else
                ruleMet = false;
        }
    }

    // If a node has met both its rule and its maximum numConnections, then it is "satisfied"
    bool isSatisfied()
    {
        if (currentConnections == numConnections && ruleMet)
            return true;
        else
            return false;
    }
}
