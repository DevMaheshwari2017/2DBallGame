using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Private Serialized Variables
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator playerAnim;
    #endregion

    #region Private Variables
    private Vector2 touchStartPos;
    private string deathAnimationName = "Death";
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        GameService.Instance.SetPlayerController(this);
    }
    void Update()
    {
        // Keyboard movement (for testing)
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontal * moveSpeed * Time.deltaTime);

        // Swipe movement (mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                touchStartPos = touch.position;

            else if (touch.phase == TouchPhase.Ended)
            {
                float deltaX = touch.position.x - touchStartPos.x;
                if (Mathf.Abs(deltaX) > 50f)
                {
                    Vector2 direction = deltaX > 0 ? Vector2.right : Vector2.left;
                    transform.Translate(direction * moveSpeed * Time.deltaTime * 20); // boost
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            //playerAnim.SetTrigger("PlatformCollision");
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GameOver"))
        {
            GameService.Instance.GetUIManager().GameOver();
        }
    }

    #endregion

    #region Public Functions
    public IEnumerator PlayDeathAnimation()
    {
        if (playerAnim == null)
        {
            Debug.LogWarning("Player Animator is not assigned!");
            GameService.Instance.GetUIManager().ExecuteGameOverLogic();
            yield break;
        }

        playerAnim.Play(deathAnimationName);

        // Wait until animation starts playing
        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName(deathAnimationName))
        {
            yield return null;
            stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        }

        // Wait until animation finishes
        while (stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        }

        GameService.Instance.GetUIManager().ExecuteGameOverLogic();
    }
    #endregion
}
