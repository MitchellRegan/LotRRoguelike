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

    //The starting grid position on the combat tiles for this character
    [HideInInspector]
    public int startingPositionCol = 0;
    [HideInInspector]
    public int startingPositionRow = 0;

    //The grid position on the combat tiles where this character currently is
    [HideInInspector]
    public int gridPositionCol = 0;
    [HideInInspector]
    public int gridPositionRow = 0;

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

    //How accurately this character can use holy magic spells in combat
    [Range(1, 100)]
    public int holyMagic = 30;
    public int holyMagicMod = 0;

    //How accurately this character can use dark magic spells in combat
    [Range(1, 100)]
    public int darkMagic = 30;
    public int darkMagicMod = 0;

    //How accurately this character can use nature magic in combat
    [Range(1, 100)]
    public int natureMagic = 30;
    public int natureMagicMod = 0;

    //List of effects that are on this character in combat
    [HideInInspector]
    public List<Effect> combatEffects;



    //Function called when this character is created
    private void Awake ()
    {
        //Getting the reference to the physical state component on this object
        this.currentState = this.GetComponent<PhysicalState>();

        //Initializing the empty list of combat effects
        this.combatEffects = new List<Effect>();
    }


    //Function called from the character manager when this character is added to the player party
    public void SetCombatPosition()
    {
        //Looping throuch each of the combat positions characters can be in
        for (int c = 0; c < 3; ++c)
        {
            for (int r = 0; r < 8; ++r)
            {
                //Bool that tracks if the current position is empty
                bool emptyPos = true;

                //Looping through each character in the player party
                foreach (Character currentChar in CharacterManager.globalReference.playerParty)
                {
                    //Making sure the character we're checking isn't an empty slot in the party and also isn't this character
                    if (currentChar != null && currentChar.charCombatStats != this)
                    {
                        //If the current character is in the same position as this character, we break out of this loop and go to the next tile
                        if (currentChar.charCombatStats.startingPositionRow == r && currentChar.charCombatStats.startingPositionCol == c)
                        {
                            //Marking this tile as not empty and breaking the loop
                            emptyPos = false;
                        }
                    }
                }

                //If we make it through all of the characters without finding someone else in this character's position
                if (emptyPos)
                {
                    //We set this character to the current row and column, then end this function
                    this.startingPositionCol = c;
                    this.startingPositionRow = r;
                    return;
                }
            }
        }
    }
}
