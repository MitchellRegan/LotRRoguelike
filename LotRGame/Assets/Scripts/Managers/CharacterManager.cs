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

    //The character that's currently selected to show the stats and skills of
    [HideInInspector]
    public Character selectedCharacter;

    //The list of characters that have died in the current game
    [HideInInspector]
    public List<DeadCharacterInfo> deadCharacters;

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

        //Initializing the list of dead characters
        this.deadCharacters = new List<DeadCharacterInfo>();
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

                //If the character was added to the first slot, they become the selected character
                if(i == 0)
                {
                    this.selectedCharacter = newCharacter_;
                }
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
        //The party group that we'll be setting as the selected group
        PartyGroup newGroup;

        //Sets the selected group
        switch(groupNumber_)
        {
            case 1:
                newGroup = PartyGroup.group1;
                break;
            case 2:
                newGroup = PartyGroup.group2;
                break;
            case 3:
                newGroup = PartyGroup.group3;
                break;
            default:
                newGroup = PartyGroup.group1;
                break;
        }

        //Looping through each character index in the selected party and setting the first one we find to the selected character
        Character charToSelect = null;
        foreach(Character charSlot in newGroup.charactersInParty)
        {
            if(charSlot != null)
            {
                charToSelect = charSlot;
                break;
            }
        }

        //If the selected character is null, that means there wasn't a character in the party, so we stick with the party we have
        if(charToSelect != null)
        {
            this.selectedGroup = newGroup;
            this.selectedCharacter = charToSelect;
        }
    }


    //Function called externally to select the next character in line in the party group
    public void SelectNextCharacter()
    {
        //Finding the index of the character that's currently selected
        int currentIndex = this.selectedGroup.charactersInParty.IndexOf(this.selectedCharacter);

        for (int i = currentIndex+1; ; ++i)
        {
            //Making sure we stay within the bounds of the player party list
            if (i >= this.selectedGroup.charactersInParty.Count)
            {
                i = 0;
            }

            //Once we find a character that isn't null, we save the reference to it and break the loop
            if (this.selectedGroup.charactersInParty[i] != null)
            {
                this.selectedCharacter = this.selectedGroup.charactersInParty[i];
                break;
            }
        }
    }


    //Function called externally to select the previous character in line in the party group
    public void SelectPreviousCharacter()
    {
        //Finding the index of the character that's currently selected
        int currentIndex = this.selectedGroup.charactersInParty.IndexOf(this.selectedCharacter);

        for(int i = currentIndex-1; ; --i)
        {
            //Making sure we stay within the bounds of the player party list
            if(i < 0)
            {
                i = this.selectedGroup.charactersInParty.Count - 1;
            }

            //Once we find a character that isn't null, we save the reference to it and break the loop
            if(this.selectedGroup.charactersInParty[i] != null)
            {
                this.selectedCharacter = this.selectedGroup.charactersInParty[i];
                break;
            }
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
    public RaceTypes.Races race = RaceTypes.Races.Human;
    //The sex of the character
    public Genders gender = Genders.Male;
    //The game object for the character that's spawned
    public GameObject characterObject;
}


//Class used in CharacterManager.cs to store information about a dead character
[System.Serializable]
public class DeadCharacterInfo
{
    public string firstName;
    public string lastName;
    public CharSpritePackage characterSprites;
}