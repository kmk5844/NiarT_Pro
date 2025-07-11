using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public bool On_ItemList_Test;

    ItemDirector itemDirector;
    public SA_ItemList itemList;
    public SA_ItemList_Test itemList_Test;


    [Header("영향받는 스크립트")]
    public Player player;
    public GameDirector gameDirector;
    public MercenaryDirector mercenaryDirector;
    public MonsterDirector monsterDirector;
    public UIDirector uiDirector;
    public MissionDirector missionDirector;

    private void Start()
    {
        itemDirector = GetComponent<ItemDirector>();
    }

    public void UseEquipItem(int num)
    {
        itemList.Item[num].Item_Count_Down();
        itemDirector.Get_Supply_Item_Information(itemList.Item[num]);
        if (!On_ItemList_Test)
        {
            switch (num)
            {
                case 0:
                    //용병 랜덤 부활;
                    break;
                case 1:
                    player.Item_Player_Heal_HP(15);
                    break;
                case 2:
                    player.Item_Player_Heal_HP(25);
                    break;
                case 3:
                    gameDirector.Item_Fuel_Charge(15);
                    break;
                case 4:
                    gameDirector.Item_Fuel_Charge(25);
                    break;
                case 5:
                    gameDirector.Item_Use_Train_Heal_HP(15);
                    break;
                case 6:
                    gameDirector.Item_Use_Train_Heal_HP(25);
                    break;
                case 7:
                    StartCoroutine(gameDirector.Item_Train_SpeedUp(15f));
                    break;
                case 8:
                    StartCoroutine(player.Item_Player_SpeedUP(1.5f, 20));
                    break;
                case 9:
                    StartCoroutine(player.Item_Player_SpeedUP(0.5f, 10));
                    StartCoroutine(player.Item_Player_Heal_HP_Auto(2f, 10));
                    break;
                case 10:
                    mercenaryDirector.Item_Use_Fatigue_Reliever(1, 10, 20);
                    break;
                case 11:
                    StartCoroutine(player.Item_Player_AtkUP(10, 20));
                    break;
                case 12:
                    //플레이어 상태이상 제거;
                    break;
                case 13:
                    player.Item_Player_Ballon_Bullet();
                    break;
                case 14:
                    StartCoroutine(gameDirector.Item_Coin_Double(10));
                    break;
                case 15:
                    gameDirector.Item_Use_Train_Turret_All_SpeedUP(15, 15);
                    break;
                case 16:
                    mercenaryDirector.Item_Use_Snack(10);
                    break;
                case 17:
                    StartCoroutine(player.Item_Player_DoubleAtkUP(5));
                    break;
                case 18:
                    player.Item_Player_Spawn_Claymore();
                    break;
                case 19:
                    player.Item_Player_Spawn_WireEntanglement();
                    break;
            }
        }
        else
        {
            switch (num)
            {
                case 0:
                    Debug.Log("부활 전용 -> 클릭 X");
                    break;
                case 1:
                    player.Item_Player_Heal_HP(5);
                    break;               
                case 2:
                    player.Item_Player_Heal_HP(10);
                    break;                
                case 3:
                    player.Item_Player_Heal_HP(15);
                    break;                
                case 4:
                    player.Item_Player_Heal_HP(20);
                    break;
                case 5:
                    gameDirector.Item_Fuel_Charge(5);
                    break;               
                case 6:
                    gameDirector.Item_Fuel_Charge(10);
                    break;             
                case 7:
                    gameDirector.Item_Fuel_Charge(15);
                    break;
                case 8:
                    gameDirector.Item_Fuel_Charge(20);
                    break;
                case 9:
                    gameDirector.Item_Use_Train_Heal_HP(5);
                    break;
                case 10:
                    gameDirector.Item_Use_Train_Heal_HP(10);
                    break;
                case 11:
                    gameDirector.Item_Use_Train_Heal_HP(15);
                    break;
                case 12:
                    gameDirector.Item_Use_Train_Heal_HP(20);
                    break;
                case 13:
                    StartCoroutine(gameDirector.Item_Train_SpeedUp(10f));
                    break;
                case 14:
                    StartCoroutine(gameDirector.Item_Train_SpeedUp(15f));
                    break;
                case 15:
                    StartCoroutine(player.Item_Player_SpeedUP(1.5f, 10));
                    break;
                case 16:
                    StartCoroutine(player.Item_Player_SpeedUP(2.5f, 10));
                    break;
                case 17:
                    StartCoroutine(player.Item_Player_SpeedUP(1f, 10));
                    StartCoroutine(player.Item_Player_Heal_HP_Auto(1f, 10));
                    break;
                case 18:
                    StartCoroutine(player.Item_Player_SpeedUP(2f, 10));
                    StartCoroutine(player.Item_Player_Heal_HP_Auto(5f, 10));
                    break;
                case 19:
                    mercenaryDirector.Item_Use_Fatigue_Reliever(1, 10, 30);
                    break;
                case 20:
                    StartCoroutine(player.Item_Player_AtkUp_Persent(3, 20));
                    break;
                case 21:
                    StartCoroutine(player.Item_Player_AtkUp_Persent(7, 20));
                    break;
                case 22:
                    StartCoroutine(player.Item_Player_Debuff_Immunity(10));
                    break;
                case 23:
                    player.Item_Player_Ballon_Bullet();
                    break;
                case 24:
                    player.Item_Player_Ballon_Bullet(); // 업그레이드 예정
                    break;
                case 25:
                    gameDirector.Item_Use_Train_Turret_All_SpeedUP(15, 20);
                    break;
                case 26:
                    break;
                case 27:
                    StartCoroutine(player.Item_Player_DoubleAtkUP(20));
                    break;
                case 28:
                    break;


            }
        }
        uiDirector.ItemCoolTime_Instantiate(itemList.Item[num]);
    }

    public void Get_SupplyItem(int num)
    {
        bool coolTime_Flag = true;
        uiDirector.View_ItemList(itemList.Item[num].Item_Sprite);
        itemDirector.Get_Supply_Item_Information(itemList.Item[num]);
        switch (num)
        {
            case 20:
                StartCoroutine(player.Item_Change_Bullet("Blood_Bullet", 10));
                break;
            case 21:
                StartCoroutine(player.Item_Change_Bullet("Bouncing_Bullet", 10));
                break;
            case 22:
                StartCoroutine(player.Item_Change_Bullet("Fire_Bullet", 10));
                break;
            case 23:
                StartCoroutine(player.Item_Player_Dagger(0.3f));
                break;
            case 24:
                StartCoroutine(monsterDirector.Item_Monster_FearFlag(2, 20));
                player.Item_Instantiate_Flag(0, 20);
                break;
            case 25:
                StartCoroutine(monsterDirector.Item_Monster_GreedFlag(2, 20));
                player.Item_Instantiate_Flag(1, 20);
                break;
            case 26:
                StartCoroutine(monsterDirector.Item_Use_Monster_CureseFlag(10, 20));
                player.Item_Instantiate_Flag(2, 20);
                break;
            case 27:
                StartCoroutine(monsterDirector.Item_Use_Monster_GiantFlag(30, 20));
                player.Item_Instantiate_Flag(3, 20);
                break;
            case 28:
                player.Item_Player_Giant_Tent();
                break;
            case 29:
                player.Item_Player_Spawn_Turret(0);
                break;
            case 30:
                StartCoroutine(player.Item_Player_AtkUP(10, 15));
                StartCoroutine(player.Item_Player_AtkDelayDown(0.3f, 15));
                break;
            case 31:
                StartCoroutine(player.Item_Player_Giant_GunAndBullet(20));
                break;
            case 32:
                player.Item_Player_Spawn_Shield(0);
                break;
            case 33:
                player.Item_Player_Spawn_Shield(1);
                StartCoroutine(player.Item_Player_Heal_HP_Auto(5,10));
                break;
            case 34:
                player.Item_Player_Heal_HP(10);
                break;
            case 35:
                mercenaryDirector.Item_Use_Gloves_Expertise(40, 15);
                break;
            case 36:
                player.Item_Player_Spawn_Dron(0);
                break;
            case 37:
                break;
            case 38:
                mercenaryDirector.Item_Use_Bear(2, 30);
                break;
            case 39:
                gameDirector.Item_Use_Train_Turret_All_SpeedUP(25, 12);
                break;
            case 40:
                player.Item_Player_Spawn_Turret(1);
                break;
            case 41:
                player.Item_Player_Minus_HP(20);
                StartCoroutine(player.Item_Player_SpeedUP(2f, 8));
                StartCoroutine(player.Item_Player_AtkUP(10, 8));
                StartCoroutine(player.Item_Player_AtkDelayDown(0.1f, 8));
                break;
            case 42:
                player.Item_Player_Spawn_Dron(1);
                break;
            case 43:
                player.Item_Gun_Change("Flash_Bang", 3);
                break;
            case 44:
                player.Item_Gun_Change("Gatling_Gun", 5);
                break;
            case 45:
                player.Item_Gun_Change("Missile_Gun", 5);
                break;
            case 46:
                player.Item_Gun_Change("Raser_Gun", 10);
                break;
            case 47:
                player.Item_Gun_Change("Fire_Gun", 10);
                break;
            case 48:
                StartCoroutine(player.Item_Change_Bullet("Stun_Bullet", 15));
                break;
            case 54:
            case 55:
            case 56:
            case 57:
            case 58:
            case 59:
            case 60:
            case 61:
            case 62:
            case 63:
            case 64:
            case 65:
            case 66:
            case 67:
            case 68:
            case 69:
                coolTime_Flag = false;
                if (!uiDirector.GetItemList_Num.Contains(num))
                {
                    uiDirector.GetItemList_Num.Add(num);
                }
                itemList.Item[num].Item_Count_UP();
                break;
            case 70:
            case 71:
            case 72:
            case 73:
            case 74:
            case 75:
            case 76:
            case 77:
            case 78:
            case 79:
                coolTime_Flag = false;
                if (!uiDirector.GetItemList_Num.Contains(num))
                {
                    uiDirector.GetItemList_Num.Add(num);
                }
                missionDirector.MaterialCount();
                itemList.Item[num].Item_Count_UP();
                break;
        }

        if (coolTime_Flag)
        {
            uiDirector.ItemCoolTime_Instantiate(itemList.Item[num]);
        }
    }
}