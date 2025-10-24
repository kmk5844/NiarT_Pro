using System.Collections;
using UnityEngine;

public class Monster_Boss_0 : Boss
{
    Animator ani;
    public Transform Fire_Zone;
    public Transform MacinGun_Fire_Zone;
    public Transform Missile_Fire_Zone;
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
    int skillNum;

    public GameObject Skill_0_Bullet;
    public GameObject Skill_3_Bullet;

    public ParticleSystem Default_Effect;
    public ParticleSystem MachineGun_Effect;
    public ParticleSystem Missile_Effect;
    public ParticleSystem DieEffect;
    ParticleSystem[] alldieEffect;
    Vector3 DieEffectOriginPos;
    bool dieEffectFlag;

    protected override void Start()
    {
        Boss_Num = 0;
        base.Start();
        move_xPos = 1f;
        move_speed = 3f;
        player = GameObject.FindWithTag("Player");
        transform.position = new Vector3(MonsterDirector.MaxPos_Sky.x + 18f, 10f, 0);
        transform.rotation = Quaternion.Euler(0,0,20);
        transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
        playType = Boss_PlayType.Spawn;

        DieEffectOriginPos = DieEffect.transform.localPosition;
        alldieEffect = DieEffect.GetComponentsInChildren<ParticleSystem>();
        ani = GetComponent<Animator>();
        col.enabled = false;

        move_delayTime = 10f;
        attack_delayTime = 0.6f;
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
            if(transform.position.x < player_pos.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, -20);
                transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
            }
            else if(transform.position.x > player_pos.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 20);
                transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
            }

            if (LR_Flag)
            {
                transform.Translate(-15f * Time.deltaTime, 0, 0, Space.World);
                if (transform.position.x < MonsterDirector.MinPos_Sky.x)
                {
                    LR_Flag = false;
                }
            }
            else
            {
                transform.Translate(15f * Time.deltaTime, 0, 0, Space.World);

                if (transform.position.x > MonsterDirector.MaxPos_Sky.x) 
                {
                    LR_Flag = true;
                }
            }

            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                GameObject defaultBullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity, monster_Bullet_List);
                defaultBullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 20, 0);
                Default_Effect.Play();
                attack_lastTime = Time.time;
            }

            if(Time.time >= move_lastTime + move_delayTime - 1.2f)
            {
                if (!skilleffect_flag)
                {
                    SkillEffect.Play();
                    skilleffect_flag = true;
                }
            }


            if (Time.time >= move_lastTime + move_delayTime)
            {
                playType = Boss_PlayType.Skill;
            }
        }

        if (playType == Boss_PlayType.Skill)
        {
            skillNum = Random.Range(0, 4);
            if(skillNum == 0)
            {
                StartCoroutine(RandomBomb());
            }
            else if(skillNum == 1)
            {
                StartCoroutine(MachineGun_Sky_Player());
            }
            else if(skillNum == 2)
            {
                StartCoroutine(MachineGun_Ground_Player());
            }else if (skillNum == 3)
            {
                StartCoroutine(PlayerBomb());
            }
            playType = Boss_PlayType.Skill_Using;
        }

        if(playType == Boss_PlayType.Skill_Using)
        {

            if(skillNum == 0)
            {
                if (transform.position.x < player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, -20);
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
                else if (transform.position.x > player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 20);
                    transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
                }

                if (LR_Flag)
                {
                    transform.Translate(-7f * Time.deltaTime, 0, 0, Space.World);
                }
                else
                {
                    transform.Translate(7f * Time.deltaTime, 0, 0, Space.World);
                }
            }else if (skillNum == 1)
            {
                if (transform.position.x < player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, -20);
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
                else if (transform.position.x > player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 20);
                    transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
                }

                float offset = Mathf.Sin(Time.time * 10f) * 3f; // 2 : 스피드, 1.0f 이동거리
                Vector2 movement = new Vector2(0f, offset);
                transform.Translate(movement * Time.deltaTime);
            }else if(skillNum == 2)
            {
                //없음
            }else if (skillNum == 3)
            {
                if (transform.position.x < player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, -20);
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
                else if (transform.position.x > player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 20);
                    transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
                }

                float offset = Mathf.Sin(Time.time * 10f) * 3f; // 2 : 스피드, 1.0f 이동거리
                Vector2 movement = new Vector2(0f, offset);
                transform.Translate(movement * Time.deltaTime);
            }
        }

        if (DieFlag)
        {
            if(playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                Destroy(gameObject, 10f);
            }
        }

        if (playType == Boss_PlayType.Die)
        {
            if (!dieEffectFlag)
            {
                ani.SetTrigger("Die");
                StartCoroutine(DieCorutine());
            }

            //DieEffect.Emit(9);
            movement = new Vector3(-3f, -7f, 0f);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    IEnumerator DieCorutine()
    {
        dieEffectFlag = true;
        Vector2 diePos = new Vector2(
           DieEffectOriginPos.x + Random.Range(-2f, 2f),
           DieEffectOriginPos.y + Random.Range(-1f, 1f)
        );
        DieEffect.transform.localPosition = diePos;

        foreach (var ps in alldieEffect)
        {
            var emission = ps.emission;

            ParticleSystem.Burst burst = emission.GetBurst(0);
            float burstCount = burst.count.constant; // 인스펙터 Burst에 적힌 값

            ps.Emit((int)burstCount);
        }

        float EmitTime = Random.Range(0.1f, 0.2f);
        yield return new WaitForSeconds(EmitTime); // 0.2초 간격으로 터짐
        dieEffectFlag = false;
    }

    IEnumerator RandomBomb()
    {
        float x1 = MonsterDirector.MinPos_Ground.x;
        float x2 = MonsterDirector.MaxPos_Ground.x;

        for (int i = 0; i < 15; i++)
        {
            float x = Random.Range(x1, x2);
            Skill_0_Bullet.GetComponent<Boss2_MissileSkill>().PlayerTarget = false;
            Instantiate(Skill_0_Bullet, new Vector3(x, 20, 0),Quaternion.Euler(0,0,-180), monster_Bullet_List.transform);
            yield return new WaitForSeconds(0.2f);
        }
        ToMove();
    }

    IEnumerator MachineGun_Sky_Player()
    {
        for (int i = 0; i < 30; i++)
        {
            float RandomY = Random.Range(-1f, 1f);
            Vector3 newPos = new Vector3(Fire_Zone.transform.position.x, Fire_Zone.transform.position.y + RandomY, Fire_Zone.transform.position.z);
            Default_Effect.Play();
            GameObject defaultBullet = Instantiate(Boss_Bullet, newPos, Quaternion.identity, monster_Bullet_List);
            defaultBullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk/2, 1, 20, 0);
            attack_lastTime = Time.time;
            yield return new WaitForSeconds(0.1f);
        }
        ToMove();
    }

    IEnumerator MachineGun_Ground_Player()
    {
        while (true)
        {
            if(transform.position.y < 23f)
            {
                transform.Translate(Vector2.up * Time.deltaTime * 10, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                transform.position = new Vector3(MonsterDirector.MinPos_Ground.x - 10f, transform.position.y, transform.position.z);
                break;
            }
            yield return null;
        }

        while (true)
        {
            if (transform.position.y > 1f)
            {
                transform.Translate(Vector2.down * Time.deltaTime * 16, Space.World);
            }
            else
            {
                break;
            }
            yield return null;
        }

        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                float RandomY = Random.Range(-0.2f, 0.7f);
                Vector3 newPos = new Vector3(MacinGun_Fire_Zone.transform.position.x, MacinGun_Fire_Zone.transform.position.y + RandomY, MacinGun_Fire_Zone.transform.position.z);
                MachineGun_Effect.Play();
                GameObject defaultBullet = Instantiate(Skill_3_Bullet, newPos, Quaternion.identity, monster_Bullet_List);
                defaultBullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk/4, Bullet_Slow, 20, 1);
                yield return new WaitForSeconds(0.03f);
            }
            yield return new WaitForSeconds(1f);
        }
        while (true)
        {
            if(transform.position.y < 10f)
            {
                transform.Translate(Vector2.up * Time.deltaTime * 15, Space.World);
            }
            else
            {
                if (transform.position.x < player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, -20);
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
                else if (transform.position.x > player_pos.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 20);
                    transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
                }
                break;
            }
            yield return null;
        }
        ToMove();
    }

    IEnumerator PlayerBomb()
    {
        for (int i = 0; i < 5; i++)
        {
            Skill_0_Bullet.GetComponent<Boss2_MissileSkill>().PlayerTarget = true;
            Missile_Effect.Play();
            Instantiate(Skill_0_Bullet, Missile_Fire_Zone.transform.position, Quaternion.identity, monster_Bullet_List);
            yield return new WaitForSeconds(0.5f);
        }
        ToMove();
    }

    private void ToMove()
    {
        if(playType != Boss_PlayType.Die)
        {
            transform.position = new Vector3(transform.position.x, 10f, 0);
            move_delayTime = Random.Range(5f, 8f);
            skilleffect_flag = false;
            move_lastTime = Time.time;
            playType = Boss_PlayType.Move;
        }
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