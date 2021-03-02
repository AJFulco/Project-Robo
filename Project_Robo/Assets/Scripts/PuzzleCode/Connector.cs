using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{

    private List<Connector> node2Connectors = new List<Connector>();

    public MoleculeNode node1 = null;
    public MoleculeNode node2 = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void finishConnection()
    {
        if (!node2.currentlyConnected.Contains(node1)) { 
            node2.currentlyConnected.Add(node1);

            Quaternion targetRot = new Quaternion(transform.rotation.x * -1, transform.rotation.y * -1, transform.rotation.z * -1, transform.rotation.w);

            Vector3 connectorPos = new Vector3((node1.transform.position.x + node2.transform.position.x) / 2, (node1.transform.position.y + node2.transform.position.y) / 2, 10);
            Connector connector = Instantiate(node1.connectorPrefab, connectorPos, targetRot, node2.transform);

            connector.node1 = node2;
            connector.node2 = node1;
            connector.transform.LookAt(node2.transform);

            if (!node2.connectorList.Contains(connector))
                node2.connectorList.Add(connector);

            if (!node2.currentlyConnected.Contains(node1))
                node2.currentlyConnected.Add(node1);
        }


    }

    public void OnDestroy()
    {
        if (node2 != null && node1 != null)
        {
            node2Connectors = node2.connectorList;

            for (int i = 0; i < node2Connectors.Count; i++)
            {
                if (node2Connectors[i] == null)
                {
                    break;
                }

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
