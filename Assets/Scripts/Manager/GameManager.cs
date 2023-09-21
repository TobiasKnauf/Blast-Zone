using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private ScoreOrbs m_scoreOrbsPref;

    public Rect MatchField;

    [HideInInspector] public bool IsRunning;
    [HideInInspector] public bool IsPaused;

    [SerializeField] private WeaponStats[] enemyWeaponStats;
    [SerializeField] private EnemyStats[] enemyStats;

    private float startScoreValue = 15f;
    public float ScoreValue;

    public float TimeSinceStart { get; set; }

    public float Tick = 0.1f;

    public float CurrentScore;

    private Dictionary<EnemyStats, float> enemiesStartHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchAction("Freeze");
        ScoreValue = startScoreValue;

        enemiesStartHealth = new Dictionary<EnemyStats, float>();

        foreach (var enemy in enemyStats)
        {
            enemiesStartHealth.Add(enemy, enemy.MaxHealth);
        }
    }

    private void Update()
    {
        if (IsRunning)
        {
            if (!Application.isFocused)
                UIManager.Instance.OnPause();

            if (IsPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                TimeSinceStart += Time.deltaTime;
                // CurrentScore += Time.deltaTime/* * PlayerController.Instance.ComboValue*/;

            }
        }
        else
            Time.timeScale = 0;
    }

    public void BuffEnemy()
    {
        foreach (var enemy in enemyStats)
        {
            enemy.MaxHealth += (enemy.MaxHealth / 100) * 25f;

            //foreach (var h in enemiesStartHealth)
            //{
            //    if (h.Key != enemy) continue;

            //    enemy.MaxHealth += h.Value / 2f;
                    
            //}
        }
    }

    public void SwitchAction(string _actionName)
    {
        m_playerInput.SwitchCurrentActionMap(_actionName);
    }

    public void OnStartGame()
    {
        IsRunning = true;
        UIManager.Instance.OnGameStart();
        SwitchAction("Player");
    }
    public void SpawnScoreOrbs(Vector2 _pos)
    {
        int amount = Random.Range(2, 6);
        ScoreOrbs sc;

        for (int i = 0; i < amount; i++)
        {
            sc = Instantiate(m_scoreOrbsPref);
            sc.Init(_pos);
        }
    }
    public void OnGameOver()
    {
        IsPaused = true;
        UIManager.Instance.OnGameOver();

        StartCoroutine(EnemySpawner.Instance.DespawnAllEntities());
        StartCoroutine(CollectibleSpawner.Instance.DespawnAllEntities());
        StartCoroutine(AirstrikeSpawner.Instance.DespawnAllEntities());

        Projectile[] ps = FindObjectsOfType<Projectile>();
        foreach (Projectile p in ps)
            Destroy(p.gameObject);

        SwitchAction("Freeze");

        if (PlayerPrefs.GetFloat("Highscore") < CurrentScore)
            PlayerPrefs.SetFloat("Highscore", CurrentScore);
    }
    public void OnRestart()
    {
        foreach (var s in enemyStats)
        {
            s.ResetStats();
        }
        foreach (var w in enemyWeaponStats)
        {
            w.ResetStats();
        }
        ScoreValue = startScoreValue;
        PlayerController.Instance.PlayerStats.ResetStats();
        PlayerController.Instance.weaponStats.ResetStats();
        UpgradeManager.Instance.ResetUpgrades();
        StartCoroutine(UIManager.Instance.OnRestart());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(MatchField.center, MatchField.size);
    }
}
