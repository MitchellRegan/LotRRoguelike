//Enum for the state of this combat manager to decide what to do on update
public enum CombatState
{
    Wait,
    IncreaseInitiative,
    SelectAction,
    PlayerInput,
    EndCombat,
    GameOver
}
