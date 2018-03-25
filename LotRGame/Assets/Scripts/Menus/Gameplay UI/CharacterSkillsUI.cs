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

    [Space(8)]

    //Reference to the sliders that display their given skill
    public Slider unarmedSlider;
    public Slider daggerSlider;
    public Slider swordSlider;
    public Slider maulSlider;
    public Slider poleSlider;
    public Slider bowSlider;

    [Space(8)]

    public Slider arcaneMagicSlider;
    public Slider holyMagicSlider;
    public Slider darkMagicSlider;
    public Slider fireMagicSlider;
    public Slider waterMagicSlider;
    public Slider windMagicSlider;
    public Slider electricMagicSlider;
    public Slider stoneMagicSlider;

    [Space(8)]

    public Slider survivalistSlider;
    public Slider socialSlider;





    //Function called when this component is enabled
    public void OnEnable()
    {
        //Finding the index of the character that's currently selected
        this.selectedCharacter = CharacterManager.globalReference.selectedCharacter;
        this.selectedCharacterSkillList = this.selectedCharacter.charSkills;

        //Updating the sliders
        this.UpdateSliders();
    }


    //Function called every frame
    private void Update()
    {
        this.UpdateSliders();
    }


    //Function called internally to update all of the sliders to the character's current values
    private void UpdateSliders()
    {
        //Setting the name field to display the selected character's name
        this.nameText.text = this.selectedCharacter.firstName + "\n" + this.selectedCharacter.lastName;
        
        this.unarmedSlider.value = this.selectedCharacter.charSkills.unarmed + this.selectedCharacter.charSkills.unarmedMod;
        this.daggerSlider.value = this.selectedCharacter.charSkills.daggers + this.selectedCharacter.charSkills.daggersMod;
        this.swordSlider.value = this.selectedCharacter.charSkills.swords + this.selectedCharacter.charSkills.swordsMod;
        this.maulSlider.value = this.selectedCharacter.charSkills.mauls + this.selectedCharacter.charSkills.maulsMod;
        this.poleSlider.value = this.selectedCharacter.charSkills.poles + this.selectedCharacter.charSkills.polesMod;
        this.bowSlider.value = this.selectedCharacter.charSkills.bows + this.selectedCharacter.charSkills.bowsMod;

        this.arcaneMagicSlider.value = this.selectedCharacter.charSkills.arcaneMagic + this.selectedCharacter.charSkills.arcaneMagicMod;
        this.holyMagicSlider.value = this.selectedCharacter.charSkills.holyMagic + this.selectedCharacter.charSkills.holyMagicMod;
        this.darkMagicSlider.value = this.selectedCharacter.charSkills.darkMagic + this.selectedCharacter.charSkills.darkMagicMod;
        this.fireMagicSlider.value = this.selectedCharacter.charSkills.fireMagic + this.selectedCharacter.charSkills.fireMagicMod;
        this.waterMagicSlider.value = this.selectedCharacter.charSkills.waterMagic + this.selectedCharacter.charSkills.waterMagicMod;
        this.windMagicSlider.value = this.selectedCharacter.charSkills.windMagic + this.selectedCharacter.charSkills.windMagicMod;
        this.electricMagicSlider.value = this.selectedCharacter.charSkills.electricMagic + this.selectedCharacter.charSkills.electricMagicMod;
        this.stoneMagicSlider.value = this.selectedCharacter.charSkills.stoneMagic + this.selectedCharacter.charSkills.stoneMagicMod;

        this.survivalistSlider.value = this.selectedCharacter.charSkills.survivalist + this.selectedCharacter.charSkills.survivalistMod;
        this.socialSlider.value = this.selectedCharacter.charSkills.social + this.selectedCharacter.charSkills.socialMod;
    }


    //Function called externally to cycle to the next character in the player party
	public void GoToNextCharacter()
    {
        //Telling the character manager to go to the next character
        CharacterManager.globalReference.SelectNextCharacter();

        //Finding the index of the character that's currently selected
        this.selectedCharacter = CharacterManager.globalReference.selectedCharacter;
        this.selectedCharacterSkillList = this.selectedCharacter.charSkills;

        //Updating the sliders
        this.UpdateSliders();
    }


    //Function called externally to cycle to the previous character in the player party
    public void GoToPrevCharacter()
    {
        //Telling the character manager to go to the previous character
        CharacterManager.globalReference.SelectPreviousCharacter();

        //Finding the index of the character that's currently selected
        this.selectedCharacter = CharacterManager.globalReference.selectedCharacter;
        this.selectedCharacterSkillList = this.selectedCharacter.charSkills;

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
