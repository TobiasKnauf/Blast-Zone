using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    public static Soundtrack Instance;
    [SerializeField] private AudioSource m_musicSource;
    [SerializeField] private float m_startVolume;
    [SerializeField] private float m_fadeDuration;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != null && Instance != this)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartMusic());
    }

    private IEnumerator StartMusic()
    {
        m_musicSource.volume = m_startVolume;
        m_musicSource.Play();

        while (m_musicSource.volume < 1)
        {
            m_musicSource.volume += Time.deltaTime / m_fadeDuration;
            yield return null;
        }
    }
}
