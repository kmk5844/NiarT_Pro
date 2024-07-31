using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Egg : MonoBehaviour
{
    public GameObject Baby_Spider_Object;
    Vector2 Init_Position;
    Transform Monster_List;

    bool GroundFlag;
    bool isQuitting;

    private void Start()
    {
        GroundFlag = false;
        //Monster_List = GameObject.Find("Monster_List").transform;
        Init_Position = new Vector2(Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x), MonsterDirector.MaxPos_Sky.y + 3f);
        transform.position = Init_Position;
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
                float DestroyTime = Random.Range(5f, 10f);
                Destroy(gameObject, DestroyTime);
            }
        }
    }
}
