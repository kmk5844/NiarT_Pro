using UnityEngine;

public class TurretFactory_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float SpawnTime;
    public int TurretCount;

    public float lastTime;

    private void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector;

        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        TurretCount = int.Parse(trainData.trainData_Special_String[1]);
    }

    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if(!trainData.DestoryFlag)
            {
                if (Time.time > lastTime + SpawnTime)
                {
                    SpawnTurret();
                    lastTime = Time.time;
                }
            }

        }
    }

    public void SpawnTurret()
    {
        for (int i = 0; i < TurretCount; i++)
        {
            // 랜덤하게 생성
            int RandomNum = Random.Range(0, 7);
            SpawnTurret(RandomNum);
        }
    }


    void SpawnTurret(int num)
    {
        float pos = Random.Range(MonsterDirector.MinPos_Ground.x + 3.5f, MonsterDirector.MinPos_Ground.x - 3.5f);
        GameObject ItemTurret = null;
        float delayTime = Random.Range(5f, 10f);
        int atk = 0;
        float atkDelay = 0;
        switch (num)
        {
            case 0:
                atk = Random.Range(10, 31);
                atkDelay = Random.Range(0.15f, 0.3f);
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Auto_Turret");
                break;
            case 1:
                atk = 0;
                atkDelay = 0;
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Rope_Turret");
                break;
            case 2:
                atk = Random.Range(20, 41);
                atkDelay = 0;
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Fire_Turret");
                break;
            case 3:
                atk = 0;
                atkDelay = Random.Range(0.1f, 0.3f);
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Flare_Turret");
                break;
            case 4:
                atk = Random.Range(40, 61);
                atkDelay = Random.Range(2f, 4f);
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Missile_Turret");
                break;
            case 5:
                atk = Random.Range(10, 25);
                atkDelay = 0;
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Laser_Turret");
                break;
            case 6:
                atk = Random.Range(10, 25);
                atkDelay = 0;
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Laser_Turret");
                break;
        }
        ItemTurret.GetComponent<Item_Mini_Turret_Director>().Set(num, delayTime, atk, atkDelay);
        if (num != 6)
        {
            Instantiate(ItemTurret, new Vector2(pos, -0.55f), Quaternion.identity);
        }
        else
        {
            Instantiate(ItemTurret, new Vector2(MonsterDirector.MinPos_Ground.x + 3f, -0.55f), Quaternion.identity);
        }
    }
}