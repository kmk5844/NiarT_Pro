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

    [Header("���� ���� ����")]
    [SerializeField] Vector2 MaxPos;
    [SerializeField] Vector2 MinPos;
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
        MaxPos = new Vector2(10f, 8f);
        MinPos = new Vector2(-6f + ((TrainCount - 1) * -10f), 4f);
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
