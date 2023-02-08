using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsManager : MonoBehaviour
{
    public static VisualsManager Instance;

    [SerializeField] private ParticleSystem m_deathParticles;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDeathParticles(Vector2 _pos, Vector2 _splashDir)
    {
        ParticleSystem p = Instantiate(m_deathParticles);
        p.transform.position = _pos;
        p.transform.rotation = Quaternion.FromToRotation(Vector2.up, _splashDir);
    }
}
