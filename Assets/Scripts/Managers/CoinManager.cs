using UnityEngine;

public class CoinManager : MonoBehaviour
{
    #region Priavte Serialized Variables

    [SerializeField] private int amountToIncrease;

    #endregion

    #region Private variables

    private const string CoinKey = "PlayerCoinAmount";

    #endregion

    #region Public Variables

    public static int coinAmount;

    #endregion
    private void Awake()
    {
        coinAmount = PlayerPrefs.GetInt(CoinKey,0);
    }
    #region Public Functions

    public void CoinCollected() 
    {
        coinAmount += amountToIncrease;
        PlayerPrefs.SetInt(CoinKey, coinAmount);
        PlayerPrefs.Save();
    }

    public void CoinSpend(int amountSpend) 
    {
        coinAmount -= amountSpend;
        PlayerPrefs.SetInt(CoinKey, coinAmount);
        PlayerPrefs.Save();
    }
    #endregion
}
