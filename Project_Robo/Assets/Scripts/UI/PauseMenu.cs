using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Rewired;

public class PauseMenu : MonoBehaviour
{
    // -- Declared Variables -- //
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public AudioMixer audioMixer;
    // -- -- //

    #region Rewired Stuff
    // Here, we establish what a name that we will use instead of "Input" 
    private Rewired.Player player;
    // The playerID lables which player you are, 0=P1, 1=P2, and so on.
    public int playerId = 0;
    // Stuff for rumble support
    //int motorIndex0 = 0;
    //int motorIndex1 = 1;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);

    } // End of Start()

    // Update is called once per frame
    void Update()
    {
        // If ESC is pressed, pause/un-pause the game
        if (player.GetButtonDown("Cancel")) // Was Input.GetButtonDwon(KeyCode.Escape)
        {
            Debug.Log("escape key?");
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    } // End of Update()

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
        SceneManager.LoadScene(0);

    } // End of LoadMainMenu()

    public void LoadOptions()
    {
        Debug.Log("Loading the options...");
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);

    } // End of LoadOptions()

    public void OptionsBack()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);

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
