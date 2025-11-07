using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public bool ShotGunFlag;
    public bool RifleFlag;
    public bool RoDanteFlag;
    public float Angle;
    public bool AcerSkillFlag;
    protected override void Start()
    {
        base.Start();
        if (ShotGunFlag)
        {
            Speed = Random.Range(30f, 45f);
        }
        Bullet_Player();
        if (AcerSkillFlag)
        {
            StartCoroutine(AcerSKill());
        }
    }

    void Bullet_Player()
    {
        Camera _cam = Camera.main;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;

        if (RifleFlag)
        {
            float angleOffset = Random.Range(-1.8f, 1.8f); // 무작위 각도 범위 설정 (-10도에서 10도 사이)
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90 + angleOffset);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else if (ShotGunFlag)
        {
            // 무작위 각도를 추가하기 위해 회전 벡터를 사용
            float angleOffset = Random.Range(-9f, 9f); // 무작위 각도 범위 설정 (-10도에서 10도 사이)
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;

            // 총알 회전 설정
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90 + angleOffset);

            // 속도 설정
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }else if (RoDanteFlag)
        {
            dir = Quaternion.Euler(0, 0,Angle) * dir;
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90 + Angle);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else
        {
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }

        Destroy(gameObject, 3f);
    }

    IEnumerator AcerSKill()
    {
        atk = atk / 2;
        float duration = 1f;
        Vector2 startScale_Size = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector2.Lerp(startScale_Size, startScale_Size * 60f, t);
            yield return null;
        }

        transform.localScale = startScale_Size * 60f;
    }

    public void Setting_Rodante(float angle_)
    {
        atk = atk / 2;
        RoDanteFlag = true;
        Angle = angle_;
    }
}
