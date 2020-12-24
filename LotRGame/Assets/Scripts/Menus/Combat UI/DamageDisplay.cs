using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    public TextMesh textMeshRef;
    public SpriteRenderer backgroundImage;
    public GameObject critBackground;

    //The spawn position of this object
    private Vector3 spawnPosition;
    //The offset from the spawned location that this text is set to
    public Vector3 startingOffset = new Vector3(0, 1, 0);
    //The offset that this moves to before fading out
    public Vector3 endOffset = new Vector3(0, 10, 0);

    //The amount of time before this text appears
    private float timeBeforeTextShows = 1;

    //The amount of time before this text starts fading out
    public float timeBeforeFade = 0.6f;

    //The curve that scales this text over the course of this lifetime
    public AnimationCurve scaleCurve;
    public float scaleMultiplier = 1;

    //The amount of time in seconds that this object is displayed before it disappears
    public float timeBeforeDestroyed = 1;
    private float destroyTimer = 0;

    //The amount of delay between damage texts that are spawned at the same time
    public float staggerTime = 0.25f;

    //The list of all types of damage and the color that they change this object's text
    public Color slashDamageColor = Color.gray;
    public Color stabDamageColor = Color.gray;
    public Color crushDamageColor = Color.gray;
    public Color arcaneDamageColor = new Color(0.33f, 0.91f, 0.81f);
    public Color holyDamageColor = new Color(0.97f, 1, 0.49f);
    public Color darkDamageColor = new Color(0.63f, 0.16f, 0.85f);
    public Color fireDamageColor = Color.red;
    public Color waterDamageColor = Color.blue;
    public Color windDamageColor = new Color(0.53f, 0.905f, 0.105f);
    public Color electricDamageColor = Color.yellow;
    public Color stoneDamageColor = new Color(0.54f, 0.26f, 0.02f);
    public Color pureDamageColor = Color.black;
    public Color natureDamageColor = Color.green;
    public Color bleedDamageColor = new Color(0.69f, 0, 0);
    public Color missColor = Color.white;

    //Static list of all existing entities of Damage Display so we can stagger their display time
    public static List<DamageDisplay> allDamageDisplayObj = new List<DamageDisplay>();



    //Function called when this object is created
    private void Awake()
    {
        if (DamageDisplay.allDamageDisplayObj == null)
        {
            DamageDisplay.allDamageDisplayObj = new List<DamageDisplay>();
        }
    }


    //Function called from the combat manager to set our damage text, color, and position
    public void SetDamageToDisplay(float timeDelay_, int damageDealt_, DamageType type_, Vector3 position_, bool isCrit_, bool isHeal_ = false)
    {
        //Adding this DamageText to the static list
        DamageDisplay.allDamageDisplayObj.Add(this);

        //Moving this object to the given position with the starting offset
        this.transform.position = position_ + this.startingOffset;
        this.spawnPosition = this.transform.position;

        //Setting the amount of time before this text appears
        this.timeBeforeTextShows = timeDelay_;

        //Finding out if there are other damage texts on this position that requires us to stagger this time delay
        float staggerDelay = this.FindTimeDelay();
        if (staggerDelay >= timeDelay_)
        {
            staggerDelay -= timeDelay_;
            staggerDelay += this.staggerTime;
        }
        this.timeBeforeTextShows += staggerDelay;

        //Setting the text for the damage dealt
        this.textMeshRef.text = "" + damageDealt_;

        //If this was a healing effect, we add a + sign
        if (isHeal_)
        {
            this.textMeshRef.text = "+" + this.textMeshRef.text;
        }

        //Setting the color of the background based on the damage type
        switch (type_)
        {
            case DamageType.Slashing:
                this.backgroundImage.color = this.slashDamageColor;
                break;
            case DamageType.Stabbing:
                this.backgroundImage.color = this.stabDamageColor;
                break;
            case DamageType.Crushing:
                this.backgroundImage.color = this.crushDamageColor;
                break;
            case DamageType.Arcane:
                this.backgroundImage.color = this.arcaneDamageColor;
                break;
            case DamageType.Holy:
                this.backgroundImage.color = this.holyDamageColor;
                break;
            case DamageType.Dark:
                this.backgroundImage.color = this.darkDamageColor;
                break;
            case DamageType.Fire:
                this.backgroundImage.color = this.fireDamageColor;
                break;
            case DamageType.Water:
                this.backgroundImage.color = this.waterDamageColor;
                break;
            case DamageType.Electric:
                this.backgroundImage.color = this.electricDamageColor;
                break;
            case DamageType.Wind:
                this.backgroundImage.color = this.windDamageColor;
                break;
            case DamageType.Pure:
                this.backgroundImage.color = this.pureDamageColor;
                break;
            case DamageType.Nature:
                this.backgroundImage.color = this.natureDamageColor;
                break;
            case DamageType.Bleed:
                this.backgroundImage.color = this.bleedDamageColor;
                break;
        }

        //Setting the font size based on if the attack crit or not
        this.critBackground.SetActive(isCrit_);

        //Setting the text and background color to be transparent if our time before text shows is greater than 0
        if (this.timeBeforeTextShows > 0)
        {
            Color textColor = this.textMeshRef.color;
            Color bgColor = this.backgroundImage.color;
            textColor.a = 0;
            bgColor.a = 0;
            this.textMeshRef.color = textColor;
            this.backgroundImage.color = bgColor;
        }
    }


    //Function called from the combat manager to show that an attack missed
    public void DisplayMiss(float timeDelay_, Vector3 position_)
    {
        //Adding this DamageText to the static list
        DamageDisplay.allDamageDisplayObj.Add(this);

        //Moving this object to the given position with the starting offset
        this.transform.position = position_ + this.startingOffset;
        this.spawnPosition = this.transform.position;

        //Setting the amount of time before this text appears
        this.timeBeforeTextShows = timeDelay_ + this.FindTimeDelay();

        this.textMeshRef.text = "MISS";
        this.backgroundImage.color = this.missColor;
        this.critBackground.SetActive(false);

        //Setting the text and background color to be transparent if our time before text shows is greater than 0
        if (this.timeBeforeTextShows > 0)
        {
            Color textColor = this.textMeshRef.color;
            Color bgColor = this.backgroundImage.color;
            textColor.a = 0;
            bgColor.a = 0;
            this.textMeshRef.color = textColor;
            this.backgroundImage.color = bgColor;
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
            if (this.timeBeforeTextShows <= 0)
            {
                Color textColor = this.textMeshRef.color;
                Color bgColor = this.backgroundImage.color;
                textColor.a = 1;
                bgColor.a = 1;
                this.textMeshRef.color = textColor;
                this.backgroundImage.color = bgColor;
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
                DamageDisplay.allDamageDisplayObj.Remove(this);
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
                float scaleMultiplier = this.scaleCurve.Evaluate(lifetimePercent) * this.scaleMultiplier;
                this.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1);

                //If the destroy timer is above the fade time, we need to start fading out
                if (this.destroyTimer > this.timeBeforeFade)
                {
                    //Finding the percent of time that's passed between the fade time and destroy time
                    float fadePercentPassed = (this.destroyTimer - this.timeBeforeFade) / (this.timeBeforeDestroyed - this.timeBeforeFade);

                    //Changing the alpha of the text and background to become transparent
                    Color textColor = this.textMeshRef.color;
                    Color bgColor = this.backgroundImage.color;
                    textColor.a = 1 - fadePercentPassed;
                    bgColor.a = 1 - fadePercentPassed;
                    this.textMeshRef.color = textColor;
                    this.backgroundImage.color = bgColor;
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
        for (int d = 0; d < DamageDisplay.allDamageDisplayObj.Count; ++d)
        {
            //Making sure we aren't looking at this object
            if (DamageDisplay.allDamageDisplayObj[d] != this)
            {
                //If the current damage text has the same start location as this one
                if (Mathf.RoundToInt(DamageDisplay.allDamageDisplayObj[d].spawnPosition.x) == Mathf.RoundToInt(this.spawnPosition.x) &&
                    Mathf.RoundToInt(DamageDisplay.allDamageDisplayObj[d].spawnPosition.y) == Mathf.RoundToInt(this.spawnPosition.y))
                {
                    //If the current damage text's time before it appears is greater than the current returned delay, we set the delay to that time
                    if (returnedDelay < DamageDisplay.allDamageDisplayObj[d].timeBeforeTextShows)
                    {
                        returnedDelay = DamageDisplay.allDamageDisplayObj[d].timeBeforeTextShows;
                    }
                }
            }
        }

        //Returning the delay time
        return returnedDelay;
    }
}
