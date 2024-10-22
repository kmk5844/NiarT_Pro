using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow_Object : MonoBehaviour
{
    int MonsterHP;
    public int Monster_Score;
    public int Monster_Coin;
    GameObject HitDamage;
    GamePlay_Tutorial_Director director;

    private void Start()
    {
        director = GameObject.Find("TutorialDirector").GetComponent<GamePlay_Tutorial_Director>();
        MonsterHP = 100;
        Monster_Score = 100;
        Monster_Coin = 100;
        transform.position = new Vector2(-13, 16);
        HitDamage = Resources.Load<GameObject>("Monster/Hit_Text");
    }

    private void LateUpdate()
    {
        if(transform.position.y > -0.25f)
        {
            transform.Translate(0, -15f * Time.deltaTime, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet")){
            int hit_atk = collision.gameObject.GetComponent<Bullet>().atk;
            HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
            HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
            HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
            Instantiate(HitDamage);
            if (MonsterHP - hit_atk > 0)
            {
                MonsterHP -= collision.gameObject.GetComponent<Bullet>().atk;
            }
            else
            {
                director.Get_Score(Monster_Score, Monster_Coin);
                Destroy(gameObject);
            }
        }
        Destroy(collision.gameObject);
    }
}
