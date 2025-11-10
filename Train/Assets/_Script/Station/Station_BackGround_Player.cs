using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform player;     // 플레이어 Transform
    public float moveSpeed = 0.1f; // 움직임 속도 (0.1~0.5 정도가 적당)

    private Vector3 startPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어의 X 위치를 기준으로 배경 이동
        float newX = startPos.x + (player.position.x * moveSpeed);
        transform.position = new Vector3(newX, startPos.y, startPos.z);
    }
}