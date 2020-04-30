//Enum for the hand that a weapon attack requires the used weapon to be in
public enum WeaponHand
{
    MainHand,//Required weapon needs to be in the main hand
    OffHand,//Required weapon needs to be in the off hand
    OneHand,//Required weapon can be in the main OR off hand as long as it's 1-handed
    TwoHand,//Required weapon needs to be 2-handed
    DualWeild//Required weapons need to be in both hands
};
