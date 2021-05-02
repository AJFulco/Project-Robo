using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ElevatorButton : MonoBehaviour
{

    [SerializeField] GameObject gamer;//this is the player, but that was already taken so i use gamer
    [SerializeField] bool firstFloor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player")) {
            gamer.GetComponent<PlayerMovement>().TeleportPlayer(firstFloor);
            firstFloor = !firstFloor;
        }
    }
}
