using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hit_Text_Damage : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer[] spriteRenderers;

    public int damage;
    public float Random_X;
    public float Random_Y;

    //TextMeshPro text;
    float StartTime;
    float DelayTime;
    float ElapsedTime;
    private void Start()
    {
        transform.position = new Vector2(Random_X, Random_Y);

        int digitCount = (damage == 0) ? 1 : (int)Mathf.Floor(Mathf.Log10(damage)) + 1;
        int temp = damage;
        for (int i = digitCount - 1; i >= 0; i--)
        {
            int digit = temp % 10;
            spriteRenderers[i].sprite = sprites[digit];
            spriteRenderers[i].gameObject.SetActive(true);
            temp /= 10;
        }

        //text = GetComponent<TextMeshPro>();
        //text.text = "-" + damage;
        DelayTime = 0.4f;
        StartTime = Time.time + 0.25f;
    }

    private void Update()
    {
        ElapsedTime = Time.time - StartTime;

        float transPercent = ElapsedTime / DelayTime;
        float xOffset = Mathf.Lerp(0f, 0.5f, transPercent);
        float yOffset = Mathf.Lerp(0f, 0.5f, transPercent);
        transform.position = new Vector2(Random_X + xOffset, Random_Y + yOffset);

        // 딜레이 시간 동안 텍스트 페이드 아웃
        float alphaPercent = Mathf.Clamp01(ElapsedTime / DelayTime);

        foreach (var sr in spriteRenderers)
        {
            Color color = sr.color;
            color.a = 1f - alphaPercent;
            sr.color = color;
        }

        //text.alpha = 1f - alphaPercent;

        // 텍스트가 완전히 사라지면 GameObject 파괴
        if (ElapsedTime >= DelayTime)
        {
            Destroy(gameObject);
        }
    }
}
