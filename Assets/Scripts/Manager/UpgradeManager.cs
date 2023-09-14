using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private UpgradeButton[] upgradeButtons;

    public ScriptableUpgrade[] AllUpgrades;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        ResetUpgrades();
    }

    public void SetUpgrades()
    {
        foreach (var btn in upgradeButtons)
        {
            btn.SetUpgrade(GetRandomUpgrade());
        }
    }
    public void ResetUpgrades()
    {
        foreach (var u in AllUpgrades)
        {
            u.ResetUpgrade();
        }
    }

    public bool UpgradesRemaining()
    {
        foreach (var u in AllUpgrades)
        {
            if (u.CurrentLevel < u.MaxLevels)
                return true;
        }

        return false;
    }

    public ScriptableUpgrade GetRandomUpgrade()
    {
        int rnd = Random.Range(0, AllUpgrades.Length);

        if (Random.Range(0, 1f) > AllUpgrades[rnd].Chance)
            return GetRandomUpgrade();

        if (AllUpgrades[rnd].CurrentLevel >= AllUpgrades[rnd].MaxLevels)
            return GetRandomUpgrade();

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
