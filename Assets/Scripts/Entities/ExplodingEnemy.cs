using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    [SerializeField] private Explosion explosion;
    private bool isExploding;

    protected override void OnDeathEffect()
    {
        Explosion e = Instantiate(explosion);
        e.transform.position = this.transform.position;
        e.Detonate(100, 3f, true);
    }

    protected override void UpdateEnemy()
    {
        if (isExploding) return;

        this.transform.position += Stats.MoveSpeed * Time.deltaTime * (Vector3)dir;

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
