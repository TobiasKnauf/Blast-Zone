using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Weapon
{
    private float tickRate = .5f;
    private float tickTimer;

    private List<IKillable> enemyList;

    public override void Launch(Vector2 _dir, int _noDmgLayer)
    {
        destroyAfter10 = false;
        destroyOnHit = false;
        destroyAfterDistance = false;
        tickTimer = tickRate;

        enemyList = new List<IKillable>();

        base.Launch(_dir, _noDmgLayer);
    }

    protected override void OnDeathEffect()
    {

    }

    protected override void Update()
    {
        base.Update();

        transform.localScale = new Vector2(
            this.transform.localScale.x,
            stats.Range);

        if (tickTimer >= tickRate)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] == null) continue;

                crits = Random.Range(0f, 1f) <= PlayerController.Instance.PlayerStats.CritChance;

                float determinedDamage = crits ? damage * PlayerController.Instance.PlayerStats.CritMulitplier : damage;

                enemyList[i].GetDamage(determinedDamage, transform.right, stats.Knockback);
                UIManager.Instance.AddCombo(.0025f);
                PlayerController.Instance.ComboValue += .0025f;

            }
            tickTimer = 0;
        }


        tickTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) return;

        IKillable obj = collision.GetComponent<IKillable>();

        enemyList.Add(obj);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) return;

        IKillable obj = collision.GetComponent<IKillable>();

        enemyList.Remove(obj);
    }

    //private void OnTriggerStay2D(Collider2D _other)
    //{
    //    if (_other.gameObject.layer == LayerMask.NameToLayer("Player")) return;

    //    IKillable obj = _other.GetComponent<IKillable>();

    //    if (tickTimer < tickRate) return;

    //    if (obj != null)
    //    {
    //        crits = Random.Range(0f, 1f) <= PlayerController.Instance.PlayerStats.CritChance;

    //        float determinedDamage = crits ? damage * PlayerController.Instance.PlayerStats.CritMulitplier : damage;

    //        obj.GetDamage(determinedDamage, transform.right, stats.Knockback);
    //        UIManager.Instance.AddCombo(.0025f);
    //        PlayerController.Instance.ComboValue += .0025f;

    //        tickTimer = 0;
    //    }
    //}
    public override void ApplyUpgrade(WeaponUpgrade _upgrade)
    {
        if (!_upgrade.WeaponTypes.Contains(stats)) return;

        foreach (var upgrade in _upgrade.Upgrades)
        {
            switch (upgrade.Type)
            {
                case EWeaponUpgradeTypes.Projectile_Speed:
                    // stats.Speed += (stats.Speed / 100) * upgrade.Amount;
                    break;
                case EWeaponUpgradeTypes.Attack_Speed:
                    stats.ReloadTime -= (tickRate / 100) * upgrade.Amount;
                    break;
                case EWeaponUpgradeTypes.Damage:
                    stats.Damage += (stats.Damage / 100) * upgrade.Amount;
                    break;
                case EWeaponUpgradeTypes.Attack_Range:
                    stats.Range += upgrade.Amount;
                    break;
                case EWeaponUpgradeTypes.Knockback:
                    stats.Knockback += (stats.Knockback / 100) * upgrade.Amount;
                    break;
                case EWeaponUpgradeTypes.Projectiles:
                    // stats.NumberOfProjectiles += (int)upgrade.Amount;
                    break;
                default:
                    break;
            }
        }
    }
}
