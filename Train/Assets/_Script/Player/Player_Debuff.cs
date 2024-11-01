using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Debuff : MonoBehaviour
{
    Player player;
    int maxHP;

    int poisonNum;
    int Max_poisonNum;
    bool PlayerStatus_poisonFlag;
    bool poisonFlag;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        maxHP = player.GetMaxHP();
        poisonNum = 0;
        Max_poisonNum = 6;
    }

    private void Update()
    {
        if (PlayerStatus_poisonFlag)
        {
            if (!poisonFlag)
            {
                StartCoroutine(CorutineDebuff());
            }
        }
    }
    public void GetDebuff(MonsterBulletType type)
    {
        if(type == MonsterBulletType.Poison)
        {
            poisonNum = 0;
            PlayerStatus_poisonFlag = true;
        }
    }

    IEnumerator CorutineDebuff()
    {
        poisonFlag = true;
        player.Player_HP -= (maxHP / 100) * 5;
        player.Blood_Effect();
        yield return new WaitForSeconds(1);
        if(poisonNum < Max_poisonNum)
        {
            poisonNum++;
        }
        else
        {
            PlayerStatus_poisonFlag = false;
        }
        poisonFlag = false;
    }
}
