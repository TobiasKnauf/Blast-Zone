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

[CreateAssetMenu(menuName = "Upgrades/Weapon Upgrade", fileName = "SO_NewWeaponUpgrade")]
public class WeaponUpgrade : ScriptableObject
{
    public string Name;

    public Sprite Icon;
    public Sprite DescriptionImage;

    public List<WeaponStats> WeaponTypes;
    public List<Upgrade<EWeaponUpgradeTypes>> Upgrades;
}

[CreateAssetMenu(menuName = "Upgrades/Player Upgrade", fileName = "SO_NewPlayerUpgrade")]
public class PlayerUpgrade : ScriptableObject
{
    public string Name;

    public Sprite Icon;
    public Sprite DescriptionImage;

    public List<Upgrade<EPlayerUpgradeTypes>> Upgrades;
}
