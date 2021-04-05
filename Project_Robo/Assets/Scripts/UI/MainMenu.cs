using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    // -- Declared Variables -- //
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    public AudioMixer audioMixer;
    // -- -- //

    [Space(2)]
    [Header("Outside References")]
    [SerializeField] private LevelManager levelMan = null; //the level manager we need refernce too.

    void Awake() {
        //find and assign the level manager so we can manipulate it form this script.    
        levelMan = FindObjectOfType<LevelManager>().GetComponent<LevelManager>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the right things are active at the start of the game.
        mainMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        Debug.Log("Loading the first scene...");
        //mainMenuUI.SetActive(false);
        //optionsMenuUI.SetActive(false);
        levelMan.playerState = 1;
        Time.timeScale = 1;

    } // End of PlayGame()

    public void LoadOptions()
    {
        Debug.Log("Loading options...");
        mainMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);

    } // End of LoadOptions()
    public void OptionsBack()
    {
        optionsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);

    } // End of OptionsBack

    public void QuitGame()
    {
        Debug.Log("Quiting the game...");
        Application.Quit();

    } // End of QuitGame()

    // Changes the Master volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);

    } // End of SetVolume(float volume)

}
