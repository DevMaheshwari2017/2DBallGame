using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    #region Priavte Serialized Variables

    [SerializeField] private int amountToIncrease;
    [SerializeField] private TextMeshProUGUI totalCoinText;
    #endregion

    #region Private variables
    private int mainMenuScene = 0;
    private int currentCoint = 0;
    private const string CoinKey = "PlayerCoinAmount";

    #endregion

    #region Public Variables

    public static int totalCoinAmount;

    #endregion
    private void Awake()
    {
        currentCoint = 0;
        totalCoinAmount = PlayerPrefs.GetInt(CoinKey,0);
        UpdateTotalCoinUI();
    }
    #region Public Functions

    public void CoinCollected() 
    {
        currentCoint += amountToIncrease;
        totalCoinAmount += amountToIncrease;
        GameService.Instance.GetUIManager().CoinCollected(currentCoint);
        PlayerPrefs.SetInt(CoinKey, totalCoinAmount);
        PlayerPrefs.Save();
    }

    public void CoinSpend(int amountSpend) 
    {
        totalCoinAmount -= amountSpend;
        PlayerPrefs.SetInt(CoinKey, totalCoinAmount);
        PlayerPrefs.Save();
        UpdateTotalCoinUI();
    }

    private void UpdateTotalCoinUI() 
    {
        var scene = SceneManager.GetActiveScene();
        if (mainMenuScene == scene.buildIndex) 
        {
            totalCoinText.text = "Total Coins: " + totalCoinAmount;
        }
    }
    #endregion
}
