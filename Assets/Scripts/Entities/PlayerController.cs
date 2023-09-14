using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour, IKillable
{
    public static PlayerController Instance;

    public PlayerStats PlayerStats;

    [SerializeField] private float m_dashSpeed;
    [SerializeField] private float m_dashDuration;
    [SerializeField] private float m_dashCooldown;

    [SerializeField] public WeaponStats weaponStats;
    [SerializeField] private ParticleSystem m_dashTrail;
    [SerializeField] private LaserBeam m_beam;
    [SerializeField] private SpriteRenderer m_armorSprite;
    [SerializeField] private FloatingDamageText damageText;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private int armor;

    private Vector2 mousePos;
    private Vector2 moveVector;
    private Vector2 dashDir;

    private float shootTimer = 10000;
    private float dashTimer = 1000;
    private float timeSinceDash = 0;
    private float comboDecreaseTimer = 0;

    private bool isDashing;
    private bool heldShooting;

    public Action LevelUp;
    public int currentLevel = 0;
    public float ChargeValue;
    public float ComboValue;

    private bool knockback;

    private float immuneTimer;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        if (PlayerStats.Armor > 0)
            m_armorSprite.gameObject.SetActive(true);

        armor = PlayerStats.Armor;

    }
    private void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        if (heldShooting)
            Shoot();

        KeepPlayerInBounds();

        if (comboDecreaseTimer > 1f)
        {
            ComboValue -= (ComboValue / 100f) * 2f;
            UIManager.Instance.AddCombo(-(ComboValue / 100f) * 2f);
            comboDecreaseTimer = 0;
        }

        if (ComboValue < 1)
            ComboValue = 1;
        if (ComboValue > 10)
            ComboValue = 10;

        #region Experience
        if (ChargeValue >= 100f)
        {
            UIManager.Instance.OpenUpgradeMenu();
        }
        #endregion

        // Timer increase
        shootTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        comboDecreaseTimer += Time.deltaTime;
        immuneTimer += Time.deltaTime;
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

        if (weaponStats.weaponType is EWeaponType.Laser)
        {
            if (_ctx.started)
            {
                m_beam.gameObject.SetActive(true);
                m_beam.Launch(-transform.right, this.gameObject.layer);
                SoundManager.Instance.PlaySound(ESound.Laser);
            }
            if (_ctx.canceled)
            {
                m_beam.gameObject.SetActive(false);
                SoundManager.Instance.StopSound(ESound.Laser);
            }
        }
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
        if (isDashing) return;
        if (shootTimer < weaponStats.ReloadTime) return;
        if (weaponStats.weaponType is EWeaponType.Laser) return;

        int leftmost = -Mathf.FloorToInt(weaponStats.NumberOfProjectiles / 2f);
        int rightmost = weaponStats.NumberOfProjectiles - Mathf.FloorToInt(weaponStats.NumberOfProjectiles / 2f);

        for (int i = leftmost; i < rightmost; i++)
        {
            Weapon w = Instantiate(weaponStats.Weapon);
            w.transform.up = transform.up;
            // set position
            w.transform.position = this.transform.position;
            w.transform.position += w.transform.up * .75f;

            if (weaponStats.NumberOfProjectiles % 2 == 0)
                w.transform.position += w.transform.right * (i + 0.5f);
            else
                w.transform.position += w.transform.right * i;

            Vector3 direction;

            if (weaponStats.NumberOfProjectiles % 2 == 0)
            {
                if (i >= 0)
                    direction = Quaternion.Euler(0, 0, -(i + 1) * 20f) * transform.up;
                else
                    direction = Quaternion.Euler(0, 0, -i * 20f) * transform.up;
            }
            else
                direction = Quaternion.Euler(0, 0, -i * 20f) * transform.up;

            w.Launch(direction, this.gameObject.layer);
            SoundManager.Instance.PlaySound(ESound.Shoot);
        }

        shootTimer = 0;
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
        if (knockback) return;

        rb.velocity = PlayerStats.MoveSpeed * Time.fixedDeltaTime * moveVector;
    }
    private void StartDash()
    {
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

        PlayerStats.Health -= _value;
        if (PlayerStats.Health <= 0)
        {
            Die(_dir);
            return;
        }

        immuneTimer = 0;
        StartCoroutine(StartKnockback(_dir, _knockbackForce));
    }
    private IEnumerator StartKnockback(Vector2 _dir, float _force)
    {
        knockback = true;
        rb.AddForce(_dir.normalized * _force * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.05f);
        knockback = false;
    }
    public void Die(Vector2 _dir, bool _spawnOrbs = false)
    {
        //m_laserBeam.gameObject.SetActive(false);
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

    public void ApplyUpgrade(PlayerUpgrade _upgrade)
    {
        foreach (var upgrade in _upgrade.Upgrades)
        {
            switch (upgrade.Type)
            {
                case EPlayerUpgradeTypes.Health:
                    PlayerStats.MaxHealth += upgrade.Amount;
                    PlayerStats.Health = PlayerStats.MaxHealth;
                    break;
                case EPlayerUpgradeTypes.Armor:
                    PlayerStats.Armor += (int)upgrade.Amount;
                    break;
                case EPlayerUpgradeTypes.Movement_Speed:
                    PlayerStats.MoveSpeed += (PlayerStats.MoveSpeed / 100) * upgrade.Amount;
                    break;
                case EPlayerUpgradeTypes.Crit_Chance:
                    PlayerStats.CritChance = upgrade.Amount;
                    break;
                case EPlayerUpgradeTypes.Crit_Multiplier:
                    PlayerStats.CritMulitplier += (PlayerStats.CritMulitplier / 100) * upgrade.Amount;
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetCharge()
    {
        currentLevel++;
        LevelUp?.Invoke();
        UIManager.Instance.ResetChargeBarInstant();
        ChargeValue = 0;

        if (currentLevel % 2 == 0)
            GameManager.Instance.BuffEnemy();
    }
}
