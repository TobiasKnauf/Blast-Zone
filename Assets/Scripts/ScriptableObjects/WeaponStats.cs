using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Weapon Stats", fileName = "SO_WeaponStats")]
public class WeaponStats : Stats
{
    [SerializeField] private string name = "New Weapon";
    [SerializeField] private Weapon weapon;
    public EWeaponType weaponType;
    [SerializeField] private float damage = 1;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float range = 20f;
    [SerializeField] private int numberOfProjectiles = 1;
    [SerializeField] private float reloadTime = 0.2f;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float knockback = 750f;

    private float runtimeDamage;
    private float runtimeSpeed;
    private float runtimeRange;
    private int runtimeNOP;
    private float runtimeReloadTime;
    private float runtimeSplashDamage;
    private float runtimeSplashRadius;
    private float runtimeKnockback;

    public string Name { get { return name; } }
    public Weapon Weapon { get { return weapon; } }
    public float Damage { get { return runtimeDamage; } set { runtimeDamage = value; } }
    public float Speed { get { return runtimeSpeed; } set { runtimeSpeed = value; } }
    public float Range { get { return runtimeRange; } set { runtimeRange = value; } }
    public int NumberOfProjectiles { get { return runtimeNOP; } set { runtimeNOP = value; } }
    public float ReloadTime { get { return runtimeReloadTime; } set { runtimeReloadTime = value; } }
    public float SplashDamage { get { return runtimeSplashDamage; } set { runtimeSplashDamage = value; } }
    public float SplashRadius { get { return runtimeSplashRadius; } set { runtimeSplashRadius = value; } }
    public float Knockback { get { return runtimeKnockback; } set { runtimeKnockback = value; } }

    private void OnEnable()
    {
        runtimeDamage = damage;
        runtimeSpeed = speed;
        runtimeRange = range;
        runtimeNOP = numberOfProjectiles;
        runtimeReloadTime = reloadTime;
        runtimeSplashDamage = splashDamage;
        runtimeSplashRadius = splashRadius;
        runtimeKnockback = knockback;

        weapon.stats = this;
    }
}
