using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Turret : Turret
{
    public CircleCollider2D circle2D;
    public GameObject BulletImage;
    bool MusicFlag;
    int atk;
    public ParticleSystem BoomEffect;

    protected override void Start()
    {
        base.Start();
        MusicFlag = false;
        atk = trainData.Train_Attack;
    }

    // Update is called once per frame
    void Update()
    {
        if (!trainData.DestoryFlag)
        {
            BulletFire();
        }
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay))
        {
            if (!MusicFlag)
            {
                StartCoroutine(Music());
                MusicFlag = true;
            }
        }
    }

    IEnumerator Music()
    {
        BulletImage.transform.localScale = Vector3.zero; // �ʱ� ũ��
        BulletImage.SetActive(true);
        BoomEffect.Play();
        float duration = 0.3f; // ���� �ð�
        float elapsed = 0f;

        float startRadius = 0f;
        float endRadius = 10f;

        circle2D.radius = 0;
        circle2D.enabled = true;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 0~1 ���� ������
            float t = elapsed / duration;

            // ���� ������ ����
            circle2D.radius = Mathf.Lerp(startRadius, endRadius, t);

            // �̹��� ũ�⵵ ���� ����
            BulletImage.transform.localScale = Vector3.one * circle2D.radius * 0.77f;

            yield return null;
        }

        // ������ ����
        circle2D.radius = endRadius;
        yield return new WaitForSeconds(0.1f); // �ʿ�� ��� ���
        BulletImage.SetActive(false);
        circle2D.enabled = false;
        MusicFlag = false;
        lastTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            if (monster != null)
            {
                monster.Damage_Monster_Item(atk);
            }
        }
    }
}