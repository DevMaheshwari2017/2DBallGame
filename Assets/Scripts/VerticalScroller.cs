using UnityEngine;

public class VerticalScroller : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Transform backgroundsParent;
    [SerializeField] private Transform bg1;
    [SerializeField] private Transform bg2;
    [SerializeField] private Transform bg3;
    [SerializeField] private float platformMovingSpeed = 5f;
    #endregion

    #region Private Variables
    private float offset = 0.02f;
    private Transform player;
    private float bgHeight;
    private Transform[] bgs;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        player = GameService.Instance.GetPlayerController().transform;
        bgHeight = bg1.GetComponent<SpriteRenderer>().bounds.size.y;
        bgs = new Transform[] { bg1, bg2, bg3 };
    }

    private void Update()
    {
        backgroundsParent.Translate(Vector3.down * Time.deltaTime * platformMovingSpeed, Space.World);
    }

    private void FixedUpdate()
    {
        RepositionBackgroundIfNeeded();
    }
    #endregion

    #region Private Methods
    private void RepositionBackgroundIfNeeded()
    {
        SortBackgroundsByY(); // Sort from lowest to highest

        Transform bottomBG = bgs[0];
        Transform middleBG = bgs[bgs.Length/2];
        Transform topBG = bgs[bgs.Length-1];

        if (player.position.y > middleBG.position.y)
        {
            // Move bottom background to top
            bottomBG.position = new Vector3(
                bottomBG.position.x,
                topBG.position.y + bgHeight - offset,
                bottomBG.position.z
            );
        }
    }

    private void SortBackgroundsByY()
    {
        System.Array.Sort(bgs, (a, b) => a.position.y.CompareTo(b.position.y));
    }
    #endregion
}
