using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Weapon
{
    [SerializeField] private SpriteRenderer renderer;

    private float distanceTravelledPercent;

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

    protected override void UpdateProjectile()
    {
        base.UpdateProjectile();

        distanceTravelledPercent = Vector2.Distance(startPos, this.transform.position) / stats.Range;

        this.transform.localScale += Vector3.one * Time.deltaTime * 3f * stats.Range * .1f;
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
}
