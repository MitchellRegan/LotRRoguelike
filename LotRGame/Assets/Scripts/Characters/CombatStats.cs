using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicalState))]
[System.Serializable]
public class CombatStats : MonoBehaviour
{
    //Reference to this character's physical state so we can get the health and energy
    [HideInInspector]
    [System.NonSerialized]
    public PhysicalState currentState;

    //The speed that this character's initiative meter increases each frame during combat
    public float currentInitiativeSpeed = 0.01f;
    public float initiativeMod = 0;

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

    //How skilled this character is at hitting targets. Directly counters evasion
    [Range(0, 50)]
    public int accuracy = 0;

    //How skilled this character is at avoiding physical attacks. Directly counters weapon skills and accuracy
    [Range(0, 50)]
    public int evasion = 10;
    
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
        //Getting the PartyGroup that this character is in
        PartyGroup ourGroup = null;
        if(PartyGroup.group1 != null && PartyGroup.group1.charactersInParty.Contains(this.GetComponent<Character>()))
        {
            ourGroup = PartyGroup.group1;
        }
        //If no party group contains this character, nothing happens
        else
        {
            return;
        }

        //Looping through each character in the player party to see if any of the combat positions overlap
        bool isOverlapping = false;
        foreach(Character partyChar in ourGroup.charactersInParty)
        {
            //Making sure the character we're checking isn't an empty slot or this character
            if(partyChar != null && partyChar.charCombatStats != this)
            {
                //If the current party character has the same row and column positions as us
                if(partyChar.charCombatStats.startingPositionCol == this.startingPositionCol && partyChar.charCombatStats.startingPositionRow == this.startingPositionRow)
                {
                    //Marking that there's overlapping positions and breaking this loop
                    isOverlapping = true;
                    break;
                }
            }
        }

        //If we make it through the first loop and there's no overlapping, we're done
        if(!isOverlapping)
        {
            return;
        }

        //Looping throuch each of the combat positions characters can be in
        for (int c = 0; c < 3; ++c)
        {
            for (int r = 0; r < 8; ++r)
            {
                //Bool that tracks if the current position is empty
                bool emptyPos = true;

                //Looping through each character in the player party
                foreach (Character currentChar in ourGroup.charactersInParty)
                {
                    //Making sure the character we're checking isn't an empty slot in the party and also isn't this character
                    if (currentChar != null && currentChar.charCombatStats != this)
                    {
                        //If the current character is in the same position as this character, we break out of this loop and go to the next tile
                        if (currentChar.charCombatStats.startingPositionRow == r && currentChar.charCombatStats.startingPositionCol == c)
                        {
                            //Marking this tile as not empty and breaking the loop
                            emptyPos = false;
                            break;
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