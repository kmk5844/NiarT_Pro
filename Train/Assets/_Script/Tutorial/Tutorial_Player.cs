using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GamePlay_Tutorial_Director;
using static PixelCrushers.DialogueSystem.UnityGUI.GUIProgressBar;

public class Tutorial_Player : MonoBehaviour
{
    public GamePlay_Tutorial_Director gameDirector;
    public Tutorial_UIDirector UIDirector;

    Animator ani;
    Rigidbody2D rigid;
    public bool jumpFlag;

    public int PlayerHP;
    [HideInInspector]
    public int Max_PlayerHP;

    float moveSpeed = 7;
    float jumpSpeed = 8.5f;
    float jumpdistance = 1.5f;
    float jumpFlagDistance;
    int jumpCount = 0;
    int jumpMaxCount = 1;

    bool ReloadingFlag;
    int firecount;
    int max_firecount = 15;
    public GameObject Reload;
    public Image ReloadGuage;

    public GameObject GunObject;
    public Transform FireZone; 
    Vector3 GunObject_Scale;
    Camera mainCam;
    private Vector3 mousePos;
    public GameObject KeyObject;
    public GameObject bullet;

    Tutorial_Train train;
    bool rotationOn;
    bool isMouseDown;
    Vector3 KeyObject_Scale;
    float lastTime;
    float Bullet_Delay;
    int Bullet_Atk;
    bool MariGold_Skill_Flag;
    bool MariGold_Skill_Fire_Flag;
    bool canDash = true;
    bool isDashing = false;
    float dashingPower = 4f;
    float dashingTime = 0.2f;
    float dashingCooldown = 1f;
    float horizontalInput;
    float MouseZ;

    [Header("Sound")]
    public AudioClip ShootSFX;
    public AudioClip ReloadingSFX;
    public AudioClip SkillUseSFX;
    public AudioClip PainSFX;

    [Header("튜토리얼 플래그")]
    public bool T_MoveFlag;
    public bool T_JumpFlag;
    public int T_JumpCount;
    public bool T_FireFlag;
    public int T_FireCount;
    public bool T_Skill_Q;
    public bool T_Skill_Q_Click;
    public bool T_Skill_Q_End;
    public bool T_Skill_E;
    public bool T_Skill_E_End;
    public bool T_SpawnItem_Flag;
    public bool T_UseItem;
    public bool T_UseItem_Flag;
    public bool T_Train;
    public bool T_Train_Flag;

    [Header("Effect")]
    public GameObject WarkEffect;
    public ParticleSystem DashEffect;
    public ParticleSystem JumpEffect;
    public ParticleSystem HealEffect;
    public GameObject HPWaringEffect;
    public ParticleSystem QSkillEffect;
    public ParticleSystem ESkillEffect;

    private void Awake()
    {
        PlayerHP = 5000;
        Max_PlayerHP = 5000;
    }
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        rotationOn = false;
        isMouseDown = false;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        jumpFlag = false;
        jumpdistance = 1.5f;
        Bullet_Delay = 0.5f;
        Bullet_Atk = 30;
        GunObject_Scale = GunObject.transform.localScale;
        KeyObject_Scale = KeyObject.transform.localScale;

        T_Skill_Q_Click = false;
        T_Skill_Q_End = false;
        T_Skill_E_End = false;
        T_SpawnItem_Flag = false;
        T_UseItem = false;
        T_UseItem_Flag = false;
        T_Train = false;
        T_Train_Flag = false;

    }

    private void Update()
    {

        if ((float)PlayerHP / (float)Max_PlayerHP * 100f < 30f)
        {
            HPWaringEffect.SetActive(true);
        }
        else
        {
            HPWaringEffect.SetActive(false);
        }

        if (T_FireFlag)
        {
            if (isDashing)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                isMouseDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }

            /*            if (Input.GetKeyDown(KeyCode.R) && firecount != 0)
                        {
                            StartCoroutine(Reloading());
                        }*/

            if (gameDirector.gameType == GameType_T.Tutorial)
            {
                if (isMouseDown)
                {
                    if (gameDirector.aimFlag)
                    {
                        gameDirector.ChangeCursor(true, true);
                    }
                    BulletFire();
                }
                else
                {
                    if (gameDirector.aimFlag)
                    {
                        gameDirector.ChangeCursor(true, false);
                    }
                }
            }
        }

        if (T_MoveFlag)
        {
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
                ani.SetBool("Move", false);
            }
            else if (Input.GetButton("Horizontal"))
            {
                ani.SetBool("Move", true);
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

        if (T_JumpFlag)
        {
            if (Input.GetButtonDown("Jump") && !jumpFlag)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                ani.SetTrigger("Jump");
            }

            if (Input.GetButtonDown("Jump") && jumpFlag && jumpCount < jumpMaxCount)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                ani.SetTrigger("Jump");
                JumpEffect.Play();
                jumpCount++;
            }

            if (jumpFlag)
            {
                WarkEffect.SetActive(false);
            }
            else
            {
                WarkEffect.SetActive(true);
            }

        }

        if (T_Skill_Q && !T_Skill_Q_End)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !T_Skill_Q_Click)
            {
                QSkillEffect.Play();

                MMSoundManagerSoundPlayEvent.Trigger(SkillUseSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                MariGold_Skill(0);
                UIDirector.skill_coolTime(0);
            }
        }

        if (T_Skill_E && !T_Skill_E_End)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ESkillEffect.Play();

                MMSoundManagerSoundPlayEvent.Trigger(SkillUseSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                MariGold_Skill(1);
                UIDirector.skill_coolTime(1);
            }
        }

        if (T_UseItem && !T_UseItem_Flag)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerHP_Item(true);
                T_UseItem_Flag = true;
            }
        }

        if (T_Train)
        {
            if (Input.GetKeyDown(KeyCode.F) && train.Player_Interaction_Flag && !T_Train_Flag)
            {
                train.UseTurret();
            }
        }

        if(train != null)
        {
            if (train.Player_Interaction_Flag && train.interactionFlag && !T_Train_Flag)
            {
                KeyObject.SetActive(true);
            }
            else
            {
                KeyObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (T_MoveFlag)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * horizontalInput, ForceMode2D.Impulse);
        }

        Debug.DrawRay(rigid.position, Vector3.down * jumpdistance, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, jumpdistance, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null)
        {
            train = rayHit.collider.GetComponentInParent<Tutorial_Train>();
        }


        if (rayHit.collider != null && rayHit.distance >= jumpFlagDistance)
        {
            jumpFlag = false;
            if (jumpCount != 0)
            {
                jumpCount = 0;
            }
        }
        else
        {
            jumpFlag = true;
        }

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rot = mousePos - GunObject.transform.position;
        MouseZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        GunObject.transform.rotation = Quaternion.Euler(0, 0, MouseZ);

        if (MouseZ >= -90 && MouseZ <= 90)
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

    void BulletFire()
    {
        if (Time.time >= lastTime + Bullet_Delay)
        {
            bullet.GetComponent<Bullet>().atk = Bullet_Atk;
            Instantiate(bullet, FireZone.position, Quaternion.identity);
            ani.SetTrigger("Shoot_0");

            if (MariGold_Skill_Flag && !MariGold_Skill_Fire_Flag)
            {
                StartCoroutine(MariGold_Skill_BulletFire());
            }
            MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);

            /* if (!ReloadingFlag)
             {
                 Instantiate(bullet, FireZone.position, Quaternion.identity);
                 ani.SetTrigger("Shoot_0");
                 T_FireCount++;


                 if (MariGold_Skill_Flag && !MariGold_Skill_Fire_Flag)
                 {
                     StartCoroutine(MariGold_Skill_BulletFire());
                 }

                 if(firecount < max_firecount - 1)
                 {
                     firecount++;
                 }
                 else
                 {
                     StartCoroutine(Reloading());
                 }

                 MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
             }*/

            lastTime = Time.time;
        }
    }
    
    IEnumerator Reloading()
    {
        ReloadingFlag  = true;
        firecount = max_firecount;
        Reload.SetActive(true);
        float elapsed = 0f;
        MMSoundManagerSoundPlayEvent.Trigger(ReloadingSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        while (elapsed < 1.5f)
        {
            elapsed += Time.deltaTime;
            float fill = Mathf.Clamp01(elapsed / 1.5f);
            ReloadGuage.fillAmount = fill;
            yield return null;
        }
        Reload.SetActive(false);
        firecount = 0;
        ReloadingFlag = false;
    }

    void MariGold_Skill(int num)
    {
        if (num == 0)
        {
            MariGold_Skill1();
        }
        else if (num == 1)
        {
            StartCoroutine(MariGold_Skill2(5));
        }
    }

    public void PlayerHP_Item(bool _Flag)
    {
        if (_Flag)
        {
            HealEffect.Play();
            PlayerHP += (5000 / 100) * 20;
        }
        else
        {
            PlayerHP -= (5000 / 100) * 30;
        }
    }

    public IEnumerator Item_41()
    {
        moveSpeed = 9;
        Bullet_Delay = 0.2f;
        yield return new WaitForSeconds(8f);
        moveSpeed = 7;
        Bullet_Delay = 0.5f;
        T_SpawnItem_Flag = true;
    }

    void MariGold_Skill1()
    {
        T_Skill_Q_Click = true;
    }

    IEnumerator MariGold_Skill2(float during)
    {
        MariGold_Skill_Flag = true;
        yield return new WaitForSeconds(during);
        T_Skill_E_End = true;
        MariGold_Skill_Flag = false;
    }

    IEnumerator MariGold_Skill_BulletFire()
    {
        MariGold_Skill_Fire_Flag = true;
        bullet.GetComponent<Bullet>().atk = Bullet_Atk;
        yield return new WaitForSeconds(0.08f);
        Instantiate(bullet, FireZone.position, Quaternion.identity);
        MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        yield return new WaitForSeconds(0.08f);
        Instantiate(bullet, FireZone.position, Quaternion.identity);
        ani.SetTrigger("Shoot_0");
        MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        MariGold_Skill_Fire_Flag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            PlayerHP -= 200;
            Destroy(collision.gameObject);
            MMSoundManagerSoundPlayEvent.Trigger(PainSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }

        if (collision.CompareTag("Respawn"))
        {
            PlayerHP -= 100;
            transform.position = Vector3.zero;
            MMSoundManagerSoundPlayEvent.Trigger(PainSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }
    }

    public float Check_GunBullet()
    {
        return Mathf.Clamp01((float)(max_firecount - firecount) / (float)max_firecount);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        if (horizontalInput > 0)
        {
            if (MouseZ >= -90 && MouseZ <= 90)
            {
                DashEffect.transform.localRotation = Quaternion.Euler(0, 90, 90);
            }
            else
            {
                DashEffect.transform.localRotation = Quaternion.Euler(0, -90, 90);
            }
        }
        else
        {
            if (MouseZ >= -90 && MouseZ <= 90)
            {
                DashEffect.transform.localRotation = Quaternion.Euler(0, -90, 90);
            }
            else
            {
                DashEffect.transform.localRotation = Quaternion.Euler(0, 90, 90);
            }
        }

        DashEffect.Play();
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f; // 대시 중 중력 비활성화
                                 //rigid.velocity = new Vector2(horizontalInput * dashingPower, 0f);
        /*
                Vector2 dashTarget = rigid.position + new Vector2(horizontalInput * dashingPower, 0f);
                rigid.MovePosition(dashTarget);
                yield return new WaitForSeconds(dashingTime);*/
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontalInput * dashingPower, 0f);

        float elapsed = 0f;

        while (elapsed < dashingTime)
        {
            float t = elapsed / dashingTime;

            // 부드러운 가속/감속을 원하면 AnimationCurve 활용
            float curvedT = EvaluateDashCurve(t);

            transform.position = Vector2.Lerp(start, end, curvedT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        rigid.gravityScale = originalGravity; // 대시 후 중력 재활성화

        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private float EvaluateDashCurve(float t)
    {
        //return 1f - Mathf.Pow(2f, -10f * t);
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
        //return t * t * (3f - 2f * t); // SmoothStep
    }
}