using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float m_damagePerTick;    

    private void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player")) return;

        IKillable obj = _other.GetComponent<IKillable>();

        if (obj != null)
        {
            obj.GetDamage(m_damagePerTick, transform.right, 50);
            UIManager.Instance.AddCombo(.005f);
            PlayerController.Instance.ComboValue += .005f;
        }
    }
}
