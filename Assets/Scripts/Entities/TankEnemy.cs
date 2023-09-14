using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    public override void OnDeathEffect()
    {
        base.OnDeathEffect();
    }

    protected override void UpdateEnemy()
    {
        this.transform.position += MoveSpeed * Time.deltaTime * (Vector3)dir;

    }
}
