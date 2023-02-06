using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private PlayerController m_player;

    [Header("Spawn Values")]
    [SerializeField] private float m_timeUntilNextEnemy = 2f;
    [SerializeField] private float m_noSpawnRadius;

    [SerializeField] private Rect[] m_noSpawnAreas;

    [Header("Enemies")]
    [SerializeField] private Enemy[] m_enemiesPref;
    [HideInInspector] public List<Enemy> AllEnemies { get; private set; }

    private float timer;

    private void Awake()
    {
        Instance = this;
        AllEnemies = new List<Enemy>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        if (timer > m_timeUntilNextEnemy)
            SpawnEnemy();

        timer += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        timer = 0;

        Vector2 spawnPos = GetRandomPosInsideArea();

        while (IsNonValidPosition(spawnPos))
            spawnPos = GetRandomPosInsideArea();

        Enemy e = Instantiate(m_enemiesPref[Random.Range(0, m_enemiesPref.Length)]);
        e.Init(spawnPos);
    }

    private bool IsNonValidPosition(Vector2 _pos)
    {
        foreach (Rect r in m_noSpawnAreas)
            if (r.Contains(_pos))
                return true;

        return Vector2.Distance(_pos, m_player.transform.position) < m_noSpawnRadius;
    }

    private Vector2 GetRandomPosInsideArea()
    {
        return new Vector2(
            Random.Range(GameManager.Instance.MatchField.xMin, GameManager.Instance.MatchField.xMax),
            Random.Range(GameManager.Instance.MatchField.yMin, GameManager.Instance.MatchField.yMax));
    }

    public void Subscribe(Enemy _enemy)
    {
        AllEnemies.Add(_enemy);
    }
    public void UnSubscribe(Enemy _enemy)
    {
        if (AllEnemies.Contains(_enemy))
            AllEnemies.Remove(_enemy);
    }

    public IEnumerator DespawnAllEnemies()
    {
        Enemy[] temp = AllEnemies.ToArray();

        float time = .35f;

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == null) continue;

            float timeDecrease = 0.05f * i;

            temp[i].GetDamage(1000, Vector2.zero, 0);
            yield return new WaitForSecondsRealtime(time - timeDecrease);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .25f);
        Gizmos.DrawSphere(m_player.transform.position, m_noSpawnRadius);

        foreach (Rect r in m_noSpawnAreas)
        {
            Gizmos.DrawCube(r.center, r.size);
        }
    }
}
