using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(ReceiveEvent))]
public class PartyGroup : MonoBehaviour
{
    //Static references to each of the party groups
    public static PartyGroup group1;
    public static PartyGroup group2;
    public static PartyGroup group3;

    //The index for which group this object is
    [Range(1, 3)]
    public int groupIndex = 1;

    //The list of all characters currently in this party and their position on the combat board
    [HideInInspector]
    public Dictionary<Character, Vector2> charactersInParty;

    //The maximum number of combat board positions
    private Vector2 combatColsRows = new Vector2(3, 8);



    //Function called when this object is created
    private void Awake()
    {
        //Initializing the character in party dictionary
        this.charactersInParty = new Dictionary<Character, Vector2>();

        //Setting the static reference for this group
        switch(this.groupIndex)
        {
            case 1:
                if(PartyGroup.group1 != null)
                {
                    Destroy(this);
                }
                else
                {
                    PartyGroup.group1 = this;
                }
                break;
            case 2:
                if (PartyGroup.group2 != null)
                {
                    Destroy(this);
                }
                else
                {
                    PartyGroup.group2 = this;
                }
                break;
            case 3:
                if (PartyGroup.group3 != null)
                {
                    Destroy(this);
                }
                else
                {
                    PartyGroup.group3 = this;
                }
                break;
        }
    }


	//Function called externally to add a character to this party group
    public bool AddCharacterToGroup(Character charToAdd_)
    {
        //If the character is already in the total player party
        if(CharacterManager.globalReference.playerParty.Contains(charToAdd_))
        {
            //Making sure the character isn't already in this group
            if (!this.charactersInParty.ContainsKey(charToAdd_))
            {
                //Checking to see if the character is in group 1 (as long as this group isn't group 1)
                if(PartyGroup.group1 != this && PartyGroup.group1.charactersInParty.ContainsKey(charToAdd_))
                {
                    //Making sure the group is on the same tile as we are
                    if(this.GetComponent<Movement>().currentTile == PartyGroup.group1.GetComponent<Movement>().currentTile)
                    {
                        //Adding the character to our group
                        Vector2 charPosition = PartyGroup.group1.charactersInParty[charToAdd_];
                        this.charactersInParty.Add(charToAdd_, charPosition);
                        //Removing the character from the other group
                        PartyGroup.group1.charactersInParty.Remove(charToAdd_);
                        //Parenting the character to this group's transform
                        charToAdd_.transform.SetParent(this.transform);
                    }
                }
                //Checking group 2
                else if(PartyGroup.group2 != this && PartyGroup.group2.charactersInParty.ContainsKey(charToAdd_))
                {
                    //Making sure the group is on the same tile as we are
                    if (this.GetComponent<Movement>().currentTile == PartyGroup.group2.GetComponent<Movement>().currentTile)
                    {
                        //Adding the character to our group
                        Vector2 charPosition = PartyGroup.group2.charactersInParty[charToAdd_];
                        this.charactersInParty.Add(charToAdd_, charPosition);
                        //Removing the character from the other group
                        PartyGroup.group2.charactersInParty.Remove(charToAdd_);
                        //Parenting the character to this group's transform
                        charToAdd_.transform.SetParent(this.transform);
                    }
                }
                //Checking group 3
                else if(PartyGroup.group3 != this && PartyGroup.group3.charactersInParty.ContainsKey(charToAdd_))
                {
                    //Making sure the group is on the same tile as we are
                    if (this.GetComponent<Movement>().currentTile == PartyGroup.group3.GetComponent<Movement>().currentTile)
                    {
                        //Adding the character to our group
                        Vector2 charPosition = PartyGroup.group3.charactersInParty[charToAdd_];
                        this.charactersInParty.Add(charToAdd_, charPosition);
                        //Removing the character from the other group
                        PartyGroup.group3.charactersInParty.Remove(charToAdd_);
                        //Parenting the character to this group's transform
                        charToAdd_.transform.SetParent(this.transform);
                    }
                }

                //Checking the character's combat position so that it doesn't overlap with someone else
                this.CheckCombatPositionForCharacter(charToAdd_);
                
                return true;
            }
        }
        //If the character isn't in the party, we make sure the player doesn't already have a full group of characters
        else if (CharacterManager.globalReference.FindEmptyPartySlots() > 0)
        {
            //Adding the character to the CharacterManager's player party
            CharacterManager.globalReference.AddCharacterToParty(charToAdd_);
            //Adding the character to our group
            Vector2 charPosition = new Vector2(1,0);
            this.charactersInParty.Add(charToAdd_, charPosition);
            //Parenting the character to this group's transform
            charToAdd_.transform.SetParent(this.transform);
            charToAdd_.transform.position = new Vector3(0, 0, 0);
            //Checking the character's combat position so that it doesn't overlap with someone else
            this.CheckCombatPositionForCharacter(charToAdd_);
            return true;
        }

        //If none of the other parameters were met, the character couldn't be added
        return false;
    }


    //Making sure two characters in the group aren't taking up the same spot in combat
    private void CheckCombatPositionForCharacter(Character charToCheck_)
    {
        //The position of the character we're checking
        Vector2 charactersPosition = this.charactersInParty[charToCheck_];
        
        foreach (Character charInGroup in this.charactersInParty.Keys)
        {
            //Making sure we aren't checking against the same character, because that will ALWAYS cause a problem
            if (charInGroup != charToCheck_)
            {
                //If someone else is already occupying the same spot as the character we're checking
                if (charactersPosition == this.charactersInParty[charInGroup])
                {
                    //Loop through each column of combat positions on the grid starting from the middle
                    for (int c = 1; c < this.combatColsRows.x; ++c)
                    {
                        //Looping through each row of combat positions in the current column
                        for(int r = 0; r < this.combatColsRows.y; ++r)
                        {
                            //Creating a vector2 to hold the new position
                            Vector2 newCombatPos = new Vector2(c, r);

                            //Checking to see if any other character currently occupies this position
                            if(!this.charactersInParty.ContainsValue(newCombatPos))
                            {
                                //If nobody else is in this new position, the character to check is set to be there
                                this.charactersInParty[charToCheck_] = newCombatPos;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
