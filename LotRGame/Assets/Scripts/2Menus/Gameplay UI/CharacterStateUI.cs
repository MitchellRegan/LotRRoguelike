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
    public Slider hungerSlider;
    public Slider thirstSlider;
    public Slider sleepSlider;
    public Slider energySlider;



    //Function called when this component is enabled
    public void OnEnable()
    {
        //If there isn't a selected character or the one we have isn't in the party anymore, we need to find one
        if (this.selectedCharacter == null || !CharacterManager.globalReference.playerParty.Contains(this.selectedCharacter))
        {
            foreach (Character playerChar in CharacterManager.globalReference.playerParty)
            {
                //As soon as we find a character that isn't null, we get the component references and the loop is broken
                if (playerChar != null)
                {
                    this.selectedCharacter = playerChar;
                    this.selectedCharacterState = this.selectedCharacter.GetComponent<PhysicalState>();
                    break;
                }
            }
        }

        //Updating the sliders
        this.UpdateTextAndSliders();
    }


    //Function used to update all of the sliders and text fields to the character's current values
    public void UpdateTextAndSliders()
    {
        //Setting the name field to display the selected character's name
        this.nameText.text = this.selectedCharacter.firstName + " " + this.selectedCharacter.lastName;
        //Setting the clan field to display the character's clan
        this.clanText.text = "of the " + this.selectedCharacter.clanName + " clan.";
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
        //Finding the index of the character that's currently selected
        int currentIndex = CharacterManager.globalReference.playerParty.IndexOf(this.selectedCharacter);

        for (int i = currentIndex; ; ++i)
        {
            //Making sure we stay within the bounds of the player party list
            if (i >= CharacterManager.globalReference.maxPartySize)
            {
                i = 0;
            }

            //Once we find a character that isn't null, we save the reference to it and break the loop
            if (CharacterManager.globalReference.GetCharacterAtIndex(i) != null)
            {
                this.selectedCharacter = CharacterManager.globalReference.GetCharacterAtIndex(i);
                this.selectedCharacterState = this.selectedCharacter.GetComponent<PhysicalState>();
                break;
            }
        }

        //Updating the sliders
        this.UpdateTextAndSliders();
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
                this.selectedCharacterState = this.selectedCharacter.GetComponent<PhysicalState>();
                break;
            }
        }

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

        this.hungerSlider.maxValue = this.selectedCharacterState.maxFood;
        this.hungerSlider.value = this.selectedCharacterState.currentFood;

        this.thirstSlider.maxValue = this.selectedCharacterState.maxWater;
        this.thirstSlider.value = this.selectedCharacterState.currentWater;

        this.sleepSlider.maxValue = this.selectedCharacterState.maxSleep;
        this.sleepSlider.value = this.selectedCharacterState.currentSleep;

        this.energySlider.maxValue = this.selectedCharacterState.maxEnergy;
        this.energySlider.value = this.selectedCharacterState.currentEnergy;
    }
}
