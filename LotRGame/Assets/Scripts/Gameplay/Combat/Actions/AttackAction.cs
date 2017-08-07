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

            //Looping through each effect that this attack can apply to see if any don't require the attack to land
            foreach(AttackEffect efc in this.effectsOnHit)
            {
                //If the effect doesn't require the attack to land, it's triggered
                if(!efc.requireHit)
                {
                    this.TriggerEffect(efc, targetTile_, actingChar);
                }
            }

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
            lightDamage -= defendingChar.charInventory.totalHolyResist;
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


        //Increasing the threat to the target based on damage dealt
        int totalDamage = 0;
        totalDamage += physDamage + magicDamage;//Adding physical and magical damage
        totalDamage += fireDamage + waterDamage + windDamage + electricDamage + rockDamage;//Adding elemental damage
        totalDamage += lightDamage + darkDamage;//Adding light/dark damage

        //If the attack crit, ALL enemies have their threat increased for 25% of the damage
        if(isCrit)
        {
            //Getting 25% of the damage to pass to all enemies
            int threatForAll = totalDamage / 4;
            CombatManager.globalReference.ApplyActionThreat(null, threatForAll, true);

            //Applying the rest of the threat to the defending character
            CombatManager.globalReference.ApplyActionThreat(defendingChar, totalDamage - threatForAll, false);
        }
        //If the attack wasn't a crit, only the defending character takes threat
        else
        {
            CombatManager.globalReference.ApplyActionThreat(defendingChar, totalDamage, false);
        }


        //Looping through each effect that this attack can cause and triggering them
        foreach(AttackEffect effect in this.effectsOnHit)
        {
            this.TriggerEffect(effect, targetTile_, actingChar);
        }
    }


    //Function called from TriggerEffect to find all targets within an effect's radius
    private List<Character> FindCharactersInAttackRange(CombatTile targetTile_, int attackRadius_)
    {
        //The list of characters that are returned
        List<Character> targets = new List<Character>();

        //If the attack radius is less than or equal to 0, it only includes the target tile
        if(attackRadius_ <= 0)
        {
            //If the target tile has an object on it with a character component, we return that character
            if(targetTile_.objectOnThisTile != null && targetTile_.objectOnThisTile.GetComponent<Character>())
            {
                targets.Add(targetTile_.objectOnThisTile.GetComponent<Character>());
                return targets;
            }
        }

        //If the radius is larger than 0, we have to find all tiles within range of the target
        List<CombatTile> tilesInRange = PathfindingAlgorithms.FindTilesInActionRange(targetTile_, attackRadius_);

        //Looping through each tile in range to find out if they have character objects on them
        foreach(CombatTile currentTile in tilesInRange)
        {
            if(currentTile.objectOnThisTile != null && currentTile.objectOnThisTile.GetComponent<Character>())
            {
                targets.Add(currentTile.objectOnThisTile.GetComponent<Character>());
            }
        }

        //Returning the list of characters
        return targets;
    }


    //Function called when an effect is triggered
    private void TriggerEffect(AttackEffect effectToTrigger_, CombatTile targetTile_, Character actingChar_)
    {
        //Finding all targets within this effect's radius
        List<Character> targets = this.FindCharactersInAttackRange(targetTile_, effectToTrigger_.effectRadius);

        //If the effected race isn't "None", we need to loop through and remove all targets that don't qualify
        if(effectToTrigger_.effectedRace != RaceTypes.Races.None)
        {
            for(int r = 0; r < targets.Count; ++r)
            {
                //If the current character's race isn't the effected race and we only hit the effected type
                if(targets[r].charRaceTypes.race != effectToTrigger_.effectedRace && effectToTrigger_.hitEffectedType)
                {
                    //We remove the character from the targets list
                    targets.RemoveAt(r);
                    r -= 1;
                }
                //If the current character's race IS the effected race, but we hit everything except that type
                else if(targets[r].charRaceTypes.race == effectToTrigger_.effectedRace && !effectToTrigger_.hitEffectedType)
                {
                    //We remove the character from the targets list
                    targets.RemoveAt(r);
                    r -= 1;
                }
            }
        }

        //If the effected subtype isn't "None", we need to loop through and remove all targets that don't qualify
        if (effectToTrigger_.effectedType != RaceTypes.Subtypes.None)
        {
            for (int t = 0; t < targets.Count; ++t)
            {
                //If the current character's subtype isn't the effected subtype and we only hit the effected type
                if (!targets[t].charRaceTypes.subtypeList.Contains(effectToTrigger_.effectedType) && effectToTrigger_.hitEffectedType)
                {
                    //We remove the character from the targets list
                    targets.RemoveAt(t);
                    t -= 1;
                }
                //If the current character's subtype IS the effected subtype, but we hit everything except that type
                else if (targets[t].charRaceTypes.subtypeList.Contains(effectToTrigger_.effectedType) && !effectToTrigger_.hitEffectedType)
                {
                    //We remove the character from the targets list
                    targets.RemoveAt(t);
                    t -= 1;
                }
            }
        }

        //Bool to check if the acting character is a player character or an enemy
        bool isActingCharPlayer = true;
        if(CombatManager.globalReference.enemyCharactersInCombat.Contains(actingChar_))
        {
            isActingCharPlayer = false;
        }

        //Now we loop through and remove all targets that don't qualify based on the type of targets that are hit
        if(effectToTrigger_.effectedTargets != AttackEffect.EffectedTargets.Everyone)
        {
            for(int e = 0; e < targets.Count; ++e)
            {
                //If the current target is an enemy character
                if(CombatManager.globalReference.enemyCharactersInCombat.Contains(targets[e]))
                {
                    switch(effectToTrigger_.effectedTargets)
                    {
                        case AttackEffect.EffectedTargets.AlliesExceptAttacker:
                            //if the target is an enemy and the acting character is a player
                            if(isActingCharPlayer)
                            {
                                //This enemy is removed, because they aren't an ally of the player
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            //If the target is an enemy and the acting character is an enemy
                            else
                            {
                                //If this enemy is the acting character, they're removed
                                if (targets[e] == actingChar_)
                                {
                                    targets.RemoveAt(e);
                                    e -= 1;
                                }
                                //If this enemy is NOT the acting character, they're included in the effect
                            }
                            break;

                        case AttackEffect.EffectedTargets.AlliesOnly:
                            //if the target is an enemy and the acting character is a player
                            if (isActingCharPlayer)
                            {
                                //This enemy is removed, because they aren't an ally of the player
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            //If the target is an enemy and the acting character is an enemy
                            //They are included in the effect because they're allies
                            break;

                        case AttackEffect.EffectedTargets.Attacker:
                            //If this enemy character isn't the attacker, they're removed
                            if(targets[e] != actingChar_)
                            {
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;

                        case AttackEffect.EffectedTargets.Defender:
                            //If this enemy character isn't the defending character, or if the target tile doesn't have a character on it, they are removed
                            if(targetTile_.objectOnThisTile == null || 
                                (targetTile_.objectOnThisTile != null && targetTile_.objectOnThisTile.GetComponent<Character>() && targetTile_.objectOnThisTile.GetComponent<Character>() != targets[e]))
                            {
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;

                        case AttackEffect.EffectedTargets.EnemiesExceptDefender:
                            //if the target is an enemy and the acting character is a player
                            if (isActingCharPlayer)
                            {
                                //If this character is the defending character
                                if(targetTile_.objectOnThisTile != null && targetTile_.objectOnThisTile.GetComponent<Character>() && targetTile_.objectOnThisTile.GetComponent<Character>() == targets[e])
                                {
                                    //The target is removed, because we exclude the defending character
                                    targets.RemoveAt(e);
                                    e -= 1;
                                }
                                //If this character isn't the defending character, they are included, because they're enemies
                            }
                            //If the target is an enemy and the acting character is an enemy
                            else
                            {
                                //This enemy character is removed because they aren't an enemy
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;

                        case AttackEffect.EffectedTargets.EnemiesOnly:
                            //if the target is an enemy and the acting character is a player
                                //They are included, because they're enemies
                            //If the target is an enemy and the acting character is an enemy
                            if(!isActingCharPlayer)
                            {
                                //This character is removed, because they're allies
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;
                    }
                }
                //If the current target is a player character
                else if(CombatManager.globalReference.playerCharactersInCombat.Contains(targets[e]))
                {
                    switch (effectToTrigger_.effectedTargets)
                    {
                        case AttackEffect.EffectedTargets.AlliesExceptAttacker:
                            //if the target is a player and the acting character is a player
                            if (isActingCharPlayer)
                            {
                                //If this player is the acting character, they're removed
                                if (targets[e] == actingChar_)
                                {
                                    targets.RemoveAt(e);
                                    e -= 1;
                                }
                                //If this player is NOT the acting character, they're included in the effect
                            }
                            //If the target is a player and the acting character is an enemy
                            else
                            {
                                //This player is removed, because they aren't an ally of the enemy
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;

                        case AttackEffect.EffectedTargets.AlliesOnly:
                            //if the target is a player and the acting character is a player
                                //They are included in the effect because they're allies
                            //If the target is a player and the acting character is an enemy
                            if (!isActingCharPlayer)
                            {
                                //This player is removed, because they aren't an ally of the enemy
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;

                        case AttackEffect.EffectedTargets.Attacker:
                            //If this player character isn't the attacker, they're removed
                            if (targets[e] != actingChar_)
                            {
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;

                        case AttackEffect.EffectedTargets.Defender:
                            //If this player character isn't the defending character, or if the target tile doesn't have a character on it, they are removed
                            if (targetTile_.objectOnThisTile == null ||
                                (targetTile_.objectOnThisTile != null && targetTile_.objectOnThisTile.GetComponent<Character>() && targetTile_.objectOnThisTile.GetComponent<Character>() != targets[e]))
                            {
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            break;

                        case AttackEffect.EffectedTargets.EnemiesExceptDefender:
                            //if the target is a player and the acting character is a player
                            if (isActingCharPlayer)
                            {
                                //This player character is removed because they aren't an enemy
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            //If the target is a player and the acting character is an enemy
                            else
                            {
                                //If this character is the defending character
                                if (targetTile_.objectOnThisTile != null && targetTile_.objectOnThisTile.GetComponent<Character>() && targetTile_.objectOnThisTile.GetComponent<Character>() == targets[e])
                                {
                                    //The target is removed, because we exclude the defending character
                                    targets.RemoveAt(e);
                                    e -= 1;
                                }
                                //If this character isn't the defending character, they are included, because they're enemies
                            }
                            break;

                        case AttackEffect.EffectedTargets.EnemiesOnly:
                            //if the target is a player and the acting character is a player
                            if (isActingCharPlayer)
                            {
                                //This character is removed, because they're allies
                                targets.RemoveAt(e);
                                e -= 1;
                            }
                            //If the target is a player and the acting character is an enemy
                                //They are included, because they're enemies
                            break;
                    }
                }
            }
        }

        //Looping through all of the filtered targets in the list
        foreach (Character targetChar in targets)
        {
            //Rolling to see if the effect hits the target or not
            float effectRoll = Random.Range(0, 1);

            //If the roll is less than the effect chance, it was sucessful
            if (effectRoll < effectToTrigger_.effectChance)
            {
                //Creating an instance of the effect object prefab and triggering it's effect
                GameObject effectObj = Instantiate(effectToTrigger_.effectToApply.gameObject, new Vector3(), new Quaternion());
                effectObj.GetComponent<Effect>().TriggerEffect(actingChar_, targetChar);
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
    //The effect applied when this is triggered
    public Effect effectToApply;

    //If true, this effect only happens if the attack lands. If false, it will happen even if the initial attack misses
    public bool requireHit = true;

    //The percent chance that the effect on hit will proc
    [Range(0, 1)]
    public float effectChance = 0.2f;

    //The radius of effect from the hit target
    [Range(0, 10)]
    public int effectRadius = 0;

    //Determines if this damages specific enemy types
    public RaceTypes.Races effectedRace = RaceTypes.Races.None;
    public RaceTypes.Subtypes effectedType = RaceTypes.Subtypes.None;
    //Determines if the effected type is the only type hit or if it's the only type ignored
    public bool hitEffectedType = true;

    //Enum that determines who is effected
    public enum EffectedTargets
    {
        Attacker,//The person making the attack
        Defender,//The person being hit by the attack
        EnemiesOnly,//Hits all enemies in the radius and ignores allies
        EnemiesExceptDefender,//Hits all enemies, but doesn't include the defender
        AlliesOnly,//Hits all allies in the radius and ignores enemies
        AlliesExceptAttacker,//Hits all allies, but doesn't include the attacker
        Everyone//Hits every ally and enemy in the radius
    };

    //The type of targets that are effected
    public EffectedTargets effectedTargets = EffectedTargets.Everyone;
}