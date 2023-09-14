using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text floatingDamage;
    [SerializeField] private float fadeTime;

    private float fadeTimer;

    public void ShowDamage(float _amount, bool _crit, Vector2 _pos)
    {
        this.transform.position = _pos;
        this.transform.position += Vector3.up * .5f;
        floatingDamage.text = $"{(int)_amount}";
        Invoke(nameof(Destroy), 3f);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
