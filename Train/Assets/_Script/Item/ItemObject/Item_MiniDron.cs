using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MiniDron : MonoBehaviour
{
    public enum MiniDronType { 
        DefaultDron,
        RaserDron,
    }

    public MiniDronType type;
    float UpDown;
    public int DronAtk;
    public float DronSpeed;
    Rigidbody2D MiniDronRid2d;
    [SerializeField]
    GameObject SpriteObject;
    Vector2 SpriteObject_InitPos;
    public BoxCollider2D DefaultDron_BoxCollider;
    public GameObject RaserObject;

    void Start()
    {
        if (DronAtk == 0)
        {
            DronAtk = 30;
        }
        MiniDronRid2d = GetComponent<Rigidbody2D>();
        //DefaultDron_BoxCollider = GetComponent<BoxCollider2D>();

        if(type == MiniDronType.DefaultDron)
        {
            DefaultDron_BoxCollider.enabled = true;
            if(RaserObject != null)
            {
                RaserObject.SetActive(false);
            }
            MiniDronRid2d.velocity = new Vector2(2f, 0);
        }
        else
        {
            DefaultDron_BoxCollider.enabled = false;
            RaserObject.GetComponent<Raser_TurretBullet>().atk = DronAtk / 2;
            RaserObject.SetActive(true);
            MiniDronRid2d.velocity = new Vector2(0, -0.5f);
        }

        UpDown = 1;
        //SpriteObject = transform.GetChild(0).gameObject;
        SpriteObject_InitPos = SpriteObject.transform.localPosition;
    }

    void FixedUpdate()
    {
        if(type == MiniDronType.DefaultDron)
        {
            if (SpriteObject.transform.localPosition.y > SpriteObject_InitPos.y + 1)
            {
                UpDown = -1;
            }
            else if(SpriteObject.transform.localPosition.y < SpriteObject_InitPos.y- 1)
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
            SpriteObject.transform.position = new Vector2(transform.position.x, transform.position.y);

            if(transform.position.y < MonsterDirector.MinPos_Ground.y)
            {
                Destroy(gameObject, 2f);
            }
        }
    }
}
