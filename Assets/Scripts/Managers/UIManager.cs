using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Private Seriliazed Variable
    [SerializeField] private int mainMenuScene = 0;

    [Space(5)]
    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pausePanelHomeBtn;
    [SerializeField] private Button pausePanelResumeBtn;
    [SerializeField] private Button pausePanelRestartBtn;
    [SerializeField] private Button pausePanelQuitBtn;
    [SerializeField] private Button pausePanelCrossBtn;

    [Space(5)]
    [Header("GameOver Panel")]
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private Button gameoverHomeBtn;
    [SerializeField] private Button gameoverRestartBtn;

    [Space(5)]
    [Header("Score")]
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Space(5)]
    [Header("Coin")]
    [SerializeField] private TextMeshProUGUI coinCollectedText;
    #endregion

    #region Private Seriliazed Variable
    private bool isGameOver = false;
    #endregion

    #region Monobehaviour Functions
    private void OnEnable()
    {
        // game over panel btns
        gameoverHomeBtn.onClick.AddListener(Home);
        gameoverRestartBtn.onClick.AddListener(Restart);

        //Pause panel btns
        pausePanelHomeBtn.onClick.AddListener(Home);
        pausePanelRestartBtn.onClick.AddListener(Restart);
        pausePanelQuitBtn.onClick.AddListener(Quit);
        pausePanelResumeBtn.onClick.AddListener(PauseAndResume);
        pausePanelCrossBtn.onClick.AddListener(CrossBtn);
    }


    private void OnDisable()
    {
        // game over panel
        gameoverHomeBtn.onClick.RemoveAllListeners();
        gameoverRestartBtn.onClick.RemoveAllListeners();

        //Pause panel btns
        pausePanelHomeBtn.onClick.RemoveAllListeners();
        pausePanelRestartBtn.onClick.RemoveAllListeners();
        pausePanelQuitBtn.onClick.RemoveAllListeners();
        pausePanelResumeBtn.onClick.RemoveAllListeners();
        pausePanelCrossBtn.onClick.RemoveAllListeners();


    }
    private void Awake()
    {
        scorePanel.SetActive(true);
        gameoverPanel.SetActive(false);
        pausePanel.SetActive(false);
        isGameOver = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseAndResume();
        }
    }
    #endregion

    #region Private Functions
    private void Quit()
    {
        Application.Quit();
    }

    private void PauseAndResume() 
    {
        if (!isGameOver)
        {
            bool isPausing = Time.timeScale == 1f;
            Time.timeScale = isPausing ? 0f : 1f;
            pausePanel.SetActive(isPausing);
        }
    }

    private void CrossBtn() 
    {
        PauseAndResume();
        EventSystem.current.SetSelectedGameObject(null);
    } 
    private void Restart() 
    {
        Time.timeScale = 1f;
        gameoverPanel.SetActive(false);
        pausePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloading scene 
    }

    private void Home() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
    #endregion

    #region Public Functions
    public void GameOver(bool playDeathAnim = false) 
    {
        isGameOver=true;
        if (playDeathAnim) 
        {
            StartCoroutine(GameService.Instance.GetPlayerController().PlayDeathAnimation());
        }
        else
        {
            ExecuteGameOverLogic();
        }
    }

    public void ExecuteGameOverLogic() 
    {
        Time.timeScale = 0f;
        gameoverPanel.SetActive(true);
    }
    public void AddScore(int amount) 
    {
        scoreText.text = "Score: " + amount.ToString();
    }

    public void CoinCollected(int amount) 
    {
        coinCollectedText.text = "Coin Collected: " + amount.ToString();
    }
    #endregion

    #region Getter

    public bool GetIsGameOver() => isGameOver;
    #endregion
}
