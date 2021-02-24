using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MoleculeNode : MonoBehaviour
{ 

    public bool moving = false;
    [SerializeField] public Color nodeColor = Color.red;
    [SerializeField] public string nodeType = "basic";
    [SerializeField] public Image thisNode;

    // Start is called before the first frame update
    void Start()
    {
        if (thisNode != null)
        {
            thisNode.color = nodeColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
