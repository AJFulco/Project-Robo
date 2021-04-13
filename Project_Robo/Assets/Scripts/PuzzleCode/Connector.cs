using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connector : MonoBehaviour
{
    // List of connectors on the second node
    private List<Connector> node2Connectors = new List<Connector>();

    // This Sprite
    [SerializeField] public List<Sprite> frames;
    [SerializeField] private Image selfImage = null;
    private int currentFrame = 0;

    // Molecule nodes this is connected to
    public MoleculeNode node1 = null;
    public MoleculeNode node2 = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFrame < (frames.Count * 2) - 1)
        {
            currentFrame++;
        }
        else
        {
            currentFrame = 0;
        }

        selfImage.sprite = frames[currentFrame / 2];
    }

    // Called by MoleculeNode to finish the connection between the two nodes
    public void finishConnection()
    {
        // If the other node2 is not currently connected to node1
        if (!node2.currentlyConnected.Contains(node1)) { 
            node2.currentlyConnected.Add(node1);

            // As for the initial rotation code, this one still likely needs a lot of tweaks
            Quaternion targetRot = new Quaternion(transform.rotation.x * -1, transform.rotation.y * -1, transform.rotation.z * -1, transform.rotation.w);

            // Place another connector near where the first one is (connectors were originally meant to face each other like: )(, but rotation needs work
            Vector3 connectorPos = new Vector3((node1.transform.position.x + node2.transform.position.x) / 2, (node1.transform.position.y + node2.transform.position.y) / 2, 10);
            Connector connector = Instantiate(node1.connectorPrefab, connectorPos, targetRot, node2.transform);

            // Assign connector variables
            connector.node1 = node2;
            connector.node2 = node1;
            connector.transform.LookAt(node2.transform);

            // the new connector and node1 to node2's lists
            if (!node2.connectorList.Contains(connector))
                node2.connectorList.Add(connector);

            if (!node2.currentlyConnected.Contains(node1))
                node2.currentlyConnected.Add(node1);
        }


    }

    // When this connector is destroyed, break its connections
    public void OnDestroy()
    {
        // If both nodes are still assigned to this connector
        if (node2 != null && node1 != null)
        {
            // assign node2Connectors for ease of code
            node2Connectors = node2.connectorList;

            // Loop through the Connector list and break connections
            for (int i = 0; i < node2Connectors.Count; i++)
            {
                if (node2Connectors[i] == null)
                {
                    break;
                }

                // Remove the other connector between the two nodes
                if (node2Connectors[i].node2 == node1)
                {
                    node2.currentlyConnected.Remove(node1);
                    node1.currentlyConnected.Remove(node2);

                    Connector temp = node2Connectors[i];
                    Destroy(node2Connectors[i].gameObject);
                    node2Connectors.RemoveAt(i);

                    break;
                }
            }


        }
    }
}
