using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearUnlockedButton : MonoBehaviour
{
    //The gear that this button is tied to
    public UnlockableGearType buttonGear = UnlockableGearType.Swords;
    //The button that we disable if this gear isn't unlocked yet
    public Button ourButton;
    //The game object reference with the lock
    public GameObject lockImage;



    //Function called when this object is enabled
    private void OnEnable()
    {
        //If the gear for this button is unlocked
        if (GameData.globalReference.playerUnlocks.IsGearUnlocked(this.buttonGear))
        {
            this.ourButton.interactable = true;
            this.lockImage.SetActive(false);
        }
        //If the gear is locked
        else
        {
            this.ourButton.interactable = false;
            this.lockImage.SetActive(true);
        }
    }
}
