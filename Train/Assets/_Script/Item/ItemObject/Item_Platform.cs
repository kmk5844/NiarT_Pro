using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Platform : MonoBehaviour
{
    [SerializeField]
    float delayTime;

    public Collider2D platformCollider;
    public float yOffset = 0.2f; // 여유 거리
    private GameObject player;
    private Collider2D playerCollider;

    private bool colliderEnabled = false;

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

        Destroy(gameObject, delayTime); // 10초 후에 오브젝트 삭제
    }

    public void SetDron(float DelayTime)
    {
        delayTime = DelayTime;
    }

    private void Update()
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
