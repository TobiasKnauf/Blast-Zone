using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESound
{
    Shoot,
    Laser,
    Hit,
    Explosion,
    Collect
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource m_shootsource;
    [SerializeField] private AudioSource m_laserSource;
    [SerializeField] private AudioSource m_hitsource;
    [SerializeField] private AudioSource m_explosionsource;
    [SerializeField] private AudioSource m_uisource;
    [SerializeField] private AudioSource m_collectsource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(ESound _soundType)
    {

        switch (_soundType)
        {
            case ESound.Shoot:
                m_shootsource.pitch = Random.Range(0.9f, 1.1f);
                m_shootsource.Play();
                break;
            case ESound.Laser:
                m_laserSource.pitch = Random.Range(0.9f, 1.1f);
                m_laserSource.Play();
                break;
            case ESound.Hit:
                m_hitsource.pitch = Random.Range(0.9f, 1.1f);
                m_hitsource.Play();
                break;
            case ESound.Explosion:
                m_explosionsource.pitch = Random.Range(0.9f, 1.1f);
                m_explosionsource.Play();
                break;
            case ESound.Collect:
                if (m_collectsource.isPlaying) break;
                m_collectsource.pitch = Random.Range(0.9f, 1.1f);
                m_collectsource.Play();
                break;
            default:
                break;
        }
    }
    public void StopSound(ESound _soundType)
    {
        switch (_soundType)
        {
            case ESound.Shoot:
                m_shootsource.Stop();
                break;
            case ESound.Laser:
                m_laserSource.Stop();
                break;
            case ESound.Hit:
                m_hitsource.Stop();
                break;
            case ESound.Explosion:
                m_explosionsource.Stop();
                break;
            case ESound.Collect:
                m_collectsource.Stop();
                break;
            default:
                break;
        }
    }
    public void PlayUISound()
    {
        //m_uisource.pitch = Random.Range(0.9f, 1.1f);
        m_uisource.Play();
    }
}
