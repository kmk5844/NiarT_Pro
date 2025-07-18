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
            // 현재 회전 값 (0~360 사이의 값)
            float zRotation = transform.eulerAngles.z;

            // 회전
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // 180도 이상이면 파괴
            if (zRotation >= 180f)
            {
                Destroy(TurretObj);
            }
        }
    }
}
