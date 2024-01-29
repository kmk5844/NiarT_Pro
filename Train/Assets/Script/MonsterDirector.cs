using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDirector : MonoBehaviour
{
    // �������� ���� ���� ��, ���������� ���� ���� �����ؾ���
    // �׸��� ������ ���� ������ ���;� �Ѵ�.
    // ���� ���̿� ���� ��ȯ�ϴ� ��ġ�� �޶�� �Ѵ�.
    [Header("���� ���� �� ����Ʈ")]
    public GameObject Monster;
    public Transform Monster_List;

    [Header("���� ���� ����")]
    public Vector2 MaxPos;
    public Vector2 MinPos;

    [Header("���� �ѵ� ����")]
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
