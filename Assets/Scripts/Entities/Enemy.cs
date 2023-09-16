using Cinemachine;
using TMPro;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IKillable
{
    public EnemyStats Stats;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer m_armorSprite;
    [SerializeField] private Animator anim;
    [SerializeField] protected Explosion explosion;
    [SerializeField] private Slowpool slowPool;
    [SerializeField] private FloatingDamageText damageText;
    protected float health;
    protected int armor;

    public float MoveSpeed;

    protected Vector2 dir;

    private float immuneTimer;

    [SerializeField] private CinemachineImpulseSource impulseSource;

    protected virtual void Start()
    {
        if (Stats.Armor > 0)
            m_armorSprite.gameObject.SetActive(true);

        armor = Stats.Armor;
    }

    public void Init(Vector2 _spawnPos)
    {
        this.transform.position = _spawnPos;
        GetComponent<BoxCollider2D>().isTrigger = true;
        health = Stats.MaxHealth;
        MoveSpeed = Stats.MoveSpeed;

        Invoke(nameof(EnableCollider), .25f);
    }
    protected abstract void UpdateEnemy();
    private void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        dir = (PlayerController.Instance.transform.position - this.transform.position).normalized;
        transform.up = dir;

        UpdateEnemy();

        immuneTimer += Time.deltaTime;
    }

    private void EnableCollider()
    {
        GetComponent<Collider2D>().isTrigger = false;
    }

    public virtual void GetDamage(float _value, Vector2 _dir, float _knockbackForce)
    {
        if (immuneTimer < GameManager.Instance.Tick) return;

        if (armor > 0)
        {
            armor--;

            if (armor <= 0)
                m_armorSprite.gameObject.SetActive(false);

            return;
        }

        FloatingDamageText d = Instantiate(damageText);
        d.ShowDamage(_value, false, this.transform.position);

        health -= _value;
        if (health <= 0)
        {
            Die(_dir);
            return;
        }

        anim.SetTrigger("Damage");

        rb.AddForce(_dir.normalized * _knockbackForce * Time.deltaTime, ForceMode2D.Impulse);
        immuneTimer = 0;

    }

    public virtual void OnDeathEffect()
    {
        if (EnemySpawner.Instance.SlowOnKill)
        {
            if (Random.Range(0f, 1f) <= .2f)
            {
                Debug.Log("Slowpool");
                Slowpool s = Instantiate(slowPool);
                s.transform.position = this.transform.position;
            }
        }

        if (EnemySpawner.Instance.ExplodeOnKill)
        {
            if (Random.Range(0f, 1f) <= .75f)
            {
                Debug.Log("Explode");
                Explosion e = Instantiate(explosion);
                e.transform.position = this.transform.position;
                e.Detonate(Stats.MaxHealth / 2f, 3f, false);
            }
        }
    }

    public void Die(Vector2 _dir, bool _spawnOrbs = true)
    {
        if (this == null) return;

        immuneTimer = -1000f;

        OnDeathEffect();

        //CameraScript.Instance.StartCoroutine(CameraScript.Instance.Shake(.1f, .02f));
        CameraScript.Instance.CameraShake(impulseSource, 0.05f);
        VisualsManager.Instance.PlayDeathParticles(this.transform.position, _dir);
        if (_spawnOrbs)
            GameManager.Instance.SpawnScoreOrbs(this.transform.position);
        EnemySpawner.Instance.UnSubscribe(this);

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        IKillable obj = _other.collider.GetComponent<IKillable>();

        if (obj != null && _other.collider.CompareTag("Player"))
            obj.GetDamage(100, transform.up, 400);
    }
}
