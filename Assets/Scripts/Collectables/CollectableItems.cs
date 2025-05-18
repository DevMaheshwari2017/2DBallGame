using UnityEngine;

public class CollectableItems : MonoBehaviour
{
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
