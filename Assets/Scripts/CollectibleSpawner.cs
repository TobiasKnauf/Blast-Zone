using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public static CollectibleSpawner Instance;

    public List<Collectible> AllCollectibles;

    [SerializeField] private Collectible[] m_collectiblePref;
    [SerializeField] private float m_cooldown;

    private float timer;

    private void Awake()
    {
        Instance = this;
        AllCollectibles = new List<Collectible>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        if (timer > m_cooldown)
            SpawnCollectible();

        timer += Time.deltaTime;
    }

    private void SpawnCollectible()
    {
        Collectible g = Instantiate(m_collectiblePref[UnityEngine.Random.Range(0, m_collectiblePref.Length)]);
        g.transform.position = GetRandomPosInsideArea();
        timer = 0;
    }

    private Vector2 GetRandomPosInsideArea()
    {
        return new Vector2(
            UnityEngine.Random.Range(GameManager.Instance.MatchField.xMin, GameManager.Instance.MatchField.xMax),
            UnityEngine.Random.Range(GameManager.Instance.MatchField.yMin, GameManager.Instance.MatchField.yMax));
    }

    public IEnumerator DespawnAllCollectibles()
    {
        Collectible[] temp = AllCollectibles.ToArray();

        float time = .35f;

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == null) continue;

            float timeDecrease = 0.05f * i;

            temp[i].Destroy();
            yield return new WaitForSecondsRealtime(time - timeDecrease);
        }
    }

    public void Subscribe(Collectible _c)
    {
        AllCollectibles.Add(_c);
    }
    public void UnSubscribe(Collectible _c)
    {
        if (AllCollectibles.Contains(_c))
            AllCollectibles.Remove(_c);
    }
}
