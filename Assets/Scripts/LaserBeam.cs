using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float m_damagePerTick;

    private void OnTriggerStay2D(Collider2D _other)
    {
        IKillable obj = _other.GetComponent<IKillable>();

        if (obj != null)
            obj.GetDamage(m_damagePerTick, transform.right, 50);
    }
}
