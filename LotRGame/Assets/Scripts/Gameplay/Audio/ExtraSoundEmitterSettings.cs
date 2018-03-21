using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class ExtraSoundEmitterSettings : MonoBehaviour
{
    //Reference to this object's Audio Source component
    private AudioSource ourAudio;

    //Type of sound this emitter plays
    public enum SoundType { SFX, Dialogue, Music, }
    public SoundType soundEmitterType = SoundType.SFX;

    //Slider for the Headphone volume, going from mute to the max
    [Range(0.0f, 1.0f)]
    public float headphoneVol = 1.0f;
    //Slider for the Computer speaker volume, going from mute to the max
    [Range(0.0f, 1.0f)]
    public float compSpeakerVol = 1.0f;
    //Slider for the  Room speaker volume, going from mute to the max
    [Range(0.0f, 1.0f)]
    public float roomspeakerVol = 1.0f;

    //Event that listens to the event manager to see if the sound settings changed
    private DelegateEvent<EVTData> soundChangeListener;



    //Function called when this object is created
    private void Awake()
    {
        this.soundChangeListener = new DelegateEvent<EVTData>(SettingsChanged);
    }


    //Starts listening for the sound change event
    private void OnEnable()
    {
        EventManager.StartListening("SoundSettingsChanged", this.soundChangeListener);
    }


    //Stops listening for the sound change event 
    private void OnDisable()
    {
        EventManager.StopListening("SoundSettingsChanged", this.soundChangeListener);
    }


    // Use this for initialization
    private void Start()
    {
        this.ourAudio = gameObject.GetComponent<AudioSource>();
        this.SettingsChanged(new EVTData());
    }


    //Called from the Event Manager when the sound settings have been changed
    private void SettingsChanged(EVTData data)
    {
        float profileVol = 1.0f;
        float emitterTypeVol = 1.0f;

        //Finds the profile volume based on what speakers are being used
        switch (SoundManager.globalReference.currentSoundProfile)
        {
            case SoundProfile.ComputerSpeakers:
                profileVol = SoundManager.globalReference.compSpeakerVol * this.compSpeakerVol;
                break;
            case SoundProfile.Headphones:
                profileVol = SoundManager.globalReference.headphoneVol * this.headphoneVol;
                break;
            case SoundProfile.RoomSpeakers:
                profileVol = SoundManager.globalReference.roomSpeakerVol * this.roomspeakerVol;
                break;
            //Uses computer speakers by default
            default:
                profileVol = SoundManager.globalReference.compSpeakerVol * this.compSpeakerVol;
                break;
        }

        //Finds the volume of the emitter type based on what kind of sound it emits
        switch (this.soundEmitterType)
        {
            case SoundType.Dialogue:
                if (SoundManager.globalReference.muteDialogue)
                {
                    emitterTypeVol = 0;
                }
                else
                {
                    emitterTypeVol = SoundManager.globalReference.dialogueVolume;
                }
                break;
            case SoundType.Music:
                if (SoundManager.globalReference.muteMusic)
                {
                    emitterTypeVol = 0;
                }
                else
                {
                    emitterTypeVol = SoundManager.globalReference.musicVolume;
                }
                break;
            case SoundType.SFX:
                if (SoundManager.globalReference.muteSoundEffects)
                {
                    emitterTypeVol = 0;
                }
                else
                {
                    emitterTypeVol = SoundManager.globalReference.soundEffectVolume;
                }
                break;
            //Uses music by default
            default:
                if (SoundManager.globalReference.muteMusic)
                {
                    emitterTypeVol = 0;
                }
                else
                {
                    emitterTypeVol = SoundManager.globalReference.musicVolume;
                }
                break;
        }

        //Sets this owner's sound emitter volume based on the settings
        this.ourAudio.volume = profileVol * emitterTypeVol;
    }
}
