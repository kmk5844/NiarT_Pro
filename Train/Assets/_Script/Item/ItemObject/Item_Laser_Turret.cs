using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Laser_Turret : MonoBehaviour
{
    public GameObject TurretObj;
    public bool laserType;
    public bool groundType;


    public GameObject[] laser;
    public int attack;
    public float SpawnDelay;


    public float rotationSpeed = 60f;

    private void Start()
    {
        laser[0].GetComponent<Bullet>().atk = attack;
        if (!laserType)
        {
            laser[1].SetActive(false);
        }
        else
        {
            laser[1].SetActive(true);
            laser[1].GetComponent<Bullet>().atk = attack;
        }

        if (groundType)
        {
            Destroy(TurretObj, SpawnDelay);
        }
    }

    void Update()
    {
        if (!groundType)
        {
            // ���� ȸ�� �� (0~360 ������ ��)
            float zRotation = transform.eulerAngles.z;

            // ȸ��
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // 180�� �̻��̸� �ı�
            if (zRotation >= 180f)
            {
                Destroy(TurretObj);
            }
        }
    }
}
