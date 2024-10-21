using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PixelCrushers.DialogueSystem.UnityGUI.GUIProgressBar;

public class Tutorial_Player : MonoBehaviour
{
    Animator ani;
    Rigidbody2D rigid;
    public bool jumpFlag;
    float moveSpeed = 7;
    float jumpSpeed = 8.5f;
    float jumpdistance = 1f;
    float jumpFlagDistance;
    public GameObject GunObject;
    public Transform FireZone; 
    Vector3 GunObject_Scale;
    Camera mainCam;
    private Vector3 mousePos;
    public AudioClip ShootSFX;
    public GameObject KeyObject;
    public GameObject bullet;

    bool rotationOn;
    bool isMouseDown;
    Vector3 KeyObject_Scale;
    float lastTime;
    float Bullet_Delay;
    int Bullet_Atk;
    bool MariGold_Skill_Flag;
    bool MariGold_Skill_Fire_Flag;

    [Header("튜토리얼 플래그")]
    public bool T_MoveFlag;
    public bool T_Move_Flag_A;
    public bool T_Move_Flag_D;   
    public bool T_JumpFlag;
    public int T_JumpCount;
    public bool T_FireFlag;
    public int T_FireCount;


    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        rotationOn = false;
        isMouseDown = false;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        jumpFlag = false;
        jumpdistance = 1f;
        Bullet_Delay = 0.5f;
        Bullet_Atk = 30;
        GunObject_Scale = GunObject.transform.localScale;
        KeyObject_Scale = KeyObject.transform.localScale;
    }

    private void Update()
    {
        if (T_FireFlag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMouseDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
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
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                ani.SetTrigger("Jump");
                T_JumpCount++;
            }
        }

        if (isMouseDown)
        {
            BulletFire();
            T_FireCount++;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (T_MoveFlag)
        {
            float h = Input.GetAxisRaw("Horizontal");
            if(h < 0f && T_Move_Flag_A)
            {
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
            }
            else if(h > 0f && T_Move_Flag_D)
            {
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
            }
        }

        Debug.DrawRay(rigid.position, Vector3.down * jumpdistance, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, jumpdistance, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null && rayHit.distance >= jumpFlagDistance)
        {
            jumpFlag = false;
        }
        else
        {
            jumpFlag = true;
        }

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

            //MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);

            lastTime = Time.time;
        }
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
}
