using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldGasStationDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject OldGasStationWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public Game_DataTable gamedatatable;
    public SA_PlayerData playerData;
    public SA_TrainData trainData;
    int totalcoin;
    int[] Parsent_FuelCharge = { 50, 75, 100 };
    int[] Parsent_Coin = { 10, 30, 50 };
    int[] ChargeFuel;
    int[] giveCoin;

    int TotalFuel;
    int currentFuel;
    int damageFuel;

    bool startFlag;

    [Header("UI")]
    public Button[] button;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        OldGasStationWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            //Debug.Log("작동");
            QualitySettings.vSyncCount = 0;
        }

        int trainNum;
        for (int i = 0; i < trainData.Train_Num.Count; i++)
        {
            trainNum = trainData.Train_Num[i];
            if (trainNum >= 10 && trainNum < 20)
            {
                int TrainFuel = int.Parse(gamedatatable.Information_Train[trainNum].Train_Special);
                TotalFuel += TrainFuel;
            }
        }
        currentFuel = ES3.Load<int>("Train_Curret_Fuel");
        damageFuel = TotalFuel - currentFuel;

        giveCoin = new int[3];
        ChargeFuel = new int[3];

        for (int i = 0; i < 3; i++)
        {
            giveCoin[i] = totalcoin * Parsent_Coin[i] / 100;
            ChargeFuel[i] = damageFuel * Parsent_FuelCharge[i] / 100;
        }
    }

    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }

    private void StartEvent()
    {
        OldGasStationWindow.SetActive(true);
        startFlag = true;
    }

    public void ClickRewardButton(int i)
    {
        int num = i;
        Change(num);
        playerData.SA_Buy_Coin(giveCoin[num]);
        CheckWindow.SetActive(true);
        for (int j = 0; j < button.Length; j++)
        {
            button[j].interactable = false;
        }
    }

    void Change(int i)
    {
        currentFuel = Mathf.Min(currentFuel + ChargeFuel[i], TotalFuel);
        ES3.Save<int>("Train_Curret_Fuel", currentFuel);
    }

    public void OldGamStationEnd()
    {
        SelectStage.SetActive(true);
    }
}
