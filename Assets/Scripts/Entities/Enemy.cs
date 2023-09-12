using UnityEngine;

public abstract class Enemy : MonoBehaviour, IKillable
{
    [SerializeField] protected float m_moveSpeed;
    [SerializeField] protected float m_maxHealth;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Animator anim;
    protected float health; 

    protected Vector2 dir;

    private float immuneTimer;

    public void Init(Vector2 _spawnPos)
    {
        this.transform.position = _spawnPos;
        GetComponent<BoxCollider2D>().isTrigger = true;
        health = m_maxHealth;

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

    protected abstract void OnDeathEffect();

    public void Die(Vector2 _dir, bool _spawnOrbs = true)
    {
        if (this == null) return;

        immuneTimer = -1000f;

        OnDeathEffect();

        CameraScript.Instance.StartCoroutine(CameraScript.Instance.Shake(.1f, .02f));
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
            obj.GetDamage(100, transform.up, 0);
    }
}
