using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellResistAbsorbPerk : Perk
{
    //The type of spell that this perk resists or absorbs
    public CombatManager.DamageType typeToResist = CombatManager.DamageType.Arcane;

    //Bool for if this perk only applies to crits
    public bool onlyWorksOnCrit = false;

    //Bool for if this perk only works for attack actions (not DoTs or HoTs)
    public bool onlyWorksOnAttackAct = false;

    //Bool for if this perk only works for DoTs and HoTs
    public bool onlyWorksOnDoTAndHoT = false;

    //Bool for if this perk completely negates the spell damage
    public bool negateAllDamage = false;

    //Bool for if this perk absorbs (heals from) all spell damage
    public bool absorbDamage = false;

    [Space(8)]

    //The base amount of spell damage to resist
    public int baseDamageToResist = 0;

    [Space(8)]

    //The amount of added spell resist based on the character's skill level
    [Range(-1f, 1)]
    public float percentOfSkillLevelResist = 0;
    //The multiplier for the amount that the percentOfSkillLevelResist gets
    public float percentResistMultiplier = 1;

    [Space(8)]

    //The number of spell resist dice to roll
    public int numberOfResistDiceToRoll = 0;
    //The number of sides on the resist dice
    public int resistDiceSideNumber = 6;
    //If this die roll is negative
    public bool dieRollIsNegative = false;



    //Function called from AttackAction.cs to get the amount of spell resist this perk awards
    public int GetSpellResistAmount(Character perkOwner_, bool isCrit_, bool isDoTOrHot_)
    {
        //The total amount of spell resist returned
        int totalResist = 0;

        //If this perk only activates during a crit and the attack didn't crit, nothing happens
        if (this.onlyWorksOnCrit && !isCrit_)
        {
            return totalResist;
        }

        //If this perk only activates for Attack Actions and the current action isn't, nothing happens
        if (this.onlyWorksOnAttackAct && isDoTOrHot_)
        {
            return totalResist;
        }

        //If this perk only activates for DoTs and HoTs and the current action isn't, nothing happens
        if (this.onlyWorksOnDoTAndHoT && !isDoTOrHot_)
        {
            return totalResist;
        }

        //Adding the base spell resist
        totalResist += this.baseDamageToResist;

        //Finding the skill type that corresponds to this perk's damage type
        SkillList resistSkill = this.GetSkillFromDamageType();

        //If the returned skill isn't the one we ignore, we calculate the player character's resist
        if (resistSkill != SkillList.Swords)
        {
            //Adding the spell resist from the player's skill level
            totalResist += Mathf.FloorToInt(this.percentOfSkillLevelResist *
                                            perkOwner_.charSkills.GetSkillLevelValueWithMod(resistSkill) *
                                            this.percentResistMultiplier);
        }

        //Multiplier for the dice rolls to see if they're negative or positive
        int diePositiveNegative = 1;
        if (this.dieRollIsNegative)
        {
            diePositiveNegative = -1;
        }

        //Looping through and adding bonus spell resist for each spell resist die
        for (int d = 0; d < this.numberOfResistDiceToRoll; ++d)
        {
            totalResist += diePositiveNegative * Random.Range(1, this.resistDiceSideNumber + 1);
        }

        //Returning the total spell resist
        return totalResist;
    }


    //Function called from GetSpellResistAmount to return the skill type for a designated damage type
    private SkillList GetSkillFromDamageType()
    {
        switch(this.typeToResist)
        {
            case CombatManager.DamageType.Arcane:
                return SkillList.ArcaneMagic;

            case CombatManager.DamageType.Holy:
                return SkillList.HolyMagic;

            case CombatManager.DamageType.Dark:
                return SkillList.DarkMagic;

            case CombatManager.DamageType.Fire:
                return SkillList.FireMagic;

            case CombatManager.DamageType.Water:
                return SkillList.WaterMagic;

            case CombatManager.DamageType.Wind:
                return SkillList.WindMagic;

            case CombatManager.DamageType.Electric:
                return SkillList.ElectricMagic;

            case CombatManager.DamageType.Nature:
                return SkillList.StoneMagic;

            //If the type isn't any of those other magic types, we return a skill that will be ignored
            default:
                return SkillList.Swords;
        }
    }
}
