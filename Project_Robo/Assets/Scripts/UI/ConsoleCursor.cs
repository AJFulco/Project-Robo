using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Rewired;

namespace UnityEngine.UI
{
    public class ConsoleCursor : MonoBehaviour
    {
        // Mandatory Rewired Stuff
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

        public int menuIndex = 0;
        public int currMenuSize = 4;

        [SerializeField] private GameObject[] mainMenuItems;
        [SerializeField] private GameObject[] optionsMenuItems;
        [SerializeField] private GameObject[] pauseMenuItems;
        [SerializeField] private GameObject MainMenuObj;
        [SerializeField] private GameObject OptionsMenuObj;
        [SerializeField] private GameObject PauseMenuObj;

        [SerializeField] private Slider volume;
        [SerializeField] private Toggle fullscreen;
        [SerializeField] private Dropdown resolution;
        [SerializeField] private Dropdown graphics;

        private MenuManager menu = null;

        // Start is called before the first frame update
        void Start()
        {
            menu = GameObject.Find("MenuCanvas").GetComponent<MenuManager>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleLocation();
        }

        private void HandleLocation()
        {
            if (MainMenuObj.activeSelf)
            {
                currMenuSize = mainMenuItems.Length;
                this.transform.position = mainMenuItems[menuIndex].transform.position;
            }

            if (OptionsMenuObj.activeSelf)
            {
                currMenuSize = optionsMenuItems.Length;
                this.transform.position = optionsMenuItems[menuIndex].transform.position;
            }

            if (PauseMenuObj.activeSelf)
            {
                currMenuSize = pauseMenuItems.Length;
                this.transform.position = pauseMenuItems[menuIndex].transform.position;
            }

            if (!MainMenuObj.activeSelf && !OptionsMenuObj.activeSelf && !PauseMenuObj.activeSelf)
            {
                this.gameObject.SetActive(false);
            }

            if (player.GetButtonDown("UIVerticalUP"))
            {
                if (menuIndex == 0)
                    menuIndex = currMenuSize - 1;
                else
                    menuIndex--;
            }
            else if (player.GetButtonDown("UIVerticalDown"))
            {
                if (menuIndex >= currMenuSize - 1)
                    menuIndex = 0;
                else
                    menuIndex++;
            }

            if (OptionsMenuObj.activeSelf)
            {
                switch(menuIndex)
                {
                    case 0:
                        if (player.GetButtonDown("UIHorizontalLeft"))
                        {
                            if (volume.value > volume.minValue)
                                volume.value -= 10;
                            else
                                volume.value = volume.minValue;

                            menu.SetVolume(volume.value);
                        }

                        if (player.GetButtonDown("UIHorizontalRight"))
                        {
                            if (volume.value < volume.maxValue)
                                volume.value += 10;
                            else
                                volume.value = volume.maxValue;

                            menu.SetVolume(volume.value);
                        }
                        break;
                    case 1:
                        if (player.GetButtonDown("UISubmit"))
                        {
                            Debug.Log("Pressed Submit");

                            if (fullscreen.isOn)
                            {
                                fullscreen.isOn = false;
                            }
                            else
                            {
                                fullscreen.isOn = true;
                            }
                        }
                        break;
                    case 2:
                        if (player.GetButtonDown("UIHorizontalLeft"))
                        {
                            if (resolution.value > 0)
                                resolution.value--;
                            else
                                resolution.value = resolution.options.Count - 1;
                        }

                        if (player.GetButtonDown("UIHorizontalRight"))
                        {
                            if (resolution.value < resolution.options.Count- 1)
                                resolution.value++;
                            else
                                resolution.value = 0;
                        }
                        break;
                    case 3:
                        if (player.GetButtonDown("UIHorizontalLeft"))
                        {
                            if (graphics.value > 0)
                                graphics.value--;
                            else
                                graphics.value = graphics.options.Count - 1;
                        }

                        if (player.GetButtonDown("UIHorizontalRight"))
                        {
                            if (graphics.value < graphics.options.Count - 1)
                                graphics.value++;
                            else
                                graphics.value = 0;
                        }
                        break;
                }
            }
        }
    }
}
