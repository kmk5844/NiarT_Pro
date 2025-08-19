using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train_InGame : MonoBehaviour
{
    public Game_DataTable trainData;
    public int Train_Index;

    [SerializeField]
    int Train_Num;
    [SerializeField]
    int Train_Num2;
    bool TurretFlag;

    [Header("선택된 기차 정보")]
    public int Train_HP; //현재체력
    [HideInInspector]
    public float HP_Parsent = 100f;
    [HideInInspector]
    public int Max_Train_HP;
    public int Train_Weight;

    int Level_Anmor;
    public int Train_Armor;

    public string Train_Type;
    public bool DestoryFlag;

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
    //특수 스트링
    [Header("특수 파라미터")]
    public string Train_Special;
    public bool Not_DestoryTrain;
    public string[] trainData_Special_String;
    //의무실
    [Header("의무실")]
    public float Train_Heal;
    [HideInInspector]
    public float Max_Train_Heal;
    public int Train_Heal_Amount;
    public float Train_Heal_timeBet;
    [Header("의무실 특수 플래그")]
    public bool isHealing;
    [Header("아이템")]
    bool ItemIronPlateFlag;
    GameObject IronPlate;

    public bool isReparing;
    public bool isRepairable;
    float era;
    float def_constant;
    Player player;
    [HideInInspector]
    public GameDirector gameDirector;
    public int UI_Level;
    bool LoseFlag;
    SpriteRenderer TrainSprite;
    public AudioClip trainHitSFX;

    [Header("엔지니어")]
    int EngineerCallHpCheck;
    int cooltime;
    bool isEngineerCallFlag;
    bool isCooltimeCheckFlag;

    private void Awake()
    {
        Not_DestoryTrain = false;
        isHealing = false;
        isRepairable = true;
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        TurretFlag = false;
        //BoosterFlag = false;
        def_constant = 100;
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        string[] name = gameObject.name.Split('_');

        if (name[0].Equals("91"))
        {
            TurretFlag = true;
            Train_Num = int.Parse(name[0]);
            Train_Num2 = int.Parse(name[1]);
        }
/*        else if (name[0].Equals("52"))
        {
            BoosterFlag = true;
            Train_Num = int.Parse(name[0]);
            Train_Num2 = int.Parse(name[1]);
        }*/
        else
        {
            Train_Num = int.Parse(gameObject.name);
        }


        Level_Anmor = gameDirector.SA_TrainData.Level_Train_Armor;
        if (TurretFlag)
        {
            Max_Train_HP = trainData.Information_Train_Turret_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Turret_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Turret_Part[Train_Num2].Train_Armor;
        }
/*        else if (BoosterFlag)
        {
            Max_Train_HP = trainData.Information_Train_Booster_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Booster_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Booster_Part[Train_Num2].Train_Armor;
        }*/
        else
        {
            if(Train_Num != 90)
            {
                Max_Train_HP = trainData.Information_Train[Train_Num].Train_HP;
                Train_Weight = trainData.Information_Train[Train_Num].Train_Weight;
                Train_Armor = trainData.Information_Train[Train_Num].Train_Armor;
            }
        }

        if(gameDirector.Select_Sub_Num == 0)
        {
            Train_HP = Max_Train_HP;
        }
        else
        {
            Train_HP = ES3.Load<int>("Train_Curret_HP_TrainIndex_" + Train_Index, Max_Train_HP);
            Train_HP = (Train_HP * Max_Train_HP) /100;
        }
        Train_Type = trainData.Information_Train[Train_Num].Train_Type;
        CheckType();
    }

    private void Start()
    {
        TrainSprite = GetComponentInChildren<SpriteRenderer>();
        gameObject.name = Train_Index + ". " + gameObject.name;
    }

    private void Update()
    {
        era = 1f - (float)Train_Armor / def_constant; //만약에 방어력 증가해주는 기관사 타게 된다면 변경 가능성이 큼
        HP_Parsent = (float)Train_HP / (float)Max_Train_HP * 100f;
        //여기서 만약 기차가 파괴 시 쓰면 좋은 함수 (업데이트 문이라면 조심)

        if (HP_Parsent < 50f)
        {
            TrainSprite.color = Color.Lerp(Color.white, Color.red, ((100f - (HP_Parsent * 2)) / 100f));
        }

        if (Train_HP <= 0)
        {
            if (!DestoryFlag)
            {
                DestoryFlag = true;
            }

            switch (Train_Type)
            {
                case "Engine":
                    if (!LoseFlag)
                    {
                        Destroy_Train(0);
                    }
                    break;
                case "Fuel":
                case "Medic":
                case "Self_Turret":
                case "Supply":
                case "Booster":
                case "Repair":
                case "TurretFactory":
                case "DronFactory":
                case "Platform":
                case "FuelSignal":
                case "Hangar":
                case "IronPlateFactory":
                case "TurretUpgrade":
                    Destroy_Train(1);
                    break;
                case "Turret":
                    Destroy_Train(2);
                    break;
                case "Quest":
                    if (!LoseFlag)
                    {
                        Destroy_Train(3);
                    }
                    break;
            }
        }
        else
        {
            if(DestoryFlag == true){
                DestoryFlag = false;
            }
        }

        if (isRepairable)
        {
            if (HP_Parsent < EngineerCallHpCheck)
            {
                if (!isEngineerCallFlag && !isCooltimeCheckFlag)
                {
                    gameDirector.EngineerCall(this);
                    isEngineerCallFlag = true;
                }
            }
        }

        if (Train_Type.Equals("Medic"))
        {
            if (Train_HP == 0 || Train_Heal == 0)
            {
                Not_DestoryTrain = false;
            }
            else
            {
                Not_DestoryTrain = true;
            }
        }
        UI_Level = CheckLevel();
    }

    public int CheckLevel()
    {
        if(Train_Num == 91)
        {
            return Train_Num2 % 10;
        }
        else
        {
            return Train_Num % 10;
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
        Train_Heal = 0;
        trainData_Special_String = trainData.Information_Train[Train_Num].Train_Special.Split(',');
        switch (Train_Type)
        {
            case "Engine":
                Train_MaxSpeed = int.Parse(trainData_Special_String[0]);
                Train_Efficient = int.Parse(trainData_Special_String[1]);
                Train_Engine_Power = int.Parse(trainData_Special_String[2]);
                break;
            case "Fuel":
                Train_Fuel = int.Parse(trainData_Special_String[0]);
                break;
            case "Turret":
                trainData_Special_String = trainData.Information_Train[Train_Num].Train_Special.Split(',');
                Train_Attack = trainData.Information_Train_Turret_Part[Train_Num2].Train_Attack;
                Train_Attack_Delay = trainData.Information_Train_Turret_Part[Train_Num2].Train_Attack_Delay;
                break;
            case "Medic":
                Train_Heal = int.Parse(trainData_Special_String[0]);
                Max_Train_Heal = Train_Heal;
                break;
        }
    }
    public void Train_MonsterHit(MonsterBullet monsterBullet)
    {
        gameDirector.Game_MonsterHit(monsterBullet.slow); //슬로우가 있어야 한다.
        int damageTaken = Mathf.RoundToInt(monsterBullet.atk * era);
        MMSoundManagerSoundPlayEvent.Trigger(trainHitSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        if(Train_HP - damageTaken < 0)
        {
            Train_HP = 0;
        }
        else
        {
            Train_HP -= damageTaken;
        }
    }

    private void Destroy_Train(int i) // 0 -> 엔진 / 1 -> 일반 / 2 -> 포탑 /3 -> 퀘스트
    {
        //엔진 기차 파괴 : 게임 END

        //일반 기차 파괴 : 무게 경감 O / 상호작용 정지 / 수리 X
        // - 연료 충전 중단

        //포탑 기차 파괴 : 무게 경감 X / 포탑 기능 정지 / 수리 O

        // 퀘스트 기차 파괴 : 게임 END

        if(i == 0)
        {
            LoseFlag = true;
            gameDirector.DestoryEngine();
        }
        else if (i == 1)
        {
            isRepairable = false;
            gameDirector.Destroy_train_weight(Train_Weight);
            Train_Weight = 0;
        }
        else if(i == 2){
            isRepairable = true;
        }
        else if(i == 3)
        {
            LoseFlag = true;
            gameDirector.MissionFail();
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
        if (isRepairable)
        {
            int heal = (int)(Max_Train_HP * (persent / 100f));
            if (Train_HP + heal < Max_Train_HP)
            {
                Train_HP += heal;
            }
            else
            {
                Train_HP = Max_Train_HP;
            }
        }

        //Train_HP += (int)(Max_Train_HP * (persent / 100f));
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

    public void Item_Spawn_IronPlate(int hp, int sp_Num)
    {
        if (!ItemIronPlateFlag)
        {
            ItemIronPlateFlag = true;
            IronPlate = Instantiate(Resources.Load<GameObject>("ItemObject/Item_Iron_Plate"), transform.position , Quaternion.identity);
            IronPlate.transform.SetParent(transform);
            IronPlate.transform.localPosition = new Vector2(-0.4f, 0.9f);
            IronPlate.GetComponent<Item_Iron_Plate>().Set(this, hp, sp_Num);
        }
        else
        {
            IronPlate.GetComponent<Item_Iron_Plate>().Heal(hp);
            IronPlate.GetComponent<Item_Iron_Plate>().changeSprite(sp_Num);
        }
    }

    public void Item_Spawn_Iron_Destory()
    {
        if (ItemIronPlateFlag)
        {
            ItemIronPlateFlag = false;
        }
    }

    public IEnumerator Item_Armor_Up(int delaytime,int persent)
    {
        int item_armor = (int)(Train_Armor * (persent / 100f));
        Train_Armor += item_armor;
        yield return new WaitForSeconds(delaytime);
        Train_Armor -= item_armor;
        //era는 업데이트문에서 처리하고 있음.
    }

    public void RepairEngineerSet(int hp, int _cooltime)
    {
        isEngineerCallFlag = false;
        isCooltimeCheckFlag = false;
        EngineerCallHpCheck = hp;
        cooltime = _cooltime;
    }

    public void EngineerDie()
    {
        isEngineerCallFlag = false;
        isCooltimeCheckFlag = false;
    }

    public void RepairEnd()
    {
        StartCoroutine(RepairCoolTime());
    }

    IEnumerator RepairCoolTime()
    {
        isEngineerCallFlag = false;
        isCooltimeCheckFlag = true;
        yield return new WaitForSeconds(cooltime);
        isCooltimeCheckFlag = false;
    }

    //save 저장
    public void GameEnd_TrainSave()
    {
        int HP_Persent = (Train_HP * 100) / Max_Train_HP;
        ES3.Save<int>("Train_Curret_HP_TrainIndex_" + Train_Index, HP_Persent);
    }
}