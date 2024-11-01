using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boss_1 : Boss
{
    public Transform Fire_Zone;

    [SerializeField]
    Boss_PlayType playType;
    Animator ani;
    bool aniFlag;

    Vector3 Move_Init_Position;
    Vector3 Jump_Init_Position;
    float move_xPos;
    Vector3 movement;
    float move_speed;

    float move_lastTime;
    float move_delayTime;
    float attack_lastTime;
    float attack_delayTime;

    protected override void Start()
    {
        Boss_Num = 1;
        ani = GetComponent<Animator>();
        aniFlag = false;
        base.Start();
        move_xPos = 1f;
        move_speed = 3f;

        transform.position = new Vector3(MonsterDirector.MaxPos_Sky.x + 18f, 0.79f, 56);
        playType = Boss_PlayType.Spawn;

        move_delayTime = 6f;
        attack_delayTime = 1.5f;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        Fire_Debuff();

        if(playType == Boss_PlayType.Spawn)
        {
            transform.Translate(-12f * Time.deltaTime, 0, 0);

            if (transform.position.x < MonsterDirector.MinPos_Ground.x - 7f)
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


            //공격
            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                playType = Boss_PlayType.Attack;
                ResetAni();
            }

            //스킬
            if (Time.time >= move_lastTime + move_delayTime)
            {
                playType = Boss_PlayType.Skill;
                ResetAni();
            }
        }

        if(playType == Boss_PlayType.Attack)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            playType = Boss_PlayType.Attack_Using;
            ResetAni();
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
                Debug.Log("스킬1");
            }
            else if (skillNum == 1)
            {
                Debug.Log("스킬2");
            }
            else if (skillNum == 2)
            {
                Debug.Log("스킬3");
            }
            else if (skillNum == 3)
            {
                Debug.Log("스킬4");
            }

            playType = Boss_PlayType.Skill_Using;
            ResetAni();
        }
    }

    void ResetAni()
    {
        aniFlag = false;
    }

    void TriggerAnimation()
    {
        if (playType == Boss_PlayType.Spawn || playType == Boss_PlayType.Move)
        {
            ani.SetTrigger("Move");
        }
    }

    //공격 애니메이션 삽입
    public void BulletFire()
    {
        GameObject defaultBullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity, monster_Bullet_List);
        defaultBullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 0);
        attack_lastTime = Time.time;
    }

    //공격 애니메이션 종료 후, 삽입.
    public void ToMove()
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
        Attack,
        Attack_Using,
        Skill,
        Skill_Using,
        Die
    }
}