using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeShiled : MonoBehaviour
{
    [SerializeField]
    int BeeNum;
    BeeShieldDirector Director;
    int BeeShieldHP = 60;
    float laser_hit_time = 0f;
    float fire_hit_time = 0f;
    GameObject HitDamage;
    Material monsterMat;
    void Start()
    {
        Director = GetComponentInParent<BeeShieldDirector>();
        monsterMat = GetComponent<SpriteRenderer>().material;
        HitDamage = Resources.Load<GameObject>("Monster/Hit_Text");
    }

    private void Damage_Monster_Collsion(Collision2D collision)
    {
        int hit_atk = collision.gameObject.GetComponent<Bullet>().atk;
        HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage);
        if (BeeShieldHP - hit_atk > 0)
        {
            BeeShieldHP -= collision.gameObject.GetComponent<Bullet>().atk;
        }
        else
        {
            if (collision.gameObject.GetComponent<Auto_TurretBullet>() != null)
            {
                if (collision.gameObject.GetComponent<Auto_TurretBullet>().MercenaryFlag)
                {
                    PlayerLogDirector.MercenaryKill(collision.gameObject.GetComponent<Auto_TurretBullet>().MercenaryNum);
                }
            }
            Director.DestroyBee(BeeNum);
        }
    }
    private void Damage_Monster_Trigger(Collider2D collision)
    {
        int hit_atk;
        if (collision.gameObject.GetComponent<Bullet>())
        {
            hit_atk = collision.gameObject.GetComponent<Bullet>().atk;
        }
        else
        {
            hit_atk = collision.gameObject.GetComponent<MonsterBullet>().atk;
            hit_atk /= 4;
        }

        HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage);
        if (BeeShieldHP - hit_atk > 0)
        {
            BeeShieldHP -= hit_atk;
        }
        else
        {
            Director.DestroyBee(BeeNum);
        }
    }

    private void Damage_Monster_Trigger_Mercenary(Collider2D collision)
    {
        int mercenary_atk = 0;
        if (collision.GetComponentInParent<Mercenary>().Type == mercenaryType.Short_Ranged)
        {
            mercenary_atk = collision.GetComponentInParent<Short_Ranged>().unit_Attack;
        }
        HitDamage.GetComponent<Hit_Text_Damage>().damage = mercenary_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage);
        if (BeeShieldHP - mercenary_atk > 0)
        {
            BeeShieldHP -= mercenary_atk;
        }
        else
        {
            PlayerLogDirector.MercenaryKill(collision.GetComponentInParent<Mercenary>().mercenaryNum);
            Director.DestroyBee(BeeNum);
        }
    }

    public void Damage_Monster_BombAndDron(int Bomb_Atk)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = Bomb_Atk * 2;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage);
        if (BeeShieldHP - Bomb_Atk * 2 > 0)
        {
            BeeShieldHP -= Bomb_Atk * 2;
        }
        else
        {
            Director.DestroyBee(BeeNum);
        }
    }

    public void Damage_Monster_Item(int Atk)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = Atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage);
        if (BeeShieldHP - Atk > 0)
        {
            BeeShieldHP -= Atk;
        }
        else
        {
            Director.DestroyBee(BeeNum);
        }
    }

    private void Damage_Item_WireEntanglement(Collider2D collision)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = 10;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage);
        if (BeeShieldHP - 10 > 0)
        {
            BeeShieldHP -= 10;
        }
        else
        {
            Director.DestroyBee(BeeNum);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // 공통적으로 적용해야됨.
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if (!collision.gameObject.name.Equals("Ballon_Bullet_Turret(Clone)"))
            {
                if (collision.gameObject.name.Equals("Fire_Arrow(Clone)"))
                {
                    FireArrow_Hit(collision);
                }
                else if (collision.gameObject.name.Equals("Fire_Bullet(Clone)"))
                {
                    FireArrow_Hit(collision);
                }
                else
                {
                    Damage_Monster_Collsion(collision);
                }
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if (collision.gameObject.name.Equals("Short_AttackCollider"))
            {
                Damage_Monster_Trigger_Mercenary(collision);
            }
        }
        if (collision.gameObject.tag.Equals("Item"))
        {
            if (collision.gameObject.name.Equals("MiniDron_Col"))
            {
                int atk = collision.GetComponentInParent<Item_MiniDron>().DronAtk;
                Damage_Monster_BombAndDron(atk);
            }
        }
        if (collision.gameObject.tag.Equals("Monster_Bullet"))
        {
            if (collision.gameObject.name.Equals("9(Clone)"))
            {
                Damage_Monster_Trigger(collision);
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if (collision.gameObject.name.Equals("Laser"))
            {
                Laser_Hit(collision, true);
            }
            if (collision.gameObject.name.Equals("Fire"))
            {
                Fire_Hit(collision);
            }
        }

        if (collision.gameObject.tag.Equals("Item"))
        {
            if (collision.gameObject.name.Equals("WireEntanglement(Clone)"))
            {
                Laser_Hit(collision, false);
            }
        }
    }
    void Laser_Hit(Collider2D collision, bool RealLaser)
    {
        if (RealLaser)
        {
            if (Time.time > laser_hit_time + 0.3f)
            {
                Damage_Monster_Trigger(collision);
                laser_hit_time = Time.time;
            }
        }
        else
        {
            if (Time.time > laser_hit_time + 0.5f)
            {
                Damage_Item_WireEntanglement(collision);
                laser_hit_time = Time.time;
            }
        }

    }
    void Fire_Hit(Collider2D collision)
    {
        if (Time.time > fire_hit_time + 0.3f)
        {
            Damage_Monster_Trigger(collision);
        }
    }

    void FireArrow_Hit(Collision2D collision)
    {
        Damage_Monster_Collsion(collision);
    }


    IEnumerator MatHitEffect()
    {
        monsterMat.SetFloat("_HitEffectBlend", 0.05f);

        float duration = 0.2f; // 올라가는 시간
        float elapsed = 0f;

        // 0 → 0.5
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            monsterMat.SetFloat("_HitEffectBlend", Mathf.Lerp(0f, 0.15f, t));
            yield return null;
        }

        // 0.5에서 0으로
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            monsterMat.SetFloat("_HitEffectBlend", Mathf.Lerp(0.15f, 0f, t));
            yield return null;
        }
    }

}
