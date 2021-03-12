using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PuzzleObject : MonoBehaviour
{ 

    [SerializeField] private GameObject selfObject;
    [SerializeField] private GameObject placementCube;
    [SerializeField] private Text taskTextPrefab;
    [SerializeField] public int puzzleID;
    [SerializeField] public string taskName = "Observe the Monitor";
    public GameObject taskHolder;
    public Text taskText;

    public bool cleared = false;
    public bool nearThis = false;

    private PuzzleUI UI = null;



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

        taskHolder = GameObject.FindGameObjectWithTag("TaskList");

        if (taskHolder != null)
        {
            taskText = Instantiate(taskTextPrefab, taskHolder.transform);

            taskText.rectTransform.Translate(new Vector3(0, -60 * (puzzleID + 1), 0));

            taskText.text = taskName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.puzzles[puzzleID].GetComponent<PuzzleMaster>().isComplete)
        {
            cleared = true;
            taskText.color = Color.green;
        }
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
