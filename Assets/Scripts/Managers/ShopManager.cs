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

public enum SkinPurchasedState 
{
    Buy,
    Purchased,
    Selected,
    Select
}
