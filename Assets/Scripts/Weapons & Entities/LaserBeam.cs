using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Weapon
{
    public override void Launch(Vector2 _dir, int _noDmgLayer)
    {
        destroyAfter10 = false;
        destroyOnHit = false;
        destroyAfterDistance = false;

        base.Launch(_dir, _noDmgLayer);
    }

    protected override void OnDeathEffect()
    {

    }

    protected override void UpdateProjectile()
    {
        base.UpdateProjectile();

        transform.localScale = new Vector2(
            this.transform.localScale.x,
            stats.Range);
    }

    private void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player")) return;

        IKillable obj = _other.GetComponent<IKillable>();

        if (obj != null)
        {
            crits = Random.Range(0f, 1f) <= PlayerController.Instance.PlayerStats.CritChance;

            float determinedDamage = crits ? damage * PlayerController.Instance.PlayerStats.CritMulitplier : damage;

            obj.GetDamage(determinedDamage, transform.right, stats.Knockback);
            UIManager.Instance.AddCombo(.005f);
            PlayerController.Instance.ComboValue += .005f;
        }
    }
}
