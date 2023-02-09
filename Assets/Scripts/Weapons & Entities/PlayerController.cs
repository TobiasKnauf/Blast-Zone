using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour, IKillable
{
    public static PlayerController Instance;

    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_dashSpeed;
    [SerializeField] private float m_maxHealth;

    [SerializeField] private float m_dashDuration;
    [SerializeField] private float m_laserBeamDuration;
    [SerializeField] private float m_shootCooldown;
    [SerializeField] private float m_dashCooldown;

    [SerializeField] private Projectile m_projectilePref;
    [SerializeField] private ParticleSystem m_dashTrail;
    [SerializeField] private LaserBeam m_laserBeam;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private Vector2 mousePos;
    private Vector2 moveVector;
    private Vector2 dashDir;

    private float health;
    private float shootTimer = 10000;
    private float dashTimer = 1000;
    private float timeSinceDash = 0;
    private float comboDecreaseTimer = 0;

    private bool isDashing;
    [HideInInspector] public bool IsBeaming;
    private bool heldShooting;

    public float ChargeValue;
    public float ComboValue;


    private void Awake()
    {
        Instance = this;
        health = m_maxHealth;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        if (heldShooting)
            Shoot();

        if (ChargeValue >= 100f)
            StartCoroutine(StartLaserBeam());

        KeepPlayerInBounds();

        if (comboDecreaseTimer > 2f)
        {
            ComboValue -= .05f;
            comboDecreaseTimer = 0;
        }

        if (ComboValue < 1)
            ComboValue = 1;
        if (ComboValue > 10)
            ComboValue = 10;

        // Timer increase
        shootTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        comboDecreaseTimer += Time.deltaTime;
    }


    private void FixedUpdate()
    {
        Move();

        UpdateDash();
    }

    #region InputSystem
    public void OnMove(CallbackContext _ctx)
    {
        moveVector = _ctx.ReadValue<Vector2>();
    }
    public void OnFire(CallbackContext _ctx)
    {
        if (_ctx.started)
            heldShooting = true;
        if (_ctx.canceled)
            heldShooting = false;
    }
    public void OnDash(CallbackContext _ctx)
    {
        StartDash();
    }
    public void OnLook(CallbackContext _ctx)
    {
        if (isDashing) return;
        if (GameManager.Instance.IsPaused) return;

        mousePos = Camera.main.ScreenToWorldPoint(_ctx.ReadValue<Vector2>());

        Vector2 direction = new(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y);
        transform.up = direction;
    }
    public void OnPause(CallbackContext _ctx)
    {
        UIManager.Instance.OnPause();
    }
    public void OnResume(CallbackContext _ctx)
    {
        UIManager.Instance.OnResume();
    }
    #endregion

    #region Combat
    private void Shoot()
    {
        if (IsBeaming) return;
        if (isDashing) return;
        if (shootTimer < m_shootCooldown) return;

        Projectile p = Instantiate(m_projectilePref);
        p.transform.position = this.transform.position;
        p.Launch(20, transform.up, 25f, this.gameObject.layer);
        SoundManager.Instance.PlaySound(ESound.Shoot);

        shootTimer = 0;
    }
    private IEnumerator StartLaserBeam()
    {
        IsBeaming = true;
        SoundManager.Instance.PlaySound(ESound.Laser);
        ChargeValue = 0;
        //UIManager.Instance.ResetCharge();
        StartCoroutine(UIManager.Instance.ResetChargeBar());
        m_laserBeam.gameObject.SetActive(true);
        yield return new WaitForSeconds(m_laserBeamDuration);
        m_laserBeam.gameObject.SetActive(false);
        IsBeaming = false;
    }
    #endregion

    #region Movement
    private void KeepPlayerInBounds()
    {
        if (this.transform.position.x < GameManager.Instance.MatchField.xMin)
            this.transform.position = new Vector2(GameManager.Instance.MatchField.xMin, this.transform.position.y);
        if (this.transform.position.x > GameManager.Instance.MatchField.xMax)
            this.transform.position = new Vector2(GameManager.Instance.MatchField.xMax, this.transform.position.y);
        if (this.transform.position.y < GameManager.Instance.MatchField.yMin)
            this.transform.position = new Vector2(this.transform.position.x, GameManager.Instance.MatchField.yMin);
        if (this.transform.position.y > GameManager.Instance.MatchField.yMax)
            this.transform.position = new Vector2(this.transform.position.x, GameManager.Instance.MatchField.yMax);
    }
    private void Move()
    {
        if (isDashing) return;                       // return if is dashing

        rb.velocity = m_moveSpeed * Time.fixedDeltaTime * moveVector;
    }
    private void StartDash()
    {
        if (IsBeaming) return;
        if (isDashing) return;                      // return if is already dashing
        if (dashTimer < m_dashCooldown) return;     // return if dash is on cooldown
        if (moveVector == Vector2.zero) return;     // return if player is not moving

        m_dashTrail.Play();
        dashDir = moveVector;
        timeSinceDash = 0;
        isDashing = true;

    }
    private void UpdateDash()
    {
        timeSinceDash += Time.deltaTime;

        if (timeSinceDash >= m_dashDuration)
        {
            m_dashTrail.Stop();
            isDashing = false;
            col.enabled = true;
            return;
        }

        col.enabled = false;
        rb.velocity = m_dashSpeed * Time.fixedDeltaTime * dashDir;
        dashTimer = 0;

    }
    #endregion

    public void GetDamage(float _value, Vector2 _dir, float _knockbackForce)
    {
        health -= _value;
        if (health <= 0)
        {
            Die(_dir);
            return;
        }
    }
    public void Die(Vector2 _dir, bool _spawnOrbs = false)
    {
        m_laserBeam.gameObject.SetActive(false);
        m_dashTrail.gameObject.SetActive(false);

        VisualsManager.Instance.PlayDeathParticles(this.transform.position, _dir);
        this.GetComponent<SpriteRenderer>().enabled = false;
        SoundManager.Instance.StopSound(ESound.Laser);
        SoundManager.Instance.PlaySound(ESound.Hit);
        GameManager.Instance.OnGameOver();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        Collectible obj = _other.GetComponent<Collectible>();

        if (obj != null)
            obj.PickUp();
    }
}
