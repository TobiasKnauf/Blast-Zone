using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player Upgrade", fileName = "SO_NewPlayerUpgrade")]
public class PlayerUpgrade : ScriptableObject
{
    public string Name;

    public Sprite Icon;
    public Sprite DescriptionImage;

    public List<Upgrade<EPlayerUpgradeTypes>> Upgrades;
}
