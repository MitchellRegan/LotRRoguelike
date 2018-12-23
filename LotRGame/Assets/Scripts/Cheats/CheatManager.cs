using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheatManager : MonoBehaviour
{
    //The three keyboard buttons that must be down to activate cheats
    public KeyCode activateKey1 = KeyCode.KeypadDivide;
    public KeyCode activateKey2 = KeyCode.KeypadMultiply;
    public KeyCode activateKey3 = KeyCode.KeypadPlus;

    [Space(8)]

    //The three keyboard buttons that must be down to deactivate cheats
    public KeyCode deactivateKey1 = KeyCode.KeypadDivide;
    public KeyCode deactivateKey2 = KeyCode.KeypadMultiply;
    public KeyCode deactivateKey3 = KeyCode.KeypadMinus;

    [Space(8)]

    //Reference to the game object that holds all of the cheats. This is what is turned on and off
    public GameObject cheatHolderObj;


    
    //Function called every frame
    private void Update()
    {
        //If cheats are not active, we check for input to activate them
        if(!this.cheatHolderObj.activeInHierarchy)
        {
            //If all three activation keys are down, cheats are active
            if(Input.GetKey(this.activateKey1) && Input.GetKey(this.activateKey2) && Input.GetKey(this.activateKey3))
            {
                Debug.Log("CHEATS ACTIVE!");
                this.cheatHolderObj.SetActive(true);
            }
        }
        //If cheats are active, we check for input to deactivate them or input to use a cheat
        else
        {
            //If all three deactivation keys are down, cheats are not active
            if (Input.GetKey(this.deactivateKey1) && Input.GetKey(this.deactivateKey2) && Input.GetKey(this.deactivateKey3))
            {
                Debug.Log("CHEATS DISABLED!");
                this.cheatHolderObj.SetActive(false);
            }
        }
    }
}