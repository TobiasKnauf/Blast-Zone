using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Weapon Upgrade", fileName = "SO_NewWeaponUpgrade")]
public class WeaponUpgrade : ScriptableObject
{
    public string Name;

    public Sprite Icon;
    public Sprite DescriptionImage;

    public List<WeaponStats> WeaponTypes;
    public List<Upgrade<EWeaponUpgradeTypes>> Upgrades;
}
