using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Coffee.UIExtensions.UIParticleAttractor;

public class Monster_Boss_3 : Boss
{
    Animator ani;
    GameObject player;

    [SerializeField]
    Boss_PlayType playType;

    bool aniFlag;

    Vector3 Move_Init_Position;

    float move_lastTime = 0f;
    float attack_lastTime = 0f;
    float move_delayTime = 5f;
    float attack_delayTime;

    int skillNum;

    [SerializeField]
    float speed;
    private Vector2 moveDir;
    private float changeTimer = 0f;
    float directionChangeInterval = 1.0f; // ���� ���� �ֱ�
    float randomAngleRange = 45f;
    // Start is called before the first frame update

    public Transform Monster_List_Sky;
    public GameObject SmallBee;
    public GameObject SoldierBee;
    public GameObject BeeShield;
    public SoldierBeeSkill SkillSoldierBee;
    public GameObject BeeHome;
    public GameObject BeeSlowObject;

    bool beeShieldFlag;
    public ParticleSystem DieEffect;
    ParticleSystem[] alldieEffect;
    Vector3 DieEffectOriginPos;
    bool dieEffectFlag;

    protected override void Start()
    {
        Boss_Num = 3;
        base.Start();

        playType = Boss_PlayType.Spawn;
        col.enabled = false;
        moveDir = Random.insideUnitCircle.normalized;

        ani = GetComponent<Animator>();
        aniFlag = false;
        Monster_List_Sky = GameObject.Find("Monster_List(Sky)").transform;
        player = GameObject.FindWithTag("Player");

        DieEffectOriginPos = DieEffect.transform.localPosition;
        alldieEffect = DieEffect.GetComponentsInChildren<ParticleSystem>();
        attack_delayTime = 4f;
        BeeShield.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void FixedUpdate()
    {
        Fire_Debuff();
        FlipMonster();
        if (playType == Boss_PlayType.Spawn)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            transform.Translate(-12f * Time.deltaTime, 0, 0, Space.World);

            if (transform.position.x < MonsterDirector.MinPos_Ground.x + 2f)
            {
                playType = Boss_PlayType.Move;
                transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                col.enabled = true;
                move_lastTime = Time.time;
                attack_lastTime = Time.time;

                Move_Init_Position = transform.position;
            }
        }

        if (playType == Boss_PlayType.Move)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }
            BossMove();

            
            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                SpawnBee();
                //��ȯ��
                attack_lastTime = Time.time;
            }

            if (Time.time >= move_lastTime + move_delayTime - 1.2f)
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
                ResetAni();
            }
        }

        if (playType == Boss_PlayType.Skill)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            if (!beeShieldFlag)
            {
                skillNum = Random.Range(0, 5);
            }
            else
            {
                skillNum = Random.Range(1, 5);
            }

            if (skillNum == 0)
            {
                StartCoroutine(BeeShieldSkill());
            }

            if (skillNum == 1)
            {
                StartCoroutine(BeeAttack());
            }

            if (skillNum == 2)
            {
                StartCoroutine(BeeHeal());
            }

            if (skillNum == 3)
            {
                StartCoroutine(BeeSpawn());
            }

            if (skillNum == 4)
            {
                StartCoroutine(BeeSlow());
            }

            playType = Boss_PlayType.SKill_Using;
        }

        if (playType == Boss_PlayType.SKill_Using)
        {
            if(skillNum == 0)
            {
                transform.Translate(Vector2.up * 2f * Time.deltaTime);
            }
            if (skillNum == 1)
            {
                float offset = Mathf.Sin(Time.time * 10f) * 3f; // 2 : ���ǵ�, 1.0f �̵��Ÿ�
                Vector2 movement = new Vector2(0f, offset);
                transform.Translate(movement * Time.deltaTime);
                // ���� ��ų ��� �� ����
            }
            if (skillNum == 2)
            {
                float offset = Mathf.Sin(Time.time * 10f) * 3f; // 2 : ���ǵ�, 1.0f �̵��Ÿ�
                Vector2 movement = new Vector2(0f, offset);
                transform.Translate(movement * Time.deltaTime);
                // ġ�� ��ų ��� �� ����
            }
            if (skillNum == 3)
            {
                transform.Translate(Vector2.up * 2f * Time.deltaTime);
                // ��ȯ ��ų ��� �� ����
            }
            if (skillNum == 4)
            {
                transform.Translate(Vector2.up * 2f * Time.deltaTime);
                // ������ �ϴ� ��ų ��� �� ����
            }

            // ��ų ��� �� ����
        }

        if (DieFlag)
        {
            if (playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                Destroy(gameObject, 10f);
            }
        }

        if (playType == Boss_PlayType.Die)
        {
            Vector3 movement;
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

    void BossMove()
    {
        // �̵�
        transform.Translate(moveDir * speed * Time.deltaTime);

        // ���� ��� üũ
        Vector3 pos = transform.position;
        if (pos.x < MonsterDirector.MinPos_Sky.x || pos.x > MonsterDirector.MaxPos_Sky.x)
        {
            moveDir.x *= -1; // �¿� �ݻ�
            pos.x = Mathf.Clamp(pos.x, MonsterDirector.MinPos_Sky.x, MonsterDirector.MaxPos_Sky.x);
            transform.position = pos;
        }
        if (pos.y < MonsterDirector.MinPos_Sky.y + 1.5f || pos.y > MonsterDirector.MaxPos_Sky.y + 1.5f)
        {
            moveDir.y *= -1; // ���� �ݻ�
            pos.y = Mathf.Clamp(pos.y, MonsterDirector.MinPos_Sky.y + 1.5f, MonsterDirector.MaxPos_Sky.y + 1.5f);
            transform.position = pos;
        }

        // ���� �ֱ⸶�� ���� ���� ��ȭ
        changeTimer += Time.deltaTime;
        if (changeTimer >= directionChangeInterval)
        {
            changeTimer = 0f;
            directionChangeInterval = Random.Range(0.2f, 0.5f);
            speed = Random.Range(8f, 16f);
            // ���� ���⿡�� ��randomAngleRange ���� ���� ȸ��
            float angle = Random.Range(-randomAngleRange, randomAngleRange);
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            Vector2 newDir = new Vector2(moveDir.x * cos - moveDir.y * sin,
                                         moveDir.x * sin + moveDir.y * cos).normalized;
            moveDir = newDir;
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
            ani.SetBool("Skill", false);
            ani.SetTrigger("Move");
        }

        if (playType == Boss_PlayType.Skill)
        {
            ani.SetBool("Skill", true);
        }

        if (playType == Boss_PlayType.Die)
        {
            ani.SetTrigger("Die");
        }
    }

    public void SpawnBee()
    {
        int rnd = Random.Range(0, 11);
        if(rnd >= 0 && rnd < 7)
        {
            Instantiate(SmallBee, new Vector3(transform.position.x, transform.position.y - 1f, 0), Quaternion.identity, Monster_List_Sky);
        }
        else
        {
            Instantiate(SoldierBee, new Vector3(transform.position.x, transform.position.y - 1f, 0), Quaternion.identity, Monster_List_Sky);
        }
    }

    IEnumerator BeeShieldSkill()
    {
        yield return new WaitForSeconds(1f);
        beeShieldFlag = true;
        BeeShield.SetActive(true);
        ToMove();
    }

    public void DestroyBeeShiled()
    {
        BeeShield.SetActive(false);
        beeShieldFlag = false;
    }

    IEnumerator BeeAttack()
    {
        int RandomAttackNum = Random.Range(0, 3);
        // 0 ���� ����
        // 1 �밢�� �밢��
        // 2 �÷��̾� �߽� ����
        if (RandomAttackNum == 0)
        {
            for(int i = -2; i < 3; i++)
            {
                SoldierBeeSkill skill =  Instantiate(SkillSoldierBee, new Vector3(player.transform.position.x - (i * 3), player.transform.position.y + 10f, 0), Quaternion.identity, Monster_List_Sky);
                skill.SetAngle(Bullet_Atk, 0f);
            }
            yield return new WaitForSeconds(0.6f);
            for(int i = 0; i < 3; i++)
            {
                SoldierBeeSkill skill = Instantiate(SkillSoldierBee, new Vector3(player.transform.position.x - 10f, MonsterDirector.MinPos_Ground.y + 0.4f + (i * 2.5f)), Quaternion.identity, Monster_List_Sky);
                skill.SetAngle(Bullet_Atk, 90f);
            }
        }
        else if (RandomAttackNum == 1)
        {
            for (int i = -2; i < 3; i++)
            {
                SoldierBeeSkill skill = Instantiate(SkillSoldierBee, new Vector3(player.transform.position.x + 8f - (i * 2), player.transform.position.y + 8f, 0), Quaternion.identity, Monster_List_Sky);
                skill.SetAngle(Bullet_Atk, -45f);
            }
            yield return new WaitForSeconds(0.6f);
            for(int i = -2; i < 3; i++)
            {
                SoldierBeeSkill skill = Instantiate(SkillSoldierBee, new Vector3(player.transform.position.x - 8f + (i * 2), player.transform.position.y + 8f, 0), Quaternion.identity, Monster_List_Sky);
                skill.SetAngle(Bullet_Atk, 45f);
            }

        }
        else if (RandomAttackNum == 2)
        {
            Vector2 PlayerPos = player.transform.position;
            for (int i = -4; i < 5; i++)
            {
                SoldierBeeSkill skill = Instantiate(SkillSoldierBee, new Vector3(PlayerPos.x + (i * 2), PlayerPos.y + 8f, 0), Quaternion.identity, Monster_List_Sky);
                skill.SetPlayer(Bullet_Atk, player_pos);
                yield return new WaitForSeconds(0.3f);
            }
        }

        yield return new WaitForSeconds(1f);
        ToMove();
    }

    IEnumerator BeeHeal()
    {
        int healCount = 0;
        for(int i = 0; i < Monster_List_Sky.childCount; i++)
        {
            if(Monster_List_Sky.GetChild(i).GetComponent<Monster>().GetMonsterNum() == 23)
            {
                Monster_List_Sky.GetChild(i).GetComponent<Monster_23>().HealBossSet(this.gameObject);
                healCount++;
                yield return new WaitForSeconds(0.3f);
            }

            if (healCount > 6)
            {
                break;
            }
        }
        yield return new WaitForSeconds(1f);
        ToMove();
    }

    public void BossHPHeal()
    {
        Monster_HP += 100;
        if (Monster_HP > Monster_Max_HP)
        {
            Monster_HP = Monster_Max_HP;
        }
    }

    IEnumerator BeeSpawn()
    {
        float rndX = Random.Range(MonsterDirector.MinPos_Sky.x  + 2f, MonsterDirector.MaxPos_Sky.x - 2f);
        GameObject beehome = Instantiate(BeeHome, new Vector2(rndX, MonsterDirector.MaxPos_Sky.y), Quaternion.identity);
        beehome.GetComponent<BeeHome>().Set(Monster_List_Sky);
        yield return new WaitForSeconds(1f);
        ToMove();
    }

    IEnumerator BeeSlow()
    {
        GameObject slowobject = Instantiate(BeeSlowObject, new Vector2(player.transform.position.x, MonsterDirector.MinPos_Ground.y - 0.05f), Quaternion.identity);
        Destroy(slowobject, 10f);
        yield return new WaitForSeconds(1f);
        ToMove();
    }

    private void ToMove()
    {
        if (playType != Boss_PlayType.Die)
        {
            move_delayTime = Random.Range(4f, 10f);
            skilleffect_flag = false;
            move_lastTime = Time.time;
            ResetAni();
            playType = Boss_PlayType.Move;
        }
    }

    private void FlipMonster()
    {
        if (player.transform.position.x - transform.position.x< 0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(-AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }


    }
    IEnumerator DieCorutine()
    {
        dieEffectFlag = true;
        Vector2 diePos = new Vector2(
           DieEffectOriginPos.x + Random.Range(-0.8f, 0.8f),
           DieEffectOriginPos.y + Random.Range(-0.8f, 0.8f)
        );
        DieEffect.transform.localPosition = diePos;

        foreach (var ps in alldieEffect)
        {
            var emission = ps.emission;

            ParticleSystem.Burst burst = emission.GetBurst(0);
            float burstCount = burst.count.constant; // �ν����� Burst�� ���� ��

            ps.Emit((int)burstCount);
        }

        float EmitTime = Random.Range(0.1f, 0.2f);
        yield return new WaitForSeconds(EmitTime); // 0.2�� �������� ����
        dieEffectFlag = false;
    }
    enum Boss_PlayType
{
    Spawn,
    Move,
    Move_Wait,
    Skill,
    SKill_Using,
    Die
    }
}
