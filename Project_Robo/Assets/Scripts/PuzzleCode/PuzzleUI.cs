using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PuzzleUI : MonoBehaviour
{
    // Mandatory Rewired stuff
    #region Rewired Stuff
    //Here, we establish what a name that we will use instead of "Input" 
    private Rewired.Player player;
    //The playerID lables which player you are, 0=P1, 1=P2, and so on.
    public int playerId = 0;
    //Stuff for rumble support
    //int motorIndex0 = 0;
    //int motorIndex1 = 1;
    #endregion

    #region Awake
    //Awake function is code that is executed before the Start or OnEnable functions. 
    private void Awake()
    {

        //THIS LINE IS CRUCIAL! IF IT IS NOT IN THE SCRIPT REWIRD WONT READ THE INPUTS FROM THE PROPER CONTROL INPUT!
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }
    #endregion

    // Inspector variables
    [SerializeField] public List<PuzzleObject> puzzleObjects;
    [SerializeField] public List<PuzzleMaster> puzzles;
    [SerializeField] public GameObject holder;
    [SerializeField] public Text puzzleNearText;
    [SerializeField] private AudioClip puzzleStart;
    [SerializeField] private CursorFollow cursor;
    public PlayerMovement playerMovement; // Get reference to the PlayerMovement

    public int currentPuzzle = 0;
    public bool nearPuzzle = false;

    // Start is called before the first frame update
    void Start()
    {
        // Hide all puzzles by default
        if (puzzles.Count > 0)
        {
            for (int i = 0; i < puzzles.Count; i++)
                puzzles[i].gameObject.SetActive(false);
        }

        // Hide the puzzle holder by default
        holder.SetActive(false);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();  
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is near a puzzle and presses Interact, display that puzzle
        if (nearPuzzle && player.GetButtonDown("Interact"))
        {
            cursor.currentNode = 2;
            togglePuzzle();
        }

        //Debug.Log(player.GetCurrentInputSources("Horizontal").Count);
    }

    // Displays the currently active puzzle to the player, or hides it if the puzzle was already being shown
    public void togglePuzzle()
    {
        // Hides the holder if it was active
        if (holder.activeSelf)
        {
            holder.SetActive(false);
            playerMovement.isInAMenu = false;
        }
        else // Displays the puzzle if it was not displayed already
        {
            if (puzzleStart != null)
            {
                GameObject.Find("PuzzleReadyCanvas").GetComponent<AudioSource>().PlayOneShot(puzzleStart);
            }

            holder.SetActive(true);
            playerMovement.isInAMenu = true;
        }

        // Hides the nearbyPuzzle text when the player opens a puzzle and vice-versa
        if (puzzleNearText.gameObject.activeSelf)
            puzzleNearText.gameObject.SetActive(false);
        else
            puzzleNearText.gameObject.SetActive(true);

        //Hide all completed puzzles from displaying in the puzzle node on play.
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

    // Updates the color of the task in the task list once the puzzle has been solved
    public void UpdateTaskColor(PuzzleMaster solved)
    {
        //A.J. Says: I don't know that this does but that's ok for now.
        int id = -1;

        // Get the index of the PuzzleMaster that was passed in
        for (int i = 0; i < puzzles.Count; i++)
        {
            if (puzzles[i] == solved)
                id = i;
        }

        // Change the color of the task in the taskList that matches the id
        if (id != -1)
        {
            for (int i = 0; i < puzzleObjects.Count; i++)
            {
                if (puzzleObjects[i].puzzleID == id)
                    puzzleObjects[i].taskText.color = new Color(100, 255, 100);
            }
        }

        //A.J.'s Method of list updating.

    }

    // Deprecated Debug code, could still be useful for testing some puzzles, but placing a PuzzleObject is likely easier
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
