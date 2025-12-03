using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_39_Zone : MonoBehaviour
{
    private bool enterFlag = false;      // Enter가 호출되면 true
    public bool PlayerInZone { get; private set; }

    [SerializeField]
    private Collider2D playerCol;        // 플레이어 collider 저장
    [SerializeField]
    private Collider2D zoneCol;          // 이 Zone collider

    Monster_39 director_39;
    Monster_50 director_50;

    private void Awake()
    {
        zoneCol = GetComponent<Collider2D>();
    }
    private void Start()
    {
        director_39 = GetComponentInParent<Monster_39>();
        director_50 = GetComponentInParent<Monster_50>();
    }   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enterFlag = true;
            playerCol = other;           // 플레이어 collider 기억해둠
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enterFlag = false;
            playerCol = null;
        }
    }

    private void Update()
    {
        // Enter된 상태라면 실제로 아직 안에 있는지 Overlap으로 재확인
        if (enterFlag && playerCol != null)
        {
            // 실제 충돌 여부 체크
            bool stillInside = zoneCol.IsTouching(playerCol);

            PlayerInZone = stillInside;

            // 물리누락으로 Exit이 호출되지 않았는데 빠져나간 경우 처리
            if (!stillInside)
            {
                enterFlag = false;
                playerCol = null;
            }
        }
        else
        {
            PlayerInZone = false;
        }


        if (director_39 != null)
        {
            if (PlayerInZone)
            {
                director_39.changeAtkFlag(true);
            }
            else
            {
                director_39.changeAtkFlag(false);
            }
        }

        if (director_50 != null)
        {
            if (PlayerInZone)
            {
                director_50.changeAtkFlag(true);
            }
            else
            {
                director_50.changeAtkFlag(false);
            }
        }
        
    }
}
