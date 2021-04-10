using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    // Inspector fields
    [SerializeField] public GameObject victory;
    [SerializeField] private AudioClip puzzlePass;

    // Script Communitcation
    private MoleculeNode[] nodes = new MoleculeNode[100];
    private List<MoleculeNode> allNodes = new List<MoleculeNode>();
    private List<MoleculeNode> goals = new List<MoleculeNode>();
    public bool isComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get an array of all MoleculeNodes in the puzzle
        nodes = GetComponentsInChildren<MoleculeNode>();
        //Debug.Log(nodes.Length);

        // Convert Array to List for easier manipulation
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].GetType() == typeof(MoleculeNode))
                allNodes.Add(nodes[i]);

            if (nodes[i] == null)
                break;
        }

        // Get a list of specifically goal nodes
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i].nodeType == "goalBasic")
                goals.Add(allNodes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if the puzzle has been completed
        bool clearCheck = true;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (!allNodes[i].satisfied)
                clearCheck = false;
        }

        if (clearCheck)
            IsComplete();
        else
        {
            List<MoleculeNode> shakingNodes = new List<MoleculeNode>();
            bool connectCheck = true;

            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i].currentConnections == 0)
                    connectCheck = false;

                if (!allNodes[i].ruleMet || allNodes[i].currentConnections < allNodes[i].numConnections)
                    shakingNodes.Add(allNodes[i]);
                else
                    allNodes[i].shaking = 0;
            }

            if (connectCheck)
            {
                for (int i = 0; i < shakingNodes.Count; i++)
                {
                    if (shakingNodes[i].shaking == 0)
                        shakingNodes[i].shaking = 120;
                }
            }
        }
    }

    // Get method for allNodes
    public List<MoleculeNode> GetNodes()
    {
        return allNodes;
    }

    // Nearby nodes returns a list of all nodes in the puzzle that within one particular node's nearbyDetector
    public List<MoleculeNode> nearbyNodes(MoleculeNode node)
    {

        List<MoleculeNode> nodeList = new List<MoleculeNode>();

        for (int i = 0; i < allNodes.Count; i++)
        {
            // Only adds a node to the list if the node is not moving and if both nodes are touching each other
            if (allNodes[i] != node && !allNodes[i].moving)
            {
                if (node.nearbyDetector.GetComponent<Collider2D>().IsTouching(allNodes[i].GetComponent<Collider2D>()) &&
                    allNodes[i].nearbyDetector.GetComponent<Collider2D>().IsTouching(node.GetComponent<Collider2D>()))
                {
                    nodeList.Add(allNodes[i]);
                }
            }
        }


        return nodeList;
    }

    // When the puzzle is complete, this method shows the victory text and plays the puzzlePass sound clip
    void IsComplete()
    {
        if (!victory.activeSelf)
        {
            victory.SetActive(true);
            isComplete = true;

            if (puzzlePass != null)
            {
                GameObject.Find("PuzzleReadyCanvas").GetComponent<AudioSource>().PlayOneShot(puzzlePass);
            }
        }
    }
}
