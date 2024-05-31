using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hit_Text_Damage : MonoBehaviour
{
    public int damage;
    public float Random_X;
    public float Random_Y;

    TextMeshPro text;
    float StartTime;
    float DelayTime;
    float ElapsedTime;
    private void Start()
    {
        transform.position = new Vector2(Random_X, Random_Y);
        text = GetComponent<TextMeshPro>();
        text.text = "-" + damage;
        DelayTime = 0.6f;
        StartTime = Time.time;
    }

    private void Update()
    {
        ElapsedTime = Time.time - StartTime;

        float transPercent = ElapsedTime / DelayTime;
        float xOffset = Mathf.Lerp(0f, 0.5f, transPercent);
        float yOffset = Mathf.Lerp(0f, 0.5f, transPercent);
        transform.position = new Vector2(Random_X + xOffset, Random_Y + yOffset);

        // ������ �ð� ���� �ؽ�Ʈ ���̵� �ƿ�
        float alphaPercent = ElapsedTime / DelayTime;
        text.alpha = 1f - alphaPercent;

        // �ؽ�Ʈ�� ������ ������� GameObject �ı�
        if (ElapsedTime >= DelayTime)
        {
            Destroy(gameObject);
        }
    }
}
