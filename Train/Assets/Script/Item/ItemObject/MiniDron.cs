using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDron : MonoBehaviour
{
    public enum MiniDronType { 
        DefaultDron,
        RaserDron,
    }

    public MiniDronType type;
    float UpDown;
    public float DronSpeed;
    Rigidbody2D MiniDronRid2d;
    GameObject SpriteObject;
    Vector2 SpriteObject_InitPos;

    void Start()
    {
        MiniDronRid2d = GetComponent<Rigidbody2D>();
        MiniDronRid2d.velocity = new Vector3(1.5f, 0);

        UpDown = 1;
        SpriteObject = transform.GetChild(0).gameObject;
        SpriteObject_InitPos = SpriteObject.transform.localPosition;
    }

    void FixedUpdate()
    {
        if(type == MiniDronType.DefaultDron)
        {
            if (SpriteObject.transform.localPosition.y > SpriteObject_InitPos.y + 2)
            {
                UpDown = -1;
            }
            else if(SpriteObject.transform.localPosition.y < SpriteObject_InitPos.y- 2)
            {
                UpDown = 1;
            }
            SpriteObject.transform.position = new Vector2(transform.position.x, SpriteObject.transform.position.y);
            SpriteObject.transform.Translate(new Vector2(0,  UpDown *DronSpeed * Time.deltaTime));

            if (transform.position.x > MonsterDirector.MaxPos_Sky.x)
            {
                Destroy(gameObject, 2f);
            }
        }
        else if(type == MiniDronType.RaserDron)
        {

        }
    }
}
