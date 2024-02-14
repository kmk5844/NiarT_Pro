using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour
{
    [Header("기차 선택")]
    public Train_DataTable trainData;
    public int TrainNum;
    [Header("선택된 기차 정보")]
    public string Train_Name;
    public int Train_HP;
    public int Train_Weight;
    public int Train_Anmor;
    public string Train_Type;
    public int Train_MaxSpeed;
    public int Train_Efficienl;
    public int Train_Engine_Power;
    public int Train_Fuel;
    public int Train_Attack;
    public float Train_Attack_Delay;
    public int Train_Food;
    public int Train_Heal;
    [Header("HP 슬라이더")]
    public Slider HP_Slider;
    
    public int cur_HP; //현재체력
    GameDirector GD;

    // Start is called before the first frame update
    private void Awake()
    {
        GD = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        Train_Name = trainData.Information_Train[TrainNum].Train_Name;
        Train_HP = trainData.Information_Train[TrainNum].Train_HP;
        cur_HP = Train_HP;
        Train_Weight = trainData.Information_Train[TrainNum].Train_Weight;
        Train_Anmor = GD.Level_ChangeArmor(trainData.Information_Train[TrainNum].Train_Armor);
        Train_Type = trainData.Information_Train[TrainNum].Train_Type;
        CheckType();
    }

    private void Update()
    {
        HP_Slider.value = (float)cur_HP / (float)Train_HP;
    }

    void CheckType()
    {
        switch (Train_Type)
        {
            case "Engine":
                Train_MaxSpeed = trainData.Information_Train[TrainNum].Train_MaxSpeed;
                Train_Efficienl = trainData.Information_Train[TrainNum].Train_Efficienl;
                Train_Engine_Power = trainData.Information_Train[TrainNum].Train_Engine_Power;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Food = 0;
                Train_Heal = 0;
                break;
            case "Fuel":
                Train_MaxSpeed = 0;
                Train_Efficienl = 0;
                Train_Engine_Power = 0;
                Train_Fuel = trainData.Information_Train[TrainNum].Train_Fuel;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Food = 0;
                Train_Heal = 0; 
                break;
            case "Attack":
                Train_MaxSpeed = 0;
                Train_Efficienl = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = trainData.Information_Train[TrainNum].Train_Attack;
                Train_Attack_Delay = trainData.Information_Train[TrainNum].Train_Attack_Delay;
                Train_Food = 0;
                Train_Heal = 0;
                break;
            case "Warehouse":
                Train_MaxSpeed = 0;
                Train_Efficienl = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Food = trainData.Information_Train[TrainNum].Train_Food;
                Train_Heal = 0;
                break;
            case "Medic":
                Train_MaxSpeed = 0;
                Train_Efficienl = 0;
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
        GD.Game_MonsterHit(monsterBullet.slow); //슬로우가 있어야 한다.
        cur_HP -= monsterBullet.atk - Train_Anmor; // 몬스터의 데미지를 넣어야 한다
        //-> 몬스터마다 쏘는 총알이 다르다고 가정하자
    }
}