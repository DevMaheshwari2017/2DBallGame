using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    /// <summary>
    /// This controls all the collectable items like coin and perform certain action when player collides and collect the object
    /// </summary>
    [SerializeField] private bool isCoin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (isCoin) 
            {
                GameService.Instance.GetCoinManager().CoinCollected();
                GameService.Instance.GetScoreManager().AddCoinScore();
                Destroy(gameObject);
            }
        }
    }
}
