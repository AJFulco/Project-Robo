using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PuzzleObject : MonoBehaviour
{ 

    [SerializeField] private GameObject selfObject;
    [SerializeField] private GameObject placementCube;
    [SerializeField] public int puzzleID;

    public bool cleared = false;
    public bool nearThis = false;

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UI.currentPuzzle = puzzleID;
            nearThis = true;
            UI.nearPuzzle = true;

            UI.puzzleNearText.gameObject.SetActive(true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (UI.puzzles[puzzleID].GetComponent<PuzzleMaster>().isComplete)
            UI.puzzleNearText.text = "This task has been completed. Press \"E\" to display it.";
        else
            UI.puzzleNearText.text = "Press \"E\" to display this task.";
    }

    public void OnTriggerExit(Collider other)
    {
        UI.currentPuzzle = -1;
        nearThis = false;
        UI.nearPuzzle = false;
        UI.puzzleNearText.gameObject.SetActive(false);
    }
}
