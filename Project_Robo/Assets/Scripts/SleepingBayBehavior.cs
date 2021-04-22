using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SleepingBayBehavior : MonoBehaviour
{
    public Animator anim;
    public LevelManager levelManagerScript;

    #region Rewired Stuff
    //Here, we establish what a name that we will use instead of "Input" 
    private Rewired.Player player;
    //The playerID lables which player you are, 0=P1, 1=P2, and so on.
    public int playerId = 0;
    //Stuff for rumble support
    //int motorIndex0 = 0;
    //int motorIndex1 = 1;
    #endregion
    private void Awake()
    {
        // Rewired stuff
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (player.GetButtonDown("Interact"))
        {
            Debug.Log("Powering down into sleeping bay");
            // Move Bitbot to the specific position for the animation of getting into sleeping bay
            //anim.SetBool("isPoweringDown", true);

            //Debug.Log("Coroutine starts");
            // Run coroutine
            //StartCoroutine(PowerDownCoroutine());

            //Debug.Log("Waited 10 seconds??");
            
            levelManagerScript.IncrementCycle();
            levelManagerScript.DeactivateSleepBays();//just
            //anim.SetBool("isPoweringUp", true);

            Debug.Log(levelManagerScript.Cycle);
        }
    }

    IEnumerator PowerDownCoroutine()
    {
        yield return new WaitForSeconds(10.0f);
    }
}
