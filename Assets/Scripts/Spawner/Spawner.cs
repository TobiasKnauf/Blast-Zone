using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour
{
    public static Spawner<T> Instance { get; private set; } 

    [SerializeField] protected T[] m_entitiesPrefab;
    [HideInInspector] public List<T> AllEntities { get; private set; }
    [SerializeField] protected float m_timeUntilNextSpawn;
    [SerializeField] protected Rect[] m_noSpawnAreas;
    [SerializeField] protected float m_noSpawnRadiusFromPlayer;


    protected Vector2 spawnPos;
    protected float timer;

    protected virtual void Awake()
    {
        Instance = this;
        AllEntities = new List<T>();
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        if (m_entitiesPrefab.Length == 0) return;

        if (timer > m_timeUntilNextSpawn)
            SpawnEntity();

        timer += Time.deltaTime;
    }

    protected virtual void SpawnEntity()
    {
        timer = 0;

        spawnPos = GetRandomPosInsideArea();

        while (IsNonValidPosition(spawnPos))
            spawnPos = GetRandomPosInsideArea();

        CreateEntity();
    }

    protected abstract void CreateEntity();

    protected Vector2 GetRandomPosInsideArea()
    {
        return new Vector2(
            UnityEngine.Random.Range(GameManager.Instance.MatchField.xMin, GameManager.Instance.MatchField.xMax),
            UnityEngine.Random.Range(GameManager.Instance.MatchField.yMin, GameManager.Instance.MatchField.yMax));
    }
    protected virtual bool IsNonValidPosition(Vector2 _pos)
    {
        foreach (Rect r in m_noSpawnAreas)
            if (r.Contains(_pos))
                return true;

        return Vector2.Distance(_pos, PlayerController.Instance.transform.position) < m_noSpawnRadiusFromPlayer;
    }

    public virtual IEnumerator DespawnAllEntities()
    {
        T[] temp = AllEntities.ToArray();

        float time = .35f;

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == null) continue;

            float timeDecrease = 0.05f * i;

            DeInitEntity(temp[i]);
            yield return new WaitForSecondsRealtime(time - timeDecrease);
        }
    }

    protected abstract void DeInitEntity(T _entity);

    public void Subscribe(T _entity)
    {
        AllEntities.Add(_entity);
    }
    public void UnSubscribe(T _entity)
    {
        if (AllEntities.Contains(_entity))
            AllEntities.Remove(_entity);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, .25f);
        if (PlayerController.Instance != null)
            Gizmos.DrawSphere(PlayerController.Instance.transform.position, m_noSpawnRadiusFromPlayer);

        foreach (Rect r in m_noSpawnAreas)
        {
            Gizmos.DrawCube(r.center, r.size);
        }
    }
}
