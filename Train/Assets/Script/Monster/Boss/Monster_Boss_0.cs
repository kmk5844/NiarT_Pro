using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boss_0 : MonoBehaviour
{
    public int Boss_HP;
    public int Boss_Atk;

    public float Bullet_Speed;
    public float Bullet_Slow;

    public int Random_Patten_Num;

    public Transform Fire_Zone;
    public GameObject Boss_Bullet;
    public Transform Target;
    public Transform Bullet_List;

    public GameObject Boss_Egg_Object;

    [SerializeField]
    Boss_PlayType playType;
    PolygonCollider2D col;

    Vector3 local_Scale;
    Vector3 Move_Init_Position;
    Vector3 Jump_Init_Position;
    float xPos;
    Vector3 movement;
    float speed;

    float move_lastTime;
    float move_delayTime;
    float attack_lastTime;
    float attack_delayTime;
    float jump_Time;
    bool heal_flag;
    int heal_count;
    int heal_max_count;

    bool skill_flag;
    int skill_count;


    private void Start()
    {
        col = transform.GetComponent<PolygonCollider2D>();
        local_Scale = transform.localScale;
        xPos = 1f;
        speed = 3f;

        Bullet_List = GameObject.Find("Bullet_List").transform;

        transform.position = new Vector2(MonsterDirector.MaxPos_Sky.x + 18f, MonsterDirector.MinPos_Ground.y);
        playType = Boss_PlayType.Spawn;
        col.enabled = false;
    }

    private void FixedUpdate()
    {
        if(playType == Boss_PlayType.Spawn)
        {
            if(transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
            }

            transform.Translate(-12f * Time.deltaTime, 0, 0);
            if(transform.position.x < MonsterDirector.MinPos_Ground.x - 7f)
            {
                playType = Boss_PlayType.Move;
                if(transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
                col.enabled = true;
                move_lastTime = Time.time;
                attack_lastTime = Time.time;
                move_delayTime = Random.Range(5f, 7f);
                move_delayTime = 5f;
                attack_delayTime = 2;
                Move_Init_Position = transform.position;
            }
        }

        if(playType == Boss_PlayType.Move)
        {
            if (Move_Init_Position.x - 1 > transform.position.x)
            {
                xPos = 1f;
                speed = Random.Range(3f, 5f);
            }
            else if (Move_Init_Position.x + 1 < transform.position.x)
            {
                xPos = -1f;
                speed = Random.Range(1f, 2f);
            }
            movement = new Vector3(xPos, 0f, 0f);
            transform.Translate(movement * speed * Time.deltaTime);

            if(Time.time >= attack_lastTime + attack_delayTime)
            {
                Boss_Bullet.GetComponent<Boss_0_Bullet>().targetPosition = Target.position;
                Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity, Bullet_List);
                attack_lastTime = Time.time;
            }

            if (Time.time >= move_lastTime + move_delayTime)
            {
                int num = Random.Range(0, 3);
                num = 1;
                if(num == 0)
                {
                    playType = Boss_PlayType.Attack;
                }
                else if(num == 1)
                {
                    playType = Boss_PlayType.Skill;
                }
                else if(num == 2)
                {
                    playType = Boss_PlayType.Jump_UP;
                    jump_Time = 0;
                    Jump_Init_Position = transform.position;
                }
            }
        }
        
        if (playType == Boss_PlayType.Skill)
        {
            int skillNum = Random.Range(0, 3);
            skillNum = 0;
            if (skillNum == 0)
            {
                StartCoroutine(Spawn_Egg_Skill());
            }
            else if(skillNum == 1)
            {

            }else if(skillNum == 2)
            {

            }
            playType = Boss_PlayType.Skill_Using;
        }

        if (playType == Boss_PlayType.Jump_UP)
        {
            if(jump_Time < 1f)
            {
                jump_Time += Time.deltaTime;
                float t = jump_Time / 1f;

                float x = Mathf.Lerp(Jump_Init_Position.x, Jump_Init_Position.x - 10, t);
                float y = Mathf.Lerp(Jump_Init_Position.y, Jump_Init_Position.y + 8, t) + 10 * Mathf.Sin(Mathf.PI * (t/2));

                transform.position = new Vector3(x, y, 0);
            }
            else
            {
                playType = Boss_PlayType.Jump_Healing;
                jump_Time = 0;
                heal_max_count = Random.Range(4, 8);
                Debug.Log(heal_max_count);
            }
        }

        if(playType == Boss_PlayType.Jump_Healing)
        {
            if (heal_count < heal_max_count + 1)
            {
                if (!heal_flag)
                {
                    StartCoroutine(HealCount());
                }
            }
            else
            {
                heal_count = 0;
                playType = Boss_PlayType.Jump_DOWN;
            }
        }

        if (playType == Boss_PlayType.Jump_DOWN)
        {
            if(jump_Time + 0.5f < 1f)
            {
                jump_Time += Time.deltaTime;
                float t = Mathf.Clamp01(jump_Time / 1f);

                float y = Mathf.Lerp(transform.position.y, Jump_Init_Position.y, t);

                transform.position = new Vector3(Jump_Init_Position.x, y, 0);
            }
            else
            {
                transform.position = Jump_Init_Position;
                move_lastTime = Time.time;
                playType = Boss_PlayType.Move;
                xPos = 1f;
                speed = 8f;
            }
        }

        if (playType == Boss_PlayType.Die)
        {

        }
    }
    IEnumerator HealCount()
    {
        heal_flag = true;
        Debug.Log("보스 힐!");
        //보스 치유하는 코드
        if (heal_count != heal_max_count)
        {
            yield return new WaitForSeconds(2);
        }
        else
        {
            yield return new WaitForSeconds(5);
        }
        heal_count++;
        heal_flag = false;
    }

    IEnumerator Spawn_Egg_Skill()
    {
        int eggNum = Random.Range(1, 5);
        for (int i = 0; i < eggNum; i++)
        {
            Instantiate(Boss_Egg_Object);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(((eggNum -1) * 0.5f) + 1.5f);

        move_lastTime = Time.time;
        playType = Boss_PlayType.Move;
    }

    enum Boss_PlayType
    {
        Spawn,
        Move,
        Attack,
        Skill,
        Skill_Using,
        Jump_UP,
        Jump_Healing,
        Jump_DOWN,
        Die
    }


    enum Boss_Patern
    {
        Spawn_Egg,
        Mcus_Rampage,
        Spider_Web,
    }
}