using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RobotPlant_Robot : MonoBehaviour
{
    public enum RobotType { 
        walk,
        bomb
    }
    public RobotType type;
    public int Atk;
    public int Speed;
    public GameObject BombObject;
    Rigidbody2D rid2D;
    bool BombFlag;
    int Move_X;

    public GameObject AtkObject;

    GameObject BombParticle;

    Transform Unit_Scale;
    float Unit_Scale_X;
    float Unit_Scale_Y;
    float Unit_Scale_Z;

    private void Start()
    {
        Move_X = 1;
        BombFlag = false;
        AtkObject.SetActive(false);
        BombObject.SetActive(false);
        BombObject.GetComponent<Item_RobotPlant_Bomb>().atk = Atk;
        type = RobotType.walk;
        rid2D = GetComponent<Rigidbody2D>();
        rid2D.velocity = Vector2.right;
        Unit_Scale = GetComponent<Transform>();
        Unit_Scale_X = Unit_Scale.localScale.x;
        Unit_Scale_Y = Unit_Scale.localScale.y;
        Unit_Scale_Z = Unit_Scale.localScale.z;
        BombParticle = Resources.Load<GameObject>("Bullet/Boss2_Skill0_Effect");
    }

    private void FixedUpdate()
    {
        if(type == RobotType.walk)
        {
            Robot_Move();
        }
        else if(type == RobotType.bomb)
        {
            rid2D.velocity = Vector2.zero;
            if (!BombFlag)
            {
                StartCoroutine(Robot_Bomb());
            }
        }
    }

    void Robot_Flip()
    {
        if (Move_X > 0)
        {
            Unit_Scale.localScale = new Vector3(-Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }
        else
        {
            Unit_Scale.localScale = new Vector3(Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }
    }

    void Robot_Move()
    {
        Robot_Flip();
        if(transform.position.x > MonsterDirector.MaxPos_Ground.x)
        {
            Move_X = -1;
        }else if(transform.position.x < MonsterDirector.MinPos_Ground.x)
        {
            Move_X = 1;
        }
        rid2D.velocity = new Vector2(Move_X * Speed, 0);
    }

    IEnumerator Robot_Bomb()
    {
        BombFlag = true;
        AtkObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Instantiate(BombParticle, transform.localPosition, Quaternion.identity);
        BombObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(type == RobotType.walk)
        {
            if (collision.CompareTag("Monster"))
            {
                type = RobotType.bomb;
            }
        }
    }
}
