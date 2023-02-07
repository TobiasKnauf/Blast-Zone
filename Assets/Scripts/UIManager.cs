using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject m_playButton;
    [SerializeField] private GameObject m_restartButton;
    [SerializeField] private TMP_Text m_scoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        m_scoreText.text = "Score: " + GameManager.Instance.ConvertScoreToInt();
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
