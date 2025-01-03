using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RobotPlant_Bomb : MonoBehaviour
{
    public GameObject Claymore;
    public int atk;
    bool bomb_flag;
    void Start()
    {
        bomb_flag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
        if (!bomb_flag)
        {
            foreach (Collider2D collider in colliders)
            {
                // 대상이 유닛 타입일 경우에만 데미지 적용
                if (collider.CompareTag("Monster"))
                {
                    collider.GetComponent<Monster>().Damage_Monster_BombAndDron(atk);
                }
            }
            bomb_flag = true;
        }

        //여기서 폭탄 애니메이션 
        Destroy(Claymore, 3f);
    }
}
