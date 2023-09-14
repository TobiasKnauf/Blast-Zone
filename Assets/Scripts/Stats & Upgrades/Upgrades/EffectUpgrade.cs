using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffectType
{
    Slow,
    Explode
}

[CreateAssetMenu(menuName = "Upgrades/Effect Upgrade", fileName = "SO_NewEffectUpgrade")]
public class EffectUpgrade : ScriptableUpgrade
{
    public EEffectType effectType;
}
