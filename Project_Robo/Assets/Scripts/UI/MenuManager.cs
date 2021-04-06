using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Rewired;


public class MenuManager : MonoBehaviour
{
    // -- Declared Variables -- //
    public GameObject mainMenuUI;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public AudioMixer audioMixer;
    [SerializeField] private LevelManager levelManager = null;  // Reference to the LevelManager
    public PuzzleUI puzzleUI;   // Reference to the PuzzleUI script

    #region Rewired Stuff
    //Here, we establish what a name that we will use instead of "Input" 
    private Rewired.Player player;
    //The playerID lables which player you are, 0=P1, 1=P2, and so on.
    public int playerId = 0;
    //Stuff for rumble support
    //int motorIndex0 = 0;
    //int motorIndex1 = 1;
    #endregion

    // -- -- //

    private void Awake()
    {
        // Find and assign the LevelManager, so we can change the game state (for cameras switching)
        levelManager = FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // Make sure the right things are active at the start of the game
        mainMenuUI.SetActive(true);
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 0;

    } // End of Start()

    // Update is called once per frame
    void Update()
    {
        // If ESC is pressed, pause/unpause the game
        /*if (player.GetButtonDown("(Un)Pause"))
        {
            Debug.Log("Player hit escape...");
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }*/
    } // Emd of Update()

    public void PlayGame()
    {
        Debug.Log("Loading the first save...");
        levelManager.playerState = 1;
        mainMenuUI.SetActive(false);
        Time.timeScale = 1;

    } // End of PlayGame()

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;

    } // End of Pause()

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;

    } // End of Resume()

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        Debug.Log("Loading the Main Menu...");
        levelManager.playerState = 0;

    } // End of LoadMainMenu()

    public void LoadOptions()
    {
        Debug.Log("Loading the options...");
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            mainMenuUI.SetActive(false);
        }
        optionsMenuUI.SetActive(true);


    } // End of LoadOptions()

    public void OptionsBack()
    {
        optionsMenuUI.SetActive(false);
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            mainMenuUI.SetActive(true);
        }

    } // End of OptionsBack()

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();

    } // End of QuitGame()

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);

    } // End of SetVolume(float volume)
}
