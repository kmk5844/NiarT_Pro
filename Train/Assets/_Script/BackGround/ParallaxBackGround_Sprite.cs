using UnityEngine;

public class ParallaxBackGround_Sprite : MonoBehaviour
{
    public GameDirector gameDirector;

    [Tooltip("깊이 계수 (멀수록 작게)")]
    public float depth = 0.5f;

    public float wrapWidth = 20f;

    Vector3 startPos;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        startPos = transform.localPosition;
    }

    void Update()
    {
        float trainSpeed = gameDirector.TrainSpeed / 100f;

        float move = trainSpeed * depth * Time.deltaTime;

        // 기차가 → , 배경은 ←
        transform.Translate(Vector3.right * move);

        // 무한 루프
        if (transform.localPosition.x <= startPos.x - wrapWidth)
        {
            transform.localPosition += Vector3.right * wrapWidth * 2f;
        }
    }
}