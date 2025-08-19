using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSignalTrain : MonoBehaviour
{

    Train_InGame trainData;
    GameDirector gameDirector;

    public float elapsed;
    public float SpawnTime;
    public int minFuel;
    public int maxFuel;

    float lastTime;
    public bool useflag;

    public GameObject Supply;

    // Start is called before the first frame update
    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();
        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        minFuel = int.Parse(trainData.trainData_Special_String[1]);
        maxFuel = int.Parse(trainData.trainData_Special_String[2]) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (!trainData.DestoryFlag)
            {
                elapsed = Time.time - lastTime;
                if (Time.time > lastTime + SpawnTime && !useflag)
                {
                    useflag = true;
                }
            }
        }
    }

    public void ClickTrain()
    {
        int Fuel = Random.Range(minFuel, maxFuel);

        GameObject supply = Instantiate(Supply, new Vector2(transform.position.x + 2, transform.position.y + 15), Quaternion.identity);
        supply.GetComponent<SupplyRefresh_Item>().FuelSignalSet(gameDirector, Fuel);

        useflag = false;
        lastTime = Time.time;
    }
}