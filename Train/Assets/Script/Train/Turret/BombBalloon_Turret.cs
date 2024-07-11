using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class BombBalloon_Turret : Turret
{
    public Transform BulletObject;
    Transform Balloon_List; // Bullet�� �־ ������, ǳ���� ���� �����ؾߵǱ� ������ Ballon���� �̿�.
    GameObject[] Orc_List;
    int BeforeRandomNum;

    protected override void Start()
    {
        base.Start();
        trainData = transform.GetComponentInParent<Train_InGame>();
        Balloon_List = GameObject.Find("Balloon_List").GetComponent<Transform>();
        BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
        train_Attack_Delay = trainData.Train_Attack_Delay;

        Orc_List = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Orc_List[i] = transform.GetChild(i).gameObject;
        }
    }
    void Update()
    {
        if (!Orc_List[BeforeRandomNum].activeSelf)
        {
            if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay) - 3)
            {
                Orc_List[BeforeRandomNum].SetActive(true);
            }
        }

        if (Balloon_List.childCount < 2)
        {
            BulletFire();
        }
    }

    void BulletFire()
    {
        int Random_X = Random.Range(0, Orc_List.Length);

        if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay))
        {
            Orc_List[Random_X].SetActive(false);
            Instantiate(BulletObject, Orc_List[Random_X].transform.position, transform.rotation, Balloon_List);
            lastTime = Time.time;
        }

        BeforeRandomNum = Random_X;
    }
}
