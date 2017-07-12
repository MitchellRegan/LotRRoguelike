using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //A static reference to this component so other scripts can reference it
    public static CharacterManager globalReference;

    //The list of characters that are currently in the player's party
    [HideInInspector]
    public List<Character> playerParty;

    //The maximum number of characters that can be in the player's party
    public int maxPartySize = 10;

    //The group of characters currently selected in the overworld
    [HideInInspector]
    public PartyGroup selectedGroup;

    //The list of characters that have died in the current game
    [HideInInspector]
    public List<Character> deadCharacters;

    //The list of all character object definitions that can be spawned based on sex and race
    public List<CharacterDefinition> listOfCharacterObjects;



    //Function called when this object is initialized
    private void Awake()
    {
        //Making sure this is the only active CharacterManager component
        if(globalReference != null)
        {
            this.enabled = false;
        }
        else
        {
            globalReference = this;
        }

        //Initializing the list of party characters with enough room for the maximum number
        this.playerParty = new List<Character>(this.maxPartySize);
        for(int i = 0; i < this.maxPartySize; ++i)
        {
            this.playerParty.Add(null);
        }
    }


    //Function used to get the player character at the given index
    public Character GetCharacterAtIndex(int characterIndex_)
    {
        //If the given index isn't within the range of the player party list, we return null
        if(characterIndex_ < 0 || characterIndex_ >= this.playerParty.Count)
        {
            return null;
        }

        return this.playerParty[characterIndex_];
    }


    //Function used to add a character to the player party. Returns true if they were added
    public bool AddCharacterToParty(Character newCharacter_)
    {
        //Looping through the player party to find an empty slot
        for(int i = 0; i < this.playerParty.Count; ++i)
        {
            //If the current slot is empty, we put the new character there
            if(this.playerParty[i] == null)
            {
                this.playerParty[i] = newCharacter_;
                //Setting the new character's position in the combat grid so that they aren't sharing a tile with someone else
                this.playerParty[i].charCombatStats.SetCombatPosition();
                return true;
            }
        }

        //If we made it through the loop without finding an empty slot, the party's full
        return false;
    }


    //Function used to switch positions of two characters in the party
    public void SwapCharacterPositions(int charIndex1_, int charIndex2_)
    {
        //If either of the character indexes is out of bounds of the player party list, nothing happens
        if(charIndex1_ < 0 || charIndex2_ < 0 || charIndex1_ >= this.playerParty.Count || charIndex2_ >= this.playerParty.Count)
        {
            return;
        }

        //Creating a placeholder to hold the reference to character 1
        Character placeholder = this.playerParty[charIndex1_];
        //Setting character 2 to character 1's position
        this.playerParty[charIndex1_] = this.playerParty[charIndex2_];
        //Placing the reference to character 1 to character 2's position
        this.playerParty[charIndex2_] = placeholder;
    }


    //Selects the party group based on the number given
    public void SelectPartyGroup(int groupNumber_)
    {
        //Sets the selected group
        switch(groupNumber_)
        {
            case 1:
                this.selectedGroup = PartyGroup.group1;
                break;
            case 2:
                this.selectedGroup = PartyGroup.group2;
                break;
            case 3:
                this.selectedGroup = PartyGroup.group3;
                break;
            default:
                this.selectedGroup = PartyGroup.group1;
                break;
        }
    }


    //Function called externally to find out how many more characters can be added to the player party
    public int FindEmptyPartySlots()
    {
        //Int to count how many empty slots there are
        int slots = 0;

        //Looping through each of the party slots
        foreach(Character charSlot in this.playerParty)
        {
            if(charSlot == null)
            {
                slots += 1;
            }
        }

        return slots;
    }


    //Function called externally On Time Advance to update each character's food/water/sleep/energy levels
    public void AdvanceTimeForAllCharacters()
    {
        //Looping through each player character in this manager
        foreach(Character c in this.playerParty)
        {
            //If the current character we're checking isn't an empty slot, we advance their time
            if(c != null)
            {
                c.charPhysState.OnTimeAdvanced();
            }
        }
    }
}


//Class used in CharacterManager.cs. Lets us create a list in the inspector to set character objects based on race and sex
[System.Serializable]
public class CharacterDefinition
{
    //The race of the character
    public Races race = Races.Human;
    //The sex of the character
    public Genders gender = Genders.Male;
    //The game object for the character that's spawned
    public GameObject characterObject;
}