using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("���� �Ѿ� ����")]
    Transform Monster_Bullet_List;
    public GameObject Bullet;
    [SerializeField]
    float Bullet_Delay;
    float lastTime;
    [SerializeField]
    GameObject player;
    Vector3 monster_SpawnPos;
    Vector3 movement;
    float xPos;
    MonsterDirector monsterdirector;

    [Header("������ �ֱ�, �ӵ�, �ִ� ���� ")]
    [SerializeField]
    float frequency;
    [SerializeField]
    float amplitude;
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    SpriteRenderer monster_Image; 

    private void Start()
    {
        monsterdirector = GameObject.Find("MonsterDirector").GetComponent<MonsterDirector>();
        monster_SpawnPos = transform.position;
        Monster_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        monster_Image = GetComponent<SpriteRenderer>();
        lastTime = 0f;
        player = GameObject.FindGameObjectWithTag("Player");

        frequency = Random.Range(5f, 15f);
        amplitude = Random.Range(0.5f, 1.5f);
        speed = Random.Range(3, 7);
        max_xPos = Random.Range(1, 9);

        xPos = -1f;
    }

    private void Update()
    {
        MonsterMove();
        BulletFire();
        FlipMonster();
    }

    void MonsterMove()
    {
        float yPos = Mathf.Sin(Time.time * frequency) * amplitude;
        if (monster_SpawnPos.x - max_xPos > transform.position.x)
        {
            xPos = 1f;
        }
        else if(monster_SpawnPos.x + max_xPos < transform.position.x)
        {
            xPos = -1f;
        }
        movement = new Vector3(xPos, yPos, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + Bullet_Delay)
        {
            Instantiate(Bullet, transform.position, transform.rotation, Monster_Bullet_List);
            lastTime = Time.time;
        }
    }

    void FlipMonster()
    {
        if(transform.position.x > player.transform.position.x)
        {
            monster_Image.flipX = true; // �ϸ鼭 ����
        }
        else
        {
            monster_Image.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            monsterdirector.MonsterDie();
        }
    }

}
