using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MonsterBullet : Bullet
{
    public int slow;
    public string target;
    [SerializeField]
    Transform Train_List;
    [SerializeField]
    Transform Mercenary_List;

    int x_scale;

    bool targetFlag;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Train_List = GameObject.Find("Train_List").transform;
        Mercenary_List = GameObject.Find("Mercenary_List").transform;
        Bullet_Monster();
    }

    protected void Bullet_Monster()
    {
        GameObject Target = GameObject.FindGameObjectWithTag("Player");
        float Rx = Random.Range(Target.transform.position.x - 20, Target.transform.position.x + 20);

        switch (target)
        {
            case ("None"):
                dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
                break;
            case ("Player"):
                if (!Target.GetComponent<Player>().isHealing)
                {
                    dir = (Target.transform.position - transform.position).normalized;
                }
                else
                {
                    dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
                }
                break;
            case ("Train"):
                Target = Find_Target(Train_List);
                Rx = Random.Range(Target.transform.position.x - 5, Target.transform.position.x + 5);
                dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
                break;
            case ("Engine"):
                Target = Find_Target(Train_List, "Engine");
                Rx = Random.Range(Target.transform.position.x - 5, Target.transform.position.x + 5);
                dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
                break;
            case ("Turret"):
                Target = Find_Target(Train_List, "Turret");
                Rx = Random.Range(Target.transform.position.x - 2, Target.transform.position.x + 2);
                dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
                break;
            case ("Mercenary"):
                Target = Find_Target(Mercenary_List);
                dir = (Target.transform.position - transform.position).normalized;
                break;
            case ("Straight"):
                if (x_scale == 1) // Straight_Left
                {
                    dir = new Vector3(1, 0, 0);

                }
                else if (x_scale == -1) // Straight_Right
                {
                    dir = new Vector3(-1, 0, 0);

                }
                else if (x_scale == 0) // Straight_Down
                {
                    dir = new Vector3(0, -1, 0);

                }
                break;
        }
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }

    private GameObject Find_Target(Transform Target_List, string type = null)
    {
        List<GameObject> Targets = new List<GameObject>();
        int rand;
        if (type == null)
        {
            rand = Random.Range(0, Target_List.childCount);
            return Target_List.GetChild(rand).gameObject;
        }
        else
        {
            if(Target_List == Train_List)
            {
                for (int i = 0; i < Target_List.childCount; i++)
                {
                    if (Target_List.GetChild(i).GetComponent<Train_InGame>().Train_Type.Equals(type))
                    {
                        if (!targetFlag)
                        {
                            targetFlag = true;
                        }
                        Targets.Add(Target_List.GetChild(i).gameObject);
                    }
                }
            }

            if (targetFlag)
            {
                rand = Random.Range(0, Targets.Count);
                return Targets[rand].gameObject;
            }
            else
            {
                return GameObject.FindGameObjectWithTag("Player");
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Intercept_Bullet"))
        {
            if (!collision.name.Equals("Bomb")) // 부딪치는 순간 삭제하기 때문에, 제외시켜야한다.
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }

    public void Get_MonsterBullet_Information(int Monster_Atk, int Monster_Slow, string Target, int X_Scale = 0)
    {
        atk = Monster_Atk;
        slow = Monster_Slow;
        target = Target;
        x_scale = X_Scale;
    }
}
