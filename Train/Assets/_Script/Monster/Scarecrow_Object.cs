using UnityEngine;

public class Scarecrow_Object : MonoBehaviour
{
    int MonsterHP;
    public int Monster_Score;
    public int Monster_Coin;
    GameObject HitDamage;
    GamePlay_Tutorial_Director director;
    public bool Monster_Type;
    bool Monster_BonceFlag;
    float rand_speed;
    float rand_distance;
    float lastTime;
    float Bullet_Delay;
    public GameObject bullet;

    private void Start()
    {
        director = GameObject.Find("TutorialDirector").GetComponent<GamePlay_Tutorial_Director>();
        if (!Monster_Type)
        {
            MonsterHP = 100;
            Monster_Score = 100;
            Monster_Coin = 100;
            transform.position = new Vector2(-13, 16);
        }
        else
        {
            MonsterHP = 80;
            Monster_Score = 200;
            Monster_Coin = 200;
            rand_speed = Random.Range(10f, 14f);
            rand_distance = Random.Range(0.5f, 1f);
            transform.position = new Vector2(transform.position.x, 16);
        }
        Monster_BonceFlag = false;
        Bullet_Delay = 3f;

        HitDamage = Resources.Load<GameObject>("Monster/Hit_Text");
    }

    private void LateUpdate()
    {
        if (!Monster_Type)
        {
            if (transform.position.y > -0.25f)
            {
                transform.Translate(0, -15f * Time.deltaTime, 0);
            }
        }
        else
        {
            if(transform.position.y > 10f)
            {
                transform.Translate(0, -15f * Time.deltaTime, 0);
            }
            else
            {
                Monster_BonceFlag = true;

            }
        }

        if (Monster_BonceFlag)
        {
            float offset = Mathf.Sin(Time.time * rand_speed) * rand_distance; // 2 : 스피드, 1.0f 이동거리
            Vector2 movement = new Vector2(0f, offset);
            transform.Translate(movement * Time.deltaTime);

            if (Time.time >= lastTime + Bullet_Delay)
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
                lastTime = Time.time;
            }
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
                director.Get_Score(Monster_Score, Monster_Coin, Monster_Type);
                Destroy(gameObject);
            }
        }
        Destroy(collision.gameObject);
    }
}
