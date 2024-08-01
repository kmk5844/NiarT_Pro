using JetBrains.Annotations;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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
    public GameObject Warning;
    public GameObject Boss_SpiderWeb_Bullet;

    Transform Player_pos;

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

    int skillCount;
    int skillMaxCount;

    private void Start()
    {
        col = transform.GetComponent<PolygonCollider2D>();
        Player_pos = GameObject.FindWithTag("Player").transform;
        local_Scale = transform.localScale;
        xPos = 1f;
        speed = 3f;

        Bullet_List = GameObject.Find("Bullet_List").transform;

        transform.position = new Vector2(MonsterDirector.MaxPos_Sky.x + 18f, MonsterDirector.MinPos_Ground.y);
        playType = Boss_PlayType.Spawn;
        col.enabled = false;

        skillMaxCount = 6;
        move_delayTime = Random.Range(5f, 7f);
        move_delayTime = 5f;
        attack_delayTime = 1.5f;
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
                if(skillCount < skillMaxCount)
                {
                    playType = Boss_PlayType.Skill;
                }
                else
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
                StartCoroutine(Warning_Mark(0));
            }else if(skillNum == 2)
            {
                StartCoroutine(Warning_Mark(1));
            }else if(skillNum == 3)
            {
                StartCoroutine(Spider_Web_Skill());
            }
            skillCount++;
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
                heal_max_count = 2;
                Debug.Log(heal_max_count);
            }
        }

        if(playType == Boss_PlayType.Jump_Healing)
        {
            if(col.enabled == true)
            {
                col.enabled = false;
            }

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
            if (col.enabled == false)
            {
                col.enabled = true;
            }

            if (jump_Time + 0.5f < 1f)
            {
                jump_Time += Time.deltaTime;
                float t = Mathf.Clamp01(jump_Time / 1f);

                float y = Mathf.Lerp(transform.position.y, Jump_Init_Position.y, t);

                transform.position = new Vector3(Jump_Init_Position.x, y, 0);
            }
            else
            {
                transform.position = Jump_Init_Position;
                ToMove();
                xPos = 1f;
                speed = 8f;
                skillCount = 1;
                //skillMaxCount = Random.Range(5, 9);
                Debug.Log(skillMaxCount);
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

        yield return new WaitForSeconds(0.5f);
        ToMove();
    }

    IEnumerator Warning_Mark(int num)
    {
        int count = 0;
        if (num == 0)
        {
            while (true)
            {
                if (MonsterDirector.MinPos_Ground.x + (count * 1.8f) < MonsterDirector.MaxPos_Ground.x)
                {
                    float xPos = MonsterDirector.MinPos_Ground.x + (count * 1.8f);
                    Instantiate(Warning, new Vector2(xPos, MonsterDirector.MinPos_Ground.y), Quaternion.identity);
                    count++;

                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    break;
                }
            }
        } else if (num == 1)
        {
            int MaxCount = 15;
            while (true)
            {
                if(count < MaxCount)
                {
                    float xPos = Player_pos.position.x;
                    Instantiate(Warning, new Vector2(xPos, MonsterDirector.MinPos_Ground.y), Quaternion.identity);
                    count++;

                    yield return new WaitForSeconds(0.05f);
                }
                else
                {
                    break;
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        ToMove();
    }

    IEnumerator Spider_Web_Skill()
    {
        for(int i = 0; i < 3; i++)
        {
            Boss_SpiderWeb_Bullet.GetComponent<Boss_0_Skill3_Bullet>().FirePosition(Player_pos.position, Random.Range(-6f, 6f));
            Instantiate(Boss_SpiderWeb_Bullet, Fire_Zone.position, Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }

        yield return new WaitForSeconds(0.5f);
        ToMove();
    }

    private void ToMove()
    {
        move_delayTime = Random.Range(10f, 15f);
        move_delayTime = 30f;
        move_lastTime = Time.time; 
        playType = Boss_PlayType.Move;
    }

    enum Boss_PlayType
    {
        Spawn,
        Move,
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
        Mcus_Rampage_Player,
        Spider_Web,
    }
}