using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Rewired;


public class MenuManager : MonoBehaviour
{
    // -- Declared Variables -- //
    public GameObject mainMenuUI;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public AudioMixer audioMixer;

    // -- -- //

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
        /*if (Input.GetButtonDown(KeyCode.Escape))
        {
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
