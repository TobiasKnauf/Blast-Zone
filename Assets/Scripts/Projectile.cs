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
        rb.AddForce(_dir * _force, ForceMode2D.Impulse);

        Invoke(nameof(Destroy), 5f);
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == noDmgLayer) return;

        IKillable obj = _other.GetComponent<IKillable>();

        if (obj != null)
            obj.GetDamage(damage, rb.velocity, 500);

        Destroy();
    }
    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
