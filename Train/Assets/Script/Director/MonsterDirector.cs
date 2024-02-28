using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDirector : MonoBehaviour
{
    // 스테이지 정보 나온 후, 스테이지에 따라 몬스터 변경해야함
    // 그리고 엑셀에 몬스터 정보도 나와야 한다.
    [Header("몬스터 정보 및 리스트")]
    public GameObject Monster;
    public Transform Monster_List;

    [Header("몬스터 스폰 설정")]
    [SerializeField] Vector2 MaxPos;
    [SerializeField] Vector2 MinPos;
    bool isSpawing = false;

    [Header("몬스터 한도 설정")]
    public int MaxMonsterNum;
    [SerializeField]
    int MonsterNum;

    [Header("기차 정보")]
    public Transform Train_List;
    int TrainCount;

    float Random_xPos;
    float Random_yPos;

    // Start is called before the first frame update
    void Start()
    {
        TrainCount = Train_List.childCount;
        MaxPos = new Vector2(10f, 8f);
        MinPos = new Vector2(-6f + ((TrainCount - 1) * -10f), 4f);
        //몬스터 소환 위치가 달라진다.
        //기차 길이에 따라 정해야한다.
    }

    // Update is called once per frame
    void Update()
    {
        MonsterNum = Monster_List.childCount;
        if(MonsterNum < MaxMonsterNum && !isSpawing)
        {
            StartCoroutine(AppearMonster());
        }
    }

    IEnumerator AppearMonster()
    {
        isSpawing = true;
        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        Random_xPos = Random.Range(MinPos.x, MaxPos.x);
        Random_yPos = Random.Range(MinPos.y, MaxPos.y);
        Instantiate(Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Monster_List);
        isSpawing = false;
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
