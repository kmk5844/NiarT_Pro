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

    [Header("���õ� ���� ����")]
    public int Train_HP;
    public int Train_Weight;

    int Level_Anmor;
    public int Train_Armor;

    public string Train_Type;
    public int Train_MaxSpeed;
    public int Train_Efficient;
    public int Train_Engine_Power;
    public int Train_Fuel;
    public int Train_Attack;
    public float Train_Attack_Delay;
    public int Train_Heal;
    public int cur_HP; //����ü��
    float HP_Parsent;
    public bool isReparing;
    public bool isRepairable;
    [Header("��� ���")]
    float era;
    [SerializeField]
    float def_constant;
    [Header("��������")]
    public int Heal_Amount;
    public int Heal_timeBet;
    public bool isHealing;
    public bool openMedicTrian;
    Player player;
    GameObject gameDirector;

    // Start is called before the first frame update
    private void Awake()
    {
        openMedicTrian = false;
        isHealing = false;
        isRepairable = true;
        gameDirector = GameObject.Find("GameDirector");
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        TurretFlag = false;
        BoosterFlag = false;
        era = 100;
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
            Train_HP = trainData.Information_Train_Turret_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Turret_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Turret_Part[Train_Num2].Train_Armor;
        }
        else if (BoosterFlag)
        {
            Train_HP = trainData.Information_Train_Booster_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Booster_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Booster_Part[Train_Num2].Train_Armor;
        }
        else
        {
            Train_HP = trainData.Information_Train[Train_Num].Train_HP;
            Train_Weight = trainData.Information_Train[Train_Num].Train_Weight;
            Train_Armor = trainData.Information_Train[Train_Num].Train_Armor;
        }
        cur_HP = Train_HP;
        Train_Type = trainData.Information_Train[Train_Num].Train_Type;
        CheckType();
    }

    private void Update()
    {
        era = 1f - (float)Train_Armor / def_constant; //���࿡ ���� �������ִ� ����� Ÿ�� �ȴٸ� ���� ���ɼ��� ŭ
        HP_Parsent = (float)cur_HP / (float)Train_HP * 100f;
        //���⼭ ���� ������ �ı� ���� �� ���� ���� �Լ�

        if (cur_HP == 0)
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
                    Destroy_Train(0);
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
        Train_MaxSpeed = 0;
        Train_Efficient = 0;
        Train_Engine_Power = 0;
        Train_Fuel = 0;
        Train_Attack = 0;
        Train_Attack_Delay = 0;
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
            case "Medic":
                Train_Heal = trainData.Information_Train[Train_Num].Train_Heal;
                break;
        }
    }
    public void Train_MonsterHit(MonsterBullet monsterBullet)
    {
        gameDirector.GetComponent<GameDirector>().Game_MonsterHit(monsterBullet.slow); //���ο찡 �־�� �Ѵ�.
        int damageTaken = Mathf.RoundToInt(monsterBullet.atk * era);
        if(cur_HP - damageTaken < 0)
        {
            cur_HP = 0;
        }
        else
        {
            cur_HP -= damageTaken;
        }
    }

    private void Destroy_Train(int i) // 0 -> Ư�� / 1 -> �Ϲ� / 2 -> ���
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
            //���� ���Ϳ� ���� End �����ؾߵȴ�.
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
        //������ �氨�̱� ������ Ŭ���� ����
        return trainArmor + (trainArmor * (Level_Anmor * 10) / 100);
    }
}