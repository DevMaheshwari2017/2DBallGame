using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private int gameScene = 1;
    private void OnEnable()
    {
        playBtn.onClick.AddListener(Play);
        quitBtn.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        playBtn.onClick.RemoveAllListeners();
        quitBtn.onClick.RemoveAllListeners();
    }
    private void Play() 
    {
        SceneManager.LoadScene(gameScene);
    }
    private void Quit() 
    {
        Application.Quit();
    }
}
