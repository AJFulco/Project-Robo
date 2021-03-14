using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PuzzleUI : MonoBehaviour
{
    #region Rewired Stuff
    //Here, we establish what a name that we will use instead of "Input" 
    private Rewired.Player player;
    //The playerID lables which player you are, 0=P1, 1=P2, and so on.
    public int playerId = 0;
    //Stuff for rumble support
    int motorIndex0 = 0;
    int motorIndex1 = 1;
    #endregion

    #region Awake
    //Awake function is code that is executed before the Start or OnEnable functions. 
    private void Awake()
    {

        //THIS LINE IS CRUCIAL! IF IT IS NOT IN THE SCRIPT REWIRD WONT READ THE INPUTS FROM THE PROPER CONTROL INPUT!
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }
    #endregion

    [SerializeField] public List<PuzzleObject> puzzleObjects;
    [SerializeField] public List<PuzzleMaster> puzzles;
    public int currentPuzzle = 0;
    [SerializeField] public GameObject holder;
    [SerializeField] public Text puzzleNearText;
    [SerializeField] private AudioClip puzzleStart;
    public PlayerMovement playerMovement; // Get reference to the PlayerMovement

    public bool nearPuzzle = false;

    // Start is called before the first frame update
    void Start()
    {
        if (puzzles.Count > 0)
        {
            for (int i = 0; i < puzzles.Count; i++)
                puzzles[i].gameObject.SetActive(false);
        }

        holder.SetActive(false);
        puzzles[0].gameObject.SetActive(true);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (nearPuzzle && player.GetButtonDown("Interact"))
            togglePuzzle();

        //Debug.Log(player.GetCurrentInputSources("Horizontal").Count);
    }

    public void togglePuzzle()
    {
        if (holder.activeSelf)
        {
            holder.SetActive(false);
            playerMovement.speed = 6;
            playerMovement.isInAMenu = false;
        }
        else
        {
            if (puzzleStart != null)
            {
                AudioSource.PlayClipAtPoint(puzzleStart, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
            }

            holder.SetActive(true);
            playerMovement.isInAMenu = true;
            //playerMovement.speed = 0;
        }

        if (puzzleNearText.gameObject.activeSelf)
            puzzleNearText.gameObject.SetActive(false);
        else
            puzzleNearText.gameObject.SetActive(true);

        for (int i = 0; i < puzzles.Count; i++)
        {
            if (i != currentPuzzle)
                puzzles[i].gameObject.SetActive(false);
            else
                puzzles[i].gameObject.SetActive(true);
        }
    }

    public void exitPuzzle()
    {
        if (holder.activeSelf)
            holder.SetActive(false);;
    }

    public void UpdateTaskColor(PuzzleMaster solved)
    {
        int id = -1;

        for (int i = 0; i < puzzles.Count; i++)
        {
            if (puzzles[i] == solved)
                id = i;
        }

        if (id != -1)
        {
            for (int i = 0; i < puzzleObjects.Count; i++)
            {
                if (puzzleObjects[i].puzzleID == id)
                    puzzleObjects[i].taskText.color = new Color(100, 255, 100);
            }
        }
    }

    public void HandlePuzzleSwitching(string direction)
    {
        if (!holder.activeSelf)
            return;

        if (direction == "previous")
        {
            if (currentPuzzle - 1 >= 0)
            {
                puzzles[currentPuzzle].gameObject.SetActive(false);
                puzzles[currentPuzzle - 1].gameObject.SetActive(true);
                currentPuzzle--;
            }
        }

        if (direction == "next")
        {
            if (puzzles[currentPuzzle].GetComponent<PuzzleMaster>().isComplete && currentPuzzle + 1 < puzzles.Count)
            {
                puzzles[currentPuzzle].gameObject.SetActive(false);
                puzzles[currentPuzzle + 1].gameObject.SetActive(true);
                currentPuzzle++;
            } 
        }
    }

}
