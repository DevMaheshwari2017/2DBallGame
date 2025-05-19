using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    #region Private Serialized Variables
    [SerializeField] private SkinData skinData;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Image skinImg;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI skinName;
    #endregion

    #region Private Variables
    private ShopManager shopManager;
    private TextMeshProUGUI purchasedStatus;
    private SkinPurchasedState skinPurchasedState;
    private const string SkinPrefKeyPrefix = "SkinState_";
    #endregion

    #region Public Variables
    public SkinPurchasedState CurrentState => skinPurchasedState;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        buyBtn.onClick.AddListener(BuyButton);
    }
    private void OnDisable()
    {
        buyBtn.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        purchasedStatus = buyBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (purchasedStatus == null) 
        {
            Debug.LogWarning($"Buy Button of {gameObject} doesn't conatin a text");
        }
        shopManager = GetComponentInParent<ShopManager>();
        if (shopManager == null) 
        {
            Debug.LogWarning($"Parent doesn't contains a ShopManager");
           
        }
        costText.text = skinData.skinCost.ToString();
        skinImg.color = skinData.skinColor;
        skinName.text = skinData.skinName.ToString();

        LoadSkinState();
    }
    #endregion

    #region Public Functions
    public void ForceToSelectState()
    {
        skinPurchasedState = SkinPurchasedState.Select;
        purchasedStatus.text = skinPurchasedState.ToString();
        SaveSkinState();
    }
    #endregion

    #region Private Functions
    private void BuyButton() 
    {
        switch (skinPurchasedState) 
        {
            case SkinPurchasedState.Select:
                skinPurchasedState = SkinPurchasedState.Selected;
                purchasedStatus.text = SkinPurchasedState.Selected.ToString();
                shopManager.ChangePlayerSkin(skinData);
                shopManager.OnSkinSelected(this);
                SaveSkinState();
                break;

            case SkinPurchasedState.Buy:
                if (shopManager.CanPurchaseSkin(skinData))
                {
                    skinPurchasedState = SkinPurchasedState.Select;
                    purchasedStatus.text = SkinPurchasedState.Select.ToString();
                    SaveSkinState();
                }
                break;
            case SkinPurchasedState.Selected:
                skinPurchasedState = SkinPurchasedState.Select;
                purchasedStatus.text = SkinPurchasedState.Select.ToString();
                shopManager.ChangePlayerSkin(skinData, true);
                SaveSkinState();
                break;
        }
    }

    private void SaveSkinState()
    {
        string key = SkinPrefKeyPrefix + skinData.skinName;
        PlayerPrefs.SetInt(key, (int)skinPurchasedState);
        PlayerPrefs.Save();
    }

    private void LoadSkinState()
    {
        string key = SkinPrefKeyPrefix + skinData.skinName;
        if (PlayerPrefs.HasKey(key))
        {
            skinPurchasedState = (SkinPurchasedState)PlayerPrefs.GetInt(key);
            purchasedStatus.text = skinPurchasedState.ToString();
            if (skinPurchasedState == SkinPurchasedState.Selected) 
            {
                shopManager.ChangePlayerSkin(skinData);
            }
        }
        else
        {
            // Default state when game is first run
            skinPurchasedState = SkinPurchasedState.Buy;
            purchasedStatus.text = SkinPurchasedState.Buy.ToString();
        }
    }
    #endregion
}
