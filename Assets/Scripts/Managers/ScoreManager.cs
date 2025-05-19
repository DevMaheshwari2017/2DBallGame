using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ScoreManager : MonoBehaviour
{
    #region Private serialized variables
    [SerializeField] private int coinScoreAmount;
    [SerializeField] private float timeScore = 0f;
    [SerializeField] private float scoreMultiplier = 1f;
    [SerializeField] private List<ScoreMultiplierData> scoreMultiplierData = new List<ScoreMultiplierData>();
    #endregion

    #region Private varibales
    private int totalScore;
    private float timePlayed = 0;
    private HashSet<int> triggeredThreshold =  new HashSet<int>();
    private float lastTimeScore;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        ResetScore();
    }
    private void Update()
    {
        timePlayed += Time.deltaTime;
        AddTimeScore();
        CheckTimePlayed();
    }
    #endregion

    #region Private Functions
    /// <summary>
    /// Add time score to total score but only if the time score has an increment of 1 or more
    /// means delta score > 0 
    /// </summary>
    private void AddTimeScore()
    {
        timeScore += Time.deltaTime * scoreMultiplier;
        int deltaScore = (int)(timeScore - lastTimeScore);
        if (deltaScore > 0)
        {
            totalScore += deltaScore;
            UpdateScore(totalScore);
            lastTimeScore = (int)timeScore;
        }
    }

    /// <summary>
    /// Update total score ui
    /// </summary>
    /// <param name="amount"></param>
    private void UpdateScore(int amount) 
    {
        if (GameService.Instance.GetUIManager() != null)
        {
            GameService.Instance.GetUIManager().AddScore(amount);
        }
    }
    /// <summary>
    /// Increased of score multiplier like subway surfer game, where when a ceratin time passes 
    /// the amount at which score increase gets greater and greater.
    /// </summary>
    private void CheckTimePlayed() 
    {

        foreach (var data in scoreMultiplierData) 
        {
            if (timePlayed >= data.timeThreshold && !triggeredThreshold.Contains(data.timeThreshold)) 
            {
                triggeredThreshold.Add(data.timeThreshold);
                scoreMultiplier = data.multiplierValue;
                Debug.Log($"Multiplier updated to {scoreMultiplier} at {data.timeThreshold} seconds.");
            }
        }
    }

    #endregion

    /// <summary>
    /// When a player collects a coin we also increment our score by a certian value
    /// </summary>
    #region Public Functions
    public void AddCoinScore() 
    {
        Debug.Log("Coin collected adding" + coinScoreAmount + " to total score: " + totalScore);
        totalScore += coinScoreAmount;
        UpdateScore(totalScore);
        Debug.Log("Total score is " + totalScore);
    }
    /// <summary>
    /// Reset Score
    /// </summary>
    public void ResetScore() 
    {
        totalScore = 0;
        timePlayed = 0;
        timeScore = 0;
        scoreMultiplier = 1f;
        triggeredThreshold.Clear();
    }
    #endregion
}

[Serializable]
public class ScoreMultiplierData 
{
    public int timeThreshold;
    public float multiplierValue;
}
