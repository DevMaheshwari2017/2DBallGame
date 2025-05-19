using UnityEngine;

public class Obstacles : MonoBehaviour
{
    /// <summary>
    /// A common level obstacle script for each obstacle
    /// Whenever a obstacle collides with player tell uimanager it's gameover.
    /// Cna be expand futher in future to perform other tasks on obstacles 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            GameService.Instance.GetUIManager().GameOver(true);
        }
    }
}
