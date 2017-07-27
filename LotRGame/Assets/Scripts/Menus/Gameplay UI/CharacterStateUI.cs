using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateUI : MonoBehaviour
{
    //Reference to the character whose state will be displayed
    private Character selectedCharacter;
    private PhysicalState selectedCharacterState;

    //Reference to the text field that displays the selected character's name, clan, sex, race, and physical features
    public Text nameText;
    public Text clanText;
    public Text sexText;
    public Text raceText;
    public Text metricText;
    public Text imperialText;

    //References to the sliders that display the selected character's health, hunger, thirst, sleep, and energy
    public Slider healthSlider;
    public Text healthValueText;

    public Slider hungerSlider;
    public Text hungerValueText;

    public Slider thirstSlider;
    public Text thirstValueText;

    public Slider sleepSlider;
    public Text sleepValueText;

    public Slider energySlider;
    public Text energyValueText;



    //Function called when this component is enabled
    public void OnEnable()
    {
        //Finding the index of the character that's currently selected
        this.selectedCharacter = CharacterManager.globalReference.selectedCharacter;
        this.selectedCharacterState = this.selectedCharacter.charPhysState;

        //Updating the sliders
        this.UpdateTextAndSliders();
    }


    //Function used to update all of the sliders and text fields to the character's current values
    public void UpdateTextAndSliders()
    {
        //Setting the name field to display the selected character's name
        this.nameText.text = this.selectedCharacter.firstName + "\n" + this.selectedCharacter.lastName;
        //Setting the clan field to display the character's clan
        this.clanText.text = "Clan: " + this.selectedCharacter.clanName;
        //Setting the sex field to display their sex
        this.sexText.text = "Sex: " + this.selectedCharacter.sex;
        //Setting the race field to display their race
        this.raceText.text = "Race: " + this.selectedCharacter.race;
        //Setting the metric text to display their height and width in cm and kg
        this.metricText.text = this.selectedCharacter.height + " cm Tall, " + this.selectedCharacter.weight + " kg.";

        //Setting the imperial text to display their height and width in lb and feet
        int pounds = Mathf.RoundToInt(this.selectedCharacter.weight * 2.2046f);
        int inches = Mathf.RoundToInt(this.selectedCharacter.height * 0.3937f);
        int feet = inches / 12;
        inches = inches % 12;
        this.imperialText.text = feet + "\'" + inches + "\" Tall, " + pounds + " lb.";
    }


    //Function called externally to cycle to the next character in the player party
    public void GoToNextCharacter()
    {
        //Telling the character manager to go to the next character
        CharacterManager.globalReference.SelectNextCharacter();

        //Finding the index of the character that's currently selected
        this.selectedCharacter = CharacterManager.globalReference.selectedCharacter;
        this.selectedCharacterState = this.selectedCharacter.charPhysState;

        //Updating the sliders
        this.UpdateTextAndSliders();
    }


    //Function called externally to cycle to the previous character in the player party
    public void GoToPrevCharacter()
    {
        //Telling the character manager to go to the previous character
        CharacterManager.globalReference.SelectPreviousCharacter();

        //Finding the index of the character that's currently selected
        this.selectedCharacter = CharacterManager.globalReference.selectedCharacter;
        this.selectedCharacterState = this.selectedCharacter.charPhysState;

        //Updating the sliders
        this.UpdateTextAndSliders();
    }


    //Function called externally to go to a character at a designated index
    public void GoToCharacterAtIndex(int characterIndex_)
    {
        //If the index given doesn't have a character, nothing happens
        if (CharacterManager.globalReference.GetCharacterAtIndex(characterIndex_) == null)
        {
            return;
        }

        //Saving the references to the given character
        this.selectedCharacter = CharacterManager.globalReference.GetCharacterAtIndex(characterIndex_);
        this.selectedCharacterState = this.selectedCharacter.GetComponent<PhysicalState>();

        //Updating the sliders for the new character
        this.UpdateTextAndSliders();
    }


    //Function called every frame
    private void Update()
    {
        //Setting the sliders to display the character's current health, hunger, thirst, sleep, and energy values
        this.healthSlider.maxValue = this.selectedCharacterState.maxHealth;
        this.healthSlider.value = this.selectedCharacterState.currentHealth;
        this.healthValueText.text = this.selectedCharacterState.currentHealth + " / " + this.selectedCharacterState.maxHealth;

        this.hungerSlider.maxValue = this.selectedCharacterState.maxFood;
        this.hungerSlider.value = this.selectedCharacterState.currentFood;
        this.hungerValueText.text = this.selectedCharacterState.currentFood + " / " + this.selectedCharacterState.maxFood;

        this.thirstSlider.maxValue = this.selectedCharacterState.maxWater;
        this.thirstSlider.value = this.selectedCharacterState.currentWater;
        this.thirstValueText.text = this.selectedCharacterState.currentWater + " / " + this.selectedCharacterState.maxWater;

        this.sleepSlider.maxValue = this.selectedCharacterState.maxSleep;
        this.sleepSlider.value = this.selectedCharacterState.currentSleep;
        this.sleepValueText.text = this.selectedCharacterState.currentSleep + " / " + this.selectedCharacterState.maxSleep;

        this.energySlider.maxValue = this.selectedCharacterState.maxEnergy;
        this.energySlider.value = this.selectedCharacterState.currentEnergy;
        this.energyValueText.text = Mathf.RoundToInt(this.selectedCharacterState.currentEnergy * 100) + " / " + (this.selectedCharacterState.maxEnergy * 100);
    }
}
