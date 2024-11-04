using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_TentacleObject : MonoBehaviour
{
    Monster_ShortAtk shortAtk_Information;
    public GameObject _Bullet;
    public GameObject _FireZone;
    [SerializeField]
    int skillNum;
    [SerializeField]
    bool Bomb;

    private void Start()
    {
        shortAtk_Information = GetComponentInChildren<Monster_ShortAtk>();
        SetAtk(skillNum);
    }

    public void SetAtk(int Num)
    {
        if(Num == 0)
        {
            shortAtk_Information.Atk = 100;
            shortAtk_Information.Force = 15;
            Bomb = false;
        }
        else if(Num == 1)
        {
            shortAtk_Information.Atk = 200;
            shortAtk_Information.Force = 20;
            Bomb = false;
        }
        else if(Num == 2)
        {
            shortAtk_Information.Atk = 50;
            shortAtk_Information.Force = 8;
            Bomb = false;
        }
        else if (Num == 3)
        {
            shortAtk_Information.Atk = 75;
            shortAtk_Information.Force = 15;
            Bomb = true;
        }
    }

    public void BombBullet()
    {
        if (Bomb)
        {
            Instantiate(_Bullet, _FireZone.transform.position, Quaternion.identity);
        }
    }

    public void SetSkillNum(int Num)
    {
        skillNum = Num;
    }

    public void AniEnd()
    {
        Destroy(gameObject);
    }
}
