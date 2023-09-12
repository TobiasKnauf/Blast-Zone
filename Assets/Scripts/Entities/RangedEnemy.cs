using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private WeaponStats weapon;

    private float shootTimer = 0f;

    protected override void OnDeathEffect()
    {

    }

    protected override void UpdateEnemy()
    {
        if (Vector2.Distance(PlayerController.Instance.transform.position, this.transform.position) >= 6f)
            this.transform.position += m_moveSpeed * Time.deltaTime * (Vector3)dir;
        else
        {
            if (shootTimer <= 0)
                Attack();
        }

        shootTimer -= Time.deltaTime;
    }

    private void Attack()
    {
        Weapon w = Instantiate(weapon.Weapon);
        w.transform.up = transform.up;
        w.transform.position = this.transform.position;
        w.transform.position += w.transform.up * .75f;

        w.Launch(w.transform.up, this.gameObject.layer);
        SoundManager.Instance.PlaySound(ESound.Shoot);

        shootTimer = weapon.ReloadTime;
    }
}
