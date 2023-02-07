using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner<Enemy>
{
    [SerializeField] private Animator m_spawnPointIdicator;
    [SerializeField] private Transform m_spawnPointCollector;
    protected override void Update()
    {
        base.Update();

        m_timeUntilNextSpawn -= Time.deltaTime * GameManager.Instance.TimeSinceStart * 0.0001f;

        if (m_timeUntilNextSpawn < .25f)
            m_timeUntilNextSpawn = .25f;
    }

    protected override void CreateEntity()
    {
        StartCoroutine(StartEnemySpawn(spawnPos));
    }

    private IEnumerator StartEnemySpawn(Vector2 _pos)
    {
        Animator a = Instantiate(m_spawnPointIdicator, m_spawnPointCollector);
        a.transform.position = _pos;

        yield return new WaitForSeconds(3f);

        Destroy(a.gameObject);
        Enemy e = Instantiate(m_entitiesPrefab[Random.Range(0, m_entitiesPrefab.Length)]);
        e.Init(_pos);
        Subscribe(e);
    }

    protected override void DeInitEntity(Enemy _entity)
    {
        _entity.Die(Vector2.zero, false);
        UnSubscribe(_entity);
    }

    public override IEnumerator DespawnAllEntities()
    {
        foreach (Transform t in m_spawnPointCollector)
        {
            Destroy(t.gameObject);
        }

        return base.DespawnAllEntities();
    }
}
