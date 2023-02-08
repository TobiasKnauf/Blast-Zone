using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private float damage;
    private int noDmgLayer;

    public void Launch(float _force, Vector2 _dir, float _damage, int _noDmgLayer)
    {
        damage = _damage;
        noDmgLayer = _noDmgLayer;
        transform.up = _dir;
        rb.AddForce(transform.up * _force, ForceMode2D.Impulse);

        Invoke(nameof(Destroy), 5f);
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == noDmgLayer) return;
        if (_other.CompareTag("Border"))
        {
            Destroy();
            return;
        }

        IKillable obj = _other.GetComponent<IKillable>();

        if (obj != null)
        {
            obj.GetDamage(damage, rb.velocity, 750);
            UIManager.Instance.AddCombo(.1f);
            PlayerController.Instance.ComboValue += .1f;
            SoundManager.Instance.PlaySound(ESound.Hit);
            Destroy(this.gameObject);
        }
    }
    private void Destroy()
    {
        UIManager.Instance.AddCombo(-.2f);
        PlayerController.Instance.ComboValue -= .2f;
        Destroy(this.gameObject);
    }
}
