using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Weapon
{
    [SerializeField] private SpriteRenderer renderer;

    private float distanceTravelledPercent;
    private float maxTravellDistance = 4f;

    public override void Launch(Vector2 _dir, int _noDmgLayer)
    {
        destroyOnHit = false;
        destroyAfter10 = false;
        destroyAfterDistance = true;

        base.Launch(_dir, _noDmgLayer);
    }

    protected override void OnDeathEffect()
    {

    }

    protected override void Update()
    {
        base.Update();

        if (Vector2.Distance(startPos, this.transform.position) >= maxTravellDistance)
            Destroy();

        distanceTravelledPercent = Vector2.Distance(startPos, this.transform.position) / maxTravellDistance;

        this.transform.localScale += Vector3.one * (Time.deltaTime * 3f) * stats.Range * .1f;
        renderer.color = new Color(
                renderer.color.r,
                renderer.color.g,
                renderer.color.b,
                1 - distanceTravelledPercent);

        damage = Mathf.Lerp(stats.Damage, stats.Damage / stats.Range, distanceTravelledPercent);
        Mathf.Clamp(damage, 0, float.MaxValue);

        knockback = Mathf.Lerp(stats.Knockback, stats.Knockback / stats.Range, distanceTravelledPercent); ;
        Mathf.Clamp(knockback, stats.Knockback / 2f, float.MaxValue);
    }
    public override void ApplyUpgrade(WeaponUpgrade _upgrade)
    {
        foreach (var upgrade in _upgrade.Upgrades)
        {
            switch (upgrade.Type)
            {
                case EWeaponUpgradeTypes.Projectile_Speed:
                    stats.Speed += (stats.Speed / 100) * upgrade.Amount;
                    break;
                case EWeaponUpgradeTypes.Attack_Speed:
                    stats.ReloadTime -= (stats.ReloadTime / 100) * upgrade.Amount;
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
                    stats.NumberOfProjectiles += (int)upgrade.Amount;
                    break;
                case EWeaponUpgradeTypes.SplashRadius:
                    stats.SplashRadius += (int)upgrade.Amount;
                    break;
                default:
                    break;
            }
        }
    }
}
