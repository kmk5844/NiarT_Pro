using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterSkillObject : MonoBehaviour
{
    float UpDown;
    public int DronAtk;
    public float DronSpeed;
    public GameObject LaserObject;
    Rigidbody2D MiniDronRid2d;
    Vector2 InitPos;
    bool DestoryFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(MonsterDirector.MinPos_Sky.x - 3f, MonsterDirector.MinPos_Sky.y + 7f);

        MiniDronRid2d = GetComponent<Rigidbody2D>();
        MiniDronRid2d.velocity = new Vector2(2f, 0);
        LaserObject.GetComponent<Raser_TurretBullet>().atk = DronAtk;
        UpDown = 2;
        InitPos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
   {
        if (transform.position.y > InitPos.y + 0.5f)
        {
            UpDown = -2;
        }
        else if (transform.position.y < InitPos.y - 0.5f)
        {
            UpDown = 2;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y);
        transform.Translate(new Vector2(DronSpeed * Time.deltaTime, UpDown * Time.deltaTime));

        if (!DestoryFlag && transform.position.x > MonsterDirector.MaxPos_Sky.x)
        {
            Destroy(gameObject, 4f);
            DestoryFlag = true;
        }
    }
}
