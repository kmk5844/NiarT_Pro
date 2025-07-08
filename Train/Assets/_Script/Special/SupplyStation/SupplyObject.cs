using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SupplyObject : MonoBehaviour
{
    [SerializeField]
    SupplyStationDirector director;
    public Vector2 minVec;
    public Vector2 maxVec;
    public float Speed;
    public bool SpawnFlag;
    public bool changeflag;
    public int direction;
    Rigidbody2D rid;

    private void Start()
    {
        rid = GetComponent<Rigidbody2D>();
        direction = 1;
        rid.simulated = false;
        SpawnFlag = false;
        changeflag = false;
    }

    private void Update()
    {
        if (!SpawnFlag && !changeflag)
        {
            transform.Translate(Vector3.right * direction * Speed * Time.deltaTime);

            // 방향 전환 조건
            if (transform.position.x < minVec.x)
            {
                direction = 1; // 오른쪽으로
                transform.position = new Vector3(minVec.x, transform.position.y, transform.position.z); // 정확히 min에 고정
            }
            else if (transform.position.x > maxVec.x)
            {
                direction = -1; // 왼쪽으로
                transform.position = new Vector3(maxVec.x, transform.position.y, transform.position.z); // 정확히 max에 고정
            }
        }

        if (SpawnFlag && !changeflag)
        {
            director.Count();
            rid.simulated = true;
            changeflag = true;
        }
    }

    public void Setting(Vector2 min, Vector2 max, float speed, SupplyStationDirector dic)
    {
        Speed = speed;
        minVec = min;
        maxVec = max;
        director = dic;
    }

    public void ClickSpace()
    {
        SpawnFlag = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Warning"))
        {
            director.GameEnd();
        }
    }
}
