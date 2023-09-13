using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    float t = 0;

    public void Detonate(float _splashDamage, float _radius, bool _hitAll = false)
    {
        //this.transform.localScale = Vector2.one * _radius * 2f;

        Collider2D[] hits;
        
        if (!_hitAll)
            hits = Physics2D.OverlapCircleAll(this.transform.position, _radius, 1 << 7);
        else
            hits = Physics2D.OverlapCircleAll(this.transform.position, _radius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].TryGetComponent(out IKillable killable))
                killable.GetDamage(
                    _splashDamage,
                    (hits[i].transform.position - this.transform.position).normalized,
                    1000f);
        }

        renderer.transform.localScale = Vector2.zero;
        renderer.enabled = true;
        StartCoroutine(DisplayExplosion(_radius));
        //StartCoroutine(Destroy());
    }

    
    private IEnumerator DisplayExplosion(float _radius)
    {
        if (t < 1f)
        {
            t += Time.deltaTime * 2f;
            renderer.transform.localScale = Vector2.Lerp(Vector2.one * _radius * 2f, Vector2.one, t);
            renderer.color = new Color(renderer.color.r, renderer.color.g + t, renderer.color.b + t, renderer.color.a - t * .75f);
            yield return new WaitForEndOfFrame();

            StartCoroutine(DisplayExplosion(_radius));

        }
        else
        {
            Destroy(this.gameObject);
            yield return null;
        }

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
