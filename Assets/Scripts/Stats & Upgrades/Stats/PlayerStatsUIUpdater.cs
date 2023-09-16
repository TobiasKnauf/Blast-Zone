using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUIUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text damage;
    [SerializeField] private TMP_Text attackspeed;
    [SerializeField] private TMP_Text projectileSpeed;
    [SerializeField] private TMP_Text critMultiplier;
    [SerializeField] private TMP_Text critChance;
    [SerializeField] private TMP_Text movespeed;
    [SerializeField] private TMP_Text attackrange;
    [SerializeField] private TMP_Text splashradius;
    [SerializeField] private TMP_Text knockback;


    void Update()
    {
        damage.text = PlayerController.Instance.weaponStats.Damage.ToString("F0");
        attackspeed.text = PlayerController.Instance.weaponStats.ReloadTime.ToString("F2") + "s";
        projectileSpeed.text = PlayerController.Instance.weaponStats.Speed.ToString("F2");
        critMultiplier.text = PlayerController.Instance.PlayerStats.CritMulitplier.ToString("F0") + "x";
        critChance.text = (PlayerController.Instance.PlayerStats.CritChance * 100f).ToString("F0");
        movespeed.text = PlayerController.Instance.PlayerStats.MoveSpeed.ToString("F0");
        attackrange.text = PlayerController.Instance.weaponStats.Range.ToString("F0");
        splashradius.text = PlayerController.Instance.weaponStats.SplashRadius.ToString("F0") + "m";
        knockback.text = PlayerController.Instance.weaponStats.Knockback.ToString("F0");
    }
}
