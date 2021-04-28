using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool isOpen = false;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open() { //opens the door when called
        isOpen = true;
        Debug.Log("The Door is now OPEN!!!!!!");
        anim.SetBool("isOpening",true);
        GameObject.Find("Open").GetComponent<BoxCollider>().enabled = false;

    }
    public void Close(){ //closes the door when called
        isOpen = false;
        Debug.Log("The Door is now Closed :(");
        anim.SetBool("isOpening", false);
        GameObject.Find("Open").GetComponent<BoxCollider>().enabled = true;
    }
}
