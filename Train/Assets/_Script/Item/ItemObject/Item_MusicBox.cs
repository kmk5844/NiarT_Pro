using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MusicBox : MonoBehaviour
{
    CircleCollider2D circle2D;
    bool MusicFlag;
    public bool changeFlag;
    public float SpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        MusicFlag = false;  
        changeFlag = false;
        circle2D = GetComponent<CircleCollider2D>();
        Destroy(gameObject, SpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!MusicFlag)
        {
            StartCoroutine(Music());
            MusicFlag = true;
        }
    }
    IEnumerator Music()
    {
        // 1차 성장: 0.5f ~ 4f
        while (circle2D.radius < 7f)
        {
            circle2D.radius += 0.2f;
            yield return null;
        }

        // 중단 및 상태 변경
        if (!changeFlag)
        {
            changeFlag = true;
        }else if (changeFlag)
        {
            changeFlag = false;
        }
        circle2D.radius = 0.5f;
        yield return new WaitForSeconds(0.5f); // 필요시 잠깐 대기
        MusicFlag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (changeFlag)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.Item_Player_Heal_HP(1);
                }
            }

            if (collision.gameObject.CompareTag("Mercenary"))
            {
                Mercenary mercenary = collision.gameObject.GetComponent<Mercenary>();
                if (mercenary != null)
                {
                    mercenary.Item_Mercenary_Heal_HP(1);
                }
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Monster"))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                if(monster != null)
                {
                    monster.Damage_Monster_Item(50);
                }
            }
        }
        
    }
}
