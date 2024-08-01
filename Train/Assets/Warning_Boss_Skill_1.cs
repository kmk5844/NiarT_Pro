using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning_Boss_Skill_1 : MonoBehaviour
{
    public GameObject Skill_Bullet;
    public GameObject Mark_Object;
    SpriteRenderer Mark_Sprite;
    public SpriteRenderer Shadow_Sprite;
    float startTime;
    float elapsedTime;
    float Mark_delayTime;
    float Shadow_delaytTime;

    bool BulletFlag;

    void Start()
    {
        Mark_Sprite = Mark_Object.GetComponent<SpriteRenderer>();
        BulletFlag = false;
        startTime = Time.time;
        Mark_delayTime = 0.5f;
        Shadow_delaytTime = 1f;
    }

    private void FixedUpdate()
    {
        elapsedTime = Time.time - startTime;

        float t = Mathf.Clamp01(elapsedTime / Mark_delayTime);

        Mark_Object.transform.Translate(0, 1* Time. deltaTime, 0);

        Color color_Mark = Mark_Sprite.color;
        color_Mark.a = Mathf.Lerp(1, 0, t);
        Mark_Sprite.color = color_Mark;

        float f = Mathf.Clamp01(elapsedTime / Shadow_delaytTime);

        Color color_Shadow = Shadow_Sprite.color;
        color_Shadow.a = Mathf.Lerp(1, 0, f);
        Shadow_Sprite.color = color_Shadow;
        Color color_OutLine = Shadow_Sprite.material.GetColor("_SolidOutline");
        color_OutLine.a = Mathf.Lerp(1, 0, f);
        Shadow_Sprite.material.SetColor("_SolidOutline", color_OutLine);

        if (elapsedTime > Mark_delayTime && !BulletFlag)
        {
            Instantiate(Skill_Bullet,transform.position, Quaternion.identity);
            BulletFlag = true;
        }

        if (elapsedTime > Shadow_delaytTime)
        {
            Destroy(gameObject);
        }
    }
}
