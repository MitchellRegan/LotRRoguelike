using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DamageText : MonoBehaviour
{
    //Reference to this object's Text component
    private Text ourText;

    //The amount of time in seconds that this object is displayed before it disappears
    public float timeBeforeDestroyed = 1;

    //Reference to the background image for when crits happen
    public Image critBackground;

    //The size of the text when displaying normal damage
    public int normalDamageFontSize = 14;
    //The size of the text when displaying crit damage
    public int critDamageFontSize = 26;

    //The list of all types of damage and the color that they change this object's text
    public Color physDamageColor = Color.gray;
    public Color magicDamageColor = Color.gray;
    public Color fireDamageColor = Color.gray;
    public Color waterDamageColor = Color.gray;
    public Color electricDamageColor = Color.gray;
    public Color windDamageColor = Color.gray;
    public Color rockDamageColor = Color.gray;
    public Color lightDamageColor = Color.gray;
    public Color darkDamageColor = Color.gray;



    //Function called when this object is created
    private void Awake()
    {
        //Gets this object's text component
        this.ourText = this.GetComponent<Text>();
        //Sets our background crit image to be hidden by default
        this.critBackground.enabled = false;
    }


    //Function called from the combat manager to set our damage text, color, and position
    public void SetDamageToDisplay(int damageDealt_, CombatManager.DamageType type_, Vector3 position_, bool isCrit_)
    {
        //Moving this object to the given position
        this.transform.position = position_;
        //Setting the text for the damage dealt
        this.ourText.text = "" + damageDealt_;

        //Setting the color of the text based on the damage type
        switch(type_)
        {
            case CombatManager.DamageType.Physical:
                this.ourText.color = this.physDamageColor;
                break;
            case CombatManager.DamageType.Magic:
                this.ourText.color = this.magicDamageColor;
                break;
            case CombatManager.DamageType.Fire:
                this.ourText.color = this.fireDamageColor;
                break;
            case CombatManager.DamageType.Water:
                this.ourText.color = this.waterDamageColor;
                break;
            case CombatManager.DamageType.Electric:
                this.ourText.color = this.electricDamageColor;
                break;
            case CombatManager.DamageType.Wind:
                this.ourText.color = this.windDamageColor;
                break;
            case CombatManager.DamageType.Rock:
                this.ourText.color = this.rockDamageColor;
                break;
            case CombatManager.DamageType.Light:
                this.ourText.color = this.lightDamageColor;
                break;
            case CombatManager.DamageType.Dark:
                this.ourText.color = this.darkDamageColor;
                break;
        }

        //Setting the font size based on if the attack crit or not
        if(isCrit_)
        {
            this.ourText.fontSize = this.critDamageFontSize;
            this.critBackground.enabled = true;
        }
        else
        {
            this.ourText.fontSize = this.normalDamageFontSize;
        }
    }


    //Function called every frame
    private void Update()
    {
        //Subtracting the time that's passed from the time we have left
        this.timeBeforeDestroyed -= Time.deltaTime;

        //If our time is up, this object is destroyed
        if(this.timeBeforeDestroyed <= 0)
        {
            Destroy(this);
        }
    }
}
