using JetBrains.Annotations;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boss_0 : Boss
{
    public Transform Fire_Zone;
    public GameObject Boss_Egg_Object;
    public GameObject Warning;
    public GameObject Boss_SpiderWeb_Bullet;

    [SerializeField]
    Boss_PlayType playType;

    Vector3 Move_Init_Position;
    Vector3 Jump_Init_Position;
    float move_xPos;
    Vector3 movement;
    float move_speed;

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

    Animator ani;
    bool aniFlag;
    bool jumpAni;
    Transform Monster_List;

    protected override void Start()
    {
        Boss_Num = 0;
        ani = GetComponent<Animator>();
        aniFlag = false;
        jumpAni = false;
        base.Start();
        move_xPos = 1f;
        move_speed = 3f;

        transform.position = new Vector2(MonsterDirector.MaxPos_Sky.x + 18f, 2f);
        playType = Boss_PlayType.Spawn;
        col.enabled = false;

        skillCount = 1;
        skillMaxCount = 5;
        move_delayTime = 5f;
        attack_delayTime = 1.5f;
        try
        {
            Monster_List = GameObject.Find("Monster_List").transform;
        }
        catch
        {
            Debug.Log("테스트 진행 중");
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        Fire_Debuff();

        if (playType == Boss_PlayType.Spawn)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
            }

            transform.Translate(-12f * Time.deltaTime, 0, 0);
            if(transform.position.x < MonsterDirector.MinPos_Ground.x - 7f)
            {
                playType = Boss_PlayType.Move;
                ResetAni();

                if (transform.localScale.x < 0)
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
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            if (Move_Init_Position.x - 1 > transform.position.x)
            {
                move_xPos = 1f;
                move_speed = Random.Range(3f, 5f);
            }
            else if (Move_Init_Position.x + 1 < transform.position.x)
            {
                move_xPos = -1f;
                move_speed = Random.Range(1f, 2f);
            }
            movement = new Vector3(move_xPos, 0f, 0f);
            transform.Translate(movement * move_speed * Time.deltaTime);

            if(Time.time >= attack_lastTime + attack_delayTime)
            {
                GameObject defaultBullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity, monster_Bullet_List);
                defaultBullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 0);
                attack_lastTime = Time.time;
            }

            if (Time.time >= move_lastTime + move_delayTime)
            {
                if(skillCount < skillMaxCount)
                {
                    playType = Boss_PlayType.Skill;
                    ResetAni();
                }
                else
                {
                    playType = Boss_PlayType.Jump_UP;
                    ResetAni();

                    jump_Time = 0;
                    Jump_Init_Position = transform.position;
                }
            }
        }
        
        if (playType == Boss_PlayType.Skill)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            int skillNum = Random.Range(0, 4);
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
            ResetAni();
        }

        if (playType == Boss_PlayType.Jump_UP)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            if (jumpAni)
            {
                if (jump_Time < 1f)
                {
                    jump_Time += Time.deltaTime;
                    float t = jump_Time / 1f;

                    float x = Mathf.Lerp(Jump_Init_Position.x, Jump_Init_Position.x - 10, t);
                    float y = Mathf.Lerp(Jump_Init_Position.y, Jump_Init_Position.y + 8, t) + 10 * Mathf.Sin(Mathf.PI * (t / 2));

                    transform.position = new Vector3(x, y, 0);
                }
                else
                {
                    playType = Boss_PlayType.Jump_Healing;
                    jump_Time = 0;
                    heal_max_count = Random.Range(4, 8);
                    jumpAni = false;
                }
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
                ResetAni();
            }
        }

        if (playType == Boss_PlayType.Jump_DOWN)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            if (col.enabled == false)
            {
                col.enabled = true;
            }

            if (jump_Time + 0.2f  < 1f)
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
                move_xPos = 1f;
                move_speed = 8f;
                skillMaxCount = Random.Range(5, 11);
            }
        }

        if (DieFlag)
        {
            if(playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                ResetAni();
            }
        }


        if (playType == Boss_PlayType.Die)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            movement = new Vector3(-1f, 0f, 0f);
            transform.Translate(movement * 1.5f * Time.deltaTime);
            Destroy(gameObject, 8f);
        }
    }
    IEnumerator HealCount()
    {
        heal_flag = true;
        Monster_HP += (int)((Monster_Max_HP * 1) / 100f);
        Debug.Log((Monster_Max_HP * 1) / 100f);
        Debug.Log(Monster_HP);
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
            Instantiate(Boss_Egg_Object, Monster_List);
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
                    GameObject WarningMark = Instantiate(Warning, new Vector2(xPos, MonsterDirector.MinPos_Ground.y), Quaternion.identity);
                    WarningMark.GetComponent<Warning_Boss_Skill_1>().GetBulletInformation(Bullet_Atk, Bullet_Slow);
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
                    float xPos = player_pos.position.x;
                    GameObject WarningMark = Instantiate(Warning, new Vector2(xPos, MonsterDirector.MinPos_Ground.y), Quaternion.identity);
                    WarningMark.GetComponent<Warning_Boss_Skill_1>().GetBulletInformation(Bullet_Atk, Bullet_Slow);
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
            Boss_SpiderWeb_Bullet.GetComponent<Boss_0_Skill3_Bullet>().Fire_GetInformation(player_pos.position, Random.Range(-6f, 6f), Bullet_Atk, Bullet_Slow, 10f);
            Instantiate(Boss_SpiderWeb_Bullet, Fire_Zone.position, Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }

        yield return new WaitForSeconds(0.5f);
        ToMove();
    }

    void TriggerAnimation()
    {
        if (playType == Boss_PlayType.Spawn || playType == Boss_PlayType.Move)
        {
            ani.SetTrigger("Move");
        }

        if (playType == Boss_PlayType.Skill)
        {
            ani.SetTrigger("Skill");
        }

        if(playType == Boss_PlayType.Jump_UP)
        {
            ani.SetTrigger("JumpUP");
        }

        if (playType == Boss_PlayType.Jump_DOWN)
        {
            ani.SetTrigger("JumpDown");
        }

        if (playType == Boss_PlayType.Die)
        {
            ani.SetTrigger("Die");
        }
    }

    void ResetAni()
    {
        aniFlag = false;
    }

    public void JumpAni_End()
    {
        jumpAni = true;
    }
    
    private void ToMove()
    {
        move_delayTime = Random.Range(5f, 8f);
        move_lastTime = Time.time; 
        playType = Boss_PlayType.Move;
        ResetAni();
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