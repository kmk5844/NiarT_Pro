using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float Speed;
    [SerializeField]
    public int slow;
    [SerializeField]
    public int atk;

    public BulletType bulletType;
    Transform player;
    Vector3 dir;
    Rigidbody2D rid;
    Camera _cam;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rid = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        if (bulletType == BulletType.Monster_Bullet)
        {
            dir = (player.position - transform.position).normalized;
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else if(bulletType == BulletType.Player_Bullet)
        {
            Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            dir = mousePos - transform.position;
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else if(bulletType == BulletType.Train_Bullet)
        {
            rid.velocity = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right * Speed;
        }
        Destroy(gameObject, 3f);
    }
}

public enum BulletType
{
    Monster_Bullet,
    Player_Bullet,
    Train_Bullet
}