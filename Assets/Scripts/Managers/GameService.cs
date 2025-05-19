using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    /// <summary>
    /// A ServiceLocator derived from GenericMonoSingleton to access all the controllers/Managers/Service from a single script - good for small level project like this
    /// </summary>
    #region Private Serialized Variables
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private CoinManager coinManager;

    #endregion

    #region Private variables
    private PlayerController playerController;
    #endregion

    #region Getters

    public UIManager GetUIManager() => uiManager;
    public PlayerController GetPlayerController() => playerController;
    public ScoreManager GetScoreManager() => scoreManager;
    public CoinManager GetCoinManager() => coinManager;

    #endregion

    #region Setters
    public void SetPlayerController(PlayerController _playerController) 
    {
        playerController = _playerController;
    }
    #endregion
}
