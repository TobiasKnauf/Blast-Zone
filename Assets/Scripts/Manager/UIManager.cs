using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject m_menuPanel;
    [SerializeField] private GameObject m_inGamePanel;
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private GameObject m_pausePanel;

    [SerializeField] private TMP_Text m_scoreText;
    [SerializeField] private TMP_Text m_pauseScore;

    [SerializeField] private Animator m_chargeAnim;
    [SerializeField] private Image m_chargeImage;

    [SerializeField] private TMP_Text m_comboText;
    [SerializeField] private Animator m_comboAnim;

    [SerializeField] private TMP_Text m_endScore;
    [SerializeField] private TMP_Text m_highScore;

    [SerializeField] private Animator m_resumeButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Highscore"))
            m_highScore.text = "Best: " + PlayerPrefs.GetFloat("Highscore").ToString("F0");
        else
            m_highScore.text = "No Score";
    }

    private void Update()
    {
        m_scoreText.text = "" + GameManager.Instance.CurrentScore.ToString("F0");
    }

    #region Charge
    public void AddCharge(float _value)
    {
        float newVal = PlayerController.Instance.ChargeValue + _value;

        if (newVal > 100f)
            newVal = 100f;

        if (newVal > 50)
            m_chargeImage.color = Color.yellow;
        if (newVal > 75)
            m_chargeImage.color = Color.red;

        m_chargeImage.fillAmount = newVal / 100f;
    }
    public void ResetCharge()
    {
        m_chargeImage.color = Color.white;
        m_chargeImage.fillAmount = 0f;
    }
    #endregion

    public void AddCombo(float _value)
    {
        float newVal = PlayerController.Instance.ComboValue + _value;

        if (newVal < 1)
            newVal = 1;
        if (newVal > 10f)
            newVal = 10f;

        if (newVal > 2f)
            m_comboText.color = Color.yellow;
        if (newVal > 5f)
            m_comboText.color = Color.red;

        m_comboText.text = "x" + newVal.ToString("F1");
        m_comboAnim.SetTrigger("AddCharge");
    }
    public void ResetCombo()
    {
        m_comboText.color = Color.white;
        m_comboText.text = "x" + PlayerController.Instance.ComboValue.ToString("F1");
    }

    public void OnGameStart()
    {
        m_menuPanel.SetActive(false);
        m_inGamePanel.SetActive(true);
    }
    public void OnGameOver()
    {
        m_inGamePanel.SetActive(false);
        m_pausePanel.SetActive(false);
        m_gameOverPanel.SetActive(true);

        m_endScore.text = "Score: " + GameManager.Instance.CurrentScore.ToString("F0");
    }
    public void OnPause()
    {
        if (!GameManager.Instance.IsRunning) return;

        if (GameManager.Instance.IsPaused) return;

        m_resumeButton.transform.localScale = Vector3.one;
        m_resumeButton.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        GameManager.Instance.IsPaused = true;
        m_pausePanel.SetActive(true);
        m_inGamePanel.SetActive(false);
        m_pauseScore.text = "Score: " + GameManager.Instance.CurrentScore.ToString("F0");
        GameManager.Instance.SwitchAction("Freeze");
    }
    public void OnResume()
    {
        if (!GameManager.Instance.IsRunning) return;

        if (!GameManager.Instance.IsPaused) return;

        m_resumeButton.transform.localScale = Vector3.one;
        m_resumeButton.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        GameManager.Instance.IsPaused = false;
        m_pausePanel.SetActive(false);
        m_inGamePanel.SetActive(true);
        GameManager.Instance.SwitchAction("Player");
    }
}
