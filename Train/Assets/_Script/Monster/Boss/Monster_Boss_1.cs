using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Monster_Boss_1 : Boss
{
    public Transform Fire_Zone;
    public GameObject TentacleObject;
    public GameObject Tentacle_Bomb_Object;
    public GameObject HowlingObject;
    GameObject player;

    [SerializeField]
    Boss_PlayType playType;
    Animator ani;
    bool aniFlag;

    Vector3 Move_Init_Position;
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
        player = GameObject.FindWithTag("Player");
        transform.position = new Vector3(MonsterDirector.MaxPos_Sky.x + 18f, 0.79f, 56);
        playType = Boss_PlayType.Spawn;

        move_delayTime = 10f;
        attack_delayTime = 1f;
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
                BulletFire();

        /*                playType = Boss_PlayType.Attack;
                        ResetAni();*/
            }

            //스킬
            if (Time.time >= move_lastTime + move_delayTime)
            {
                //move_lastTime = Time.time;
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
                StartCoroutine(skill_Tentacle(0));
            }
            else if (skillNum == 1)
            {
                StartCoroutine(skill_Tentacle(1));
            }
            else if (skillNum == 2)
            {
                StartCoroutine(skill_Tentacle(2));
            }
            else if (skillNum == 3)
            {
                StartCoroutine(skill_Tentacle(3));
            }

            playType = Boss_PlayType.Skill_Using;
            ResetAni();
        }
    }

    IEnumerator skill_Tentacle(int skill)
    {
        float xPos;
        Vector2 pos;
        if(skill == 0)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(0);
            for (int i = 0; i < 15; i++)
            {
                xPos = Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, Quaternion.identity, monster_Bullet_List);
                yield return new WaitForSeconds(0.15f);
            }
        }else if(skill == 1)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(1);
            Quaternion rot = Quaternion.Euler(0f, 0f, -30f);
            for (int i = 0; i < 8; i++)
            {
                xPos = MonsterDirector.MinPos_Ground.x + (i * 3.3f);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, rot, monster_Bullet_List);
                yield return new WaitForSeconds(0.2f);
            }
        }else if(skill == 2)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(2);
            for(int i = 0; i < 20; i++)
            {
                Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(-30f, 30f));
                xPos = Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, rot, monster_Bullet_List);
                yield return new WaitForSeconds(0.15f);
            }
        }else if(skill == 3)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(3);
            float playerPos = player.transform.position.x;
            for(int i =  -2; i < 3; i++)
            {
                xPos = playerPos + (i * 3.5f);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, Quaternion.identity, monster_Bullet_List);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        ToMove();
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