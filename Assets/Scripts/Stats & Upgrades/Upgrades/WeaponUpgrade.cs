using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Weapon Upgrade", fileName = "SO_NewWeaponUpgrade")]
public class WeaponUpgrade : ScriptableUpgrade
{
    public List<WeaponStats> WeaponTypes;
    public List<Upgrade<EWeaponUpgradeTypes>> Upgrades;
}
