//Enum used in RemoveEffectsEffect.cs
public enum EffectTypeToRemove
{
    Any, //Removes any kind of effect
    Beneficial, //HoT and positive stat effects
    Negative, //DoT and negative stat effects
    DoT, //Damage over Time effects
    DoTType, //Damage over Time effect of a specific damage type
    HoT, //Heal over Time effects
    HoTType, //Heal over Time effect of a specific damage type
    ModifyStat, //Any modify stat effect
    StatBoost, //Modify stat effects that are beneficial
    StatNerf //Modify stat effects that are negative
};