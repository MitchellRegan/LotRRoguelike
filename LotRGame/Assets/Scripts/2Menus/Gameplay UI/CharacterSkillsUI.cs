using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillsUI : MonoBehaviour
{
    //Reference to the character whose skills will be displayed
    private Character selectedCharacter;
    private Skills selectedCharacterSkillList;

    //Reference to the text field that displays the selected character's name
    public Text nameText;

    //Reference to the sliders that display their given skill
    public Slider cookingSlider;
    public Slider healingSlider;
    public Slider craftingSlider;

    public Slider foragingSlider;
    public Slider trackingSlider;
    public Slider fishingSlider;

    public Slider climbingSlider;
    public Slider hidingSlider;
    public Slider swimmingSlider;



    //Function called when this component is enabled
    public void OnEnable()
    {
        //If there isn't a selected character or the one we have isn't in the party anymore, we need to find one
        if(this.selectedCharacter == null || !CharacterManager.globalReference.playerParty.Contains(this.selectedCharacter))
        {
            foreach(Character playerChar in CharacterManager.globalReference.playerParty)
            {
                //As soon as we find a character that isn't null, we get the component references and the loop is broken
                if(playerChar != null)
                {
                    this.selectedCharacter = playerChar;
                    this.selectedCharacterSkillList = this.selectedCharacter.GetComponent<Skills>();
                    break;
                }
            }
        }

        //Updating the sliders
        this.UpdateSliders();
    }


    //Function called internally to update all of the sliders to the character's current values
    private void UpdateSliders()
    {
        //Setting the name field to display the selected character's name
        this.nameText.text = this.selectedCharacter.firstName + "'s Skills";

        this.cookingSlider.value = this.selectedCharacterSkillList.cooking + this.selectedCharacterSkillList.cookingMod;
        this.healingSlider.value = this.selectedCharacterSkillList.healing + this.selectedCharacterSkillList.healingMod;
        this.craftingSlider.value = this.selectedCharacterSkillList.crafting + this.selectedCharacterSkillList.craftingMod;

        this.foragingSlider.value = this.selectedCharacterSkillList.foraging + this.selectedCharacterSkillList.foragingMod;
        this.trackingSlider.value = this.selectedCharacterSkillList.tracking + this.selectedCharacterSkillList.trackingMod;
        this.fishingSlider.value = this.selectedCharacterSkillList.fishing + this.selectedCharacterSkillList.fishingMod;

        this.climbingSlider.value = this.selectedCharacterSkillList.climbing + this.selectedCharacterSkillList.climbingMod;
        this.hidingSlider.value = this.selectedCharacterSkillList.hiding + this.selectedCharacterSkillList.hidingMod;
        this.swimmingSlider.value = this.selectedCharacterSkillList.swimming + this.selectedCharacterSkillList.swimmingMod;
    }


    //Function called externally to cycle to the next character in the player party
	public void GoToNextCharacter()
    {
        //Finding the index of the character that's currently selected
        int currentIndex = CharacterManager.globalReference.playerParty.IndexOf(this.selectedCharacter);

        for(int i = currentIndex; ; ++i)
        {
            //Making sure we stay within the bounds of the player party list
            if(i >= CharacterManager.globalReference.maxPartySize)
            {
                i = 0;
            }

            //Once we find a character that isn't null, we save the reference to it and break the loop
            if(CharacterManager.globalReference.GetCharacterAtIndex(i) != null)
            {
                this.selectedCharacter = CharacterManager.globalReference.GetCharacterAtIndex(i);
                this.selectedCharacterSkillList = this.selectedCharacter.GetComponent<Skills>();
                break;
            }
        }

        //Updating the sliders
        this.UpdateSliders();
    }


    //Function called externally to cycle to the previous character in the player party
    public void GoToPrevCharacter()
    {
        //Finding the index of the character that's currently selected
        int currentIndex = CharacterManager.globalReference.playerParty.IndexOf(this.selectedCharacter);

        for (int i = currentIndex; ; --i)
        {
            //Making sure we stay within the bounds of the player party list
            if (i < 0)
            {
                i = CharacterManager.globalReference.maxPartySize - 1;
            }

            //Once we find a character that isn't null, we save the reference to it and break the loop
            if (CharacterManager.globalReference.GetCharacterAtIndex(i) != null)
            {
                this.selectedCharacter = CharacterManager.globalReference.GetCharacterAtIndex(i);
                this.selectedCharacterSkillList = this.selectedCharacter.GetComponent<Skills>();
                break;
            }
        }

        //Updating the sliders
        this.UpdateSliders();
    }


    //Function called externally to go to a character at a designated index
    public void GoToCharacterAtIndex(int characterIndex_)
    {
        //If the index given doesn't have a character, nothing happens
        if(CharacterManager.globalReference.GetCharacterAtIndex(characterIndex_) == null)
        {
            return;
        }

        //Saving the references to the given character
        this.selectedCharacter = CharacterManager.globalReference.GetCharacterAtIndex(characterIndex_);
        this.selectedCharacterSkillList = this.selectedCharacter.GetComponent<Skills>();

        //Updating the sliders for the new character
        this.UpdateSliders();
    }
}
