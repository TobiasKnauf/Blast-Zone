using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : Spawner<Collectible>
{
    protected override void CreateEntity()
    {
        Collectible c = Instantiate(m_entitiesPrefab[UnityEngine.Random.Range(0, m_entitiesPrefab.Length)]);
        c.transform.position = spawnPos;
        Subscribe(c);
    }

    protected override void DeInitEntity(Collectible _entity)
    {
        UnSubscribe(_entity);
        _entity.DeInit();
    }
}
