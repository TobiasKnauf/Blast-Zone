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

    public float TimeSinceStart { get; private set; }

    public float Tick = 0.1f;

    public float CurrentScore;

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
                CurrentScore += Time.deltaTime * PlayerController.Instance.ComboValue;
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
        StartCoroutine(UIManager.Instance.OnRestart());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(MatchField.center, MatchField.size);
    }
}
