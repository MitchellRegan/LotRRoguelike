using UnityEngine;
using System.Collections;

public class GoToLevel : MonoBehaviour
{
    //Reference to the GameManager gameobject's EventManager script
    private EventManager EventManager;

    //Name of the level to go to
    public string LevelName;

    //If we need a fade transition going to the level
    public bool FadeOut = false;

    //Amount of time for the fade transition to take
    public float FadeTime = 0;


    void Start()
    {
        EventManager = GameObject.Find("GameManager").GetComponent<EventManager>();
    }

    //Function called externally 
    public void ChangeLevel()
    {
        EventManager.SendGoToLevel(LevelName, FadeOut, FadeTime);
    }
}
