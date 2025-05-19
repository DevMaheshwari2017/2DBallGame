using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Private Serialized Variables
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private float swipeMoveDistance = 2f;
    [SerializeField] private float swipeDuration = 0.2f;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private float slidingPower = 20f;
    [SerializeField] private float squashAmount = 0.2f;
    [SerializeField] private float squashDuration = 0.1f;
    [SerializeField] private float squashAnimationDuration = 1.0f;
    [SerializeField] private SelectedSkinDataHolder selectedSkinDataHolder;
    #endregion

    #region Private Variables
    private bool isSwiping;
    private Vector3 originalScale;
    private Vector2 touchStartPos;
    private string deathAnimationName = "PlayerDeath";
    private Rigidbody2D playerRB;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        /// Change Player color based on skin selected from shop usin selectedSkinDataHolder SO
        if (selectedSkinDataHolder != null && selectedSkinDataHolder.selectedSkin != null)
        {
            GetComponent<SpriteRenderer>().color = selectedSkinDataHolder.selectedSkin.skinColor;
        }
        playerRB = GetComponent<Rigidbody2D>();
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameService.Instance.SetPlayerController(this);
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (GameService.Instance.GetUIManager().GetIsGameOver())
            return;

        /// Keyboard movement
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontal * moveSpeed * Time.deltaTime);

        /// Swipe movement mobile
        if (Input.touchCount > 0 && !isSwiping)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                touchStartPos = touch.position;

            else if (touch.phase == TouchPhase.Ended)
            {
                float deltaX = touch.position.x - touchStartPos.x;
                if (Mathf.Abs(deltaX) > swipeThreshold)
                {
                    Vector2 direction = deltaX > 0 ? Vector2.right : Vector2.left;
                    Vector2 moveTo = (Vector2)transform.position + direction * swipeMoveDistance;
                    StartCoroutine(SmoothMove(moveTo));
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /// Playing squash animation when player collided with platforms

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            if (collision.contacts.Length > 0)
            {
                Vector2 normal = collision.contacts[0].normal;

                // Determine squeeze axis
                if (Mathf.Abs(normal.y) > Mathf.Abs(normal.x))
                {
                    // Vertical collision → squash vertically (flatten height)
                    StartCoroutine(Squash(new Vector3(1 + squashAmount, 1 - squashAmount, 1)));
                }
                else
                {
                    // Horizontal collision → squash horizontally
                    StartCoroutine(Squash(new Vector3(1 - squashAmount, 1 + squashAmount, 1)));
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /// gameover
        if (collision.gameObject.CompareTag("GameOver"))
        {
            GameService.Instance.GetUIManager().GameOver();
        }
    }

    #endregion

    #region Public Functions
    public IEnumerator PlayDeathAnimation()
    {
        /// Playing death animation, if player dies from obstacle interaction
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
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
        yield return new WaitForSeconds(0.5f);
        GameService.Instance.GetUIManager().ExecuteGameOverLogic();
    }
    #endregion

    #region Private Functions

    private IEnumerator SmoothMove(Vector2 endPosition)
    {
        isSwiping = true;
        Vector2 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < swipeDuration)
        {
            playerRB.MovePosition(Vector2.Lerp(startPosition, endPosition, elapsed / swipeDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerRB.MovePosition(endPosition);
        isSwiping = false;
    }
    private IEnumerator Squash(Vector3 targetScale)
    {
        ///Sqaush animation 

        // Animate to squashed scale
        float elapsed = 0f;
        while (elapsed < squashDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / squashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Hold the squashed scale briefly
        transform.localScale = targetScale;
        yield return new WaitForSeconds(squashAnimationDuration);

        // Animate back to original scale
        elapsed = 0f;
        while (elapsed < squashDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / squashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
    #endregion
}
