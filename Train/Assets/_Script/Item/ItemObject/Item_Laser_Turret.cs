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
        // ���� ȸ�� �� (0~360 ������ ��)
        float zRotation = transform.eulerAngles.z;

        // ȸ��
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // 180�� �̻��̸� �ı�
        if (zRotation >= 180f)
        {
            Destroy(gameObject);
        }
    }
}
