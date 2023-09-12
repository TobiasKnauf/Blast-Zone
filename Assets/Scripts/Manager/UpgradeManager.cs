using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private UpgradeButton[] upgradeButtons;

    [SerializeField] private ScriptableUpgrade[] AllUpgrades;

    private void Awake()
    {
        Instance = this;
    }

    public void SetUpgrades()
    {
        foreach (var btn in upgradeButtons)
        {
            btn.SetUpgrade(GetRandomUpgrade());
        }
    }
    
    public ScriptableUpgrade GetRandomUpgrade()
    {
        int rnd = Random.Range(0, AllUpgrades.Length);
        return AllUpgrades[rnd];
    }
    public void ResetButtons()
    {
        foreach (var btn in upgradeButtons)
        {
            btn.Button.onClick.RemoveAllListeners();
        }
    }
}
