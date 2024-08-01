using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Skill3_Bullet : MonoBehaviour
{
    [SerializeField]
    public Vector3 PlayerPos;
    [SerializeField]
    public float Xpos;

    public GameObject Sub_Bullet;

    Vector3 Pos;
    Vector3 Init_Pos;
    float Bullet_Time;
    // Start is called before the first frame update
    void Start()
    {
        Init_Pos = transform.position;
        Pos = new Vector3(PlayerPos.x + Xpos, transform.position.y + 3, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Bullet_Time < 1.5f)
        {
            Bullet_Time += Time.deltaTime;
            float t = Bullet_Time / 1.5f;

            float x = Mathf.Lerp(Init_Pos.x, Pos.x, t);
            float y = Mathf.Lerp(Init_Pos.y, Pos.y, t) + 2* Mathf.Sin(Mathf.PI *t/2);

            transform.position= new Vector3(x, y, 0);
        }
        else
        {
            Instantiate(Sub_Bullet, transform.position, Quaternion.Euler(0, 0, -22.5f));
            Instantiate(Sub_Bullet, transform.position, Quaternion.Euler(0, 0, -45));
            Instantiate(Sub_Bullet, transform.position, Quaternion.Euler(0, 0, -67.5f));
            Instantiate(Sub_Bullet, transform.position, Quaternion.Euler(0, 0, -90));
            Instantiate(Sub_Bullet, transform.position, Quaternion.Euler(0, 0, -112.5f));
            Instantiate(Sub_Bullet, transform.position, Quaternion.Euler(0, 0, -135));
            Instantiate(Sub_Bullet, transform.position, Quaternion.Euler(0, 0, -157.5f));
            Destroy(gameObject);
        }
    }

    public void FirePosition(Vector3 Player, float _xPos) {
        PlayerPos = Player;
        Xpos = _xPos; 
    }

}
