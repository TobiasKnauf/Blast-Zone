using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Button Button;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text levelsText;

    public void SetUpgrade(ScriptableUpgrade _upgrade)
    {
        icon.sprite = _upgrade.Icon;
        name.text = _upgrade.Name;
        descriptionText.text = _upgrade.DescriptionText;
        levelsText.text = $"Level: {_upgrade.CurrentLevel} / {_upgrade.MaxLevels}";

        Button.onClick.AddListener(() => PickUpgrade(_upgrade));
    }

    private void PickUpgrade(ScriptableUpgrade _upgrade)
    {
        if (_upgrade is WeaponUpgrade)
            PlayerController.Instance.weaponStats.Weapon.ApplyUpgrade((WeaponUpgrade)_upgrade);
        else if (_upgrade is PlayerUpgrade)
            PlayerController.Instance.ApplyUpgrade((PlayerUpgrade)_upgrade);
        else if(_upgrade is EffectUpgrade)
        {
            EffectUpgrade u = (EffectUpgrade)_upgrade;

            switch (u.effectType)
            {
                case EEffectType.Slow:
                    EnemySpawner.Instance.SlowOnKill = true;
                    break;
                case EEffectType.Explode:
                    EnemySpawner.Instance.ExplodeOnKill = true;
                    break;
                default:
                    break;
            }
        }


        _upgrade.CurrentLevel++;

        PlayerController.Instance.ResetCharge();
        UIManager.Instance.CloseUpgradeMenu();
        UpgradeManager.Instance.ResetButtons();
    }
}