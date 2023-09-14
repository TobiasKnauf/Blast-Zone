using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableUpgrade : ScriptableObject
{
    public string Name;

    public Sprite Icon;
    public string DescriptionText;

    public int MaxLevels;
    public int CurrentLevel;

    [Range(0f, 1f)] public float Chance;

    public void ResetUpgrade()
    {
        CurrentLevel = 0;
    }
}
