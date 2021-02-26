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
        /*if (new AxisCoordinateMode)
        {

        }*/
    }

    public void togglePuzzle1()
    {
        if (puzzle1.activeSelf)
            puzzle1.SetActive(false);
        else
            puzzle1.SetActive(true);
    }

}
