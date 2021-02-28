using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{

    private ArrayList nodes = new ArrayList();
    private List<MoleculeNode> allNodes = new List<MoleculeNode>();

    // Start is called before the first frame update
    void Start()
    {
        nodes.Add(GetComponentsInChildren<MoleculeNode>());
        Debug.Log(nodes.Count);

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetType() == typeof(MoleculeNode))
                allNodes.Add((MoleculeNode)nodes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<MoleculeNode> nearbyNodes(MoleculeNode node, int maxConnectors)
    {
        List<MoleculeNode> nodeList = new List<MoleculeNode>(capacity: maxConnectors);
        int connectedNodes = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (allNodes[i] != node)
            {
                if (node.GetComponentInChildren<Collider2D>().IsTouching(node.GetComponent<Collider2D>()))
                {
                    nodeList.Add(allNodes[i]);
                    connectedNodes++;
                    Debug.Log("Added");
                }

                if (connectedNodes == maxConnectors)
                    break;
            }
        }


        return nodeList;
    }
}
