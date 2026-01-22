using UnityEngine;

public class ParallaxBackGround_Sprite : MonoBehaviour
{
    public GameDirector gameDirector;

    [Tooltip("깊이 계수 (멀수록 작게)")]
    public float depth = 0.5f;

    public float wrapWidth = 20f;

    Vector3 startPos;
    public bool leftAndRight = false;
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        startPos = transform.localPosition;
    }

    void Update()
    {
        float trainSpeed = gameDirector.TrainSpeed / 100f;
        float move = trainSpeed * depth * Time.deltaTime;

        // 배경 왼쪽 이동
        if (!leftAndRight)
        {
            transform.Translate(Vector3.left * move);
        }
        else
        {
            transform.Translate(Vector3.right * move);
        }

        // 무한 루프 처리
        if (transform.localPosition.x <= startPos.x - wrapWidth)
        {
            transform.localPosition += Vector3.right * wrapWidth * 2f;
        }
    }
}