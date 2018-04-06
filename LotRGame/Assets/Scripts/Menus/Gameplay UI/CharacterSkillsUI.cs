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
    public Slider shieldSlider;

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
        
        this.unarmedSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Unarmed);
        this.daggerSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Daggers);
        this.swordSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Swords);
        this.maulSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Mauls);
        this.poleSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Poles);
        this.bowSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Bows);
        this.shieldSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Shields);

        this.arcaneMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.ArcaneMagic);
        this.holyMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.HolyMagic);
        this.darkMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.DarkMagic);
        this.fireMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.FireMagic);
        this.waterMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.WaterMagic);
        this.windMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.WindMagic);
        this.electricMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.ElectricMagic);
        this.stoneMagicSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.StoneMagic);

        this.survivalistSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Survivalist);
        this.socialSlider.value = this.selectedCharacter.charSkills.GetSkillLevelValueWithMod(SkillList.Social);
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
