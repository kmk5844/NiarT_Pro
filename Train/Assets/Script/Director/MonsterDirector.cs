using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDirector : MonoBehaviour
{
    // �������� ���� ���� ��, ���������� ���� ���� �����ؾ���
    // �׸��� ������ ���� ������ ���;� �Ѵ�.
    [Header("���� ���� �� ����Ʈ")]
    public GameObject Monster;
    public Transform Monster_List;
    List<int> Emerging_Monster_List;

    [Header("���� ���� ���� ����")]
    [SerializeField] Vector2 MaxPos_Sky;
    [SerializeField] Vector2 MinPos_Sky;
    bool isSpawing = false;

    [Header("���� �ѵ� ����")]
    public int MaxMonsterNum;
    [SerializeField]
    int MonsterNum;

    [Header("���� ����")]
    public Transform Train_List;
    int TrainCount;

    float Random_xPos;
    float Random_yPos;

    // Start is called before the first frame update
    void Start()
    {
        TrainCount = Train_List.childCount;
        MaxPos_Sky = new Vector2(10f, 8f);
        MinPos_Sky = new Vector2(-6f + ((TrainCount - 1) * -10f), 4f);
        //���� ��ȯ ��ġ�� �޶�����.
        //���� ���̿� ���� ���ؾ��Ѵ�.
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
        Random_xPos = Random.Range(MinPos_Sky.x, MaxPos_Sky.x);
        Random_yPos = Random.Range(MinPos_Sky.y, MaxPos_Sky.y);
        int MonsterRandom = Random.Range(0, Emerging_Monster_List.Count + 1);
        GameObject MonsterObject = Instantiate(Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Monster_List);
        MonsterObject.GetComponent<Monster_0>().Monster_Num = MonsterRandom;
        isSpawing = false;
    }

    public void Get_Monster_List(List<int> GameDirector_Monster_List)
    {
        Emerging_Monster_List = GameDirector_Monster_List;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(MaxPos_Sky.x, MaxPos_Sky.y, 0), new Vector3(MaxPos_Sky.x, MinPos_Sky.y, 0));
        Gizmos.DrawLine(new Vector3(MaxPos_Sky.x, MinPos_Sky.y, 0), new Vector3(MinPos_Sky.x, MinPos_Sky.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Sky.x, MinPos_Sky.y, 0), new Vector3(MinPos_Sky.x, MaxPos_Sky.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Sky.x, MaxPos_Sky.y, 0), new Vector3(MaxPos_Sky.x, MaxPos_Sky.y, 0));
    }
}
