using UnityEngine;

public class SpecialOffensePlant : Plants
{
    public float damage; // Damage for all Special Offense plants

    public override float GetDamage() // Override the Getdamage from base class
    {
        return damage;
    }
}
