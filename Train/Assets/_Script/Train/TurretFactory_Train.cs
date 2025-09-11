using UnityEngine;

public class TurretFactory_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float elasped;
    public float SpawnTime;
    public int TurretCount;

    public float lastTime;
    public float pausedElapsed;

    private void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector;
        pausedElapsed = 0;

        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        TurretCount = int.Parse(trainData.trainData_Special_String[1]);

    }

    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (!trainData.DestoryFlag)
            {
                // 프레임 단위 누적
                elasped += Time.deltaTime;

                // 강제로 초 단위로 변환
                int elapsedSeconds = Mathf.FloorToInt(elasped);
                int spawnSeconds = Mathf.FloorToInt(SpawnTime);

                while (elapsedSeconds >= spawnSeconds)
                {
                    SpawnTurret();

                    // 초 단위 차감
                    elapsedSeconds -= spawnSeconds;

                    // elasped도 차감
                    elasped -= spawnSeconds;
                }
            }
        }
        else if (gameDirector.gameType == GameType.Refreshing)
        {
            // Refresh 상태에서는 누적 시간을 그대로 유지
            pausedElapsed = Time.time - lastTime;
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
        float pos = Random.Range(MonsterDirector.MinPos_Ground.x + 3.5f, MonsterDirector.MaxPos_Ground.x - 3.5f);
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
                atk = Random.Range(40, 61);
                atkDelay = Random.Range(1f, 3f);
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Missile_Turret2");
                break;
            case 6:
                atk = Random.Range(10, 25);
                atkDelay = Random.Range(0, 3);
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Laser_Turret");
                break;
            case 7:
                atk = Random.Range(10, 25);
                atkDelay = 0;
                ItemTurret = Resources.Load<GameObject>("ItemObject/Mini_Laser_Turret_2");
                break;
        }

        Debug.Log(ItemTurret);
        ItemTurret.GetComponent<Item_Mini_Turret_Director>().Set(num, delayTime, atk, atkDelay);

        if (num != 7)
        {
            Instantiate(ItemTurret, new Vector2(pos, -0.55f), Quaternion.identity);
        }
        else
        {
            Instantiate(ItemTurret, new Vector2(MonsterDirector.MinPos_Ground.x + 3.5f, -0.9f), Quaternion.identity);
        }
    }
}