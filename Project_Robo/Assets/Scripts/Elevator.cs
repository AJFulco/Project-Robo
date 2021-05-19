using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private bool atBottom = true;
    private Animator Anim = null;
    [SerializeField] private AudioSource Up = null;
    [SerializeField] private AudioSource Down = null;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!Anim.GetBool("MoveUp") && !Anim.GetBool("MoveDown") && atBottom)
            {
                MoveUp();
            }
            else if (!Anim.GetBool("MoveUp") && !Anim.GetBool("MoveDown") && !atBottom)
            {
                MoveDown();
            }
        }
    }

    private void MoveUp()
    {
        StartCoroutine(stopAnim());
        Anim.SetBool("MoveUp", true);
        Anim.SetBool("MoveDown", false);
        atBottom = false;
        Debug.Log("Activated!");
        Up.Play();
    }

    private void MoveDown()
    {
        StartCoroutine(stopAnim());
        Anim.SetBool("MoveUp", false);
        Anim.SetBool("MoveDown", true);
        atBottom = true;
        Down.Play();
    }

    IEnumerator stopAnim()
    {
        yield return new WaitForSeconds(2.3f);
        Anim.SetBool("MoveUp", false);
        Anim.SetBool("MoveDown", false);
    }
}
