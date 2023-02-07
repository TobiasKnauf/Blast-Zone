using UnityEngine;

public class Airstrike : MonoBehaviour
{
    [SerializeField] private float m_damage;
    [SerializeField] private SpriteRenderer m_areaSprite;
    [SerializeField] private SpriteRenderer m_outlineSprite;

    private float explosionRadius;
    private float timeTillExplode;
    private float startTime;

    public void Init(Vector2 _pos)
    {
        transform.position = _pos;
        startTime = Time.time;
        explosionRadius = Random.Range(2f, 7f);
        timeTillExplode = explosionRadius - (explosionRadius / 2f) / 1.5f;
        m_outlineSprite.transform.localScale = Vector2.one * explosionRadius * 2;
        Invoke(nameof(Detonate), timeTillExplode);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        float elapsedTime = Time.time - startTime;
        float t = elapsedTime / timeTillExplode;
        t = Mathf.Clamp01(t);

        m_areaSprite.transform.localScale = Vector2.Lerp(Vector2.one, Vector2.one * explosionRadius * 2, t);
    }

    private void Detonate()
    {
        if (Vector2.Distance(this.transform.position, PlayerController.Instance.transform.position) <= explosionRadius)
            PlayerController.Instance.GetDamage(m_damage, PlayerController.Instance.transform.position - this.transform.position, 500);

        Enemy[] temp = EnemySpawner.Instance.AllEntities.ToArray();
        foreach (Enemy e in temp)
        {
            if (e == null) continue;

            if (Vector2.Distance(this.transform.position, e.transform.position) <= explosionRadius)
                e.GetDamage(m_damage, e.transform.position - this.transform.position, 500);
        }

        CameraScript.Instance.StartCoroutine(CameraScript.Instance.Shake(.4f, .05f));

        AirstrikeSpawner.Instance.UnSubscribe(this);
        Destroy(this.gameObject);
    }
}
