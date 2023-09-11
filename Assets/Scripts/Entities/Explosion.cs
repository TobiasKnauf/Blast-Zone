using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;

    public void Detonate(float _splashDamage, float _radius)
    {
        this.transform.localScale = Vector2.one * _radius * 2f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _radius, 1 << 7);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].TryGetComponent(out IKillable killable))
                killable.GetDamage(
                    _splashDamage,
                    (hits[i].transform.position - this.transform.position).normalized,
                    1000f);
        }

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        while (renderer.color.a > 0)
        {
            renderer.color = new Color(
                renderer.color.r, 
                renderer.color.g, 
                renderer.color.b, 
                renderer.color.a - (.5f * Time.deltaTime));
            yield return null;
        }

        Destroy(this.gameObject);
    }

}
