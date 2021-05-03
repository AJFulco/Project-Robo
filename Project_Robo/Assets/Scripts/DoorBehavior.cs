using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool isOpen = false;
    public Animator anim;
    private AudioSource audioComponent;
    [SerializeField] AudioClip openSound;

    // Start is called before the first frame update
    void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open() { //opens the door when called
        this.GetComponent<BoxCollider>().enabled = false;
        if(openSound != null) audioComponent.PlayOneShot(openSound);
        isOpen = true;
        Debug.Log(this.name + "is now OPEN!!!!!!");
        anim.SetBool("isOpening",true);
       

    }
    public void Close(){ //closes the door when called
        isOpen = false;
        Debug.Log("The Door is now Closed :(");
        anim.SetBool("isOpening", false);
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
