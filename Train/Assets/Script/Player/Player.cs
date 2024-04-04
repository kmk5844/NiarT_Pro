using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SA_PlayerData playerData;

    Train_InGame train;

    [Header("����")]
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    int Bullet_Atk; //������ �ο� ��, atk�� �ʿ��� (����� �ѹ��� �׵��� ����)
    [SerializeField]
    float Bullet_Delay;
    public Transform Bullet_Fire_Transform;
    Transform Player_Bullet_List;
    float lastTime;
    [Header("���� ������Ʈ")]
    public GameObject GunSprite;

    [Header("ü��")]
    public int Player_HP;
    private int Max_HP;

    [Header("����")]
    [SerializeField]
    int Player_Armor;
    float era;
    [SerializeField]
    float def_constant;

    [Header("�̵� �ӵ�")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    Rigidbody2D rigid;
    int moveX;
    bool jumpFlag;
    bool isMouseDown;

    [Header("�ǹ���")]
    public bool isHealing;
    
    Vector3 respawnPosition;
    void Start()
    {
        respawnPosition = transform.position;
        isHealing = false;
        jumpFlag = false;

        Bullet = playerData.Bullet;
        Player_HP = playerData.HP;
        Player_Armor = playerData.Armor;
        Bullet_Atk = playerData.Atk;
        Bullet_Delay = playerData.Delay;
        moveSpeed = playerData.MoveSpeed;
        //���̹����� ���߿� = playerData.Gun;

        Max_HP = Player_HP; 
        lastTime = 0;
        rigid = GetComponent<Rigidbody2D>();
        Player_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        Level();
        era = 1f - (float)Player_Armor / def_constant;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        try
        {
            if (train.Train_Type.Equals("Medic"))
            {
                if (Input.GetKeyDown(KeyCode.R) && Check_HpParsent() < 70f && !isHealing)
                {
                    if (train.openMedicTrian)
                    {
                        if (!train.GetComponentInChildren<Medic_Train>().isMercenaryHealing)
                        {
                            isHealing = true;
                            OnOff_Sprite(true);
                        }
                        else
                        {
                            // ġ�� ���Դϴ�.
                        }
                    }
                    else
                    {
                        //�ı��Ǿ� ����� �� �����ϴ�.
                    }
                }
            }
        }catch
        {
            Debug.Log("���� ���� ��");
        }


        if (!isHealing) //ġ�ᰡ �ƴ� ��, �ɾ�ٴϸ鼭 ���� ��
        {
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }

            if (!jumpFlag && Input.GetButtonDown("Jump"))
            {
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }

            if (isMouseDown)
            {
                BulletFire();
            }
        }
        else // ġ������ ��, ����Ű ���X
        {
            if (Check_HpParsent() <= 70f && !train.isHealing)
            {
                StartCoroutine(train.Train_Healing());
            }
            else if (Check_HpParsent() >= 70f || !train.openMedicTrian)
            {
                OnOff_Sprite(false);
                isHealing = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isHealing) { //ġ�� ���� �ƴ� ��, ������.
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        }

        if (rigid.velocity.x > moveSpeed)
        {
            moveX = 1;
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < moveSpeed * (-1))
        {
            moveX = -1;
            rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
        }

        Debug.DrawRay(rigid.position, Vector3.down * 2, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 3f, LayerMask.GetMask("Platform"));
        
        if(rayHit.collider != null)
        {
            train = rayHit.collider.GetComponentInParent<Train_InGame>();
        }

        if(rayHit.collider != null && rayHit.distance >= 0.5f)
        {
            jumpFlag = false;
        }
        else
        {
            jumpFlag = true;
        }
    }
    void BulletFire()
    {
        if (Time.time >= lastTime + Bullet_Delay)
        {
            GameObject bullet = Instantiate(Bullet, Bullet_Fire_Transform.position, Quaternion.identity, Player_Bullet_List);
            bullet.GetComponent<Bullet>().atk = Bullet_Atk;
            lastTime = Time.time;
        }
    }

    void OnOff_Sprite(bool flag)
    {
        if (flag)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GunSprite.SetActive(false);
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GunSprite.SetActive(true);
        }
    }
    void Level()
    {
        int Level_Atk = playerData.Level_Player_Atk;
        int Level_AtkDelay = playerData.Level_Player_AtkDelay;
        int Level_HP = playerData.Level_Player_HP;
        int Level_Armor = playerData.Level_Player_Armor;
        int Level_Speed = playerData.Level_Player_Speed;

        Bullet_Atk = Bullet_Atk + (((Bullet_Atk * Level_Atk * 10)) / 100);
        Bullet_Delay = Bullet_Delay - (((Bullet_Delay * Level_AtkDelay * 10)) / 100);
        Player_HP = Player_HP + (((Player_HP * Level_HP) * 10) / 100);
        Player_Armor = Player_Armor + (((Player_Armor * Level_Armor) * 10) / 100);
        moveSpeed = moveSpeed + (((moveSpeed * Level_Speed) * 10) / 100);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            transform.position = new Vector3(respawnPosition.x, 1, 0); ;
        }
    }

    public float Check_HpParsent()
    {
        return (float)Player_HP / (float)Max_HP * 100f;
    }

    public void Heal_HP(int Medic_Heal)
    {
        Player_HP += Medic_Heal;
    }

    public int Check_moveX()
    {
        return moveX;
    }
}