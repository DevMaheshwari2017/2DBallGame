using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform bg1;
    [SerializeField] private Transform bg2;

    private Transform player;
    private float colliderSzie;
    private float highestY;

    private void Start()
    {
        player = GameService.Instance.GetPlayerController().gameObject.transform;
        highestY = player.position.y;
        colliderSzie = bg1.GetComponent<BoxCollider2D>().size.y;
    }

    private void FixedUpdate()
    {
        SwitchBG();
    }

    private void SwitchBG() 
    {
        if (transform.position.y > bg2.position.y) 
        {
            bg1.position = new Vector3(bg1.position.x, bg1.position.y + colliderSzie, bg1.position.z);
            Transform temp = bg1;
            bg1 = bg2;
            bg2 = temp;
        }
    }
    void LateUpdate()
    {
        if (player.position.y > highestY)
        {
            highestY = player.position.y;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, highestY, transform.position.z), 0.2f);    
        }
    }
}
