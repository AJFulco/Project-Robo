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
    [SerializeField] private ConsoleCursor cursor = null;
    public PlayerMovement playerMovement;   // Reference to the player movement script
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

        cursor = FindObjectOfType<ConsoleCursor>().GetComponent<ConsoleCursor>();

        // Rewired stuff
        player = Rewired.ReInput.players.GetPlayer(playerId);
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
        playerMovement.isInAMenu = true;

        cursor.gameObject.SetActive(true);
        cursor.currMenuSize = 4;
        cursor.menuIndex = 0;
    } // End of Start()

    // Update is called once per frame
    void Update()
    {
        // If ESC is pressed, pause/unpause the game
        if (player.GetButtonDown("(Un)Pause"))
        {
            //Debug.Log("Player hit escape...");
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        HandleCursor();
    } // Emd of Update()

    public void HandleCursor()
    {
        // mouseMode determines what puzzle control scheme is currently in use
        bool mouseMode = true;

        // Check Rewired to see what controller is currently being used
        if (player.controllers.GetLastActiveController() == player.controllers.GetLastActiveController<Keyboard>() ||
            player.controllers.GetLastActiveController() == player.controllers.GetLastActiveController<Mouse>())
        {
            mouseMode = true;
        }
        else
        {
            mouseMode = false;
        }

        if (mouseMode)
        {
            if (cursor.gameObject.activeSelf)
                cursor.gameObject.SetActive(false);
        }
        else
        {
            if (!cursor.gameObject.activeSelf)
            {
                if (mainMenuUI.activeSelf || pauseMenuUI.activeSelf || optionsMenuUI.activeSelf)
                {
                    cursor.menuIndex = 0;
                    cursor.gameObject.SetActive(true);
                }
            }
        }
    }

    public void PlayGame()
    {
        //Debug.Log("Loading new game...");
        levelManager.playerState = 1;
        mainMenuUI.SetActive(false);
        playerMovement.isInAMenu = false;
        Time.timeScale = 1;


    } // End of PlayGame()

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        playerMovement.isInAMenu = true;
        Cursor.lockState = CursorLockMode.None; // Unlock cursor into the active window
        Cursor.visible = true;                  // Show the cursor
        Time.timeScale = 0;
        gameIsPaused = true;

    } // End of Pause()

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        playerMovement.isInAMenu = false;
        Cursor.lockState = CursorLockMode.Locked;   // Unlock cursor into the active window
        Cursor.visible = false;                     // Show the cursor
        Time.timeScale = 1;
        gameIsPaused = false;

    } // End of Resume()

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        Debug.Log("Loading the Main Menu...");
        levelManager.playerState = 0;
        playerMovement.isInAMenu = true;

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
        playerMovement.isInAMenu = true;


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
        playerMovement.isInAMenu = true;

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
