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

    /// <summary>
    /// Whenever player collects a coin we add it to playerprefs
    /// </summary>
    public void CoinCollected() 
    {
        currentCoint += amountToIncrease;
        totalCoinAmount += amountToIncrease;
        GameService.Instance.GetUIManager().CoinCollected(currentCoint);
        PlayerPrefs.SetInt(CoinKey, totalCoinAmount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Whenever player spends some coins we update playerprefs
    /// </summary>
    public void CoinSpend(int amountSpend) 
    {
        totalCoinAmount -= amountSpend;
        PlayerPrefs.SetInt(CoinKey, totalCoinAmount);
        PlayerPrefs.Save();
        UpdateTotalCoinUI();
    }

    /// <summary>
    /// Updating tottal coin ui in main menu scene
    /// </summary>
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
