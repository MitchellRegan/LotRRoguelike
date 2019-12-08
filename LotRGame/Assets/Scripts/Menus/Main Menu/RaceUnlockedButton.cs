using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceUnlockedButton : MonoBehaviour
{
    //The race that this button is tied to
    public Races buttonRace = Races.Human;
    //The button that we disable if this race isn't unlocked yet
    public Button ourButton;
    //The game object reference with the lock
    public GameObject lockImage;



    //Function called when this object is enabled
    private void OnEnable()
    {
        //If the race for this button is unlocked
        if(GameData.globalReference.playerUnlocks.IsRaceUnlocked(this.buttonRace))
        {
            this.ourButton.interactable = true;
            this.lockImage.SetActive(false);
        }
        //If the race is locked
        else
        {
            this.ourButton.interactable = false;
            this.lockImage.SetActive(true);
        }
    }
}
