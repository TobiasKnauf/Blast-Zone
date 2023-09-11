using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponUpgradeTypes
{
    Projectile_Speed,
    Attack_Speed,
    Damage,
    Attack_Range,
    Knockback,
    Projectiles,
}
public enum EPlayerUpgradeTypes
{
    Health,
    Armor,
    Movement_Speed,
    Crit_Chance,
    Crit_Multiplier,
}

[System.Serializable]
public class Upgrade<T>
{
    public T Type;
    public float Amount;
}
