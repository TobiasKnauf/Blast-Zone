using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOrbs : Collectible
{
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Animator m_anim;

    public void Init(Vector2 _pos)
    {
        CollectibleSpawner.Instance.Subscribe(this);

        transform.position = _pos;
        transform.localScale = Vector2.one * Random.Range(0.2f, 0.4f);
        m_rb.AddForce(Random.insideUnitCircle * Time.deltaTime * 300f, ForceMode2D.Impulse);

        Invoke(nameof(Despawn), 5f);
    }

    public override void PickUp()
    {
        float scoreValue = 15 * transform.localScale.x * PlayerController.Instance.ComboValue;
        //float chargeValue = 7.5f * transform.localScale.x;
        float chargeValue = 7.5f * transform.localScale.x / Mathf.Clamp((GameManager.Instance.TimeSinceStart / 120f), 1, float.MaxValue);


        GameManager.Instance.CurrentScore += scoreValue;

        PlayerController.Instance.ChargeValue += chargeValue;
        UIManager.Instance.AddCharge(chargeValue);

        SoundManager.Instance.PlaySound(ESound.Collect);

        DeInit();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRunning || GameManager.Instance.IsPaused) return;

        float distanceToPlayer = Vector2.Distance(PlayerController.Instance.transform.position, this.transform.position);
        if (distanceToPlayer <= 3f)
        {
            Vector2 dir = PlayerController.Instance.transform.position - this.transform.position;
            m_rb.AddForce(Time.deltaTime * dir * (1 / distanceToPlayer) * 400f, ForceMode2D.Force);
        }
    }

    private void Despawn()
    {
        StartCoroutine(DespawnCor());
    }

    private IEnumerator DespawnCor()
    {
        m_anim.SetTrigger("Despawn");

        const int timeWait = 3;

        for (int i = 0; i < timeWait; i++)
        {
            yield return new WaitForSeconds(i);
            m_anim.speed += 1 / timeWait * 2;
        }

        DeInit();
    }
}
