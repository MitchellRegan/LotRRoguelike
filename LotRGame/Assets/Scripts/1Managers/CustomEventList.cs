﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;


//Class that's sent through all of our custom events as an argument. Contains an instance of all custom classes
public class EVTData
{
    public UISoundCueEVT soundCue = null;
    public UIMusicCueEVT musicCue = null;
    public SoundCutoutEVT soundCutout = null;
    public SceneTransitionEVT sceneTransition = null;
}


//Event data used when a UI element needs to play a sound
public class UISoundCueEVT
{
    //The sound audio clip that will be played
    public AudioClip soundToPlay = null;
    //The volume of the sound
    public float soundVolume = 1;

    //Public constructor for this class
    public UISoundCueEVT(AudioClip soundToPlay_, float volume_)
    {
        this.soundToPlay = soundToPlay_;
        this.soundVolume = volume_;
    }
}


//Event data used when a UI element needs to play music
public class UIMusicCueEVT
{
    //The music audio clip that will be played
    public AudioClip musicToPlay = null;
    //The volume of the music
    public float musicVolume = 1;

    //Public constructor for this class
    public UIMusicCueEVT(AudioClip musicToPlay_, float volume_)
    {
        this.musicToPlay = musicToPlay_;
        this.musicVolume = volume_;
    }
}


//used when the sound cuts out after an impactful sound cue
public class SoundCutoutEVT
{
    //how long the cutout lasts
    public float stopDuration = 0;
    //How long it takes for sounds to return to normal levels again
    public float fadeInDuration = 0;

    //How low the music volume is set when the cutout initially happens
    [Range(0, 1.0f)]
    public float musicLowPoint = 0;
    //How low the dialogue volume is set when the cutout initially happens
    [Range(0, 1.0f)]
    public float dialogueLowPoint = 0;
    //How low the SFX volume is set when the cutout initially happens
    [Range(0, 1.0f)]
    public float sfxLowPoint = 0;

    //Public constructor for this class
    public SoundCutoutEVT(float totalDuration_, float fadeDuration_, float musicLow_, float dialogueLow_, float sfxLow_)
    {
        this.stopDuration = totalDuration_;
        this.fadeInDuration = fadeDuration_;
        this.musicLowPoint = musicLow_;
        this.dialogueLowPoint = dialogueLow_;
        this.sfxLowPoint = sfxLow_;
    }
}


//Event data used when we transition to a new scene
public class SceneTransitionEVT
{
    //The name of the scene to switch to
    public string newSceneName = "";
    //The amount of time it takes to transition to the new scene
    public float transitionTime = 1;

    //Public constructor for this class
    public SceneTransitionEVT(string newSceneName_, float transitionTime_)
    {
        this.newSceneName = newSceneName_;
        this.transitionTime = transitionTime_;
    }
}