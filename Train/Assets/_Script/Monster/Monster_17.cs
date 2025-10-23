using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_17 : Monster
{
    Vector3 movement;
    float xPos;

    [SerializeField]
    bool AttackFlag;
    Animator anicon;

    float speed = 2f;      // ������ �̵� �ӵ�
    float jumpHeight = 1.5f;   // ���� ����
    float jumpFrequency = 3f;  // ���� �ֱ� (Sin �ֱ� �ӵ�)

    private Vector3 startPos;
    private float elapsedTime = 0f;
    bool bombflag = false;
    bool loading = false;
    int count = 0;

    protected override void Start()
    {
        Monster_Num = 17;
        Bullet_Delay = 6; // ������ ��� ī��Ʈ�� ���� ���� 
        //BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();
        //transform.localPosition = MonsterDirector.MaxPos_Sky;
        MonsterDirector_Pos = transform.localPosition;
        Spawn_Init_Pos =
                new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 5f);
        transform.localPosition = Spawn_Init_Pos;

        speed = 4f;
        jumpHeight = Random.Range(1.5f, 5f);
        jumpFrequency = Random.Range(3f, 5f);
        xPos = -1f;
        AttackFlag = false;

        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
        anicon = GetComponent<Animator>();
        anicon.SetTrigger("Jump");
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        Check_ItemSpeedFlag();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (!AttackFlag)
            {
                MonsterMove();
            }
            FlipMonster();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (AttackFlag && !bombflag)
            {
                BombReady();
                bombflag = true;
            }
        }
    }

    void BombReady()
    {
        if (!warningFlag)
        {
            WarningEffect.Play();
            warningFlag = true;
        }

        anicon.SetBool("AttackFlag", true);
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = 1f;
        float height = Random.Range(3f, 5f);
        base.WalkEffect.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x, t);
            float yPos = Mathf.Sin(Mathf.PI * t) * height + Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y, t); ;
            transform.localPosition = new Vector2(xPos, yPos);
            //Debug.Log(yPos);

            yield return null;
        }
        anicon.SetTrigger("Jump");
        transform.localPosition = MonsterDirector_Pos;
        startPos = new Vector2(transform.position.x, transform.position.y + 0.3f);
        monster_gametype = Monster_GameType.Fighting;
        Monster_coroutine = null;
    }

    void MonsterMove()
    {
        elapsedTime += Time.deltaTime * jumpFrequency;


        // Sin ������ ���Ʒ� ���� (0~1 ������ ���� �ڿ�������)
        float yOffset = Mathf.Abs(Mathf.Sin(elapsedTime)) * jumpHeight;

        // X ���� �̵�
        float xOffset = xPos * speed * Time.deltaTime;

        transform.Translate(xOffset, yOffset - (transform.position.y - startPos.y), 0f);


        if (!loading && Mathf.Sin(elapsedTime) <= 0f)
        {
            loading = true;
            jumpHeight = Random.Range(1.5f, 5f);
            jumpFrequency = Random.Range(3f, 5f);
            // �� �ȿ��� ���� ���� ó�� (��: ���� ��ȯ, ���� ���� ��)
        }

        // ���� ������ ���۵Ǹ� �ٽ� false�� �ʱ�ȭ
        if (elapsedTime >= Mathf.PI)
        {
            elapsedTime = 0f;
            anicon.SetTrigger("Jump");
            count++;
            if (count > Bullet_Delay)
            {
                AttackFlag = true;
            }
            loading = false; // ���� ���� ���� ����
        }

        // ��� ���� �� ���� ����
        if (transform.position.x >= MonsterDirector.MaxPos_Ground.x - 2f)
        {
            xPos = -1f; // ������ �Ѱ� �� ���� �̵�
        }
        else if (transform.position.x <= MonsterDirector.MinPos_Ground.x + 2f)
        {
            xPos = 1f;  // ���� �Ѱ� �� ������ �̵�
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    void Check_ItemSpeedSpawn()
    {
        if (MonsterDirector.Item_curseFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_cursePersent_Spawn;
            Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
        }
        else if (MonsterDirector.Item_giantFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_giantPersent_Spawn;
            Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
        }
        else
        {
            Item_Monster_Speed = 0;
        }
    }

    void Check_ItemSpeedFlag()
    {
        Item_Monster_Speed_ChangeFlag = base.Item_Monster_Speed_ChangeFlag;
        Item_Monster_SpeedFlag = base.Item_Monster_SpeedFlag;
        if (Item_Monster_Speed_ChangeFlag)
        {
            if (Item_Monster_SpeedFlag)
            {
                Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
                Item_Monster_Speed_ChangeFlag = false;
            }
            else
            {
                Item_Monster_Speed = 0;
                Item_Monster_Speed_ChangeFlag = false;
            }
        }
    }
}
