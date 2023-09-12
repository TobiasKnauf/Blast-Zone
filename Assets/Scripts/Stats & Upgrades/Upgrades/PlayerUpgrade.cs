using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player Upgrade", fileName = "SO_NewPlayerUpgrade")]
public class PlayerUpgrade : ScriptableUpgrade
{
    public List<Upgrade<EPlayerUpgradeTypes>> Upgrades;
}
