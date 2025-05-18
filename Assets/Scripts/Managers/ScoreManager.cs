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


    private void UpdateScore(int amount) 
    {
        if (GameService.Instance.GetUIManager() != null)
        {
            GameService.Instance.GetUIManager().AddScore(amount);
        }
    }
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

    #region Public Functions
    public void AddCoinScore() 
    {
        Debug.Log("Coin collected adding" + coinScoreAmount + " to total score: " + totalScore);
        totalScore += coinScoreAmount;
        UpdateScore(totalScore);
        Debug.Log("Total score is " + totalScore);
    }
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
