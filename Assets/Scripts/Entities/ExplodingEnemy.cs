using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    private bool isExploding;

    public override void OnDeathEffect()
    {
        Explosion e = Instantiate(explosion);
        e.transform.position = this.transform.position;
        e.Detonate(Stats.MaxHealth / 2f, 3f, true);
    }

    protected override void UpdateEnemy()
    {
        if (isExploding) return;

        this.transform.position += MoveSpeed * Time.deltaTime * (Vector3)dir;

        if (Vector2.Distance(PlayerController.Instance.transform.position, this.transform.position) <= 1f)
            Die(Vector2.zero);
    }

    //private IEnumerator Explode()
    //{
    //    isExploding = true;

    //    //start animation

    //    Die(Vector2.zero);

    //    yield return null;
    //}
}
