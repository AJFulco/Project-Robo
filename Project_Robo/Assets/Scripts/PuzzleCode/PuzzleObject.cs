using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PuzzleObject : MonoBehaviour
{ 

    [SerializeField] private GameObject selfObject;
    [SerializeField] private GameObject placementCube;
    [SerializeField] public int puzzleID;

    public bool cleared = false;
    public bool near = false;

    private PuzzleUI UI = null;
    public BoxCollider collider = null;



    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("PuzzleReadyCanvas").GetComponent<PuzzleUI>();

        if (selfObject != null)
        {
            Instantiate(selfObject, transform.position, transform.rotation, transform);
        }

        if (placementCube != null)
        {
            Destroy(placementCube.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.puzzles[puzzleID].GetComponent<PuzzleMaster>().isComplete)
            cleared = true;
        else
            cleared = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            UI.currentPuzzle = puzzleID;

            for (int i = 0; i < UI.puzzles.Count; i++)
            {
                if (i == puzzleID)
                    UI.puzzles[i].SetActive(true);
                else
                    UI.puzzles[i].SetActive(false);
            }

            UI.togglePuzzle();
        }
    }
}
