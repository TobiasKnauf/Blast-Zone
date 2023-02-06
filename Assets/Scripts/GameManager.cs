using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerInput m_playerInput;

    public Rect MatchField;

    [HideInInspector] public bool IsRunning;
    [HideInInspector] public bool IsPaused;

    public float Tick = 0.1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchAction("Freeze");
    }

    private void Update()
    {
        if (IsRunning)
        {
            if (IsPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        else
            Time.timeScale = 0;
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

    public void OnGameOver()
    {
        IsPaused = true;
        UIManager.Instance.OnGameOver();
        StartCoroutine(EnemySpawner.Instance.DespawnAllEnemies());
        StartCoroutine(CollectibleSpawner.Instance.DespawnAllCollectibles());

        Projectile[] ps = FindObjectsOfType<Projectile>();
        foreach (Projectile p in ps)
            Destroy(p.gameObject);

        Explosive[] es = FindObjectsOfType<Explosive>();
        foreach (Explosive exp in es)
            Destroy(exp.gameObject);

        SwitchAction("Freeze");
    }
    public void OnRestart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(MatchField.center, MatchField.size);
    }
}
