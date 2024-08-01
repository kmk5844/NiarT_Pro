using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Boss_0_Egg : MonoBehaviour
{
    public GameObject Egg_Object;
    public GameObject Baby_Spider_Object;
    Vector2 Init_Position;

    bool GroundFlag;
    bool isQuitting;
    float value;
    bool flag;
    float Speed;

    private void Start()
    {
        GroundFlag = false;
        //Monster_List = GameObject.Find("Monster_List").transform;
        Init_Position = new Vector2(Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x), MonsterDirector.MaxPos_Sky.y + 3f);
        transform.position = Init_Position;
        flag = false;
        Speed = Random.Range(0.5f, 1f);
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            Instantiate(Baby_Spider_Object, transform.position, Quaternion.identity); 
        }
    }

    private void FixedUpdate()
    {
        if(transform.position.y > MonsterDirector.MinPos_Ground.y)
        {
            transform.Translate(0, -12f* Time.deltaTime, 0);
        }
        else
        {
            if (!GroundFlag)
            {
                GroundFlag = true;
                transform.position = new Vector2(transform.position.x, MonsterDirector.MinPos_Ground.y);
                value = Speed * Time.deltaTime;
                float DestroyTime = Random.Range(5f, 10f);
                Destroy(gameObject, DestroyTime);
            }
        }

        if (GroundFlag)
        {
            if (Egg_Object.transform.localScale.x > 1.2f && !flag)
            {
                flag = true;
                value = -Speed * Time.deltaTime;
            }
            else if (Egg_Object.transform.localScale.x < 1f && flag)
            {
                flag = false;
                value = Speed * Time.deltaTime;
            }

            Egg_Object.transform.localScale =
                new Vector3(
                    Egg_Object.transform.localScale.x + value,
                    Egg_Object.transform.localScale.x + value,
                    Egg_Object.transform.localScale.x + value
                );
        }
    }
}
