using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Button Button;
    [SerializeField] private Image img;

    public void SetUpgrade(ScriptableUpgrade _upgrade)
    {
        img.sprite = _upgrade.DescriptionImage;
        Button.onClick.AddListener(() => PickUpgrade(_upgrade));
    }

    private void PickUpgrade(ScriptableUpgrade _upgrade)
    {
        if (_upgrade is WeaponUpgrade)
            PlayerController.Instance.weaponStats.Weapon.ApplyUpgrade((WeaponUpgrade)_upgrade);
        else if (_upgrade is PlayerUpgrade)
            PlayerController.Instance.ApplyUpgrade((PlayerUpgrade)_upgrade);

        PlayerController.Instance.ResetCharge();
        UIManager.Instance.CloseUpgradeMenu();
        UpgradeManager.Instance.ResetButtons();
    }
}
