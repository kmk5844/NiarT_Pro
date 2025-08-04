using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrain : MonoBehaviour
{
    public Collider2D platformCollider;
    public float yOffset = 0.2f; // 여유 거리
    private GameObject player;
    private Collider2D playerCollider;

    private bool colliderEnabled = false;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<Collider2D>();

        float playerBottom = playerCollider.bounds.min.y;
        float platformTop = platformCollider.bounds.max.y;

        if (playerBottom > platformTop + yOffset)
        {
            platformCollider.isTrigger = false;
            colliderEnabled = true;
        }
        else
        {
            platformCollider.isTrigger = true;
            colliderEnabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float playerBottom = playerCollider.bounds.min.y;
        float platformTop = platformCollider.bounds.max.y;

        if (!colliderEnabled)
        {
            if (playerBottom > platformTop + yOffset)
            {
                platformCollider.isTrigger = false;
                colliderEnabled = true;
            }
        }
        else // 이미 콜라이더가 활성화된 상태
        {
            if (playerBottom < platformTop - yOffset)
            {
                platformCollider.isTrigger = true;
                colliderEnabled = false;
            }
        }
    }
}
