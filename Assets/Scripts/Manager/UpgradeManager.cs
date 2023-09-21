using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private UpgradeButton[] upgradeButtons;

    public ScriptableUpgrade[] AllUpgrades;

    [HideInInspector] public List<ScriptableUpgrade> ChosenUpgrades;

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
#if !UNITY_EDITOR
            StartCoroutine(DelayButton(btn.Button));
#endif
        }
    }

    private IEnumerator DelayButton(Button b)
    {
        b.interactable = false;
        yield return new WaitForSecondsRealtime(1.5f);
        b.interactable = true;
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
            if (u is WeaponUpgrade)
            {
                WeaponUpgrade w = (WeaponUpgrade)u;

                if (!w.WeaponTypes.Contains(PlayerController.Instance.weaponStats))
                    continue;
            }

            if (u.CurrentLevel < u.MaxLevels)
            {
                return true;
            }
        }

        return false;
    }

    public ScriptableUpgrade GetRandomUpgrade()
    {
        if (!UpgradesRemaining()) return null;

        int rnd = Random.Range(0, AllUpgrades.Length);

        if (AllUpgrades[rnd] is WeaponUpgrade)
        {
            WeaponUpgrade w = (WeaponUpgrade)AllUpgrades[rnd];

            if (!w.WeaponTypes.Contains(PlayerController.Instance.weaponStats))
                return GetRandomUpgrade();
        }

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
