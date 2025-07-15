using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Laser_Turret : MonoBehaviour
{
    public GameObject TurretObj;
    public GameObject laser;
    public float rotationSpeed = 60f;

    private void Start()
    {
        laser.GetComponent<Bullet>().atk = 20;
    }

    void Update()
    {
        // 현재 회전 값 (0~360 사이의 값)
        float zRotation = transform.eulerAngles.z;

        // 회전
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // 180도 이상이면 파괴
        if (zRotation >= 180f)
        {
            Destroy(gameObject);
        }
    }
}
