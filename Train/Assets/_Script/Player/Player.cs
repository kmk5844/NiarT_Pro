using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Player : MonoBehaviour
{
    GameObject gamedirector_object;
    GameDirector gamedirector;
    GameType gameDirectorType;
    Player_Chage playerchageDirector;
    UIDirector uidirector;
    
    public int PlayerNum;

    [SerializeField]
    private SA_PlayerData playerData;
    [SerializeField]
    private Player_Debuff playerDebuff;
    Train_InGame train;

    [Header("무기")]
    [SerializeField]
    private GameObject playerBullet;
    [SerializeField]
    int Bullet_Atk;
    int Default_Atk;
    [SerializeField]
    float Bullet_Delay;
    public Transform Bullet_Fire_Transform;
    Transform Player_Bullet_List;
    float lastTime;

    [Header("체력")]
    public int Player_HP;
    private int Max_HP;

    [Header("방어력")]
    [SerializeField]
    int Player_Armor;
    float era;
    [SerializeField]
    float def_constant;

    [Header("이동 속도")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    Rigidbody2D rigid;
    //int moveX;
    bool rotationOn;
    bool jumpFlag;
    bool isMouseDown;
    float jumpdistance;
    float jumpFlagDistance;

    [Header("상호작용")]
    public bool isHealing;
    public bool isSelfTurretAtacking;
    [HideInInspector]
    public Vector3 maxRespawnPosition;
    [HideInInspector]
    public Vector3 minRespawnPosition;

    [Header("무기 오브젝트")]
    public GameObject GunObject;
    Vector3 GunObject_Scale;
    Camera mainCam;
    private Vector3 mousePos;
    public AudioClip ShootSFX;
    public AudioClip FlashBangSFX;
    public AudioClip FireSFX;
    public AudioClip LaserSFX;
    public List<GameObject> GunObject_List;
    int GunIndex;

    private AudioSource _sfxSource;

    //아이템 부분
    [Header("아이템 장착 부분")]
    public Transform ItemTransform;
    int item_Atk;
    float item_Delay;
    Vector2 minGroundPos;
    Vector2 maxGroundPos;
    Vector2 minSkyPos;
    Vector2 maxSkyPos;
    [HideInInspector]
    public static bool Item_GunFlag; // True : 보급 몬스터가 총 관련된 아이템 떨구지 않도록 방지
    public bool Item_Gun_TimeFlag;
    public static float Item_Gun_ClickTime;
    float Item_Gun_Max_ClickTime;
    public bool Item_Gun_CountFlag;
    public static int Item_Gun_ClickCount;
    int Item_Gun_Max_ClickCount;
    public GameObject Item_GunSpecial_Bullet;
    bool Item_GunSpecial_BulletFlag;

    bool MariGold_Skill_Flag;
    bool MariGold_Skill_Fire_Flag;

    [Header("상호작용 Key")]
    public GameObject KeyObject;
    Vector3 KeyObject_Scale;
    int KeyCount;

    Animator ani;

    void Start()
    {
        gamedirector_object = GameObject.Find("GameDirector");
        gamedirector = gamedirector_object.GetComponent<GameDirector>();
        uidirector = gamedirector.UI_DirectorObject.GetComponent<UIDirector>();
        playerchageDirector = GetComponent<Player_Chage>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ani = GetComponent<Animator>();
        isHealing = false;
        jumpFlag = false;
        jumpdistance = 1f;

        playerBullet = playerData.Bullet;
        Player_HP = playerData.HP;
        Player_Armor = playerData.Armor;
        Bullet_Atk = playerData.Atk;
        Bullet_Delay = playerData.Delay;
        moveSpeed = playerData.MoveSpeed;
        PlayerNum = playerData.Player_Num;
        playerchageDirector.ChangePlayer(PlayerNum);
        Bullet_Fire_Transform = playerchageDirector.Set_FireZone(PlayerNum);
        //GetComponentInChildren<InputManager>().FireZoneTransform = Bullet_Fire_Transform;

        GunObject_Scale = GunObject.transform.localScale;
        KeyObject_Scale = KeyObject.transform.localScale;

        rotationOn = false;
        Max_HP = Player_HP; 
        lastTime = 0;
        rigid = GetComponent<Rigidbody2D>();
        Player_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        Level();
        era = 1f - (float)Player_Armor / def_constant;

        isHealing = false;
        isSelfTurretAtacking = false;

        GunIndex = 0;
        KeyCount = 0;

        item_Atk = 0;
        item_Delay = 0;

        Item_GunFlag = false;
        Item_Gun_TimeFlag = false;
        Item_Gun_ClickTime = 0;
        Item_Gun_CountFlag = false;
        Item_Gun_ClickCount = 0;

        MariGold_Skill_Flag = false;
    }

    private void Update()
    {
        gameDirectorType = gamedirector.gameType;

        if (gameDirectorType == GameType.Starting ||  gameDirectorType == GameType.Playing || gameDirectorType == GameType.Boss)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMouseDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                isMouseDown = false;
            }

            if (isMouseDown)
            {
                gamedirector.ChangeCursor(true, true);
            }
            else
            {
                gamedirector.ChangeCursor(true, false);
            }
        }
        else if (gameDirectorType == GameType.Ending)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
            }
        }

        if (gamedirector.SpawnTrainFlag && train != null)
        {
            if (train.Train_Type.Equals("Medic"))
            {
                if (Check_HpParsent() < 75f && !isHealing)
                {
                    KeyObject.SetActive(true);
                }
                else
                {
                    KeyObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.F) && Check_HpParsent() < 75f && !isHealing && KeyCount == 0)
                {
                    if (train.Not_DestoryTrain)
                    {
                        if (!train.GetComponentInChildren<Medic_Train>().isMercenaryHealing)
                        {
                            OnOff_Sprite(true);
                            isHealing = true;
                            KeyCount = 1;
                        }
                        else
                        {
                            // 치료 중입니다.
                        }
                    }
                    else
                    {
                        //파괴되어 사용할 수 없습니다.
                    }
                }
            }else if (train.Train_Type.Equals("Dash"))
            {
                if (train.GetComponentInChildren<Dash_Train>().UseFlag)
                {
                    KeyObject.SetActive(true);
                }
                else
                {
                    KeyObject.SetActive(false);
                }

                if(Input.GetKeyDown(KeyCode.F) && train.GetComponentInChildren<Dash_Train>().UseFlag)
                {
                    train.GetComponentInChildren<Dash_Train>().UseDash();
                    StartCoroutine(Item_Player_SpeedUP(train.Train_Dash_PalyerAmount, train.Train_Dash_Second));
                }
            }else if (train.Train_Type.Equals("Supply"))
            {
                if (train.GetComponentInChildren<Supply_Train>().UseFlag)
                {
                    KeyObject.SetActive(true);
                }
                else
                {
                    KeyObject.SetActive(false);
                }

                if(Input.GetKeyDown(KeyCode.F) && train.GetComponentInChildren<Supply_Train>().UseFlag){
                    train.GetComponentInChildren<Supply_Train>().UseSupply();
                }
            }else if (train.Train_Type.Equals("Self_Turret"))
            {
                if (train.GetComponentInChildren<SelfTurret_Train>().UseFlag)
                {
                    KeyObject.SetActive(true);
                }
                else
                {
                    KeyObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.F) && train.GetComponentInChildren<SelfTurret_Train>().UseFlag)
                {
                    StartCoroutine(train.GetComponentInChildren<SelfTurret_Train>().UseSelfTurret());
                    OnOff_Sprite(true);
                    isSelfTurretAtacking = true;
                }
            }
            else
            {
                KeyObject.SetActive(false);
            }
        }

        if (isHealing) // 치료 중
        {
            IEnumerator train_Heal;
            train_Heal = train.Train_Healing();

            if (Check_HpParsent() < 75f && !train.isHealing)
            {
                if (KeyCount == 1)
                {
                    KeyCount = 2;
                }
                StartCoroutine(train_Heal);
            }
            else if (Check_HpParsent() >= 75f || !train.Not_DestoryTrain)
            {
                OnOff_Sprite(false);
                isHealing = false;
            }

            if(Input.GetKeyUp(KeyCode.F) && KeyCount == 2) {
                KeyCount = 3;
            }
            else if (Input.GetKeyDown(KeyCode.F) && KeyCount == 3)
            {
                StopCoroutine(train_Heal);
                OnOff_Sprite(false);
                train.isHealing = false;
                isHealing = false;
                KeyCount = 0;
            }
        }
        else if (isSelfTurretAtacking) // 터렛 공격 중
        {
            if (!train.GetComponentInChildren<SelfTurret_Train>().isAtacking)
            {
                OnOff_Sprite(false);
                isSelfTurretAtacking = false;
            }
        }
        else // 일반
        {
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
                ani.SetBool("Move", false);
            }else if (Input.GetButton("Horizontal"))
            {
                ani.SetBool("Move", true);
            }


            if (Input.GetButtonDown("Jump") && !jumpFlag)
            {
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                ani.SetTrigger("Jump");
            }

            if (isMouseDown)
            {
                if (Item_Gun_TimeFlag)
                {
                    Item_Gun_ClickTime += Time.deltaTime;
                    if (Item_Gun_ClickTime > Item_Gun_Max_ClickTime)
                    {
                        Item_Gun_Default();
                    }
                }

                switch (GunIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        BulletFire();
                        break;
                    case 4:
                    case 5:
                        SpecailBulletFire(true);
                        break;

                }
            }
            else
            {
                if (GunIndex == 4 || GunIndex == 5)
                {
                    SpecailBulletFire(false);
                }
            }
        }

        if (rigid.velocity.x > moveSpeed)
        {
            //moveX = 1;
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < moveSpeed * (-1))
        {
            //moveX = -1;
            rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
        }
    }

    void FixedUpdate()
    {
        if (isHealing) { }
        else if (isSelfTurretAtacking) { }
        else {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        }

        if (gameDirectorType == GameType.Starting || gameDirectorType == GameType.Playing || gameDirectorType == GameType.Boss || gameDirectorType == GameType.Ending)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rot = mousePos - GunObject.transform.position;
            float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
            GunObject.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (rotZ >= -90 && rotZ <= 90)
            {
                GunObject.transform.localScale = new Vector3(GunObject_Scale.x, GunObject_Scale.y, GunObject_Scale.z);
            }
            else
            {
                GunObject.transform.localScale = new Vector3(GunObject_Scale.x, -1 * GunObject_Scale.y, GunObject_Scale.z);
                //KeyObject.transform.localScale = KeyObject_Scale;
            }

            if (mousePos.x > transform.position.x)
            {
                rotationOn = true;
                KeyObject.transform.localScale = new Vector3(-KeyObject_Scale.x, KeyObject_Scale.y, KeyObject_Scale.z);
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                rotationOn = false;
                KeyObject.transform.localScale = new Vector3(KeyObject_Scale.x, KeyObject_Scale.y, KeyObject_Scale.z);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        Debug.DrawRay(rigid.position, Vector3.down * jumpdistance, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, jumpdistance, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null)
        {
            train = rayHit.collider.GetComponentInParent<Train_InGame>();
        }

        if (rayHit.collider != null && rayHit.distance >= jumpFlagDistance)
        {
            jumpFlag = false;
        }
        else
        {
            jumpFlag = true;
        }

        if(transform.position.y < -3f)
        {
            Respawn();
        }
    }
    void BulletFire()
    {
        if (Time.time >= lastTime + (Bullet_Delay + item_Delay))
        {
            GameObject bullet = playerBullet;
            bullet.GetComponent<Bullet>().atk = Bullet_Atk + item_Atk;
            if(PlayerNum == 0)
            {
                Instantiate(bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
                ani.SetTrigger("Shoot_0");
                if (MariGold_Skill_Flag && !MariGold_Skill_Fire_Flag)
                {
                    StartCoroutine(MariGold_Skill_BulletFire());
                }
            }
            else if(PlayerNum == 1) //페요테
            {
                if (!Item_GunFlag)
                {
                    Instantiate(bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
                    if (!Item_GunSpecial_BulletFlag)
                    {
                        for (int i = 0; i < 4; i++) // 5개의 샷건 탄환을 발사
                        {
                            Instantiate(bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
                        }
                    }
                    ani.SetTrigger("Shoot_1");
                }
                else
                {
                    Instantiate(bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
                }
            }
            else
            {
                Instantiate(bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
            }

            if (GunIndex == 1)
            {
                MMSoundManagerSoundPlayEvent.Trigger(FlashBangSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            }
            else
            {
                MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            }

            lastTime = Time.time;
            if (Item_Gun_CountFlag)
            {
                Item_Gun_ClickCount++;

                if (Item_Gun_ClickCount == Item_Gun_Max_ClickCount)
                {
                    Item_Gun_Default();
                }
            }
        }
    }

    void SpecailBulletFire(bool OnOff)
    {
        if (OnOff)
        {
            if(_sfxSource == null)
            {
                if (GunIndex == 4)
                {
                    _sfxSource = MMSoundManagerSoundPlayEvent.Trigger(LaserSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position, loop : true, ID: 21);
                }
                else if (GunIndex == 5)
                {
                    _sfxSource = MMSoundManagerSoundPlayEvent.Trigger(FireSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position, loop: true, ID: 22);
                }
            }
            Item_GunSpecial_Bullet.SetActive(true);
        }
        else
        {
            if(_sfxSource != null)
            {
                if (GunIndex == 4)
                {
                    MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 21, _sfxSource);
                }
                else if (GunIndex == 5)
                {
                    MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 22, _sfxSource);
                }
                _sfxSource = null;
            }
            Item_GunSpecial_Bullet.SetActive(false);
        }
    }

    void OnOff_Sprite(bool flag)
    {
        if (flag)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            transform.GetComponent<CapsuleCollider2D>().enabled = false;
            GunObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetComponent<CapsuleCollider2D>().enabled = true;
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GunObject.SetActive(true);
        }
    }
    void Level()
    {
        int Level_Atk = playerData.Level_Player_Atk;
        int Level_AtkDelay = playerData.Level_Player_AtkDelay;
        int Level_HP = playerData.Level_Player_HP;
        int Level_Armor = playerData.Level_Player_Armor;
        int Level_Speed = playerData.Level_Player_Speed;

        Bullet_Atk = Bullet_Atk + (((Bullet_Atk * Level_Atk * 5)) / 100);
        Default_Atk = Bullet_Atk;
        Bullet_Delay = Bullet_Delay - (((Bullet_Delay * Level_AtkDelay)) / 200);
        Player_HP = Player_HP + (((Player_HP * Level_HP) * 10) / 100);
        Player_Armor = Player_Armor + (((Player_Armor * Level_Armor) * 10) / 100);
        moveSpeed = moveSpeed + (((moveSpeed * Level_Speed)) / 100);
    }

    public void MonsterHit(int MonsterBullet_Atk)
    {
        int damageTaken = Mathf.RoundToInt(MonsterBullet_Atk * era);
        if (Player_HP - damageTaken < 0)
        {
            Player_HP = 0;
        }
        else
        {
            Player_HP -= damageTaken;
        }
    }

    void Respawn()
    {
        Player_HP -= (((Player_HP * 10) / 100) + 50);
        if (transform.position.x > 0)
        {
            transform.position = new Vector3(maxRespawnPosition.x, 1, 0);
        }
        else
        {
            transform.position = new Vector3(minRespawnPosition.x, 1, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            MonsterBullet bullet = collision.GetComponent<MonsterBullet>();
            MonsterHit(bullet.atk);
            if(bullet.bulletType != MonsterBulletType.Nomal)
            {
                playerDebuff.GetDebuff(bullet.bulletType);
            }
            Blood_Effect();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Monster_ShortAttack"))
        {
            Monster_ShortAtk short_info = collision.GetComponent<Monster_ShortAtk>();
            MonsterHit(short_info.Atk);
            Blood_Effect();
            ShortAtk_PlayerEffect(short_info.xPos, short_info.Force);
        }
    }

    void ShortAtk_PlayerEffect(float xPos, float force)
    {
        Vector2 v = new Vector2(xPos, 0.9f);
        rigid.AddForce(v * force, ForceMode2D.Impulse);
    }

    public float Check_HpParsent()
    {
        return (float)Player_HP / (float)Max_HP * 100f;
    }

    public void Heal_HP(int Medic_Heal)
    {
        Player_HP += Medic_Heal;
    }

    public void P_Buff(Bard_Type type, int amount, bool flag)
    {
        if (flag)
        {
            switch (type) {
                case Bard_Type.HP_Buff:
                    Player_HP += amount;
                    Max_HP += amount;
                    break;
                case Bard_Type.Atk_Buff:
                    Bullet_Atk += amount;
                    break;
                case Bard_Type.Def_Buff:
                    Player_Armor += amount;
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case Bard_Type.HP_Buff:
                    Player_HP -= amount;
                    Max_HP -= amount;
                    break;
                case Bard_Type.Atk_Buff:
                    Bullet_Atk -= amount;
                    break;
                case Bard_Type.Def_Buff:
                    Player_Armor -= amount;
                    break;
            }
        }
    }

    public void Blood_Effect()
    {
        uidirector.Player_Blood_Ani();
        ani.SetTrigger("Hurt");
    }

    public int GetMaxHP()
    {
        return Max_HP;
    }

    //Item 부분
    private void Check_Pos()
    {
        minSkyPos = MonsterDirector.MinPos_Sky;
        maxSkyPos = MonsterDirector.MaxPos_Sky;
        minGroundPos = MonsterDirector.MinPos_Ground;
        maxGroundPos = MonsterDirector.MaxPos_Ground;
    }

    public void Item_Player_Heal_HP(float persent)
    {
        Player_HP += (int)(Max_HP * (persent / 100f));
    }

    public void Item_Player_Minus_HP(float persent)
    {
        Player_HP -= (int)(Max_HP * (persent / 100f));
    }

    public IEnumerator Item_Player_SpeedUP(float speed, int delayTime)
    {
        moveSpeed += speed;
        yield return new WaitForSeconds(delayTime);
        moveSpeed -= speed;
    }

    public IEnumerator Item_Player_Heal_HP_Auto(float perseent, int delaytime)
    {
        while(delaytime > 0)
        {
            Item_Player_Heal_HP(perseent);
            yield return new WaitForSeconds(1);
            delaytime -= 1;
        }
    }

    public IEnumerator Item_Player_AtkUP(int AddAtk, int delayTime)
    {
        item_Atk += AddAtk;
        yield return new WaitForSeconds(delayTime);
        item_Atk -= AddAtk;
    }

    public IEnumerator Item_Player_AtkDelayDown(float atkdelaytime, int delayTime)
    {
        item_Delay -= atkdelaytime;
        yield return new WaitForSeconds(delayTime);
        item_Delay += atkdelaytime;
    }

    public IEnumerator Item_Player_DoubleAtkUP(int delayTime)
    {
        item_Atk += Bullet_Atk;
        yield return new WaitForSeconds(delayTime);
        item_Atk -= Bullet_Atk;
    }

    public void Item_Player_Ballon_Bullet()
    {
        GameObject ballon = Resources.Load<GameObject>("Bullet/Turret/Ballon_Bullet_Turret");
        ballon.GetComponent<Bullet>().atk = Bullet_Atk;
        Instantiate(ballon,transform.position, Quaternion.identity, Player_Bullet_List);
    }

    public void Item_Player_Giant_Tent()
    {
        Check_Pos();
        float pos = Random.Range(minGroundPos.x, maxGroundPos.x);
        GameObject tent = Resources.Load<GameObject>("ItemObject/Giant_TentObject");
        tent.GetComponent<Item_Shield>().HP = 1000;
        Instantiate(tent, new Vector2(pos, -1.2f), Quaternion.identity);
    }

    public void Item_Instantiate_Flag(int flagNum, float delayTime)
    {
        Destroy(Instantiate(Resources.Load<GameObject>("ItemObject/Flag" + flagNum), new Vector2(transform.position.x, -0.45f), Quaternion.identity), delayTime);
    }

    public void Item_Player_Spawn_Turret(int num)
    {
        Check_Pos();
        float pos = Random.Range(minGroundPos.x + 3.5f, maxGroundPos.x - 3.5f);
        switch (num)
        {
            case 0:
                Instantiate(Resources.Load<GameObject>("ItemObject/Mini_Auto_Turret"), new Vector2(pos, -0.55f), Quaternion.identity);
                break;
            case 1:
                GameObject Plant = Resources.Load<GameObject>("ItemObject/RobotPlant");
                Instantiate(Plant, new Vector2(pos, -0.45f), Quaternion.identity);
                break;
        }
    }

    public void Item_Player_Spawn_Dron(int num)
    {
        Check_Pos();
        switch (num)
        {
            case 0:
                GameObject MiniDron = Resources.Load<GameObject>("ItemObject/MiniDron");
                MiniDron.GetComponent<Item_MiniDron>().DronAtk = 15;
                Vector2 pos = new Vector2(minSkyPos.x - 2, (minSkyPos.y + maxSkyPos.y) / 2);
                Instantiate(MiniDron, pos, Quaternion.identity);
                break;
            case 1:
                GameObject MiniRaserDron = Resources.Load<GameObject>("ItemObject/MiniRaserDron");
                MiniRaserDron.GetComponent<Item_MiniDron>().DronAtk = 6;
                pos = new Vector2(minSkyPos.x -2, maxSkyPos.y + 1);
                Instantiate(MiniRaserDron, pos, Quaternion.identity);
                break;
        }
}
    public void Item_Player_Spawn_Shield(int num)
    {
        GameObject shield;
        switch (num)
        {
            case 0:
                shield = Resources.Load<GameObject>("ItemObject/MiniShield");
                shield.GetComponent<Item_Shield>().HP = 1000;
                Instantiate(shield, ItemTransform);
                break;
            case 1:
                shield = Resources.Load<GameObject>("ItemObject/HealingShield");
                shield.GetComponent<Item_Shield>().HP = 500;
                Instantiate(shield, ItemTransform);
                break;
        }
    }

    public IEnumerator Item_Player_Giant_GunAndBullet(float delayTime)
    {
        Vector3 GiantBullet = new Vector3(1f, 1f, 1f);
        playerBullet.transform.localScale = playerBullet.transform.localScale + GiantBullet;
        yield return new WaitForSeconds(delayTime);
        playerBullet.transform.localScale = playerBullet.transform.localScale - GiantBullet;
    }

    public void Item_Player_Spawn_Claymore()
    {
        GameObject ClaymoreObject = Resources.Load<GameObject>("ItemObject/Claymore");
        float Pos;
        if (rotationOn)
        {
            Pos = transform.position.x + 1;
        }
        else
        {
            Pos = transform.position.x - 1;
        }
        Instantiate(ClaymoreObject, new Vector2(Pos, -0.7f),Quaternion.identity);
    }

    public void Item_Player_Spawn_WireEntanglement()
    {
        GameObject WireEntanglementObject = Resources.Load<GameObject>("ItemObject/WireEntanglement");
        Instantiate(WireEntanglementObject, new Vector2(transform.position.x, -0.65f), Quaternion.identity);
    }

    public IEnumerator Item_Player_Dagger(float delayTime)
    {
        GameObject Dagger = Resources.Load<GameObject>("ItemObject/Dagger");
        yield return new WaitForSeconds(delayTime + 1);
        Instantiate(Dagger, new Vector2(transform.position.x, transform.position.y ), Quaternion.Euler(0, 0, 180), Player_Bullet_List);
        yield return new WaitForSeconds(delayTime);
        Instantiate(Dagger, new Vector2(transform.position.x, transform.position.y ), Quaternion.Euler(0, 0, 135), Player_Bullet_List);
        yield return new WaitForSeconds(delayTime);
        Instantiate(Dagger, new Vector2(transform.position.x, transform.position.y ), Quaternion.Euler(0, 0, 90), Player_Bullet_List);
        yield return new WaitForSeconds(delayTime);
        Instantiate(Dagger, new Vector2(transform.position.x, transform.position.y ), Quaternion.Euler(0, 0, 45), Player_Bullet_List);
        yield return new WaitForSeconds(delayTime);
        Instantiate(Dagger, new Vector2(transform.position.x, transform.position.y ), Quaternion.Euler(0, 0, 0), Player_Bullet_List);
    }

    public IEnumerator Item_Change_Bullet(string BulletName, int delayTime)
    {
        Item_GunSpecial_BulletFlag = true;
        playerBullet = Resources.Load<GameObject>("Bullet/Player/Special/" + BulletName);
        yield return new WaitForSeconds(delayTime);
        Item_GunSpecial_BulletFlag = false;
        playerBullet = playerData.Bullet;
    }

    public void Item_Gun_Change(string Weapon, int max)
    {
        Item_GunFlag = true;
        GunObject_List[0].SetActive(false);
        switch (Weapon) { 
            case "Flash_Bang":
                GunIndex = 1;
                GunObject_List[GunIndex].SetActive(true);
                Bullet_Fire_Transform = GunObject_List[GunIndex].GetComponent<Transform>().GetChild(0);
                playerBullet = Resources.Load<GameObject>("Bullet/Player/Special/Flash_Bang_Bullet");
                Item_Gun_CountFlag = true;
                Item_Gun_ClickCount = 0;
                Item_Gun_Max_ClickCount = max;
                break;
            case "Gatling_Gun":
                GunIndex = 2;
                GunObject_List[GunIndex].SetActive(true);
                Bullet_Fire_Transform = GunObject_List[GunIndex].GetComponent<Transform>().GetChild(0);
                Bullet_Atk = 10;
                Bullet_Delay = 0.1f;
                Item_Gun_TimeFlag = true;
                Item_Gun_ClickTime = 0f;
                Item_Gun_Max_ClickTime = max;
                break;
            case "Missile_Gun":
                GunIndex = 3;
                GunObject_List[GunIndex].SetActive(true);
                Bullet_Fire_Transform = GunObject_List[GunIndex].GetComponent<Transform>().GetChild(0);
                playerBullet = Resources.Load<GameObject>("Bullet/Player/Special/Missile_Player_Bullet");
                Bullet_Atk = 35;
                Item_Gun_CountFlag = true;
                Item_Gun_ClickCount = 0;
                Item_Gun_Max_ClickCount = max;
                break;
            case "Raser_Gun":
                GunIndex = 4;
                GunObject_List[GunIndex].SetActive(true);
                Item_GunSpecial_Bullet = GunObject_List[GunIndex].GetComponent<Transform>().GetChild(0).gameObject;
                Item_GunSpecial_Bullet.GetComponent<Bullet>().atk = 8;
                Item_Gun_TimeFlag = true;
                Item_Gun_ClickTime = 0f;
                Item_Gun_Max_ClickTime = max;
                break;
            case "Fire_Gun":
                GunIndex = 5;
                GunObject_List[GunIndex].SetActive(true);
                Item_GunSpecial_Bullet = GunObject_List[GunIndex].GetComponent<Transform>().GetChild(0).gameObject;
                Item_GunSpecial_Bullet.GetComponent<Bullet>().atk = 10;
                Item_Gun_TimeFlag = true;
                Item_Gun_ClickTime = 0f;
                Item_Gun_Max_ClickTime = max;
                break;
        }
    }

    private void Item_Gun_Default()
    {
        Item_GunFlag = false;
        Item_Gun_TimeFlag = false;
        Item_Gun_CountFlag = false;

        GunObject_List[GunIndex].SetActive(false);
        switch (GunIndex) {
            case 1:
                Bullet_Atk = Default_Atk;
                playerBullet = playerData.Bullet;
                break;
            case 2:
                Bullet_Atk = Default_Atk;
                Bullet_Delay = playerData.Delay;
                break;
            case 3:
                Bullet_Atk = Default_Atk;
                playerBullet = playerData.Bullet;
                break;
            case 4:
            case 5:
                SpecailBulletFire(false);
                Item_GunSpecial_Bullet = null;
                break;

        }
        GunIndex = 0;
        GunObject_List[GunIndex].SetActive(true);
        Bullet_Fire_Transform = GunObject_List[GunIndex].GetComponent<Transform>().GetChild(0);
    }

    public IEnumerator MariGold_Skill2(float during)
    {
        MariGold_Skill_Flag = true;
        yield return new WaitForSeconds(during);
        MariGold_Skill_Flag = false;
    }

    IEnumerator MariGold_Skill_BulletFire()
    {
        MariGold_Skill_Fire_Flag = true;
        GameObject bullet = playerBullet;
        bullet.GetComponent<Bullet>().atk = Bullet_Atk + item_Atk;
        yield return new WaitForSeconds(0.08f);
        Instantiate(bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
        MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        yield return new WaitForSeconds(0.08f);
        Instantiate(bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
        ani.SetTrigger("Shoot_0");
        MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        MariGold_Skill_Fire_Flag = false;
    }
}