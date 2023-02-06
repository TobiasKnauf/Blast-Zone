using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private float m_explosionRadius;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_timeTillExplode;

    private void Start()
    {
        Invoke(nameof(Detonate), m_timeTillExplode);
    }

    private void Detonate()
    {
        if (Vector2.Distance(this.transform.position, PlayerController.Instance.transform.position) <= m_explosionRadius)
            PlayerController.Instance.GetDamage(m_damage, PlayerController.Instance.transform.position - this.transform.position, 500);

        Enemy[] enemies = EnemySpawner.Instance.AllEnemies.ToArray();
        foreach (Enemy e in enemies)
            if (Vector2.Distance(this.transform.position, e.transform.position) <= m_explosionRadius)
                e.GetDamage(m_damage, e.transform.position - this.transform.position, 500);

        CameraScript.Instance.StartCoroutine(CameraScript.Instance.Shake(.4f, .05f));

        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, m_explosionRadius);
    }
}
