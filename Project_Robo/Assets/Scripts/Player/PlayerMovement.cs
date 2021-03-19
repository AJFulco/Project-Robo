using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NEED THIS TO BE ABLE TO USE ANY KIND OF REWIRD FEATURES
using Rewired;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    // -- Declared Variables -- //
    public CharacterController controller;
    public float speed = 15f;    // Player movement speed
    public float turnSmoothTime = 0.3f;     // Smooth the player turning
    float turnSmoothVelocity;
    public Transform cam;   // Reference to the Main Camera
    public Animator anim;
    public bool isInAMenu;  // Is a bool that checks if the player is busy
    public CinemachineFreeLook thirdPersonCamera;    // Reference to the ThirdPersonCamera
    private int cameraYAxisMaxSpeed = 10;
    private int cameraXAxisMaxSpeed = 300;
    // -- -- //

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
    private void Awake() {
        
        //THIS LINE IS CRUCIAL! IF IT IS NOT IN THE SCRIPT REWIRD WONT READ THE INPUTS FROM THE PROPER CONTROL INPUT!
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Old Unity input system, negative 1 - 1
        float vertical = Input.GetAxisRaw("Vertical"); // Old Unity input system, negative 1 - 1

        // Stop mouse-camera movement if in a puzzle menu
        if (isInAMenu == false)
        {
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            // Allow camera movement
            thirdPersonCamera.m_YAxis.m_MaxSpeed = cameraYAxisMaxSpeed;
            thirdPersonCamera.m_XAxis.m_MaxSpeed = cameraXAxisMaxSpeed;

            // Allow movement
            if (direction.magnitude >= 0.1f)
            {
                // Have the player point in the direction they are moving
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
        else
        {
            // Prevent camera movement
            thirdPersonCamera.m_YAxis.m_MaxSpeed = 0;
            thirdPersonCamera.m_XAxis.m_MaxSpeed = 0;
        }

        /*
        //Example of Rewired code
        if (player.GetButtonDown("Action")) //in the "" you write the name of the action you labled in the Rewired Input Manager
        {
            //notice how we used "player" instead of "Input"
            return;
        }
        */
    }
}
