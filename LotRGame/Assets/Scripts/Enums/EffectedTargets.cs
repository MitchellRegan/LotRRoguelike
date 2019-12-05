//Enum that determines who is effected
public enum EffectedTargets
{
    Defender,//The person being hit by the attack
    Attacker,//The person making the attack
    EnemiesOnly,//Hits all enemies in the radius and ignores allies
    EnemiesExceptDefender,//Hits all enemies, but doesn't include the defender
    AlliesOnly,//Hits all allies in the radius and ignores enemies
    AlliesExceptAttacker,//Hits all allies, but doesn't include the attacker
    Everyone//Hits every ally and enemy in the radius
};