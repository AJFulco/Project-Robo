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

    [SerializeField] public List<GameObject> puzzles;
    [SerializeField] public List<GameObject> puzzleObjects;
    public int currentPuzzle = 0;
    [SerializeField] public GameObject holder;
    [SerializeField] public Text puzzleNear;

    // Start is called before the first frame update
    void Start()
    {
        if (puzzles.Count > 0)
        {
            for (int i = 0; i < puzzles.Count; i++)
                puzzles[i].SetActive(false);
        }

        holder.SetActive(false);
        puzzles[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void togglePuzzle()
    {
        if (holder.activeSelf)
            holder.SetActive(false);
        else
            holder.SetActive(true);
    }

    public void exitPuzzle()
    {
        if (holder.activeSelf)
            holder.SetActive(false);;
    }

    public void HandlePuzzleSwitching(string direction)
    {
        if (!holder.activeSelf)
            return;

        if (direction == "previous")
        {
            if (currentPuzzle - 1 >= 0)
            {
                puzzles[currentPuzzle].SetActive(false);
                puzzles[currentPuzzle - 1].SetActive(true);
                currentPuzzle--;
            }
        }

        if (direction == "next")
        {
            if (puzzles[currentPuzzle].GetComponent<PuzzleMaster>().isComplete && currentPuzzle + 1 < puzzles.Count)
            {
                puzzles[currentPuzzle].SetActive(false);
                puzzles[currentPuzzle + 1].SetActive(true);
                currentPuzzle++;
            } 
        }
    }

}
