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
}
