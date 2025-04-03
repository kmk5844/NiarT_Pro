using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boss_2 : Boss
{
    public Transform Fire_Zone;
    GameObject player;

    [SerializeField]
    Boss_PlayType playType;

    Vector3 Move_Init_Position;
    float move_xPos;
    Vector3 movement;
    float move_speed;

    float move_lastTime;
    float move_delayTime;
    float attack_lastTime;
    float attack_delayTime;

    bool LR_Flag;

    protected override void Start()
    {
        Boss_Num = 2;
        base.Start();
        move_xPos = 1f;
        move_speed = 3f;
        player = GameObject.FindWithTag("Player");
        transform.position = new Vector3(MonsterDirector.MaxPos_Sky.x + 18f, 10f, 56);
        transform.rotation = Quaternion.Euler(0,0,20);
        transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
        playType = Boss_PlayType.Spawn;

        move_delayTime = 10f;
        attack_delayTime = 1f;
        LR_Flag = false;
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
            transform.Translate(-12f * Time.deltaTime, 0, 0, Space.World);

            if (transform.position.x < MonsterDirector.MinPos_Ground.x - 7f)
            {
                playType = Boss_PlayType.Move;
                LR_Flag = false;
                transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                transform.rotation = Quaternion.Euler(0, 0, -20);
                col.enabled = true;
                move_lastTime = Time.time;
                attack_lastTime = Time.time;

                Move_Init_Position = transform.position;
            }
        }

        if (playType == Boss_PlayType.Move)
        {
            if (LR_Flag)
            {
                transform.Translate(-12f * Time.deltaTime, 0, 0, Space.World);
                if (transform.position.x < MonsterDirector.MinPos_Sky.x)
                {
                    LR_Flag = false;
                    transform.rotation = Quaternion.Euler(0,0,-20);
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
            }
            else
            {
                transform.Translate(12f * Time.deltaTime, 0, 0, Space.World);
                
                if (transform.position.x > MonsterDirector.MaxPos_Sky.x) 
                {
                    LR_Flag = true;
                    transform.rotation = Quaternion.Euler(0,0,20);
                    transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
                }
            }

            if(Time.time >= attack_lastTime + attack_delayTime)
            {
                Debug.Log("발사");
                attack_lastTime = Time.time;
            }

            if(Time.time >= move_lastTime + move_delayTime)
            {
                playType = Boss_PlayType.Skill;
            }
        }

        if (playType == Boss_PlayType.Skill)
        {
            int skillNum = Random.Range(0, 3);
            if(skillNum == 0)
            {
                Debug.Log("스킬 사용 0");
                StartCoroutine(RandomBomb());
            }
            else if(skillNum == 1)
            {
                Debug.Log("스킬 사용 1");
                StartCoroutine(MachineGun_Sky_Player());
            }
            else if(skillNum == 2)
            {
                Debug.Log("스킬 사용 2");
                StartCoroutine(MachineGun_Ground_Player());
            }
            playType = Boss_PlayType.Skill_Using;
        }

        if (DieFlag)
        {
            if(playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
            }
        }

        if (playType == Boss_PlayType.Die)
        {
            movement = new Vector3(-2f, -3f, 0f);
            transform.Translate(movement * 1.5f * Time.deltaTime);
            Destroy(gameObject, 8f);
        }

    }
    IEnumerator RandomBomb()
    {
        yield return new WaitForSeconds(2f);
        ToMove();
    }

    IEnumerator MachineGun_Sky_Player()
    {
        yield return new WaitForSeconds(2f);
        ToMove();
    }

    IEnumerator MachineGun_Ground_Player()
    {
        yield return new WaitForSeconds(2f);
        ToMove();
    }

    private void ToMove()
    {
        move_delayTime = Random.Range(5f, 8f);
        move_lastTime = Time.time;
        playType = Boss_PlayType.Move;
    }


    enum Boss_PlayType
    {
        Spawn,
        Move,
        Move_Wait,
        Skill,
        Skill_Using,
        Die
    }
}