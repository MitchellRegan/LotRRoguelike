using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    //The skill used for determining accuracy
    public Weapon.WeaponType weaponSkillUsed = Weapon.WeaponType.Punching;

    //Enum that determines how enemy evasion and armor affects the chance of this attack hitting
    public enum attackTouchType { Regular, IgnoreEvasion, IgnoreArmor, IgnoreEvasionAndArmor};
    public attackTouchType touchType = attackTouchType.Regular;

    //The percent chance that this attack will crit
    [Range(0, 1)]
    public float critChance = 0.2f;

    //The damage multiplier applied when this weapon crits
    public int critMultiplier = 2;

    //The amount added to the user's hit chance 
    public int accuracyBonus = 0;

    //The list of damage dice that are rolled when this attack deals damage
    public List<AttackDamage> damageDealt;

    //The list of effects that can proc when this attack hits
    public List<AttackEffect> effectsOnHit;



    //Function inherited from Action.cs and called from CombatManager.cs so we can attack a target
    public override void PerformAction(CombatTile targetTile_)
    {
        //Reference to the character performing this attack
        Character actingChar = CombatManager.globalReference.actingCharacters[0];
        //Reference to the character that's being attacked
        Character defendingChar;

        //Looping through and triggering all combat effects on the acting character that happen on attack
        foreach(Effect e in actingChar.charCombatStats.combatEffects)
        {
            e.EffectOnAttack();

            //Checking to see if the character has died due to some effect. If so, we break the loop
            if(actingChar.charPhysState.currentHealth <= 0)
            {
                break;
            }
        }

        //Making sure there's a character on the targeted tile
        if(targetTile_.objectOnThisTile != null && targetTile_.objectOnThisTile.GetComponent<Character>())
        {
            defendingChar = targetTile_.objectOnThisTile.GetComponent<Character>();
        }
        //If there isn't a character on the tile, nothing happens because it misses anything it could hit
        else
        {
            CombatManager.globalReference.DisplayMissedAttack(targetTile_);
            return;
        }

        //Before calculating damage, we need to find out if this attack hit. We start by rolling 1d100 to hit and adding this attack's accuracy bonus
        int hitRoll = Random.Range(1, 100) + this.accuracyBonus;
        //Adding the correct skill modifier of the acting character to their hit roll
        switch (this.weaponSkillUsed)
        {
            case Weapon.WeaponType.Punching:
                hitRoll += actingChar.charCombatStats.punching + actingChar.charCombatStats.punchingMod;
                break;
            case Weapon.WeaponType.Sword:
                hitRoll += actingChar.charCombatStats.swords + actingChar.charCombatStats.swordsMod;
                break;
            case Weapon.WeaponType.Dagger:
                hitRoll += actingChar.charCombatStats.daggers + actingChar.charCombatStats.daggersMod;
                break;
            case Weapon.WeaponType.Axe:
                hitRoll += actingChar.charCombatStats.axes + actingChar.charCombatStats.axesMod;
                break;
            case Weapon.WeaponType.Spear:
                hitRoll += actingChar.charCombatStats.spears + actingChar.charCombatStats.spearsMod;
                break;
            case Weapon.WeaponType.Bow:
                hitRoll += actingChar.charCombatStats.bows + actingChar.charCombatStats.bowsMod;
                break;
            case Weapon.WeaponType.Improvised:
                hitRoll += actingChar.charCombatStats.improvised + actingChar.charCombatStats.improvisedMod;
                break;
            case Weapon.WeaponType.HolyMagic:
                hitRoll += actingChar.charCombatStats.holyMagic + actingChar.charCombatStats.holyMagicMod;
                break;
            case Weapon.WeaponType.DarkMagic:
                hitRoll += actingChar.charCombatStats.darkMagic + actingChar.charCombatStats.darkMagicMod;
                break;
            case Weapon.WeaponType.NatureMagic:
                hitRoll += actingChar.charCombatStats.natureMagic + actingChar.charCombatStats.natureMagicMod;
                break;
        }

        //Finding the hit target's resistance and subtracting it from the attacker's hit roll
        switch(this.touchType)
        {
            case attackTouchType.Regular:
                hitRoll -= (defendingChar.charCombatStats.evasion + defendingChar.charInventory.totalPhysicalArmor);
                break;
            case attackTouchType.IgnoreArmor:
                hitRoll -= defendingChar.charCombatStats.evasion;
                break;
            case attackTouchType.IgnoreEvasion:
                hitRoll -= defendingChar.charInventory.totalPhysicalArmor;
                break;
            case attackTouchType.IgnoreEvasionAndArmor:
                //Nothing is subtracted
                break;
        }

        //If the hit roll is still above 66%, they hit. If not, the attack misses
        if(hitRoll <= 66)
        {
            //Miss
            CombatManager.globalReference.DisplayMissedAttack(targetTile_);
            return;
        }

        //Checking to see if this attack crits
        int critMultiplier = 1; //Set to 1 in case we don't crit so it won't change anything
        float critRoll = Random.Range(0, 1);
        bool isCrit = false;
        //If the crit roll is below the crit chance, the attack crits and we change the multiplier
        if(critRoll < this.critChance)
        {
            critMultiplier = this.critMultiplier;
            isCrit = true;
        }

        //The total amount of damage for each type that will be dealt with this attack
        int physDamage = 0;
        int magicDamage = 0;
        int fireDamage = 0;
        int waterDamage = 0;
        int electricDamage = 0;
        int windDamage = 0;
        int rockDamage = 0;
        int lightDamage = 0;
        int darkDamage = 0;

        //Looping through each damage type for this attack
        foreach(AttackDamage atk in this.damageDealt)
        {
            //Int to hold all of the damage for the current attack
            int atkDamage = 0;

            //Adding the base damage
            atkDamage += atk.baseDamage;

            //Looping through each individual die rolled
            for(int d = 0; d < atk.diceRolled; ++d)
            {
                //Finding the value rolled on the current die
                atkDamage += Random.Range(1, atk.diceSides);
            }

            //Adding the current attack's damage to the correct type
            switch(atk.type)
            {
                case AttackDamage.DamageType.Physical:
                    physDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Magic:
                    magicDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Light:
                    lightDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Dark:
                    darkDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Fire:
                    fireDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Water:
                    waterDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Electric:
                    electricDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Wind:
                    windDamage += atkDamage * critMultiplier;
                    break;
                case AttackDamage.DamageType.Rock:
                    rockDamage += atkDamage * critMultiplier;
                    break;
            }
        }

        //Subtracting the defending character's magic resistances 
        if (magicDamage > 0)
        {
            magicDamage -= defendingChar.charInventory.totalMagicResist;
        }
        if(fireDamage > 0)
        {
            fireDamage -= defendingChar.charInventory.totalFireResist;
        }
        if (waterDamage > 0)
        {
            waterDamage -= defendingChar.charInventory.totalWaterResist;
        }
        if (electricDamage > 0)
        {
            electricDamage -= defendingChar.charInventory.totalElectricResist;
        }
        if (windDamage > 0)
        {
            windDamage -= defendingChar.charInventory.totalWindResist;
        }
        if (rockDamage > 0)
        {
            rockDamage -= defendingChar.charInventory.totalRockResist;
        }
        if (lightDamage > 0)
        {
            lightDamage -= defendingChar.charInventory.totalLightResist;
        }
        if (darkDamage > 0)
        {
            darkDamage -= defendingChar.charInventory.totalDarkResist;
        }

        //Dealing damage to the defending character and telling the combat manager to display how much was dealt
        defendingChar.charPhysState.DamageCharacter(physDamage);
        CombatManager.globalReference.DisplayDamageDealt(physDamage, CombatManager.DamageType.Physical, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(magicDamage);
        CombatManager.globalReference.DisplayDamageDealt(magicDamage, CombatManager.DamageType.Magic, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(fireDamage);
        CombatManager.globalReference.DisplayDamageDealt(fireDamage, CombatManager.DamageType.Fire, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(waterDamage);
        CombatManager.globalReference.DisplayDamageDealt(waterDamage, CombatManager.DamageType.Water, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(windDamage);
        CombatManager.globalReference.DisplayDamageDealt(windDamage, CombatManager.DamageType.Wind, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(electricDamage);
        CombatManager.globalReference.DisplayDamageDealt(electricDamage, CombatManager.DamageType.Electric, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(rockDamage);
        CombatManager.globalReference.DisplayDamageDealt(rockDamage, CombatManager.DamageType.Rock, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(lightDamage);
        CombatManager.globalReference.DisplayDamageDealt(lightDamage, CombatManager.DamageType.Light, targetTile_, isCrit);

        defendingChar.charPhysState.DamageCharacter(darkDamage);
        CombatManager.globalReference.DisplayDamageDealt(darkDamage, CombatManager.DamageType.Dark, targetTile_, isCrit);


        //Looping through each effect that this attack can cause and rolling to see if they happen
        foreach(AttackEffect effect in this.effectsOnHit)
        {
            float effectRoll = Random.Range(0, 1);
            //If the roll is less than the effect chance, it was sucessful
            if(effectRoll < effect.effectChance)
            {
                //Creating an instance of the effect object prefab and triggering it's effect
                GameObject effectObj = Instantiate(effect.effectOnHit.gameObject, new Vector3(), new Quaternion());
                effectObj.GetComponent<Effect>().TriggerEffect(actingChar, defendingChar);
            }
        }
    }
}

//Class used in AttackAction.cs to determine damage dealt when an attack hits
[System.Serializable]
public class AttackDamage
{
    //The type of damage that's inflicted
    public enum DamageType { Physical, Magic, Fire, Water, Electric, Wind, Rock, Light, Dark };
    public DamageType type = DamageType.Physical;

    //The amount of damage inflicted before dice rolls
    public int baseDamage = 0;

    //The number of dice that are rolled
    public int diceRolled = 1;
    //The highest value of the type of die rolled
    public int diceSides = 6;
}

//Class used in AttackAction.cs to determine what effect can be applied when an attack happens and its chance of happening
[System.Serializable]
public class AttackEffect
{
    //The effect applied when this attack hits
    public Effect effectOnHit;

    //The percent chance that the effect on hit will proc
    [Range(0, 1)]
    public float effectChance = 0.2f;
}