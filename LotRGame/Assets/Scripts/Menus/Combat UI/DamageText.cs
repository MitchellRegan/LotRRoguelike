using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    //Reference to the Text component
    public Text ourText;
    //Reference to the background image component
    public Image background;

    //The amount of time in seconds that this object is displayed before it disappears
    public float timeBeforeDestroyed = 1;

    //The max offset from the hit position that this can be moved
    public float maxOffset = 1;

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



    
    //Function called from the combat manager to set our damage text, color, and position
    public void SetDamageToDisplay(int damageDealt_, CombatManager.DamageType type_, Vector3 position_, bool isCrit_, bool isHeal_ = false)
    {
        //Moving this object to the given position
        this.transform.position = position_;

        //Finding the random position offset
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float offset = Random.Range(0, this.maxOffset);
        float xOffset = Mathf.Cos(angle) * offset;
        float yOffset = Mathf.Sin(angle) * offset;
        this.transform.position += new Vector3(xOffset, yOffset, 0);

        //Setting the text for the damage dealt
        this.ourText.text = "" + damageDealt_;

        //If this was a healing effect, we add a + sign
        if(isHeal_)
        {
            this.ourText.text = "+" + this.ourText.text;
        }

        //Setting the color of the text based on the damage type
        switch(type_)
        {
            case CombatManager.DamageType.Physical:
                this.background.color = this.physDamageColor;
                break;
            case CombatManager.DamageType.Magic:
                this.background.color = this.magicDamageColor;
                break;
            case CombatManager.DamageType.Fire:
                this.background.color = this.fireDamageColor;
                break;
            case CombatManager.DamageType.Water:
                this.background.color = this.waterDamageColor;
                break;
            case CombatManager.DamageType.Electric:
                this.background.color = this.electricDamageColor;
                break;
            case CombatManager.DamageType.Wind:
                this.background.color = this.windDamageColor;
                break;
            case CombatManager.DamageType.Rock:
                this.background.color = this.rockDamageColor;
                break;
            case CombatManager.DamageType.Holy:
                this.background.color = this.lightDamageColor;
                break;
            case CombatManager.DamageType.Dark:
                this.background.color = this.darkDamageColor;
                break;
        }

        //Setting the font size based on if the attack crit or not
        if(isCrit_)
        {
            this.ourText.fontSize = this.critDamageFontSize;
        }
        else
        {
            this.ourText.fontSize = this.normalDamageFontSize;
        }
    }


    //Function called from the combat manager to show that an attack missed
    public void DisplayMiss(Vector3 position_)
    {
        //Moving this object to the given position
        this.transform.position = position_;

        //Finding the random position offset
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float offset = Random.Range(0, this.maxOffset);
        float xOffset = Mathf.Cos(angle) * offset;
        float yOffset = Mathf.Sin(angle) * offset;
        this.transform.position += new Vector3(xOffset, yOffset, 0);

        this.ourText.text = "MISS";
        this.ourText.fontSize = this.normalDamageFontSize;
        this.background.color = Color.black;
    }


    //Function called every frame
    private void Update()
    {
        //Subtracting the time that's passed from the time we have left
        this.timeBeforeDestroyed -= Time.deltaTime;

        //If our time is up, this object is destroyed
        if(this.timeBeforeDestroyed <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
