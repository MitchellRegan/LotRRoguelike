using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //The mathematical interpolator that we'll use to find damage using min/max ranges
    private Interpolator ourInterpolator;

    //The list of all characters involved in this combat
    [HideInInspector]
    public List<Character> charactersInCombat;



	// Use this for initialization
	private void Awake ()
    {
        this.ourInterpolator = new Interpolator();
	}


    //Function called when one character attacks another. Returns the damage based on the mathematical distribution
    public int CalculateDamage(int minDamage_, int maxDamage_, EaseType distribution_)
    {
        //If the max damage is 0, no damage can be dealt
        if(maxDamage_ == 0)
        {
            return 0;
        }

        //Int to hold the damage that's returned. It's automatically set to the min
        int totalDamage = minDamage_;
        //Finding the difference between the min and max
        int damageDiff = maxDamage_ - minDamage_;

        //Setting the distribution type of the interpolator and resetting the progress
        this.ourInterpolator.ease = distribution_;
        this.ourInterpolator.ResetTime();
        //Setting the interp to a random value from 0 - 1
        this.ourInterpolator.AddTime(Random.Range(0, 1));

        //Adding a weighted percentage of the difference to the total
        totalDamage += damageDiff * Mathf.RoundToInt(this.ourInterpolator.GetProgress());

        return totalDamage;
    }
}
