using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour 
{
    bool playerDetection = false;

    private void Update()
    {
        if (playerDetection)
        {
            print("Dialogue Started");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerBody")
        {
            playerDetection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerDetection = false;
    }
}
