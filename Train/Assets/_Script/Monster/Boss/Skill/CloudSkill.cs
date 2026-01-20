using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSkill : MonoBehaviour
{
    public GameObject BulletObj;

    float destoryTime = 8f;
    int atk;
    float slow;
    int speed;

    bool Flag;

    float lastTime;
    float duration = 0.08f;

    private void Start()
    {
        lastTime = Time.time;
        Destroy(gameObject, destoryTime);
    }

    private void Update()
    {
        if (Flag)
        {
            if(lastTime + duration < Time.time)
            {
                BulletFire();
                lastTime = Time.time;
            }
        }
    }

    public void SetCloud(int atk_, float slow_, int speed_)
    {
        atk = atk_;
        slow = slow_;
        speed = speed_;
    }

    void BulletFire()
    {
        Vector2 vec = new Vector2(transform.position.x + Random.Range(-4f, 4f), transform.position.y);
        GameObject bullet_ = Instantiate(BulletObj, vec, Quaternion.identity);
        bullet_.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(atk, slow, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !Flag)
        {
            Flag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Flag)
        {
            Flag = false;
        }
    }

}
