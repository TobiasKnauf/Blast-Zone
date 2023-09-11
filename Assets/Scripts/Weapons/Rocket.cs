using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Weapon
{
    [SerializeField] private Explosion explosion;

    public override void Launch(Vector2 _dir, int _noDmgLayer)
    {
        destroyOnHit = true;
        destroyAfter10 = true;
        destroyAfterDistance = true;

        base.Launch(_dir, _noDmgLayer);
    }

    protected override void OnDeathEffect()
    {
        float damage = crits ? stats.SplashDamage * PlayerController.Instance.PlayerStats.CritMulitplier : stats.SplashDamage;

        Explosion e = Instantiate(explosion);
        e.transform.position = this.transform.position;
        e.Detonate(damage, stats.SplashRadius);
    }
}
