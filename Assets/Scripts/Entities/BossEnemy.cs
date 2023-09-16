using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Enemy
{
    [SerializeField] private Image healthbar;

    protected override void Start()
    {
        base.Start();

        Stats.MaxHealth = 1000 + (PlayerController.Instance.currentLevel * 500f);
        health = Stats.MaxHealth;
    }
    protected override void UpdateEnemy()
    {
        this.transform.position += MoveSpeed * Time.deltaTime * (Vector3)dir;

        healthbar.fillAmount = health / Stats.MaxHealth;
    }
    public override void GetDamage(float _value, Vector2 _dir, float _knockbackForce)
    {
        base.GetDamage(_value, _dir, _knockbackForce / 3f);
    }
    public override void OnDeathEffect()
    {
        Explosion e = Instantiate(explosion);
        e.transform.position = this.transform.position;
        e.Detonate(Stats.MaxHealth / 2f, 6f, false);
    }
}
