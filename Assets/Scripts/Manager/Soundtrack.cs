using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.sceneLoaded += OnSceneLoad;

        StartCoroutine(StartMusic());
    }

    private void OnSceneLoad(Scene _scene, LoadSceneMode _lsm)
    {
        StartCoroutine(StartMusic());
    }

    public IEnumerator StartMusic()
    {
        m_musicSource.volume = m_startVolume;

        if (!m_musicSource.isPlaying)
            m_musicSource.Play();

        while (m_musicSource.volume < 1)
        {
            m_musicSource.volume += Time.deltaTime / m_fadeDuration;
            yield return null;
        }
    }
}
