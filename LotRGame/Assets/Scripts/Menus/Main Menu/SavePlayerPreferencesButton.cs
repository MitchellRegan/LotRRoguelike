using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPreferencesButton : MonoBehaviour
{
    //Function called externally from UI Buttons
    public void SavePlayerPreferences()
    {
        SaveLoadManager.globalReference.SavePlayerPreferences();
    }


    public void ChangeGlobalVolume(float newVol_)
    {
        SoundManager.globalReference.ChangeGlobalVolume(newVol_);
    }


    public void ChangeMusicVolume(float newVol_)
    {
        SoundManager.globalReference.ChangeMusicVolume(newVol_);
    }


    public void ChangeSoundEffectVolume(float newVol_)
    {
        SoundManager.globalReference.ChangeSoundEffectVolume(newVol_);
    }


    public void ChangeDialogueVolume(float newVol_)
    {
        SoundManager.globalReference.ChangeDialogueVolume(newVol_);
    }
}
