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
    private float cameraYAxisMaxSpeed;
    private float cameraXAxisMaxSpeed;
    
    //I dont know which vector 3 does what, but you need these to make the character 
    //move with the camera n' stuff. 
    public Vector3 direction; // a vector three your need for your camera angle stuff
    public Vector3 moveDir;  //a Vector 3 you need to make the player move.


    [Space(2)]
    [Header("Gravity Stuff")]
    //detecting the ground and making the player know when it is not grounded. 
    [SerializeField] private float gravity; //the strengh of the player gravity.
    public LayerMask whatIsGround; //A specific layer we get intel from
    public Transform groundPoint; //empty game object used to detect the ground
    private bool isGrounded; //if this is true the player is on the ground
    public float groundDistance; //how far the player check to see if it is on the ground 
    private Vector3 gravDir; //gravity direction. Vector 3 that I need to make gravity!
    // -- -- //

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
    private void Awake() {
        
        //THIS LINE IS CRUCIAL! IF IT IS NOT IN THE SCRIPT REWIRD WONT READ THE INPUTS FROM THE PROPER CONTROL INPUT!
        player = Rewired.ReInput.players.GetPlayer(playerId);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cameraYAxisMaxSpeed = thirdPersonCamera.m_YAxis.m_MaxSpeed;
        cameraXAxisMaxSpeed = thirdPersonCamera.m_XAxis.m_MaxSpeed;
    }

    private void FixedUpdate(){

        //moveDir.y += gravity * Time.deltaTime; //this make the player do the gravity thing. 

        float horizontal = Input.GetAxisRaw("Horizontal"); // Old Unity input system, negative 1 - 1
        float vertical = Input.GetAxisRaw("Vertical"); // Old Unity input system, negative 1 - 1

        // Stop mouse-camera movement if in a puzzle menu
        if (isInAMenu == false)
        {
            //Movement and gravity code. 
            direction = new Vector3(horizontal, 0f, vertical).normalized;

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
                moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
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

    // Update is called once per frame
    void Update()
    {
        #region Player Gravity Shit
        //check to see of the player is on the ground. 
        isGrounded = Physics.CheckSphere(groundPoint.position, groundDistance, whatIsGround);
        //Debug.Log("the grounded value is " + isGrounded);

        //update how much gravity should be applied to the player so if they fall it looks right
        gravDir.y += gravity * Time.deltaTime;

        //Apply the updated gravity info to the charaCon Comp so he falls.
        controller.Move(gravDir * Time.deltaTime);
        controller.Move(gravDir * speed * Time.deltaTime);

        //If the player is grounded keep the gravity strenght at -2 so he doesnt build up
        //too much speed on the ground and also so he doesn't eventually fall through
        //the floor.
        if (isGrounded && gravDir.y < 0)
        {
            gravDir.y = -2f;
        }
        #endregion
    }
}
