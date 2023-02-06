using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject m_playButton;
    [SerializeField] private GameObject m_restartButton;

    private void Awake()
    {
        Instance = this;
    }

    public void OnGameStart()
    {
        m_playButton.SetActive(false);
    }
    public void OnGameOver()
    {
        m_restartButton.SetActive(true);
    }
}
