using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDirector : MonoBehaviour
{
    // 스테이지 정보 나온 후, 스테이지에 따라 몬스터 변경해야함
    // 그리고 엑셀에 몬스터 정보도 나와야 한다.
    // 기차 길이에 따라 소환하는 위치가 달라야 한다.
    [Header("몬스터 정보 및 리스트")]
    public GameObject Monster;
    public Transform Monster_List;

    [Header("몬스터 스폰 설정")]
    public Vector2 MaxPos;
    public Vector2 MinPos;

    [Header("몬스터 한도 설정")]
    public int MaxMonsterNum;
    [SerializeField]
    int MonsterNum;

    float Random_xPos;
    float Random_yPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MonsterNum < MaxMonsterNum)
        {
            StartCoroutine(AppearMonster());
        }
    }

    IEnumerator AppearMonster()
    {
        MonsterNum++;
        yield return new WaitForSeconds(Random.Range(0.0f, 5.0f));
        Random_xPos = Random.Range(MinPos.x, MaxPos.x);
        Random_yPos = Random.Range(MinPos.y, MaxPos.y);
        Instantiate(Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Monster_List);
    }

    public void MonsterDie()
    {
        MonsterNum--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(MaxPos.x, MaxPos.y, 0), new Vector3(MaxPos.x, MinPos.y, 0));
        Gizmos.DrawLine(new Vector3(MaxPos.x, MinPos.y, 0), new Vector3(MinPos.x, MinPos.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos.x, MinPos.y, 0), new Vector3(MinPos.x, MaxPos.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos.x, MaxPos.y, 0), new Vector3(MaxPos.x, MaxPos.y, 0));
    }
}
