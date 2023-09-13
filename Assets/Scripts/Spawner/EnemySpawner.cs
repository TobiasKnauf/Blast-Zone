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

        m_timeUntilNextSpawn -= Time.deltaTime * GameManager.Instance.TimeSinceStart / 10000f;

        if (m_timeUntilNextSpawn < .25f)
            m_timeUntilNextSpawn = .25f;
    }

    protected override bool IsNonValidPosition(Vector2 _pos)
    {
        if (Vector2.Distance(spawnPos, PlayerController.Instance.transform.position) > (25f * Mathf.Clamp((GameManager.Instance.TimeSinceStart / 300f), 1f, 3f)))
            return true;

        return base.IsNonValidPosition(_pos);
    }

    protected override void CreateEntity()
    {
        StartCoroutine(StartEnemySpawn(spawnPos));
    }

    private IEnumerator StartEnemySpawn(Vector2 _pos)
    {
        int rnd = Random.Range(0, m_entitiesPrefab.Length);

        if (Random.Range(0, 1f) > m_entitiesPrefab[rnd].Stats.SpawnChance)
        {
            StartCoroutine(StartEnemySpawn(_pos));
            yield break;
        }

        Animator a = Instantiate(m_spawnPointIdicator, m_spawnPointCollector);
        a.transform.position = _pos;

        yield return new WaitForSeconds(3f);

        Destroy(a.gameObject);
        Enemy e = Instantiate(m_entitiesPrefab[rnd]);
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
