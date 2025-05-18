using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Animator anim = collision.gameObject.GetComponent<Animator>();
            anim.SetTrigger("Death");
            GameService.Instance.GetUIManager().GameOver(true);
        }
    }
}
