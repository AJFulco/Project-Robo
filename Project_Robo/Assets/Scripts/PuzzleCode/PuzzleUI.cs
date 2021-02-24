using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PuzzleUI : MonoBehaviour
{ 
    [SerializeField] public GameObject puzzle1;
    private ArrayList puzzle1Nodes;

    // Start is called before the first frame update
    void Start()
    {
        puzzle1.SetActive(false);
        puzzle1Nodes.Add(puzzle1.GetComponentsInChildren<Image>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void togglePuzzle1()
    {
        if (puzzle1.activeSelf)
            puzzle1.SetActive(false);
        else
            puzzle1.SetActive(true);
    }

}
