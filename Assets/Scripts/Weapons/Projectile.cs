using UnityEngine;

public class Projectile : Weapon
{
    public override void Launch(Vector2 _dir, int _noDmgLayer)
    {
        destroyOnHit = true;
        destroyAfter10 = true;
        destroyAfterDistance = true;

        base.Launch(_dir, _noDmgLayer);
    }

    protected override void OnDeathEffect()
    {

    }
}
