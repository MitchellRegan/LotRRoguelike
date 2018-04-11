using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    //Static list of all existing entities of Damage Text so we can stagger their display time
    public static List<DamageText> allDamageTextObj = new List<DamageText>();

    //Reference to the Text component
    public Text ourText;
    //Reference to the background image component
    public Image background;

    //The amount of time before this text appears
    private float timeBeforeTextShows = 1;

    //The amount of time before this text starts fading out
    public float timeBeforeFade = 0.6f;

    //The amount of time in seconds that this object is displayed before it disappears
    public float timeBeforeDestroyed = 1;
    private float destroyTimer = 0;

    //The amount of delay between damage texts that are spawned at the same time
    public float staggerTime = 0.25f;

    //The spawn position of this object
    private Vector3 spawnPosition;
    //The offset from the spawned location that this text is set to
    public Vector3 startingOffset = new Vector3(0, 1, 0);
    //The offset that this moves to before fading out
    public Vector3 endOffset = new Vector3(0, 10, 0);

    //The curve that scales this text over the course of this lifetime
    public AnimationCurve scaleCurve;

    //The size of the text when displaying normal damage
    public int normalDamageFontSize = 14;
    //The size of the text when displaying crit damage
    public int critDamageFontSize = 26;

    //The list of all types of damage and the color that they change this object's text
    public Color slashDamageColor = Color.gray;
    public Color stabDamageColor = Color.gray;
    public Color crushDamageColor = Color.gray;
    public Color arcaneDamageColor = Color.gray;
    public Color holyDamageColor = Color.gray;
    public Color darkDamageColor = Color.gray;
    public Color fireDamageColor = Color.gray;
    public Color waterDamageColor = Color.gray;
    public Color windDamageColor = Color.gray;
    public Color electricDamageColor = Color.gray;
    public Color stoneDamageColor = Color.gray;
    public Color pureDamageColor = Color.black;
    public Color natureDamageColor = Color.black;



    //Function called when this object is created
    private void Awake()
    {
        if(DamageText.allDamageTextObj == null)
        {
            DamageText.allDamageTextObj = new List<DamageText>();
        }
    }


    //Function called from the combat manager to set our damage text, color, and position
    public void SetDamageToDisplay(float timeDelay_, int damageDealt_, CombatManager.DamageType type_, Vector3 position_, bool isCrit_, bool isHeal_ = false)
    {
        //Adding this DamageText to the static list
        DamageText.allDamageTextObj.Add(this);

        //Moving this object to the given position with the starting offset
        this.spawnPosition = position_;
        this.transform.position = this.spawnPosition + this.startingOffset;

        //Setting the amount of time before this text appears
        this.timeBeforeTextShows = timeDelay_;

        //Finding out if there are other damage texts on this position that requires us to stagger this time delay
        float staggerDelay = this.FindTimeDelay();
        if(staggerDelay >= timeDelay_)
        {
            staggerDelay -= timeDelay_;
            staggerDelay += this.staggerTime;
        }
        this.timeBeforeTextShows += staggerDelay;

        //Setting the text color to black
        this.ourText.color = Color.black;

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
            case CombatManager.DamageType.Slashing:
                this.background.color = this.slashDamageColor;
                break;
            case CombatManager.DamageType.Stabbing:
                this.background.color = this.stabDamageColor;
                break;
            case CombatManager.DamageType.Crushing:
                this.background.color = this.crushDamageColor;
                break;
            case CombatManager.DamageType.Arcane:
                this.background.color = this.arcaneDamageColor;
                break;
            case CombatManager.DamageType.Holy:
                this.background.color = this.holyDamageColor;
                break;
            case CombatManager.DamageType.Dark:
                this.background.color = this.darkDamageColor;
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
            case CombatManager.DamageType.Pure:
                this.background.color = this.pureDamageColor;
                break;
            case CombatManager.DamageType.Nature:
                this.background.color = this.natureDamageColor;
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

        //Setting the text and background color to be transparent if our time before text shows is greater than 0
        if (this.timeBeforeTextShows > 0)
        {
            this.ourText.color = new Color(this.ourText.color.r, this.ourText.color.g, this.ourText.color.b, 0);
            this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 0);
        }
    }


    //Function called from the combat manager to show that an attack missed
    public void DisplayMiss(float timeDelay_, Vector3 position_)
    {
        //Adding this DamageText to the static list
        DamageText.allDamageTextObj.Add(this);

        //Moving this object to the given position with the starting offset
        this.spawnPosition = position_;
        this.transform.position = this.spawnPosition + this.startingOffset;

        //Setting the amount of time before this text appears
        this.timeBeforeTextShows = timeDelay_ + this.FindTimeDelay();

        this.ourText.text = "MISS";
        this.ourText.fontSize = this.normalDamageFontSize;
        this.background.color = Color.white;

        //Setting the text and background color to be transparent if our time before text shows is greater than 0
        if (this.timeBeforeTextShows > 0)
        {
            this.ourText.color = new Color(this.ourText.color.r, this.ourText.color.g, this.ourText.color.b, 0);
            this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 0);
        }
    }


    //Function called every frame
    private void Update()
    {
        //If this text isn't showing yet
        if (this.timeBeforeTextShows > 0)
        {
            this.timeBeforeTextShows -= Time.deltaTime;

            //If the time is up, we make the text visible
            if(this.timeBeforeTextShows <= 0)
            {
                this.ourText.color = new Color(this.ourText.color.r, this.ourText.color.g, this.ourText.color.b, 1);
                this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 1);
            }
        }
        //If the text is showing
        else
        {
            //Adding the time that's passed to the timer
            this.destroyTimer += Time.deltaTime;

            //If our time is up, this object is removed from the static list and destroyed
            if (this.destroyTimer >= this.timeBeforeDestroyed)
            {
                DamageText.allDamageTextObj.Remove(this);
                Destroy(this.gameObject);
            }
            //If the time isn't up but it is above the time until fade, we start fading
            else
            {
                //Finding the percent of time that's passed before destroyed
                float lifetimePercent = this.destroyTimer / this.timeBeforeDestroyed;

                //Finding the percent difference between the start and end offsets
                Vector3 offsetPercent = (this.endOffset - this.startingOffset) * lifetimePercent;
                offsetPercent += this.startingOffset + this.spawnPosition;
                this.transform.position = offsetPercent;

                //Finding the scale multiplier using the lifetime percent and our animation curve
                float scaleMultiplier = this.scaleCurve.Evaluate(lifetimePercent);
                this.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1);

                //If the destroy timer is above the fade time, we need to start fading out
                if (this.destroyTimer > this.timeBeforeFade)
                {
                    //Finding the percent of time that's passed between the fade time and destroy time
                    float fadePercentPassed = (this.destroyTimer - this.timeBeforeFade) / (this.timeBeforeDestroyed - this.timeBeforeFade);

                    //Changing the alpha of the text and background to become transparent
                    this.ourText.color = new Color(this.ourText.color.r, this.ourText.color.g, this.ourText.color.b, 1 - fadePercentPassed);
                    this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 1 - fadePercentPassed);
                }
            }
        }
    }


    //Function called from SetDamageToDisplay and DisplayMiss so we can delay when this object appears if there's more than 1 in the same location
    private float FindTimeDelay()
    {
        //The amount of time that's returned
        float returnedDelay = 0;

        //Looping through all of the Damage Text objects in the static list to try and find any with the same spawn location
        for(int d = 0; d < DamageText.allDamageTextObj.Count; ++d)
        {
            //Making sure we aren't looking at this object
            if(DamageText.allDamageTextObj[d] != this)
            {
                //If the current damage text has the same start location as this one
                if (Mathf.RoundToInt(DamageText.allDamageTextObj[d].spawnPosition.x) == Mathf.RoundToInt(this.spawnPosition.x) && 
                    Mathf.RoundToInt(DamageText.allDamageTextObj[d].spawnPosition.y) == Mathf.RoundToInt(this.spawnPosition.y))
                {
                    //If the current damage text's time before it appears is greater than the current returned delay, we set the delay to that time
                    if(returnedDelay < DamageText.allDamageTextObj[d].timeBeforeTextShows)
                    {
                        returnedDelay = DamageText.allDamageTextObj[d].timeBeforeTextShows;
                    }
                }
            }
        }

        //Returning the delay time
        return returnedDelay;
    }
}
