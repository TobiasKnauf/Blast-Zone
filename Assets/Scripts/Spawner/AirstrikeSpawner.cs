using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeSpawner : Spawner<Airstrike>
{
    protected override void Update()
    {
        base.Update();

        m_timeUntilNextSpawn = 4 - (.125f * (PlayerController.Instance.currentLevel + 1));

        if (m_timeUntilNextSpawn < 2f)
            m_timeUntilNextSpawn = 2f;


    }
    protected override bool IsNonValidPosition(Vector2 _pos)
    {
        if (Vector2.Distance(spawnPos, PlayerController.Instance.transform.position) > 5f)
            return true;

        return base.IsNonValidPosition(_pos);
    }

    protected override void CreateEntity()
    {
        Airstrike obj = Instantiate(m_entitiesPrefab[Random.Range(0, m_entitiesPrefab.Length)]);

        obj.Init(spawnPos);
        Subscribe(obj);
    }

    protected override void DeInitEntity(Airstrike _entity)
    {
        UnSubscribe(_entity);
        Destroy(_entity.gameObject);
    }
}
