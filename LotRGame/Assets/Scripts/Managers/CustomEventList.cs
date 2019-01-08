using UnityEngine;
using UnityEngine.Events;
using System.Collections;


//Class that's sent through all of our custom events as an argument. Contains an instance of all custom classes
public class EVTData
{
    public TimePassedEVT timePassed = null;
    public UISoundCueEVT soundCue = null;
    public UIMusicCueEVT musicCue = null;
    public SoundCutoutEVT soundCutout = null;
    public SceneTransitionEVT sceneTransition = null;
    public CombatTransitionEVT combatTransition = null;
    public ScreenShakeEVT screenShake = null;
    public PromptQuestEVT promptQuest = null;
    public CharacterDeathEVT characterDeath = null;
    public SaveDataEVT saveData = null;
    public LoadDataEVT loadData = null;
}


//Event data used to pass time
public class TimePassedEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 255;

    //The total number of days that have passed
    public int days = 0;
    //The time of day when the event was called
    public int timeOfDay = 0;
    //The amount of time that will pass during this event
    public int timePassed = 0;

    //Public constructor for this class
    public TimePassedEVT(int days_, int timeOfDay_, int timePassed_)
    {
        this.days = days_;
        this.timeOfDay = timeOfDay_;
        this.timePassed = timePassed_;
    }
}


//Event data used when a UI element needs to play a sound
public class UISoundCueEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 254;

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
    //The num used to call this event from the event manager
    public static byte eventNum = 253;

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
    //The num used to call this event from the event manager
    public static byte eventNum = 252;

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
    //The num used to call this event from the event manager
    public static byte eventNum = 251;

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


//Event data when we transition to and from the combat screen
public class CombatTransitionEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 250;

    //The amount of time it takes to fade to black
    public float fadeToBlackTime = 1;
    //The amount of time we'll stay on a black screen
    public float stayOnBlackTime = 1;
    //The amount of time it takes to fade back in
    public float fadeInTime = 1;

    //Bool for if combat is starting (True) or ending (False)
    public bool startingCombat = true;

    //The UnityEvent that's invoked when the transition gets to black
    public UnityEvent eventOnBlack = null;

    //Public constructor for this class
    public CombatTransitionEVT(bool startingCombat_, float fadeToBlackTime_ = 1, float stayOnBlackTime_ = 1, float fadeInTime_ = 1, UnityEvent eventOnBlack_ = null)
    {
        this.fadeToBlackTime = fadeToBlackTime_;
        this.stayOnBlackTime = stayOnBlackTime_;
        this.fadeInTime = fadeInTime_;
        this.startingCombat = startingCombat_;
        this.eventOnBlack = eventOnBlack_;
    }
}


//Event data when we start a screen shake
public class ScreenShakeEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 249;

    //The amount of time that the screen will shake once triggered
    public float screenShakeDuration = 1;
    //The percent of the maximum amount the screen can shake
    [Range(0, 1)]
    public float screenShakePower = 0.5f;
    //The curve that determines how fast the shake returns to normal
    public EaseType screenShakeCurve = EaseType.SineIn;

    //Public constructor for this class
    public ScreenShakeEVT(float screenShakeDuration_, float screenShakePower_, EaseType screenShakeCurve_)
    {
        this.screenShakeDuration = screenShakeDuration_;
        this.screenShakePower = screenShakePower_;
        this.screenShakeCurve = screenShakeCurve_;
    }
}


//Event data when we prompt a quest in QuestPromptUI.cs
public class PromptQuestEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 248;

    //The quest giver that we're going to display info about
    public Quest questToPrompt;

    //Public constructor for this class
    public PromptQuestEVT(Quest questToPrompt_)
    {
        this.questToPrompt = questToPrompt_;
    }
}


//Event data when we change the sound settings
public class ChangeSoundSettings
{
    //The num used to call this event from the event manager
    public static byte eventNum = 247;
}


//Event data for when a character dies
public class CharacterDeathEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 246;

    //The character that just died
    public Character deadCharacter;

    //Public constructor for this class
    public CharacterDeathEVT(Character deadCharacter_)
    {
        this.deadCharacter = deadCharacter_;
    }
}


//Event data for when we're saving data
public class SaveDataEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 245;

    //Bool for if we're starting the save (true) or done saving (false)
    public bool startingSave = false;


    //Public constructor for this class
    public SaveDataEVT(bool startingSave_)
    {
        this.startingSave = startingSave_;
    }
}


//Event data for when we're loading data
public class LoadDataEVT
{
    //The num used to call this event from the event manager
    public static byte eventNum = 245;

    //Bool for if we're starting the load (true) or currently loading (false)
    public bool startingLoad = false;

    //Int for the number of load updates that will happen before it finishes. Only applicable if startingLoad is true
    public int totalLoadUpdates = 1;


    //Public constructor for this class
    public LoadDataEVT(bool startingLoad_, int totalLoadUpdates_ = 1)
    {
        this.startingLoad = startingLoad_;
        this.totalLoadUpdates = totalLoadUpdates_;
    }
}