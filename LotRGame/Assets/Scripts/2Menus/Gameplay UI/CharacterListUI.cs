using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListUI : MonoBehaviour
{
    //A reference to the character that this component displays
    private Character displayedCharacter;
    private PhysicalState characterState;

    //The index location of the displayed character in the PlayerParty list in CharacterManager.cs
    [Range(0, 9)]
    public int characterIndex = 0;

    //A reference to the UI element that hides the character info if there's no character in this slot
    public GameObject minimizer;

    //The text field that has this character's name
    public Text nameText;
    //The slider that represents this character's health bar
    public Slider healthBar;



	// Use this for initialization
	private void Start ()
    {
        this.SetCharacterInfo();
	}
	

	// Update is called once per frame
	private void Update ()
    {
        //Updates the player health if there's a player in the given index
		if(this.displayedCharacter != null)
        {
            this.healthBar.value = this.characterState.currentHealth;
        }
        //If the displayed character is empty but the character exists, we need to get the reference to it
        else if (CharacterManager.globalReference.GetCharacterAtIndex(this.characterIndex) != null)
        {
            this.SetCharacterInfo();
        }
        //If there's no character at the index, the info is hidden
        else
        {
            this.minimizer.SetActive(false);
        }
	}


    //Make functions to switch with characters at differ index
    public void MoveCharacterPosition(int indexToMoveTo_)
    {
        CharacterManager.globalReference.SwapCharacterPositions(this.characterIndex, indexToMoveTo_);
    }


    //Function used to get the character info at the given index. Used when setting or changing character positions
    public void SetCharacterInfo()
    {
        //Getting the reference to the character at the given index
        this.displayedCharacter = CharacterManager.globalReference.GetCharacterAtIndex(this.characterIndex);

        //Sets the stats if the character exists
        if (this.displayedCharacter != null)
        {
            //Gets the character state component reference
            this.characterState = this.displayedCharacter.GetComponent<PhysicalState>();

            //Setting the name text to the character's name
            this.nameText.text = this.displayedCharacter.firstName + " " + this.displayedCharacter.lastName;

            //Setting the range of the health bar slider
            this.healthBar.maxValue = this.characterState.maxHealth;
            this.healthBar.value = this.characterState.currentHealth;
        }
    }
}
