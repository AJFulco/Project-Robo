using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{

    [SerializeField] public GameObject victory;

    private MoleculeNode[] nodes = new MoleculeNode[100];
    private List<MoleculeNode> allNodes = new List<MoleculeNode>();
    private List<MoleculeNode> goals = new List<MoleculeNode>();

    // Start is called before the first frame update
    void Start()
    {
        nodes = GetComponentsInChildren<MoleculeNode>();
        Debug.Log(nodes.Length);

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].GetType() == typeof(MoleculeNode))
                allNodes.Add(nodes[i]);

            if (nodes[i] == null)
                break;
        }

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i].nodeType == "goalBasic")
                goals.Add(allNodes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool clearCheck = true;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (!allNodes[i].satisfied)
                clearCheck = false;
        }

        if (clearCheck)
            IsComplete();
    }

    public List<MoleculeNode> nearbyNodes(MoleculeNode node)
    {
        List<MoleculeNode> nodeList = new List<MoleculeNode>();
        int connectedNodes = 0;

        for (int i = 0; i < allNodes.Count; i++)
        {
            
            if (allNodes[i] != node && !allNodes[i].moving)
            {
                if (node.nearbyDetector.GetComponent<Collider2D>().IsTouching(allNodes[i].GetComponent<Collider2D>()) &&
                    allNodes[i].nearbyDetector.GetComponent<Collider2D>().IsTouching(node.GetComponent<Collider2D>()))
                {
                    nodeList.Add(allNodes[i]);
                    connectedNodes++;
                }
            }
        }


        return nodeList;
    }

    void IsComplete()
    {
        if (!victory.activeSelf)
        {
            victory.SetActive(true);
        }
    }
}
