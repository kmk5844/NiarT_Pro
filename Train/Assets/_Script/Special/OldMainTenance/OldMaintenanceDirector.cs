using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldMaintenanceDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject OldMainTenanceWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public Game_DataTable gamedatatable;
    public SA_PlayerData playerData;
    public SA_TrainData trainData;
    public SA_TrainTurretData turretData;

    int totalcoin;

    int[] Parsent_TrainHPCharge = {50, 75, 100 };
    int[] Parsent_Coin = { 10, 30, 50 };

    int[] Total_TrainHP;
    int[] Current_TrainHP;
    int[] Damage_TrainHP;
    int[,] Heal_TrainHP;

    int[] giveCoin;
    bool startFlag;

    [Header("UI")]
    public Button[] button;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        OldMainTenanceWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            //Debug.Log("작동");
            QualitySettings.vSyncCount = 0;
        }

        totalcoin = playerData.Coin;
        giveCoin = new int[3];
        int trainNum;
        int turretNum = 0;
        Total_TrainHP = new int[trainData.Train_Num.Count];
        Current_TrainHP = new int[trainData.Train_Num.Count];
        Damage_TrainHP = new int[trainData.Train_Num.Count];
        Heal_TrainHP = new int[trainData.Train_Num.Count,3];

        for (int i = 0; i < trainData.Train_Num.Count; i++)
        {
            trainNum = trainData.Train_Num[i];

            if (trainData.Train_Num[i] == 91)
            {
                Total_TrainHP[i] = gamedatatable.Information_Train_Turret_Part[turretNum].Train_HP;
                turretNum++;
            }
            else if (trainData.Train_Num[i] == 90)
            {
                Total_TrainHP[i] = 15000;
            }
            else
            {
                Total_TrainHP[i] = gamedatatable.Information_Train[trainNum].Train_HP;
            }

            Current_TrainHP[i] = ES3.Load<int>("Train_Curret_HP_TrainIndex_" + i, Total_TrainHP[i]);
            Damage_TrainHP[i] = Total_TrainHP[i] - Current_TrainHP[i];

            Heal_TrainHP[i,0] = Damage_TrainHP[i] * Parsent_TrainHPCharge[0] / 100;
            Heal_TrainHP[i,1] = Damage_TrainHP[i] * Parsent_TrainHPCharge[1] / 100;
            Heal_TrainHP[i,2] = Damage_TrainHP[i] * Parsent_TrainHPCharge[2] / 100;
        }

        for (int i = 0; i < 3; i++)
        {
            giveCoin[i] = totalcoin * Parsent_Coin[i] / 100;
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
        OldMainTenanceWindow.SetActive(true);
        startFlag = true;
    }

    public void ClickRewardButton(int i)
    {
        int num = i;
        Reward(num);
        playerData.SA_Buy_Coin(giveCoin[num]);
        CheckWindow.SetActive(true);
        for (int j = 0; j < button.Length; j++)
        {
            button[j].interactable = false;
        }
    }

    void Reward(int num)
    {
        for(int i = 0; i < trainData.Train_Num.Count; i++)
        {
            Current_TrainHP[i] = Mathf.Min(Current_TrainHP[i] + Heal_TrainHP[i, num], Total_TrainHP[i]);
            ES3.Save<int>("Train_Curret_HP_TrainIndex_" + i, Current_TrainHP[i]);
        }
    }

    public void OldMaintenanaceEnd()
    {
        SelectStage.SetActive(true);
    }
}
