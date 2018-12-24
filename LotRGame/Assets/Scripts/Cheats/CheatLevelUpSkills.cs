using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatLevelUpSkills : MonoBehaviour
{
    //The amount that each skill is increased when a cheat is used
    public int skillLevelsToGive = 1;

    //The buttons for cycling forward through the skill list
    public KeyCode cycleSkillForwardButton = KeyCode.F1;
    //The button for cycling backward through the skill list
    public KeyCode cycleSkillBackwardButton = KeyCode.F2;
    //The button for cycling forward through the player list
    public KeyCode cycleSelectedPlayerButton = KeyCode.F3;
    //The button for increasing the selected character's skill
    public KeyCode increaseSkillButton = KeyCode.F4;

    //Reference to the player character that's selected
    private Character selectedCharacter;

    //Reference to the index of the selected skill to increase
    private int selectedSkillIndex = 0;



    //Function called every frame
    private void Update()
    {
        //If the button to increase the selected player's skill is pressed
        if(Input.GetKeyDown(this.increaseSkillButton))
        {
            this.IncreaseSkill();
        }
        //If the button to cycle the selected skill forward is pressed
        else if(Input.GetKeyDown(this.cycleSkillForwardButton))
        {
            this.CycleSelectedSkill(true);
        }
        //If the button to cycle the selected skill backward is pressed
        else if(Input.GetKeyDown(this.cycleSkillBackwardButton))
        {
            this.CycleSelectedSkill(false);
        }
        //If the button to cycle the selected character is pressed
        else if(Input.GetKeyDown(this.cycleSelectedPlayerButton))
        {
            this.CycleSelectedPlayerCharacter();
        }
        //If the /? button is pressed we debug the keys for this script
        else if (Input.GetKeyDown(KeyCode.Slash))
        {
            Debug.Log("CHEAT: Level Up Skills info: Cycle Skill Forward: " + this.cycleSkillForwardButton + 
                ", Cycle Skill Backward: " + this.cycleSkillBackwardButton + 
                ", Cycle Selected Player Character: " + this.cycleSelectedPlayerButton +
                ", Increase Skill Button: " + this.increaseSkillButton);
        }
    }


    //Function called from Update to increase the selected player's skill
    private void IncreaseSkill()
    {
        //If we're not in the gameplay scene, nothing happens
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log("CHEAT: Level Up Skills, Increase Skill: ERROR not in gameplay scene");
            return;
        }

        //If we're in combat, nothing happens
        if (CombatManager.globalReference.GetComponent<Canvas>().enabled)
        {
            Debug.Log("CHEAT: Level Up Skills, Increase Skill: ERROR cannot spawn encounter while in combat");
            return;
        }

        //If the selected character is null we need to find the first player character
        if(this.selectedCharacter == null)
        {
            for (int pc = 0; pc < PartyGroup.group1.charactersInParty.Count; ++pc)
            {
                //If the character at this index isn't null, they're the selected player
                if (PartyGroup.group1.charactersInParty[pc] != null &&
                    PartyGroup.group1.charactersInParty[pc].charPhysState.currentHealth > 0)
                {
                    this.selectedCharacter = PartyGroup.group1.charactersInParty[pc];
                    break;
                }
            }
        }

        //If the selected character is still null, something has gone wrong
        if(this.selectedCharacter == null)
        {
            Debug.Log("CHEAT: Level Up Skills, Increase Skill: ERROR no valid character selected. DEBUG THIS");
            return;
        }

        //Increasing the selected character's skill by some amount
        this.selectedCharacter.charSkills.LevelUpSkill(this.GetSkillFromIndex(this.selectedSkillIndex), this.skillLevelsToGive);

        Debug.Log("CHEAT: Level Up Skills, Increase Skill: Increasing " + this.selectedCharacter.firstName + 
            "'s " + this.GetSkillFromIndex(this.selectedSkillIndex) + " level by " + 
            this.skillLevelsToGive + ". New level: " + this.selectedCharacter.charSkills.GetSkillLevelValue(this.GetSkillFromIndex(this.selectedSkillIndex)));
    }


    //Function called from Update to cycle the selected skill to increase
    private void CycleSelectedSkill(bool cycleForward_)
    {
        //If we cycle forward
        if(cycleForward_)
        {
            this.selectedSkillIndex += 1;

            //If the index goes above 16, we circle back to 0
            if(this.selectedSkillIndex > 16)
            {
                this.selectedSkillIndex = 0;
            }
        }
        //If we cycle backward
        else
        {
            this.selectedSkillIndex -= 1;

            //If the index goes below 0, we circle back to 16
            if(this.selectedSkillIndex < 0)
            {
                this.selectedSkillIndex = 16;
            }
        }
        
        Debug.Log("CHEAT: Level Up Skills, Cycle Selected Skill: Selected " + this.GetSkillFromIndex(this.selectedSkillIndex));
    }


    //Function called from Update to cycle through the player party characters
    private void CycleSelectedPlayerCharacter()
    {
        //Bool for if we've found the currently selected character
        bool foundSelectedChar = false;

        //Looping through all of characters in the player group
        for (int pc = 0; pc < PartyGroup.group1.charactersInParty.Count; ++pc)
        {
            Debug.Log("First Loop, index: " + pc + ", found selected: " + foundSelectedChar);
            //If the character at this index isn't null or dead
            if (PartyGroup.group1.charactersInParty[pc] != null &&
                PartyGroup.group1.charactersInParty[pc].charPhysState.currentHealth > 0)
            {
                Debug.Log("Index " + pc + " is a valid character");
                //If the current character is the one we're already selecting, we ignore it
                if (this.selectedCharacter == PartyGroup.group1.charactersInParty[pc])
                {
                    Debug.Log("Found the selected character " + this.selectedCharacter.firstName);
                    foundSelectedChar = true;
                }
                //If the current character isn't the current character
                else
                {
                    .Debug.Log("Found a diff")
                    //If the selected character is null or we've already found the previously selected character, we set this character as the selected one
                    if (this.selectedCharacter == null || foundSelectedChar)
                    {
                        this.selectedCharacter = PartyGroup.group1.charactersInParty[pc];
                        foundSelectedChar = true;
                        break;
                    }
                }
            }
        }

        //If we made it through the loop without finding the previously selected character
        if(!foundSelectedChar)
        {
            Debug.Log("Haven't found selected character");
            //We loop back around and select the first one found
            for (int c = 0; c < PartyGroup.group1.charactersInParty.Count; ++c)
            {
                //If the character at this index isn't null, they're the selected player
                if (PartyGroup.group1.charactersInParty[c] != null &&
                    PartyGroup.group1.charactersInParty[c].charPhysState.currentHealth > 0)
                {
                    this.selectedCharacter = PartyGroup.group1.charactersInParty[c];
                    break;
                }
            }
        }

        Debug.Log("CHEAT: Level Up Skill, Cycle Selected Player Character: Selected " + this.selectedCharacter.firstName);
    }


    //Function called from IncreaseSkill and CycleSelectedSkill to get the skill type from our index
    private SkillList GetSkillFromIndex(int index_)
    {
        //Switch statement based on the given index
        switch(index_)
        {
            case 0:
                return SkillList.Unarmed;

            case 1:
                return SkillList.Daggers;

            case 2:
                return SkillList.Swords;

            case 3:
                return SkillList.Mauls;

            case 4:
                return SkillList.Poles;

            case 5:
                return SkillList.Bows;

            case 6:
                return SkillList.Shields;

            case 7:
                return SkillList.ArcaneMagic;

            case 8:
                return SkillList.HolyMagic;

            case 9:
                return SkillList.DarkMagic;

            case 10:
                return SkillList.FireMagic;

            case 11:
                return SkillList.WaterMagic;

            case 12:
                return SkillList.WindMagic;

            case 13:
                return SkillList.ElectricMagic;

            case 14:
                return SkillList.StoneMagic;

            case 15:
                return SkillList.Survivalist;

            case 16:
                return SkillList.Social;

            default:
                Debug.Log("CHEAT: Level Up Skill, Get Skill From Index: ERROR invalid index given");
                return SkillList.Unarmed;
        }
    }
}
