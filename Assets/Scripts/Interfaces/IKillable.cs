using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    public void GetDamage(float _value, Vector2 _dir, float _knockbackForce);
    public void Die(Vector2 _dir);
}
