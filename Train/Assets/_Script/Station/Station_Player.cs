using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_Player : MonoBehaviour
{
    public StationDirector stationDirector;
    public bool OpenUIFlag;

    Animator ani;
    Rigidbody2D rigid;
    bool jumpFlag;

    float moveX;

    float moveSpeed = 7;
    float jumpSpeed = 8.5f;
    float jumpdistance = 1.4f;
    float jumpFlagDistance;

    bool DoorFlag = false;
    bool DoorFlag_Train;
    bool DoorFlag_Store;
    bool DoorFlag_Training;
    bool DoorFlag_Start;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        jumpFlag = false;
        jumpdistance = 1.4f;
    }

    private void Update()
    {
        if (!OpenUIFlag && !stationDirector.Option_Flag)
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

            if (Input.GetButtonDown("Jump") && !jumpFlag)
            {
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                ani.SetTrigger("Jump");
            }

            if (Input.GetKeyDown(KeyCode.F) && DoorFlag)
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
                ani.SetBool("Move", false);

                if (DoorFlag_Train)
                {
                    stationDirector.ClickLobbyButton(1);
                }

                if (DoorFlag_Store)
                {
                    stationDirector.ClickLobbyButton(2);
                }

                if (DoorFlag_Training)
                {
                    stationDirector.ClickLobbyButton(3);
                }

                if (DoorFlag_Start)
                {
                    stationDirector.ClickLobbyButton(5);
                }
            }
        }
        else
        {
            ani.SetBool("Move", false);
        }/*
                if (Input.GetKeyDown(KeyCode.P))
                {
                    stationDirector.ClickLobbyButton(2);
                }
        */

        if (rigid.velocity.x > moveSpeed)
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < moveSpeed * (-1))
        {
            rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
        }

        if(moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (!OpenUIFlag && !stationDirector.Option_Flag)
        {
            if (h < 0f)
            {
                moveX = 1;
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
            }
            else if (h > 0f)
            {
                moveX = -1;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoorFlag = true;
        if (collision.name.Equals("Door_TrainMainTenance"))
        {
            DoorFlag_Train = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            //stationDirector.activeNotice(0, true);
        }

        if (collision.name.Equals("Door_Store"))
        {
            DoorFlag_Store = true;
            //stationDirector.activeNotice(1, true);
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (collision.name.Equals("Door_Training"))
        {
            DoorFlag_Training = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            //stationDirector.activeNotice(2, true);
        }

        if (collision.name.Equals("Door_Start"))
        {
            DoorFlag_Start = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            //stationDirector.activeNotice(3, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DoorFlag = false;
        if (collision.name.Equals("Door_TrainMainTenance"))
        {
            DoorFlag_Train = false;
            //stationDirector.activeNotice(0, false);
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.name.Equals("Door_Store"))
        {
            DoorFlag_Store = false;
            //stationDirector.activeNotice(1, false);
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.name.Equals("Door_Training"))
        {
            DoorFlag_Training = false;
            //stationDirector.activeNotice(2, false);
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.name.Equals("Door_Start"))
        {
            DoorFlag_Start = false;
            //stationDirector.activeNotice(3, false);
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
