using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_5 : Monster
{
    public GameObject Egg_Object;
    public GameObject Baby_Spider_Object;
    Vector2 Init_Position;

    bool GroundFlag;
    float Bounce_value;
    bool Bounce_flag;
    float Bounce_Speed;

    Transform MonsterList;
    float EgglastTime;
    float DestroyTime;

    protected override void Start()
    {
        Monster_Num = 5;

        base.Start();
        GroundFlag = false;
        MonsterList = transform.parent.transform;
        Init_Position = new Vector2(Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x), MonsterDirector.MaxPos_Sky.y + 3f);
        transform.position = Init_Position;
        Bounce_flag = false;
        lastTime = 0;
        Bounce_Speed = Random.Range(0.5f, 1f);
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
    }

    protected override void FixedUpdate()
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
                Bounce_value = Bounce_Speed * Time.deltaTime;
                DestroyTime = Random.Range(5f, 10f);
                EgglastTime = Time.time;
            }
        }

        if (GroundFlag)
        {
            if (Egg_Object.transform.localScale.x > 1.2f && !Bounce_flag)
            {
                Bounce_flag = true;
                Bounce_value = -Bounce_Speed * Time.deltaTime;
            }
            else if (Egg_Object.transform.localScale.x < 1f && Bounce_flag)
            {
                Bounce_flag = false;
                Bounce_value = Bounce_Speed * Time.deltaTime;
            }

            Egg_Object.transform.localScale =
                new Vector3(
                    Egg_Object.transform.localScale.x + Bounce_value,
                    Egg_Object.transform.localScale.x + Bounce_value,
                    Egg_Object.transform.localScale.x + Bounce_value
                );

            if (Time.time > EgglastTime + DestroyTime)
            {
                Instantiate(Baby_Spider_Object, transform.position, Quaternion.identity, MonsterList);
                Destroy(gameObject);
            }
        }
    }
}
