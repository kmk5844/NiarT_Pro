using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train_InGame : MonoBehaviour
{
    public Train_DataTable trainData;
    public int TrainNum;
    [Header("선택된 기차 정보")]
    public string Train_Name;
    public int Train_HP;
    public int Train_Weight;

    int Level_Anmor;
    public int Train_Anmor;

    public string Train_Type;
    public int Train_MaxSpeed;
    public int Train_Efficient;
    public int Train_Engine_Power;
    public int Train_Fuel;
    public int Train_Attack;
    public float Train_Attack_Delay;
    public int Train_Food;
    public int Train_Heal;
    [Header("HP 슬라이더")]
    Slider HP_Slider;
    public int cur_HP; //현재체력
    public bool isReparing;
    public bool isRepairable;
    [Header("방어 상수")]
    float era;
    [SerializeField]
    float def_constant;
    [Header("힐링기차")]
    public int Heal_Amount;
    public int Heal_timeBet;
    public bool isHealing;
    public bool openMedicTrian;
    Player player;
    GameObject GD;

    // Start is called before the first frame update
    private void Awake()
    {
        openMedicTrian = false;
        isHealing = false;
        isRepairable = true;
        GD = GameObject.Find("GameDirector");
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Train_Name = trainData.Information_Train[TrainNum].Train_Name;
        Train_HP = trainData.Information_Train[TrainNum].Train_HP;
        HP_Slider = GetComponentInChildren<Slider>();
        cur_HP = Train_HP;
        Train_Weight = trainData.Information_Train[TrainNum].Train_Weight;

        Level_Anmor = GD.GetComponent<GameDirector>().SA_TrainData.Level_Train_Armor;
        Train_Anmor = Level_ChangeArmor(trainData.Information_Train[TrainNum].Train_Armor);

        Train_Type = trainData.Information_Train[TrainNum].Train_Type;
        CheckType();
    }

    private void Update()
    {
        era = 1f - (float)Train_Anmor / def_constant; //만약에 방어력 증가해주는 기관사 타게 된다면 변경 가능성이 큼
        HP_Slider.value = (float)cur_HP / (float)Train_HP;

        //여기서 만약 기차가 파괴 당할 시 쓰면 좋은 함수

        if(cur_HP == 0)
        {
            switch (Train_Type)
            {
                case "Engine":
                    Destroy_Train(0);
                    break;
                case "Fuel":
                    Destroy_Train(1);
                    break;
                case "Attack":
                    Destroy_Train(0);
                    break;
                case "Medic":
                    Destroy_Train(1);
                    break;
            }
        }

        if (Train_Type.Equals("Medic"))
        {
            if(cur_HP == 0 || Train_Heal ==0)
            {
                openMedicTrian = false;
            }
            else
            {
                openMedicTrian = true;
            }
        }
    }

    void CheckType()
    {
        switch (Train_Type)
        {
            case "Engine":
                Train_MaxSpeed = trainData.Information_Train[TrainNum].Train_MaxSpeed;
                Train_Efficient = trainData.Information_Train[TrainNum].Train_Efficient;
                Train_Engine_Power = trainData.Information_Train[TrainNum].Train_Engine_Power;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Food = 0;
                Train_Heal = 0;
                break;
            case "Fuel":
                Train_MaxSpeed = 0;
                Train_Efficient = 0;
                Train_Engine_Power = 0;
                Train_Fuel = trainData.Information_Train[TrainNum].Train_Fuel;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Food = 0;
                Train_Heal = 0; 
                break;
            case "Attack":
                Train_MaxSpeed = 0;
                Train_Efficient = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = trainData.Information_Train[TrainNum].Train_Attack;
                Train_Attack_Delay = trainData.Information_Train[TrainNum].Train_Attack_Delay;
                Train_Food = 0;
                Train_Heal = 0;
                break;
            case "Warehouse":
                Train_MaxSpeed = 0;
                Train_Efficient = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Food = trainData.Information_Train[TrainNum].Train_Food;
                Train_Heal = 0;
                break;
            case "Medic":
                Train_MaxSpeed = 0;
                Train_Efficient = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Food = 0;
                Train_Heal = trainData.Information_Train[TrainNum].Train_Heal;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Train_MonsterHit(bullet);
            Destroy(collision.gameObject);
        }
    }
    private void Train_MonsterHit(Bullet monsterBullet)
    {
        GD.GetComponent<GameDirector>().Game_MonsterHit(monsterBullet.slow); //슬로우가 있어야 한다.
        int damageTaken = Mathf.RoundToInt(monsterBullet.atk * era);
        if(cur_HP - damageTaken < 0)
        {
            cur_HP = 0;
        }
        else
        {
            cur_HP -= damageTaken;
        }
        //-> 몬스터마다 쏘는 총알이 달라야 함.
    }

    private void Destroy_Train(int i) // 0 -> 특수 / 1 -> 일반 / 2 -> 배달
    {
        if(i == 0)
        {
            isRepairable = true;
        }
        else if(i == 1){
            isRepairable = false;
            GD.GetComponent<GameDirector>().Destroy_train_weight(Train_Weight);
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

        if (Train_Heal - Heal_Amount < 0)
        {
            Train_Heal = 0;
        }
        else
        {
            Train_Heal -= Heal_Amount;
        }
        player.Player_HP += Heal_Amount;
        yield return new WaitForSeconds(Heal_timeBet);

        isHealing = false;
    }

    public int Level_ChangeArmor(int trainArmor)
    {
        //데미지 경감이기 때문에 클수록 유리
        return trainArmor + (trainArmor * (Level_Anmor * 10) / 100);
    }
}