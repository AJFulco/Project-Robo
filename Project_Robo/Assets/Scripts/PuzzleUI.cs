using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PuzzleUI : MonoBehaviour
{ 
    [SerializeField] public GameObject puzzle1;


    // Start is called before the first frame update
    void Start()
    {
        puzzle1.SetActive(false);
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
