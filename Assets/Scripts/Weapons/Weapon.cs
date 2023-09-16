using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
    Bullet,
    Laser,
    Shockwave,
    Rocket,
}

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [HideInInspector] public WeaponStats stats;
    private int noDmgLayer;
    protected bool destroyOnHit = true;
    protected bool destroyAfter10 = true;
    protected bool destroyAfterDistance = false;

    protected bool crits;
    protected float damage;
    protected float knockback;

    protected Vector2 startPos;

    public virtual void Launch(Vector2 _dir, int _noDmgLayer)
    {
        noDmgLayer = _noDmgLayer;
        transform.up = _dir;
        startPos = this.transform.position;
        damage = stats.Damage;
        knockback = stats.Knockback;
        crits = Random.Range(0f, 1f) <= PlayerController.Instance.PlayerStats.CritChance;

        rb.AddForce(transform.up * stats.Speed, ForceMode2D.Impulse);
    }

    private void Start()
    {
        if (destroyAfter10)
            Invoke(nameof(Destroy), 10f);
    }

    protected virtual void Update()
    {
        if (destroyAfterDistance)
        {
            if (Vector2.Distance(startPos, this.transform.position) >= stats.Range)
                Destroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == noDmgLayer) return;
        if (stats.weaponType is EWeaponType.Laser) return;

        IKillable obj = _other.GetComponent<IKillable>();

        if (obj != null)
        {
            float determinedDamage = crits ? damage * PlayerController.Instance.PlayerStats.CritMulitplier : damage;

            obj.GetDamage(determinedDamage, rb.velocity, knockback);
            UIManager.Instance.AddCombo(.1f);
            PlayerController.Instance.ComboValue += .1f;
            SoundManager.Instance.PlaySound(ESound.Hit);

            if (destroyOnHit)
                DestroyOnHit();
        }
    }
    protected abstract void OnDeathEffect();
    protected virtual void DestroyOnHit()
    {
        Destroy(this.gameObject);
        OnDeathEffect();
    }
    protected virtual void Destroy()
    {
        UIManager.Instance.AddCombo(-.2f);
        PlayerController.Instance.ComboValue -= .2f;
        OnDeathEffect();
        Destroy(this.gameObject);
    }
    public virtual void ApplyUpgrade(WeaponUpgrade _upgrade)
    {
        if (!_upgrade.WeaponTypes.Contains(stats)) return;

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
                    stats.Range += (stats.Range / 100) * upgrade.Amount;
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
