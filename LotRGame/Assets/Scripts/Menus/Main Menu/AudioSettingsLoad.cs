using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsLoad : MonoBehaviour
{
    //The slider for Global volume
    public Slider globalVolumeSlider;
    //The toggle for muting global volume
    public Toggle muteAllToggle;

    //The slider for Music volume
    public Slider musicVolumeSlider;
    //The toggle for muting music
    public Toggle muteMusicToggle;

    //The slider for SFX volume
    public Slider soundEffectVolumeSlider;
    //The toggle for muting SFX
    public Toggle muteSFXToggle;

    //The slider for Dialogue volume
    public Slider dialogueVolumeSlider;
    //The toggle for muting dialogue
    public Toggle muteDialogueToggle;



	//Function called when this component is enabled
    private void OnEnable()
    {
        //Making sure the global volume slider exists
        if(this.globalVolumeSlider != null)
        {
            this.globalVolumeSlider.value = SoundManager.globalReference.globalVolume;
            this.muteAllToggle.isOn = SoundManager.globalReference.muteAll;
        }

        //Making sure the music volume slider exists
        if(this.musicVolumeSlider != null)
        {
            this.musicVolumeSlider.value = SoundManager.globalReference.musicVolume;
            this.muteMusicToggle.isOn = SoundManager.globalReference.muteMusic;
        }

        //Making sure the SFX volume slider exists
        if(this.soundEffectVolumeSlider != null)
        {
            this.soundEffectVolumeSlider.value = SoundManager.globalReference.soundEffectVolume;
            this.muteSFXToggle.isOn = SoundManager.globalReference.muteSoundEffects;
        }

        //Making sure the dialogue volume slider exists
        if(this.dialogueVolumeSlider != null)
        {
            this.dialogueVolumeSlider.value = SoundManager.globalReference.dialogueVolume;
            this.muteDialogueToggle.isOn = SoundManager.globalReference.muteDialogue;
        }
    }
}
