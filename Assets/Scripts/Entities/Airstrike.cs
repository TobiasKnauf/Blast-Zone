using Cinemachine;
using UnityEngine;

public class Airstrike : MonoBehaviour
{
    [SerializeField] private float m_damage;
    [SerializeField] private SpriteRenderer m_areaSprite;
    [SerializeField] private SpriteRenderer m_outlineSprite;

    [SerializeField] private CinemachineImpulseSource impulseSource;


    private float explosionRadius;
    private float timeTillExplode;
    private float startTime;

    public void Init(Vector2 _pos)
    {
        transform.position = _pos;
        startTime = Time.time;
        explosionRadius = Random.Range(2f, 7f);
        timeTillExplode = explosionRadius - (explosionRadius / 2f) / 2f;
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
        float damage = (m_damage * (PlayerController.Instance.currentLevel + 1));
        damage += (damage / 100) * 15f;

        if (Vector2.Distance(this.transform.position, PlayerController.Instance.transform.position) <= explosionRadius)
            PlayerController.Instance.GetDamage(damage, PlayerController.Instance.transform.position - this.transform.position, 500);

        Enemy[] temp = EnemySpawner.Instance.AllEntities.ToArray();
        foreach (Enemy e in temp)
        {
            if (e == null) continue;

            if (Vector2.Distance(this.transform.position, e.transform.position) <= explosionRadius)
                e.GetDamage(damage, e.transform.position - this.transform.position, 500);
        }

        //CameraScript.Instance.StartCoroutine(CameraScript.Instance.Shake(.4f, .05f));
        CameraScript.Instance.CameraShake(impulseSource, 0.1f);

        SoundManager.Instance.PlaySound(ESound.Explosion);
        AirstrikeSpawner.Instance.UnSubscribe(this);
        Destroy(this.gameObject);
    }
}
