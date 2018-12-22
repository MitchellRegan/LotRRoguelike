using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheatManager : MonoBehaviour
{
    //Static reference to this cheat manager
    public static CheatManager globalReference;

    //Bool for if cheats are active
    private bool cheatsActive = false;

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

    //The list of keyboard buttons to activate cheats
    public List<CheatButtonActivator> cheatList;



	//Function used for initialization to set the global reference
    private void Awake()
    {
        if(globalReference != null && globalReference != this)
        {
            Destroy(this.gameObject);
        }
        else if(globalReference == null)
        {
            globalReference = this;
        }
    }


    //Function called every frame
    private void Update()
    {
        //If cheats are not active, we check for input to activate them
        if(!this.cheatsActive)
        {
            //If all three activation keys are down, cheats are active
            if(Input.GetKey(this.activateKey1) && Input.GetKey(this.activateKey2) && Input.GetKey(this.activateKey3))
            {
                Debug.Log("CHEATS ACTIVE!");
                this.cheatsActive = true;
            }
        }
        //If cheats are active, we check for input to deactivate them or input to use a cheat
        else
        {
            //If all three deactivation keys are down, cheats are not active
            if (Input.GetKey(this.deactivateKey1) && Input.GetKey(this.deactivateKey2) && Input.GetKey(this.deactivateKey3))
            {
                Debug.Log("CHEATS DISABLED!");
                this.cheatsActive = false;
            }

            //Looping through all of our cheats to see if any have been activated
            for(int c = 0; c < this.cheatList.Count; ++c)
            {
                //Checking to see if the button for this cheat was pressed
                if(Input.GetKeyDown(this.cheatList[c].activationKey))
                {
                    this.cheatList[c].cheatEvent.Invoke();
                }
            }
        }
    }
}

[System.Serializable]
public class CheatButtonActivator
{
    //The keyboard button that activates this cheat
    public KeyCode activationKey = KeyCode.KeypadEnter;
    //The unity event called when activated
    public UnityEvent cheatEvent;
}