using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private int gameScene = 1;
    private void OnEnable()
    {
        playBtn.onClick.AddListener(Play);
        quitBtn.onClick.AddListener(Quit);
        shopBtn.onClick.AddListener(Shop);
    }

    private void OnDisable()
    {
        playBtn.onClick.RemoveAllListeners();
        quitBtn.onClick.RemoveAllListeners();
        shopBtn.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        shopPanel.SetActive(false);
    }
    private void Play() 
    {
        SceneManager.LoadScene(gameScene);
    }
    private void Quit() 
    {
        Application.Quit();
    }    
    private void Shop() 
    {
        shopPanel.SetActive(true);
    }
}
