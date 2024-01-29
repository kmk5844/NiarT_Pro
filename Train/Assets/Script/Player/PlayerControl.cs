using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("무기")]
    public GameObject Bullet;
    [SerializeField] float Bullet_Delay;
    public Transform bulletTransform;
    Transform Player_Bullet_List;
    float lastTime;

    [Header("이동 속도")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    Rigidbody2D rigid;

    bool jumpFlag;
    // Start is called before the first frame update
    void Start()
    {
        jumpFlag = false;
        lastTime = 0;
        rigid = GetComponent<Rigidbody2D>();
        Player_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        if (!jumpFlag && Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
        BulletFire();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > moveSpeed)
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }else if(rigid.velocity.x < moveSpeed * (-1))
        {
            rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
        }

        Debug.DrawRay(rigid.position, Vector3.down, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));

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
            Instantiate(Bullet, bulletTransform.position, Quaternion.identity, Player_Bullet_List);
            lastTime = Time.time;
        }
    }
}
