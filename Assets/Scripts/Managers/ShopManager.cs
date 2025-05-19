using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    #region Private serialized Variables
    [SerializeField] private GameObject notEnoughCoinMessage;
    [SerializeField] private List<SkinItem> skinItems = new List<SkinItem>();
    [SerializeField] private SelectedSkinDataHolder selectedSkinDataHolder;
    [SerializeField] private SkinData defaultSkinData;
    [SerializeField] private Button crossBtn;
    [SerializeField] private CoinManager coinManager;   
    #endregion

    #region Private Variables
    private const string CoinKey = "PlayerCoinAmount";
    #endregion

    #region Monobeahaviou
    private void OnEnable()
    {
        crossBtn.onClick.AddListener(CloseShop);
    }


    private void OnDisable()
    {
        crossBtn.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        notEnoughCoinMessage.SetActive(false);
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Whenever player select a skin from his purchased skins, all the other skins become select = Unselected
    /// </summary>
    /// <param name="selectedSkin"></param>
    public void OnSkinSelected(SkinItem selectedSkin)
    {
        foreach (var item in skinItems)
        {
            if (item != selectedSkin && item.CurrentState == SkinPurchasedState.Selected)
            {
                item.ForceToSelectState();
            }
        }
    }
    /// <summary>
    /// Checks is user has enough coins to purchase skins
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool CanPurchaseSkin(SkinData data) 
    {
        int currentCoins = PlayerPrefs.GetInt(CoinKey);

        if (currentCoins >= data.skinCost)
        {   
            coinManager.CoinSpend(data.skinCost);
            // need to notify coin ui
            return true;
        }
        else 
        {
            StartCoroutine(ShowNotEnoughCoinMessage());
            return false;
        }
            
    }

    /// <summary>
    /// Chnage player skin based to deafult or another color based on the skindata and optional bool
    /// </summary>
    /// <param name="skinData"></param>
    /// <param name="toDefault"></param>
    public void ChangePlayerSkin(SkinData skinData, bool toDefault = false) 
    {
        if (toDefault)
        {
            selectedSkinDataHolder.selectedSkin = defaultSkinData;
        }
        else
        {
            selectedSkinDataHolder.selectedSkin = skinData;
        }
    }
    #endregion

    #region Private Functions
    /// <summary>
    /// Shows Not enough coin error message for 2 sec
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowNotEnoughCoinMessage() 
    {
        notEnoughCoinMessage.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        notEnoughCoinMessage.SetActive(false);
    }
    private void CloseShop()
    {
        gameObject.SetActive(false);
    }

    #endregion
}

/// <summary>
/// Enum which tracks the state of skins
/// </summary>
public enum SkinPurchasedState 
{
    Buy,
    Purchased,
    Selected,
    Select
}
