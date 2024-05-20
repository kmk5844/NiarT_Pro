using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDirector : MonoBehaviour
{
    // �������� ���� ���� ��, ���������� ���� ���� �����ؾ���
    // �׸��� ������ ���� ������ ���;� �Ѵ�.
    public Game_DataTable EX_GameData;
    public SA_PlayerData SA_PlayerData;
    public bool TestMonsterCount;

    [Header("���� ���� �� ����Ʈ")]
    public Transform Monster_List;
    List<int> Emerging_Monster_List;

    [Header("���� ���� ���� ����")]
    [SerializeField] Vector2 MaxPos_Sky;
    [SerializeField] Vector2 MinPos_Sky;

    [Header("���� ���� ���� ����")]
    [SerializeField] Vector2 MaxPos_Ground;
    [SerializeField] Vector2 MinPos_Ground;

    public bool GameDirector_StartFlag;
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

    private void Awake()
    {
        GameDirector_StartFlag = false;
        if (TestMonsterCount)
        {
            MaxMonsterNum = 1;
        }
        else
        {
            MaxMonsterNum = EX_GameData.Information_Stage[SA_PlayerData.Stage].Monster_Count;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TrainCount = Train_List.childCount;
        MaxPos_Sky = new Vector2(5f, 6f);
        MinPos_Sky = new Vector2((-10f * TrainCount), 3f);

        MaxPos_Ground = new Vector2(3f, -0.9f);
        MinPos_Ground = new Vector2(((-10f * (TrainCount)+3)), -0.9f);
        //���� ��ȯ ��ġ�� �޶�����.
        //���� ���̿� ���� ���ؾ��Ѵ�.
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDirector_StartFlag)
        {
            MonsterNum = Monster_List.childCount;
            if (MonsterNum < MaxMonsterNum && !isSpawing)
            {
                StartCoroutine(AppearMonster());
            }
        }
    }

    IEnumerator AppearMonster()
    {
        isSpawing = true;
        yield return new WaitForSeconds(Random.Range(1f, 7f));
        int MonsterRandomIndex = Random.Range(0, Emerging_Monster_List.Count);
        Check_Sky_OR_Ground_Monster(Emerging_Monster_List[MonsterRandomIndex]);
        isSpawing = false;
    }

    private void Check_Sky_OR_Ground_Monster(int Monster_Num)
    {
        if(EX_GameData.Information_Monster[Monster_Num].Monster_Type == "Sky")
        {
            Random_xPos = Random.Range(MinPos_Sky.x, MaxPos_Sky.x);
            Random_yPos = Random.Range(MinPos_Sky.y, MaxPos_Sky.y);
        }
        else if(EX_GameData.Information_Monster[Monster_Num].Monster_Type == "Ground")
        {
            Random_xPos = Random.Range(MinPos_Ground.x, MaxPos_Ground.x);
            Random_yPos = Random.Range(MinPos_Ground.y, MaxPos_Ground.y);
        }
        GameObject Monster = Resources.Load<GameObject>("Monster/Monster_" + Monster_Num);
        Instantiate(Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Monster_List);
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

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(MaxPos_Ground.x, MaxPos_Ground.y, 0), new Vector3(MaxPos_Ground.x, MinPos_Ground.y, 0));
        Gizmos.DrawLine(new Vector3(MaxPos_Ground.x, MinPos_Ground.y, 0), new Vector3(MinPos_Ground.x, MinPos_Ground.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Ground.x, MinPos_Ground.y, 0), new Vector3(MinPos_Ground.x, MaxPos_Ground.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Ground.x, MaxPos_Ground.y, 0), new Vector3(MaxPos_Ground.x, MaxPos_Ground.y, 0));
    }
}
