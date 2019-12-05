using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //A reference to this manager that can be accessed anywhere
    public static SoundManager globalReference;

    //The current method that the user is hearing the sound effects
    public SoundProfile currentSoundProfile = SoundProfile.ComputerSpeakers;

    public bool muteMusic = false;
    [Range(0, 1.0f)]
    public float musicVolume = 1.0f;

    public bool muteSoundEffects = false;
    [Range(0, 1.0f)]
    public float soundEffectVolume = 1.0f;

    public bool muteDialogue = false;
    [Range(0, 1.0f)]
    public float dialogueVolume = 1.0f;

    public bool muteAll = false;
    [Range(0, 1.0f)]
    public float globalVolume = 1.0f;

    //Slider for the headphone volume, going from mute to max
    [Range(0, 1.0f)]
    public float headphoneVol = 1.0f;
    //Slider for the computer speaker volume, going from mute to max
    [Range(0, 1.0f)]
    public float compSpeakerVol = 1.0f;
    //Slider for the Room speaker volume, going from mute to max
    [Range(0, 1.0f)]
    public float roomSpeakerVol = 1.0f;



    //Function called when this object is created
    private void Awake()
    {
        //If there isn't already a static reference to this manager, this instance becomes the static reference
        if (SoundManager.globalReference == null)
        {
            SoundManager.globalReference = this.GetComponent<SoundManager>();
        }
        //Otherwise, we destroy this component, because there can only be 1 sound manager
        else
        {
            Destroy(this);
        }
    }


    //Public function that sends out an EVENT for all SoundEmitterExtraSettings components to receive
    public void ChangeSoundProfile(SoundProfile newProfile_)
    {
        //Changing the sound profile to the given one
        globalReference.currentSoundProfile = newProfile_;
        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Public function that changes the volume of a 
    public void ChangeProfileVolume(SoundProfile changedProfile_, float newVolume_)
    {
        //Makes sure that the volume given is within 0 and 1
        float goodVol = newVolume_;
        if (goodVol > 1.0f)
        {
            goodVol = 1.0f;
        }
        else if (goodVol < 0)
        {
            goodVol = 0;
        }

        //Changing the correct sound profile volume to the given volume
        switch (changedProfile_)
        {
            case SoundProfile.Headphones:
                globalReference.headphoneVol = goodVol;
                break;
            case SoundProfile.ComputerSpeakers:
                globalReference.compSpeakerVol = goodVol;
                break;
            case SoundProfile.RoomSpeakers:
                globalReference.roomSpeakerVol = goodVol;
                break;
            default:
                globalReference.compSpeakerVol = goodVol;
                break;
        }


        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Changes the global volume
    public void ChangeGlobalVolume(float newVolume_)
    {
        globalReference.globalVolume = newVolume_;

        //Making sure the global volume is within 0 and 1
        if (globalReference.globalVolume > 1)
        {
            globalReference.globalVolume = 1.0f;
        }
        else if (globalReference.globalVolume < 0)
        {
            globalReference.globalVolume = 0;
        }

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Changes the music volume
    public void ChangeMusicVolume(float newVolume_)
    {
        globalReference.musicVolume = newVolume_;

        //Making sure the music volume is between 0 and 1
        if (globalReference.musicVolume > 1)
        {
            globalReference.musicVolume = 1.0f;
        }
        else if (globalReference.musicVolume < 0)
        {
            globalReference.musicVolume = 0;
        }

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Changes the sound effect volume
    public void ChangeSoundEffectVolume(float newVolume_)
    {
        globalReference.soundEffectVolume = newVolume_;

        //Making sure the sound effect volume is between 0 and 1
        if (globalReference.soundEffectVolume > 1)
        {
            globalReference.soundEffectVolume = 1.0f;
        }
        else if (globalReference.soundEffectVolume < 0)
        {
            globalReference.soundEffectVolume = 0;
        }

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Changes the dialogue volume
    public void ChangeDialogueVolume(float newVolume_)
    {
        globalReference.dialogueVolume = newVolume_;

        //Making sure the dialogue volume is between 0 and 1
        if (globalReference.dialogueVolume > 1)
        {
            globalReference.dialogueVolume = 1.0f;
        }
        else if (globalReference.dialogueVolume < 0)
        {
            globalReference.dialogueVolume = 0;
        }

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Function called externally to mute all volume
    public void ToggleMuteAll(bool isMuted_)
    {
        globalReference.muteAll = isMuted_;

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Function called externally to mute music
    public void ToggleMuteMusic(bool isMuted_)
    {
        globalReference.muteMusic = isMuted_;

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        this.DispatchSoundChangeEvt();
    }


    //Function called externally to mute dialogue
    public void ToggleMuteDialogue(bool isMuted_)
    {
        globalReference.muteDialogue = isMuted_;

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        DispatchSoundChangeEvt();
    }


    //Function called externally to mute sound effects
    public void ToggleMuteSFX(bool isMuted_)
    {
        globalReference.muteSoundEffects = isMuted_;

        //Dispatching an EVT to change all ExtraSoundEmitterSettings to use the new profile
        DispatchSoundChangeEvt();
    }


    //Function called internally to update all ExtraSoundEmitterSettings.cs scripts to use the new manager settings
    private void DispatchSoundChangeEvt()
    {
        EventManager.TriggerEvent(ChangeSoundSettings.eventNum);
    }
}
