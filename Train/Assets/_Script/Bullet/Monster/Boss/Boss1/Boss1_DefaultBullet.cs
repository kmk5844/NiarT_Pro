using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_DefaultBullet : MonsterBullet
{
    GameObject BombParticle;
    float force;
    float delayTime;
    Animator ani;


    protected override void Start()
    {
        BombParticle = Resources.Load<GameObject>("Bullet/10_Effect");
        base.Start();
        force = Random.Range(5f, 7f);
        delayTime = Random.Range(0.2f, 5f);
        ani = GetComponent<Animator>();
        Vector2 forcePos = new Vector2(1f, 1.2f);
        rid.AddForce(forcePos * force, ForceMode2D.Impulse);
        StartCoroutine(BombDelay());
    }

    IEnumerator BombDelay()
    {
        yield return new WaitForSeconds(delayTime);
        ani.SetTrigger("Bomb");
    }

    public void BombDestory()
    {
        Instantiate(BombParticle, transform.localPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
