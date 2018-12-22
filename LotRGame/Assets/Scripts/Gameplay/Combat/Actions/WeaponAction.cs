using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction : AttackAction
{
    //Enum for the hand that this weapon attack requires the used weapon to be in
    public enum WeaponHand
    {
        MainHand,//Required weapon needs to be in the main hand
        OffHand,//Required weapon needs to be in the off hand
        OneHand,//Required weapon can be in the main OR off hand as long as it's 1-handed
        TwoHand,//Required weapon needs to be 2-handed
        DualWeild//Required weapons need to be in both hands
    };
    public WeaponHand requiredWeaponHand = WeaponHand.OneHand;

    //The number of times this attack hits
    public int numberOfHits = 1;

    //The amount of damage added to the weapon's damage
    public int addedDamage = 0;

    //The multiplier for the weapon damage before added damage
    public float damageMultiplier = 1;



	//Function called from CombatActionPanelUI.cs and ActionList.cs to check if this action can be used
    public bool CanCharacterUseAction(Character charToCheck_)
    {
        //Bool to return
        bool canUse = false;

        //switch statement so we can check the character based on our weapon hand type
        switch(this.requiredWeaponHand)
        {
            case WeaponHand.MainHand:
                //Checking the character's main hand weapon to see if it matches our weapon skill type
                if (charToCheck_.charInventory.rightHand != null)
                {
                    if(charToCheck_.charInventory.rightHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                //If the main hand is empty and this action is for unarmed skills, they can use it
                else if(this.weaponSkillUsed == SkillList.Unarmed)
                {
                    canUse = true;
                }
                break;

            case WeaponHand.OffHand:
                //Checking the character's off hand weapon to see if it matches our weapon skill type
                if (charToCheck_.charInventory.leftHand != null)
                {
                    if (charToCheck_.charInventory.leftHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                //If the off hand is empty and this action is for unarmed skills
                else if(this.weaponSkillUsed == SkillList.Unarmed)
                {
                    //Making sure the main hand weapon isn't holding a 2-handed weapon
                    if(charToCheck_.charInventory.rightHand == null || 
                        charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.OneHand)
                    {
                        canUse = true;
                    }
                }
                break;

            case WeaponHand.OneHand:
                //Checking the character's main hand weapon to see if it matches our weapon skill type and size
                if (charToCheck_.charInventory.rightHand != null && charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.OneHand)
                {
                    if (charToCheck_.charInventory.rightHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                        //As long as the main hand weapon works, we don't need to check the off hand
                        break;
                    }
                }
                //If the main hand is empty and this action is for unarmed skills, they can use it
                else if (this.weaponSkillUsed == SkillList.Unarmed)
                {
                    canUse = true;
                    break;
                }
                //Checking the character's off hand weapon to see if it matches our weapon skill type
                if (charToCheck_.charInventory.leftHand != null)
                {
                    if (charToCheck_.charInventory.leftHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                //If the off hand is empty and this action is for unarmed skills
                else if (this.weaponSkillUsed == SkillList.Unarmed)
                {
                    //Making sure the main hand weapon isn't holding a 2-handed weapon
                    if (charToCheck_.charInventory.rightHand == null ||
                        charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.OneHand)
                    {
                        canUse = true;
                    }
                }
                break;

            case WeaponHand.TwoHand:
                //Checking the character's main hand weapon to see if it matches our weapon skill type and size
                if (charToCheck_.charInventory.rightHand != null && charToCheck_.charInventory.rightHand.size == Weapon.WeaponSize.TwoHands)
                {
                    if (charToCheck_.charInventory.rightHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                else if(this.weaponSkillUsed == SkillList.Unarmed)
                {
                    //Making sure both hands aren't holding anything
                    if(charToCheck_.charInventory.rightHand == null && charToCheck_.charInventory.leftHand == null)
                    {
                        canUse = true;
                    }
                }
                break;

            case WeaponHand.DualWeild:
                //If this weapon skill is unarmed, we need to make sure the character isn't holding anything
                if(this.weaponSkillUsed == SkillList.Unarmed)
                {
                    if(charToCheck_.charInventory.rightHand == null && charToCheck_.charInventory.leftHand == null)
                    {
                        canUse = true;
                    }
                }
                //If the weapon skill isn't unarmed, we need to make sure the character is holding the correct weapons in both hands
                else if(charToCheck_.charInventory.rightHand != null && charToCheck_.charInventory.leftHand != null)
                {
                    if(charToCheck_.charInventory.rightHand.weaponType == this.weaponSkillUsed && charToCheck_.charInventory.leftHand.weaponType == this.weaponSkillUsed)
                    {
                        canUse = true;
                    }
                }
                break;
        }

        return canUse;
    }


    //Function inherited from AttackAction.cs and called from CombatManager.cs so we can attack a target
    public override void PerformAction(CombatTile targetTile_)
    {
        //Calling the base function to start the cooldown time
        this.BeginActionCooldown();

        //Reference to the character performing this attack
        Character actingChar = CombatManager.globalReference.actingCharacters[0];
        //Reference to the character that's being attacked
        Character defendingChar;

        //Getting the tile that the acting character is on
        CombatTile actingCharTile = CombatManager.globalReference.FindCharactersTile(actingChar);
        CharacterSpriteBase cSprite = CombatManager.globalReference.GetCharacterSprite(actingChar);

        //Setting the direction the acting character faces
        this.SetDirectionFacing(targetTile_, actingCharTile, cSprite);

        //Looping through and triggering all combat effects on the acting character that happen on attack
        foreach (Effect e in actingChar.charCombatStats.combatEffects)
        {
            e.EffectOnAttack();

            //Checking to see if the character has died due to some effect. If so, we break the loop
            if (actingChar.charPhysState.currentHealth <= 0)
            {
                break;
            }
        }

        //Looping through and creating each of the launched projectiles for this attack
        Vector3 casterTile = CombatManager.globalReference.FindCharactersTile(CombatManager.globalReference.actingCharacters[0]).transform.position;
        foreach (ProjectileLauncher projectile in this.projectilesToLaunch)
        {
            GameObject newProjectile = GameObject.Instantiate(projectile.gameObject, casterTile, new Quaternion());
            //Parenting the projectile to the combat manager canvas
            newProjectile.transform.SetParent(CombatManager.globalReference.transform);
            //Telling the projectile to start moving
            newProjectile.GetComponent<ProjectileLauncher>().StartTravelPath(casterTile, targetTile_.transform.position);
        }

        //Making sure there's a character on the targeted tile
        if (targetTile_.objectOnThisTile != null && targetTile_.objectOnThisTile.GetComponent<Character>())
        {
            defendingChar = targetTile_.objectOnThisTile.GetComponent<Character>();
        }
        //If there isn't a character on the tile, nothing happens because it misses anything it could hit
        else
        {
            //If there are no attack damage rolls (like if the attack was just to inflict an effect) the "Miss" text isn't shown
            if (this.damageDealt.Count > 0)
            {
                CombatManager.globalReference.DisplayMissedAttack(this.timeToCompleteAction, targetTile_);
            }

            //If the visual effect doesn't require a hit to be spawned, we create it at the target tile
            if (this.spawnVisualOnMiss && this.visualEffectOnHit != null)
            {
                GameObject visual = GameObject.Instantiate(this.visualEffectOnHit.gameObject, targetTile_.transform.position, new Quaternion(), CombatManager.globalReference.transform);
            }

            //Looping through each effect that this attack can apply to see if any don't require the attack to land
            foreach (AttackEffect efc in this.effectsOnHit)
            {
                //If the effect doesn't require the attack to land, it's triggered
                if (!efc.requireHit)
                {
                    this.TriggerEffect(efc, targetTile_, actingChar);
                }
            }

            return;
        }
        
        //Before calculating damage, we need to find out if this attack hit. We start by rolling 1d100 to hit and adding this attack's accuracy bonus
        int hitRoll = this.FindAttackRoll(actingChar, defendingChar);

        //If the hit roll is still above 20%, they hit. If not, the attack misses
        if (hitRoll <= CombatManager.baseHitDC)
        {
            //If there are no attack damage rolls (like if the attack was just to inflict an effect) the "Miss" text isn't shown
            if (this.damageDealt.Count > 0)
            {
                //Miss
                CombatManager.globalReference.DisplayMissedAttack(this.timeToCompleteAction, targetTile_);
            }

            //If the visual effect doesn't require a hit to be spawned, we create it at the target tile
            if (this.spawnVisualOnMiss && this.visualEffectOnHit != null)
            {
                GameObject visual = GameObject.Instantiate(this.visualEffectOnHit.gameObject, targetTile_.transform.position, new Quaternion(), CombatManager.globalReference.transform);
            }

            //Looping through each effect that this attack can apply to see if any don't require the attack to land
            foreach (AttackEffect efc in this.effectsOnHit)
            {
                //If the effect doesn't require the attack to land, it's triggered
                if (!efc.requireHit)
                {
                    this.TriggerEffect(efc, targetTile_, actingChar);
                }
            }

            //Giving the attacking character skill EXP for a miss
            this.GrantSkillEXP(actingChar, this.weaponSkillUsed, true);

            return;
        }

        //Giving the attacking character skill EXP for a hit
        this.GrantSkillEXP(actingChar, this.weaponSkillUsed, false);

        //Looping through for each time this action hits
        for (int h = 0; h < this.numberOfHits; ++h)
        {
            //Checking to see if this attack crits
            float critMultiplier = 1; //Set to 1 in case we don't crit so it won't change anything
            float critRoll = Random.Range(0, 1);
            bool isCrit = false;

            //Float for the bonus damage multiplier from perks
            float critMultiplierBoost = 0;

            //Looping through all of the acting character's perks to see if they have perks for crit chance or multiplier
            foreach (Perk charPerk in actingChar.charPerks.allPerks)
            {
                //If the current perk increases crit chance, we need to see if it applies to this attack
                if (charPerk.GetType() == typeof(CritChanceBoostPerk))
                {
                    CritChanceBoostPerk critPerk = charPerk.GetComponent<CritChanceBoostPerk>();

                    //If the perk applies to this attack's required skill check
                    if (critPerk.boostAllSkills || critPerk.skillCritToBoost == this.weaponSkillUsed)
                    {
                        //If the perk applies to any kind of weapon size or the size requirement matches this action's required size, we increase the crit chance
                        if(critPerk.noSizeRequirement || critPerk.weaponSizeRequirement == this.requiredWeaponHand)
                        {
                            critRoll -= critPerk.critChanceBoost;
                        }
                    }
                }
                //If the current perk increases crit damage multipliers, we see if it applies to this attack
                else if(charPerk.GetType() == typeof(CritMultiplierPerk))
                {
                    CritMultiplierPerk critPerk = charPerk.GetComponent<CritMultiplierPerk>();

                    //If the perk applies to this attack's required skill check
                    if(critPerk.boostAllSkills || critPerk.skillCritToBoost == this.weaponSkillUsed)
                    {
                        //If the perk applies to any kind of weapon size or the size requirement matches this action's required size, we increase the crit chance
                        if (critPerk.noSizeRequirement || critPerk.weaponSizeRequirement == this.requiredWeaponHand)
                        {
                            critMultiplierBoost += critPerk.critMultiplierBoost;
                        }
                    }
                }
            }

            //If the crit roll is below the crit chance, the attack crits and we change the multiplier
            if (critRoll < this.critChance)
            {
                critMultiplier = this.critMultiplier + critMultiplierBoost;
                isCrit = true;
            }

            //Dictionary for the total amount of damage for each type that will be dealt with this attack
            Dictionary<CombatManager.DamageType, int> damageTypeTotalDamage = new Dictionary<CombatManager.DamageType, int>();
            //Dictionary for if all of the spell damage types for if the damage is completely negated
            Dictionary<CombatManager.DamageType, SpellResistTypes> spellResistDictionary = new Dictionary<CombatManager.DamageType, SpellResistTypes>();

            //Initializing the dictionaries correctly
            this.InitializeDamageDictionaries(damageTypeTotalDamage, spellResistDictionary);

            //Getting the damage from the used weapon
            this.GetWeaponDamage(actingChar, damageTypeTotalDamage, h);

            //Looping through each damage type for this attack
            foreach (AttackDamage atk in this.damageDealt)
            {
                //Int to hold all of the damage for the current attack
                int atkDamage = 0;

                //Adding the base damage
                atkDamage += atk.baseDamage;

                //Looping through each individual die rolled
                for (int d = 0; d < atk.diceRolled; ++d)
                {
                    //Finding the value rolled on the current die
                    atkDamage += Random.Range(1, atk.diceSides);
                }

                //Multiplying the damage by the crit multiplier
                atkDamage = Mathf.RoundToInt(atkDamage * critMultiplier);

                //Looping through the perks of the character that used this ability to see if they have any damage type boost perks
                foreach (Perk charPerk in actingChar.charPerks.allPerks)
                {
                    //If the perk boosts a damage type that's the same as this damage type, we boost it
                    if (charPerk.GetType() == typeof(DamageTypeBoostPerk) && atk.type == charPerk.GetComponent<DamageTypeBoostPerk>().damageTypeToBoost)
                    {
                        atkDamage += charPerk.GetComponent<DamageTypeBoostPerk>().GetDamageBoostAmount(actingChar, isCrit, false, atk.type);
                    }
                }

                //Looping through the defending character's perks to see if they have any spell resist or absorb perks
                foreach (Perk defPerk in defendingChar.charPerks.allPerks)
                {
                    if (defPerk.GetType() == typeof(SpellResistAbsorbPerk))
                    {
                        SpellResistAbsorbPerk resistPerk = defPerk.GetComponent<SpellResistAbsorbPerk>();

                        //Checking to see if the current damage type is the same as this spell resist perk
                        if (resistPerk.typeToResist == atk.type)
                        {
                            //Checking to see if the damage is negated entirely
                            if (resistPerk.negateAllDamage)
                            {
                                //If the resist type for this spell isn't on absorb, we can negate it. ALWAYS have preference to absorb because it heals
                                if (spellResistDictionary[atk.type] != SpellResistTypes.Absorb)
                                {
                                    spellResistDictionary[atk.type] = SpellResistTypes.Negate;
                                }
                            }
                            //Checking to see if the damage is absorbed to heal the target
                            else if (resistPerk.absorbDamage)
                            {
                                spellResistDictionary[atk.type] = SpellResistTypes.Absorb;
                                //Applying the damage reduction so the defender isn't healed as much
                                damageTypeTotalDamage[atk.type] -= resistPerk.GetSpellResistAmount(defendingChar, isCrit, false);
                            }
                            //Otherwise we just get the amount that it normally resists
                            else
                            {
                                damageTypeTotalDamage[atk.type] -= resistPerk.GetSpellResistAmount(defendingChar, isCrit, false);
                            }
                        }
                    }
                }

                //Adding the current attack's damage to the correct type
                damageTypeTotalDamage[atk.type] += atkDamage;
            }
            
            //Looping through the attacking character's perks to see if there's any bonus damage to add to this attack
            foreach (Perk charPerk in actingChar.charPerks.allPerks)
            {
                //If the perk is a damage boosting perk, we get the bonus damage from it
                if (charPerk.GetType() == typeof(SkillDamageBoostPerk))
                {
                    int perkDamage = charPerk.GetComponent<SkillDamageBoostPerk>().GetDamageBoostAmount(actingChar, isCrit, false);

                    //Applying the perk's added damage to the correct damage type
                    damageTypeTotalDamage[charPerk.GetComponent<SkillDamageBoostPerk>().damageBoostType] += perkDamage;
                }
            }

            //Subtracting the target's melee and spell damage resistance from our attack damage
            this.SubtractResistances(damageTypeTotalDamage, defendingChar);

            //Dealing damage to the target
            this.DealDamage(damageTypeTotalDamage, spellResistDictionary, defendingChar, targetTile_, isCrit);

            //Dealing threat to the target
            this.DealThreat(damageTypeTotalDamage, actingChar, defendingChar, isCrit);

            //Creating the visual effect at the target tile if it isn't null
            if (this.visualEffectOnHit != null)
            {
                GameObject visual = GameObject.Instantiate(this.visualEffectOnHit.gameObject, targetTile_.transform.position, new Quaternion(), CombatManager.globalReference.transform);
            }

            //Looping through each effect that this attack can cause and triggering them
            foreach (AttackEffect effect in this.effectsOnHit)
            {
                this.TriggerEffect(effect, targetTile_, actingChar);
            }
        }
    }


    //Function called from PerformAction to get the damage dealt by the player weapon(s)
    private void GetWeaponDamage(Character abilityUser_, Dictionary<CombatManager.DamageType, int> damageTypeTotalDamage_, int attackNumber_)
    {
        //If this attack isn't using unarmed attacks, we get the weapon damage
        if (this.weaponSkillUsed != SkillList.Unarmed)
        {
            //Getting the correct weapon based on the hand this action uses
            switch (this.requiredWeaponHand)
            {
                case WeaponHand.MainHand:
                    //Getting the damage that the main hand weapon deals
                    int mhDamage = abilityUser_.charInventory.rightHand.weaponDamage.baseDamage;
                    for(int mhDice = 0; mhDice < abilityUser_.charInventory.rightHand.weaponDamage.diceRolled; ++mhDice)
                    {
                        mhDamage += Random.Range(0, abilityUser_.charInventory.rightHand.weaponDamage.diceSides) + 1;
                    }
                    //Applying the damage to the damage type
                    damageTypeTotalDamage_[abilityUser_.charInventory.rightHand.weaponDamage.type] += mhDamage;
                    break;

                case WeaponHand.OffHand:
                    //Getting the damage that the off hand weapon deals
                    int ohDamage = abilityUser_.charInventory.leftHand.weaponDamage.baseDamage;
                    for (int ohDice = 0; ohDice < abilityUser_.charInventory.leftHand.weaponDamage.diceRolled; ++ohDice)
                    {
                        ohDamage += Random.Range(0, abilityUser_.charInventory.leftHand.weaponDamage.diceSides) + 1;
                    }
                    //Applying the damage to the damage type
                    damageTypeTotalDamage_[abilityUser_.charInventory.leftHand.weaponDamage.type] += ohDamage;
                    break;

                case WeaponHand.OneHand:
                    int onhDamage = 0;
                    //If the player is dual weilding and both weapons are of the same type of this attack
                    if((abilityUser_.charInventory.rightHand != null && abilityUser_.charInventory.rightHand.weaponType == this.weaponSkillUsed) &&
                        abilityUser_.charInventory.leftHand != null && abilityUser_.charInventory.leftHand.weaponType == this.weaponSkillUsed)
                    {
                        //If the current numbered attack is even, we use the main hand weapon
                        if(attackNumber_ % 2 == 0)
                        {
                            //Getting the damage that the main hand weapon deals
                            onhDamage = abilityUser_.charInventory.rightHand.weaponDamage.baseDamage;
                            for (int mhDice = 0; mhDice < abilityUser_.charInventory.rightHand.weaponDamage.diceRolled; ++mhDice)
                            {
                                onhDamage += Random.Range(0, abilityUser_.charInventory.rightHand.weaponDamage.diceSides) + 1;
                            }
                            //Applying the damage to the damage type
                            damageTypeTotalDamage_[abilityUser_.charInventory.rightHand.weaponDamage.type] += onhDamage;
                        }
                        //If the current numbered attack is odd, we use the off hand weapon
                        else
                        {
                            //Getting the damage that the off hand weapon deals
                            onhDamage = abilityUser_.charInventory.leftHand.weaponDamage.baseDamage;
                            for (int ohDice = 0; ohDice < abilityUser_.charInventory.leftHand.weaponDamage.diceRolled; ++ohDice)
                            {
                                onhDamage += Random.Range(0, abilityUser_.charInventory.leftHand.weaponDamage.diceSides) + 1;
                            }
                            //Applying the damage to the damage type
                            damageTypeTotalDamage_[abilityUser_.charInventory.leftHand.weaponDamage.type] += onhDamage;
                        }
                    }
                    //If the main hand weapon is the same type as this action
                    if(abilityUser_.charInventory.rightHand != null && abilityUser_.charInventory.rightHand.weaponType == this.weaponSkillUsed)
                    {
                        //Getting the damage that the main hand weapon deals
                        onhDamage = abilityUser_.charInventory.rightHand.weaponDamage.baseDamage;
                        for (int mhDice = 0; mhDice < abilityUser_.charInventory.rightHand.weaponDamage.diceRolled; ++mhDice)
                        {
                            onhDamage += Random.Range(0, abilityUser_.charInventory.rightHand.weaponDamage.diceSides) + 1;
                        }
                        //Applying the damage to the damage type
                        damageTypeTotalDamage_[abilityUser_.charInventory.rightHand.weaponDamage.type] += onhDamage;
                    }
                    //If the off hand weapon is the same type as this action
                    else if(abilityUser_.charInventory.leftHand != null && abilityUser_.charInventory.leftHand.weaponType == this.weaponSkillUsed)
                    {
                        //Getting the damage that the off hand weapon deals
                        onhDamage = abilityUser_.charInventory.leftHand.weaponDamage.baseDamage;
                        for (int ohDice = 0; ohDice < abilityUser_.charInventory.leftHand.weaponDamage.diceRolled; ++ohDice)
                        {
                            onhDamage += Random.Range(0, abilityUser_.charInventory.leftHand.weaponDamage.diceSides) + 1;
                        }
                        //Applying the damage to the damage type
                        damageTypeTotalDamage_[abilityUser_.charInventory.leftHand.weaponDamage.type] += onhDamage;
                    }
                    break;

                case WeaponHand.TwoHand:
                    //Getting the damage that the 2 handed weapon in the main hand deals
                    int thDamage = abilityUser_.charInventory.rightHand.weaponDamage.baseDamage;
                    for (int mhDice = 0; mhDice < abilityUser_.charInventory.rightHand.weaponDamage.diceRolled; ++mhDice)
                    {
                        thDamage += Random.Range(0, abilityUser_.charInventory.rightHand.weaponDamage.diceSides) + 1;
                    }
                    //Applying the damage to the damage type
                    damageTypeTotalDamage_[abilityUser_.charInventory.rightHand.weaponDamage.type] += thDamage;
                    break;

                case WeaponHand.DualWeild:
                    int dwDamage = 0;
                    //If the current attack number is even, we use the main hand
                    if (attackNumber_ % 2 == 0)
                    {
                        //Getting the damage that the main hand weapon deals
                        dwDamage = abilityUser_.charInventory.rightHand.weaponDamage.baseDamage;
                        for (int mhDice = 0; mhDice < abilityUser_.charInventory.rightHand.weaponDamage.diceRolled; ++mhDice)
                        {
                            dwDamage += Random.Range(0, abilityUser_.charInventory.rightHand.weaponDamage.diceSides) + 1;
                        }
                        //Applying the damage to the damage type
                        damageTypeTotalDamage_[abilityUser_.charInventory.rightHand.weaponDamage.type] += dwDamage;
                    }
                    //If the current attack number is odd, we use the off hand
                    else
                    {
                        //Getting the damage that the off hand weapon deals
                        dwDamage = abilityUser_.charInventory.leftHand.weaponDamage.baseDamage;
                        for (int ohDice = 0; ohDice < abilityUser_.charInventory.leftHand.weaponDamage.diceRolled; ++ohDice)
                        {
                            dwDamage += Random.Range(0, abilityUser_.charInventory.leftHand.weaponDamage.diceSides) + 1;
                        }
                        //Applying the damage to the damage type
                        damageTypeTotalDamage_[abilityUser_.charInventory.leftHand.weaponDamage.type] += dwDamage;
                    }
                    break;
            }
        }
    }
}
