using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronFactoryTrain : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float elapsed;
    public float SpawnTime;
    public int minHP;
    public int maxHP;

    public float lastTime;
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
                elapsed = Time.time - lastTime;
                if (Time.time > lastTime + SpawnTime)
                {
                    SpanwDron();
                    lastTime = Time.time;
                }
            }
        }
    }

    void SpanwDron()
    {
        GameObject MiniDeffeceDron = Resources.Load<GameObject>("ItemObject/MiniDeffenceDron");
        Vector2 pos = new Vector2(MonsterDirector.MinPos_Sky.x - 2, 3f);
        GameObject dron = Instantiate(MiniDeffeceDron, pos, Quaternion.identity);
        int hp = Random.Range(minHP, maxHP);
        hp *= 100;
        dron.GetComponent<Item_MiniDron>().DeffeceDronSet(hp);
    }
}
