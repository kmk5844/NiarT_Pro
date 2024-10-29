using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train_InGame : MonoBehaviour
{
    public Game_DataTable trainData;

    [SerializeField]
    int Train_Num;
    [SerializeField]
    int Train_Num2;
    bool TurretFlag;
    bool BoosterFlag;

    [Header("선택된 기차 정보")]
    public int Train_HP; //현재체력
    [HideInInspector]
    public float HP_Parsent;
    [HideInInspector]
    public int Max_Train_HP;
    public int Train_Weight;

    int Level_Anmor;
    public int Train_Armor;

    public string Train_Type;
    //엔진
    [Header("엔진")]
    public int Train_MaxSpeed;
    public int Train_Efficient;
    public int Train_Engine_Power;
    //연료
    [Header("연료")]
    public int Train_Fuel;
    //포탑
    [Header("포탑")]
    public int Train_Attack;
    public float Train_Attack_Delay;
    //부스터
    [Header("부스터")]
    public int Train_WarningSpeed;
    public int Train_BoosterFuel;
    public int Train_UseFuel;
    public int Train_BoosterSpeedUP;
    public int Train_BoosterTime;
    //특수 스트링
    [Header("특수 파라미터")]
    public string Train_Special;
    public bool Not_DestoryTrain;


    //의무실
    [Header("의무실")]
    public float Train_Heal;
    [HideInInspector]
    public float Max_Train_Heal;
    public int Train_Heal_Amount;
    public float Train_Heal_timeBet;
    [Header("의무실 특수 플래그")]
    public bool isHealing;
    [Header("수동 포탑")]
    public int Train_Self_UseFuel;
    public int Train_Self_Attack;
    public float Train_Self_Attack_Delay;
    public int Train_Self_Second;
    [Header("연락실")]
    public int Train_Supply_UseFuel;
    public int Train_Supply_Grade;
    public int Train_Supply_Count;
    [Header("대쉬 보드")]
    public int Train_Dash_UseFuel;
    public float Train_Dash_PalyerAmount;
    public int Train_Dash_Second;

    public bool isReparing;
    public bool isRepairable;
    float era;
    float def_constant;
    Player player;
    [HideInInspector]
    public GameObject gameDirector;

    private void Awake()
    {
        Not_DestoryTrain = false;
        isHealing = false;
        isRepairable = true;
        gameDirector = GameObject.Find("GameDirector");
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        TurretFlag = false;
        BoosterFlag = false;
        def_constant = 100;
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        string[] name = gameObject.name.Split('_');

        if (name[0].Equals("51"))
        {
            TurretFlag = true;
            Train_Num = int.Parse(name[0]);
            Train_Num2 = int.Parse(name[1]);
        }
        else if (name[0].Equals("52"))
        {
            BoosterFlag = true;
            Train_Num = int.Parse(name[0]);
            Train_Num2 = int.Parse(name[1]);
        }
        else
        {
            Train_Num = int.Parse(gameObject.name);
        }

        Level_Anmor = gameDirector.GetComponent<GameDirector>().SA_TrainData.Level_Train_Armor;
        if (TurretFlag)
        {
            Max_Train_HP = trainData.Information_Train_Turret_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Turret_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Turret_Part[Train_Num2].Train_Armor;
        }
        else if (BoosterFlag)
        {
            Max_Train_HP = trainData.Information_Train_Booster_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Booster_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Booster_Part[Train_Num2].Train_Armor;
        }
        else
        {
            Max_Train_HP = trainData.Information_Train[Train_Num].Train_HP;
            Train_Weight = trainData.Information_Train[Train_Num].Train_Weight;
            Train_Armor = trainData.Information_Train[Train_Num].Train_Armor;
        }
        Train_HP = Max_Train_HP;
        Train_Type = trainData.Information_Train[Train_Num].Train_Type;
        CheckType();
    }

    private void Update()
    {
        era = 1f - (float)Train_Armor / def_constant; //만약에 방어력 증가해주는 기관사 타게 된다면 변경 가능성이 큼
        HP_Parsent = (float)Train_HP / (float)Max_Train_HP * 100f;
        //여기서 만약 기차가 파괴 시 쓰면 좋은 함수 (업데이트 문이라면 조심)

        if (Train_HP <= 0)
        {
            switch (Train_Type)
            {
                case "Engine":
                    Destroy_Train(0);
                    break;
                case "Fuel":
                    Destroy_Train(1);
                    break;
                case "Turret":
                    Destroy_Train(1);
                    break;
                case "Medic":
                    Destroy_Train(1);
                    break;
                case "Booster":
                    Destroy_Train(1);
                    break;
            }
        }

        if (Train_Type.Equals("Medic"))
        {
            if(Train_HP == 0 || Train_Heal ==0)
            {
                Not_DestoryTrain = false;
            }
            else
            {
                Not_DestoryTrain = true;
            } 
        }
    }

    void CheckType()
    {
        Train_MaxSpeed = 0;
        Train_Efficient = 0;
        Train_Engine_Power = 0;
        Train_Fuel = 0;
        Train_Attack = 0;
        Train_Attack_Delay = 0;
        Train_WarningSpeed = 0;
        Train_BoosterFuel = 0;
        Train_UseFuel = 0;
        Train_BoosterSpeedUP = 0;
        Train_BoosterTime = 0;
        Train_Heal = 0;
        switch (Train_Type)
        {
            case "Engine":
                Train_MaxSpeed = trainData.Information_Train[Train_Num].Train_MaxSpeed;
                Train_Efficient = trainData.Information_Train[Train_Num].Train_Efficient;
                Train_Engine_Power = trainData.Information_Train[Train_Num].Train_Engine_Power;
                break;
            case "Fuel":
                Train_Fuel = trainData.Information_Train[Train_Num].Train_Fuel;
                break;
            case "Turret":
                Train_Attack = trainData.Information_Train_Turret_Part[Train_Num2].Train_Attack;
                Train_Attack_Delay = trainData.Information_Train_Turret_Part[Train_Num2].Train_Attack_Delay;
                break;
            case "Booster":
                Train_WarningSpeed = trainData.Information_Train_Booster_Part[Train_Num2].Train_WarningSpeed;
                Train_BoosterFuel = trainData.Information_Train_Booster_Part[Train_Num2].Train_BoosterFuel;
                Train_UseFuel = trainData.Information_Train_Booster_Part[Train_Num2].Train_UseFuel;
                Train_BoosterSpeedUP = trainData.Information_Train_Booster_Part[Train_Num2].Train_BoosterSpeedUP;
                break;
            case "Medic":
                string[] trainData_Special_String = trainData.Information_Train[Train_Num].Train_Special.Split(',');
                Train_Heal = int.Parse(trainData_Special_String[0]);
                Train_Heal_Amount = int.Parse(trainData_Special_String[1]);
                Train_Heal_timeBet = float.Parse(trainData_Special_String[2]);
                Max_Train_Heal = Train_Heal;
                break;
            case "Self_Turret":
                trainData_Special_String = trainData.Information_Train[Train_Num].Train_Special.Split(',');
                Train_Self_UseFuel = int.Parse(trainData_Special_String[0]);
                Train_Self_Attack = int.Parse(trainData_Special_String[1]);
                Train_Self_Attack_Delay = float.Parse(trainData_Special_String[2]);
                Train_Self_Second = int.Parse(trainData_Special_String[3]);
                break;
/*            case "Dash":
                trainData_Special_String = trainData.Information_Train[Train_Num].Train_Special.Split(',');
                Train_Dash_UseFuel = int.Parse(trainData_Special_String[0]);
                Train_Dash_PalyerAmount = float.Parse(trainData_Special_String[1]);
                Train_Dash_Second = int.Parse(trainData_Special_String[2]);
                break;*/
            case "Supply":
                trainData_Special_String = trainData.Information_Train[Train_Num].Train_Special.Split(',');
                Train_Supply_UseFuel = int.Parse(trainData_Special_String[0]);
                Train_Supply_Grade = int.Parse(trainData_Special_String[1]);
                Train_Supply_Count = int.Parse(trainData_Special_String[2]);
                break;
        }
    }
    public void Train_MonsterHit(MonsterBullet monsterBullet)
    {
        gameDirector.GetComponent<GameDirector>().Game_MonsterHit(monsterBullet.slow); //슬로우가 있어야 한다.
        int damageTaken = Mathf.RoundToInt(monsterBullet.atk * era);
        if(Train_HP - damageTaken < 0)
        {
            Train_HP = 0;
        }
        else
        {
            Train_HP -= damageTaken;
        }
    }

    private void Destroy_Train(int i) // 0 -> 특수 / 1 -> 일반 / 2 -> 퀘스트
    {
        if(i == 0)
        {
            isRepairable = true;
        }
        else if(i == 1){
            isRepairable = false;
            gameDirector.GetComponent<GameDirector>().Destroy_train_weight(Train_Weight);
            Train_Weight = 0;
        }
        else
        {
            //게임 디렉터에 가서 End 선언해야된다.
        }
    }

    public IEnumerator Train_Healing()
    {
        isHealing = true;

        if (Train_Heal - Train_Heal_Amount < 0)
        {
            Train_Heal = 0;
        }
        else
        {
            Train_Heal -= Train_Heal_Amount;
        }
        player.Player_HP += Train_Heal_Amount;
        yield return new WaitForSeconds(Train_Heal_timeBet);

        isHealing = false;
    }

    public int Level_ChangeArmor(int trainArmor)
    {
        //데미지 경감이기 때문에 클수록 유리
        return trainArmor + (trainArmor * (Level_Anmor * 10) / 100);
    }

    //item 부분
    public void Item_Train_Heal_HP(float persent)
    {
        Train_HP += (int)(Max_Train_HP * (persent / 100f));
    }

    public IEnumerator Item_Train_Turret_SpeedUP(float persent, float delayTime)
    {
        if (Train_Type.Equals("Turret")){
            transform.GetComponentInChildren<Turret>().Item_Turret_Attack_Speed_UP(persent, true);
            transform.GetComponentInChildren<Turret>().Item_Turret_Rotattion_Speed_UP(persent, true);
            yield return new WaitForSeconds(delayTime);
            transform.GetComponentInChildren<Turret>().Item_Turret_Attack_Speed_UP(persent, false);
            transform.GetComponentInChildren<Turret>().Item_Turret_Rotattion_Speed_UP(persent, false);
        }
    }
}