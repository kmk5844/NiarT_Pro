using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronFactoryTrain : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float elapsed;
    public float SpawnTime;
    public float pausedElapsed;
    public int minHP;
    public int maxHP;

    public float lastTime;

    public ParticleSystem UseEffect;
    private void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector;
        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        minHP = int.Parse(trainData.trainData_Special_String[1]);
        maxHP = int.Parse(trainData.trainData_Special_String[2]);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (!trainData.DestoryFlag)
            {
                // 프레임 단위 누적
                elapsed += Time.deltaTime;

                // 강제로 초 단위로 변환
                int elapsedSeconds = Mathf.FloorToInt(elapsed);
                int spawnSeconds = Mathf.FloorToInt(SpawnTime);

                while (elapsedSeconds >= spawnSeconds)
                {
                    SpanwDron();

                    // 초 단위 차감
                    elapsedSeconds -= spawnSeconds;

                    // elasped도 차감
                    elapsed -= spawnSeconds;
                }
            }
        }
        else if (gameDirector.gameType == GameType.Refreshing)
        {
            // Refresh 상태에서는 누적 시간을 그대로 유지
            pausedElapsed = Time.time - lastTime;
        }
    }

    void SpanwDron()
    {
        GameObject MiniDeffeceDron = Resources.Load<GameObject>("ItemObject/MiniDeffenceDron");
        Vector2 pos = new Vector2(MonsterDirector.MinPos_Sky.x - 2, 3f);
        UseEffect.Play();
        GameObject dron = Instantiate(MiniDeffeceDron, pos, Quaternion.identity);
        int hp = Random.Range(minHP, maxHP);
        hp *= 80;
        dron.GetComponent<Item_MiniDron>().DeffeceDronSet(hp);
    }
}
