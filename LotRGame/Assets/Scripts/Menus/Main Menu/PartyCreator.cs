using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCreator : MonoBehaviour
{
    //The total number of skill points that can be allocated to the main character
    public int pointsToAllocate = 20;
    //The number of remaining points that can be allocated
    private int currentPoints = 0;

    //Dictionary to hold the number of points that track how many skill points are currently allocated to each skill
    private Dictionary<SkillList, int> allocatedSkillPoints;



    // Use this for initialization
    public void Awake ()
    {
        //Resetting the current points for the main character
        this.currentPoints = this.pointsToAllocate;

        //Initializing the allocated skill dictionary
        this.allocatedSkillPoints = new Dictionary<SkillList, int>();

        this.allocatedSkillPoints.Add(SkillList.Punching, 0);
        this.allocatedSkillPoints.Add(SkillList.Daggers, 0);
        this.allocatedSkillPoints.Add(SkillList.Swords, 0);
        this.allocatedSkillPoints.Add(SkillList.Axes, 0);
        this.allocatedSkillPoints.Add(SkillList.Spears, 0);
        this.allocatedSkillPoints.Add(SkillList.Bows, 0);
        this.allocatedSkillPoints.Add(SkillList.Improvised, 0);

        this.allocatedSkillPoints.Add(SkillList.HolyMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.DarkMagic, 0);
        this.allocatedSkillPoints.Add(SkillList.NatureMagic, 0);

        this.allocatedSkillPoints.Add(SkillList.Cooking, 0);
        this.allocatedSkillPoints.Add(SkillList.Healing, 0);
        this.allocatedSkillPoints.Add(SkillList.Crafting, 0);

        this.allocatedSkillPoints.Add(SkillList.Foraging, 0);
        this.allocatedSkillPoints.Add(SkillList.Tracking, 0);
        this.allocatedSkillPoints.Add(SkillList.Fishing, 0);

        this.allocatedSkillPoints.Add(SkillList.Climbing, 0);
        this.allocatedSkillPoints.Add(SkillList.Hiding, 0);
        this.allocatedSkillPoints.Add(SkillList.Swimming, 0);
    }
	

    //Function called externally to allocate a point to a skill
    public void allocateSkillPoint(SkillList skillToIncrease_)
    {
        //If we don't have any remaining points to allocate, nothing happens
        if(this.currentPoints < 1)
        {
            return;
        }

        //Removing a point from the remaining pool
        this.currentPoints -= 1;

        //Adding a point to the given skill
        this.allocatedSkillPoints[skillToIncrease_] += 1;
    }


    //Function called externally to de-allocate a point from a skill
    public void deallocateSkillPoint(SkillList skillToDecrease_)
    {
        //Making sure the skill that we're removing a point from has points to remove
        if(this.allocatedSkillPoints[skillToDecrease_] < 1)
        {
            return;
        }

        //Removing a point from the given skill
        this.allocatedSkillPoints[skillToDecrease_] -= 1;

        //Adding a point to the remaining pool
        this.currentPoints += 1;
    }
}

public enum SkillList
{
    //Combat skills
    Punching,
    Daggers,
    Swords,
    Axes,
    Spears,
    Bows,
    Improvised,
    
    //Magic skills
    HolyMagic,
    DarkMagic,
    NatureMagic,

    //Creative
    Cooking,
    Healing,
    Crafting,

    //Survival
    Foraging,
    Tracking,
    Fishing,

    //Tactial
    Climbing,
    Hiding,
    Swimming
}
