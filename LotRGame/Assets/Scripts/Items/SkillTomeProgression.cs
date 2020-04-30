using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by SkillTome to determine the amount of skill points recieved when used
[System.Serializable]
public class SkillTomeProgression
{
    //The low end of this skill range
    [Range(0, 99)]
    public int skillRangeMin = 0;
    //The high end of this skill range
    [Range(1, 100)]
    public int skillRangeMax = 1;

    [Space(8)]

    //The minimum skill points to allocate in this level range
    public int newSkillMin = 0;
    //The maximum skill points to allocate in this level range
    public int newSkillMax = 1;


    public ProgressionCurves progressionCurve = ProgressionCurves.Linear;
}