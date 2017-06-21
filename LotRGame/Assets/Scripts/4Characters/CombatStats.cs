using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicalState))]
public class CombatStats : MonoBehaviour
{
    //Reference to this character's physical state so we can get the health and energy
    [HideInInspector]
    public PhysicalState currentState;

    //The speed that this character's initiative meter increases each frame during combat
    public float currentInitiativeSpeed = 0.01f;

    //The number of spaces this character can move as a standard action in combat
    public uint combatSpeed = 5;

    //How skilled this character is at avoiding physical attacks. Directly counters weapon skills (below)
    [Range(0, 50)]
    public int evasion = 10;

    //How accurately this character can punch
    [Range(1, 100)]
    public int punching = 30;
    public int punchingMod = 0;

    //How accurately this character can use daggers in combat
    [Range(1, 100)]
    public int daggers = 30;
    public int daggersMod = 0;

    //How accurately this character can use swords in combat
    [Range(1, 100)]
    public int swords = 30;
    public int swordsMod = 0;

    //How accurately this character can use axes in combat
    [Range(1, 100)]
    public int axes = 30;
    public int axesMod = 0;

    //How accurately this character can use spears in combat
    [Range(1, 100)]
    public int spears = 30;
    public int spearsMod = 0;

    //How accurately this character can use bows in combat
    [Range(1, 100)]
    public int bows = 30;
    public int bowsMod = 0;

    //How accurately this character can use improvised weapons in combat
    [Range(1, 100)]
    public int improvised = 30;
    public int improvisedMod = 0;



    //Function called when this object is created
    private void Awake ()
    {
        //Getting the reference to the physical state component on this object
        this.currentState = this.GetComponent<PhysicalState>();
	}
}
