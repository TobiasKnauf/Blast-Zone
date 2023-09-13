using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    protected override void OnDeathEffect()
    {

    }

    protected override void UpdateEnemy()
    {
        this.transform.position += Stats.MoveSpeed * Time.deltaTime * (Vector3)dir;

    }
}
