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
    public bool satisfied = false;
    public bool ruleMet = false;
    [SerializeField] private bool canMove;
    [SerializeField] public int numConnections;
    [SerializeField] public Color nodeColor = Color.red;
    [SerializeField] public string nodeType = "basic";
    [SerializeField] public Image thisNode;
    [SerializeField] public Image cursor;
    [SerializeField] public Sprite triangle;
    [SerializeField] public Image spikes;
    [SerializeField] public GameObject nearbyDetector;
    [SerializeField] public Connector connectorPrefab;

    public List<Connector> connectorList = new List<Connector>();
    public List<MoleculeNode> currentlyConnected = new List<MoleculeNode>();
    List<MoleculeNode> nearby = new List<MoleculeNode>();
    public int currentConnections = 0;

    public RectTransform rect = null;
    public PuzzleMaster master = null;
    public CursorFollow follow = null;
    public Collider2D collide = null;

    private Image childSpikes = null;
    private static float delta = 0.001f;

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
            childSpikes = Instantiate<Image>(spikes, transform.position, transform.rotation, master.transform);
            childSpikes.rectTransform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            childSpikes.color = nodeColor;

            Image[] spikeArray = childSpikes.GetComponentsInChildren<Image>();

            for (int i = 0; i < spikeArray.Length; i++)
                spikeArray[i].color = nodeColor;
        }
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

        currentConnections = currentlyConnected.Count;

        HandleMotion();
        HandleConnections();

        if (nodeType != "basic" && nodeType != "goalBasic")
            HandleRules();

        satisfied = isSatisfied();
    }

    void HandleMotion()
    {
        if (moving)
        {
            rect.transform.position = cursor.rectTransform.position;

            if (childSpikes != null)
                childSpikes.rectTransform.position = cursor.rectTransform.position;
        }
    }

    void HandleConnections()
    {
        if (!moving)
        {
            if (master != null && nearbyDetector != null)
                nearby = master.nearbyNodes(this);

            if (nearby.Count > 0)
            {
                for (int i = 0; i < nearby.Count; i++)
                {
                    MoleculeNode currentNode = nearby[i];

                    if (!currentlyConnected.Contains(currentNode) && currentConnections < numConnections && currentNode.currentConnections < currentNode.numConnections)
                    {
                        Vector3 connectorPos = new Vector3((transform.position.x + nearby[i].transform.position.x) / 2, (transform.position.y + nearby[i].transform.position.y) / 2, 10);
                        Connector connector = Instantiate(connectorPrefab, connectorPos, transform.rotation, transform);

                        connector.node1 = this;
                        connector.node2 = nearby[i];
                        connector.transform.LookAt(transform);
                        connectorList.Add(connector);
                        currentlyConnected.Add(currentNode);
                        connector.finishConnection();
                        currentConnections++;
                    }

                }

                for (int i = 0; i < currentlyConnected.Count; i++)
                {
                    if (!nearby.Contains(currentlyConnected[i]))
                    {
                        for (int j = 0; j < connectorList.Count; j++)
                        {
                            if (connectorList[j].node2 == currentlyConnected[i])
                            {
                                Connector temp = connectorList[j];
                                Destroy(connectorList[j].gameObject);
                                connectorList.RemoveAt(j);
                                
                            }
                        }
                        currentlyConnected.RemoveAt(i);
                    }
                }
            }
        }
        else
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

    void HandleRules()
    {
        if (nodeType == "spiked")
        {
            int sameColor = 0;

            if (currentlyConnected.Count > 0)
            {
                for (int i = 0; i < currentlyConnected.Count; i++)
                {
                    if (Mathf.Abs(currentlyConnected[i].nodeColor.linear.r - nodeColor.linear.r) <= delta &&
                        Mathf.Abs(currentlyConnected[i].nodeColor.linear.g - nodeColor.linear.g) <= delta &&
                        Mathf.Abs(currentlyConnected[i].nodeColor.linear.b - nodeColor.linear.b) <= delta)
                        sameColor++;
                } 
            }

            //Debug.Log("sameColor: " + sameColor);

            if (sameColor == 1)
                ruleMet = true;
        }
    }

    bool isSatisfied()
    {
        if (currentConnections == numConnections && ruleMet)
            return true;
        else
            return false;
    }
}
