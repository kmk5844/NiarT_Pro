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
    public SkillDirector skillDirector;

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
                    player.Item_Player_Ballon_Bullet(0);
                    break;
                case 14:
                    StartCoroutine(gameDirector.Item_Coin_Plus(10));
                    break;
                case 15:
                    gameDirector.Item_Use_Train_Turret_All_SpeedUP(15, 15);
                    break;
                case 16:
                    mercenaryDirector.Item_Use_Snack();
                    break;
                case 17:
                    StartCoroutine(player.Item_Player_DoubleAtkUP(5));
                    break;
                case 18:
                    player.Item_Player_Spawn_Claymore();
                    break;
                case 19:
                    StartCoroutine(player.Item_Player_Spawn_WireEntanglement(20));
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
                    player.Item_Player_Ballon_Bullet(0);
                    break;
                case 24:
                    player.Item_Player_Ballon_Bullet(1);
                    break;
                case 25:
                    gameDirector.Item_Use_Train_Turret_All_SpeedUP(15, 20);
                    break;
                case 26:
                    mercenaryDirector.Item_Use_Snack();
                    break;
                case 27:
                    StartCoroutine(player.Item_Player_DoubleAtkUP(20));
                    break;
                case 28:
                    player.Item_SmallPortion(10);
                    break;
            }
            uiDirector.ItemCoolTime_Instantiate(itemList.Item[num]);
        }
    }

    public void Get_SupplyItem(int num)
    {
        bool coolTime_Flag = true;
        if (!On_ItemList_Test)
        {
            uiDirector.View_ItemList(itemList.Item[num].Item_Sprite);
            itemDirector.Get_Supply_Item_Information(itemList.Item[num]);
        }


        if (!On_ItemList_Test)
        {
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
                    player.Item_Player_Dagger(0, 0);
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
                    player.Item_Player_Giant_Tent(0);
                    break;
                case 29:
                    player.Item_Player_Spawn_Turret(0,20, 20, 0.15f);
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
                    StartCoroutine(player.Item_Player_Heal_HP_Auto(5, 10));
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
                    player.Item_Player_Spawn_RobotPlant();
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
                    player.Item_Gun_Change("Laser_Gun", 10);
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
                    player.Item_Player_Ballon_Bullet(0);
                    break;
                case 24:
                    player.Item_Player_Ballon_Bullet(1);
                    break;
                case 25:
                    gameDirector.Item_Use_Train_Turret_All_SpeedUP(15, 20);
                    break;
                case 26:
                    mercenaryDirector.Item_Use_Snack();
                    break;
                case 27:
                    StartCoroutine(player.Item_Player_DoubleAtkUP(20));
                    break;
                case 28:
                    player.Item_SmallPortion(10);
                    break;
                case 29:
                    StartCoroutine(gameDirector.Item_Coin_Plus(10));
                    break;
                case 30:
                    player.Item_Player_Spawn_Claymore();
                    break;
                case 31:
                    StartCoroutine(player.Item_Player_Spawn_WireEntanglement(20));
                    break;
                case 32:
                    player.Item_Player_Spawn_Turret(0, 10, 20, 0.15f);
                    break;
                case 33:
                    player.Item_Player_Spawn_Turret(0, 30, 40, 0.15f);
                    break;
                case 34:
                    player.Item_Player_Giant_Tent(0);
                    break;
                case 35:
                    player.Item_Player_Giant_Tent(1);
                    break;
                case 36:
                    player.Item_Player_Giant_Tent(2);
                    break;
                case 37:
                    player.Item_Player_Giant_Tent(3);
                    break;
                case 38:
                    player.Item_Player_Spawn_Turret(1, 30, 0, 0);
                    break;
                case 39:
                    player.Item_Player_Spawn_Turret(2, 30, 50, 0);
                    break;
                case 40:
                    player.Item_Player_Spawn_Turret(3, 10, 0, 0.1f);
                    break;
                case 41:
                    player.Item_Player_Spawn_Turret(3, 10, 0, 0.1f);
                    player.Item_Player_Spawn_Turret(3, 10, 0, 0.1f);
                    break;
                case 42:
                    player.Item_Player_Spawn_Turret(4, 10, 50, 3f);
                    break;
                case 43:
                    player.Item_Player_Spawn_Turret(4, 30, 60, 2f);
                    break;
                case 44:
                    player.Item_Player_Spawn_Turret(5, 0, 20, 0);
                    break;  
                case 45:
                    player.Item_Player_Spawn_Turret(5, 1, 20, 0);
                    break;
                case 46:
                    player.Item_Player_Spawn_Turret(6, 5, 20, 0);
                    break;
                case 47:
                    // 2단 점프 개발 후 진행 예정
                    StartCoroutine(player.Item_Player_SpringShoose(10));
                    break;
                case 48:
                    player.Item_Player_Spawn_SonicDevice(10);
                    break;
                case 49:
                    player.Item_Player_Spawn_SonicDevice(20);
                    break;
                case 50:
                    monsterDirector.Item_Player_Spawn_Scarecrow(0);
                    break;
                case 51:
                    monsterDirector.Item_Player_Spawn_Scarecrow(1);
                    break;
                case 52:
                    player.Item_Player_Dagger(0, 0);
                    break;
                case 53:
                    player.Item_Player_Dagger(0, 1);
                    break;
                case 54:
                    player.Item_Player_Dagger(0, 2);
                    break;
                case 55:
                    player.Item_Player_Dagger(0, 3);
                    break;
                case 56:
                    player.Item_Player_Dagger(1, 0);
                    break;
                case 57:
                    player.Item_Player_Dagger(1, 1);
                    break;
                case 58:
                    player.Item_Player_Dagger(1, 2);
                    break;
                case 59:
                    player.Item_Player_Dagger(1, 3);
                    break;
                case 60:
                    StartCoroutine(monsterDirector.Item_Monster_FearFlag(2, 20));
                    player.Item_Instantiate_Flag(0, 20);
                    break;
                case 61:
                    StartCoroutine(monsterDirector.Item_Monster_GreedFlag(2, 20));
                    player.Item_Instantiate_Flag(1, 20);
                    break;
                case 62:
                    StartCoroutine(monsterDirector.Item_Use_Monster_CureseFlag(10, 20));
                    player.Item_Instantiate_Flag(2, 20);
                    break;
                case 63:
                    StartCoroutine(monsterDirector.Item_Use_Monster_GiantFlag(30, 20));
                    player.Item_Instantiate_Flag(3, 20);
                    break;
                case 64:
                    player.Item_Player_Spawn_Random_Turret(3);
                    break;
                case 65:
                    StartCoroutine(player.Item_Player_AtkUP(10, 15));
                    StartCoroutine(player.Item_Player_AtkDelayDown(0.3f, 15));
                    break;
                case 66:
                    player.Item_Player_Spawn_Shield(0);
                    break;
                case 67:
                    player.Item_Player_Spawn_Shield(1);
                    break;
                case 68:
                    player.Item_Player_Spawn_Shield(2);
                    break;
                case 69:
                    player.Item_Player_Heal_HP(10);
                    break;
                case 70:
                    gameDirector.Item_Fuel_Charge(10);
                    break;
                case 71:
                    gameDirector.Item_Use_Train_Heal_HP(10);
                    break;
                case 72:
                    skillDirector.Item_Skill_CoolTime_Set(10);
                    break;
                case 73:
                    gameDirector.Item_Use_Map(1);
                    break;
                case 74:
                    StartCoroutine(player.Item_Player_Giant_GunAndBullet(20));
                    break;
                case 75:
                    gameDirector.Item_Use_Train_Turret_All_SpeedUP(10, 12);
                    break;
                case 76:
                    player.Item_Player_Heal_HP(20);
                    player.Item_Drink_Bear();
                    break;
                case 77:
                    player.Item_Player_Spawn_RobotPlant();
                    break;
                case 78:
                    player.Item_Player_Minus_HP(30);
                    StartCoroutine(player.Item_Player_SpeedUP(3f, 20));
                    StartCoroutine(player.Item_Player_AtkUP(10, 20));
                    StartCoroutine(player.Item_Player_AtkDelayDown(0.1f, 20));
                    break;
                case 79:
                    player.Item_Player_Spawn_Dron(0);
                    break;
                case 80:
                    player.Item_Player_Spawn_Dron(1);
                    break;
                case 81:
                    player.Item_Player_Spawn_Dron(2);
                    break;
                case 82:
                    player.Item_Player_Spawn_Dron(3);
                    break;
                case 83:
                    player.Item_Player_Spawn_Dron(4);
                    break;
                case 84:
                    player.Item_Player_Spawn_Dron(5);
                    break;
                case 85:
                    player.Item_Player_Spawn_Dron(6);
                    break;
                case 86:
                    player.Item_Player_Spawn_Dron(7);
                    break;
                case 87:
                    //보급 작업
                    break;
                case 88:
                    gameDirector.Item_Spawn_Train_BulletproofPlate(500);
                    break;
                case 89:
                    gameDirector.Item_Spawn_Train_BulletproofPlate(1000);
                    break;
                case 90:
                    gameDirector.Item_Spawn_Train_BulletproofPlate(1500);
                    break;
                case 91:
                    player.Item_Spawn_MusicBox(30);
                    break;
                case 92:
                    gameDirector.Item_Use_Lucky_Coin();
                    break;
                case 93:
                    gameDirector.Item_Use_Lucky_Coin();
                    break;
                case 94:
                    player.Item_Player_Heal_HP(15);
                    break;
                case 95:
                    player.Item_Player_Heal_HP(30);
                    break;
                case 96:
                    StartCoroutine(player.Item_Player_ArmorUP(10, 10));
                    break;
                case 97:
                    StartCoroutine(player.Item_Player_ArmorUP(20, 10));
                    break;
                case 98:
                    StartCoroutine(player.Item_Player_ArmorUP(10, 20));
                    break;
                case 99:
                    gameDirector.Item_Train_Armor_Up(20, 10);
                    break;
                case 100:
                    gameDirector.Item_Train_Armor_Up(30, 10);
                    break;
                case 101:
                    gameDirector.Item_Train_Armor_Up(20, 20);
                    break;
                case 102:
                    StartCoroutine(player.Item_Change_Bullet("Stun_Bullet", 15));
                    break;
                case 103:
                    StartCoroutine(player.Item_Change_Bullet("Blood_Bullet", 10));
                    break;
                case 104:
                    StartCoroutine(player.Item_Change_Bullet("Bouncing_Bullet", 10));
                    break;
                case 105:
                    StartCoroutine(player.Item_Change_Bullet("Fire_Bullet", 10));
                    break;
                case 106:
                    player.Item_Gun_Change("Flash_Bang", 3);
                    break;
                case 107:
                    player.Item_Gun_Change("Grenade", 2);
                    break;
                case 108:
                    player.Item_Gun_Change("Signal_Flare", 1);
                    break;
                case 109:
                    player.Item_Gun_Change("Gatling_Gun", 5);
                    break;
                case 110:
                    player.Item_Gun_Change("Missile_Gun", 5);
                    break;
                case 111:
                    player.Item_Gun_Change("Laser_Gun", 10);
                    break;
                case 112:
                    player.Item_Gun_Change("Fire_Gun", 10);
                    break;
                case 113:
                    player.Item_Gun_Change("Grenade_Launcher", 5);
                    break;
                case 114:
                    player.Item_Gun_Change("ShotGun", 10);
                    break;
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 120:
                case 121:
                case 122:
                case 123:
                case 124:
                case 125:
                case 126:
                case 127:
                case 128:
                case 129:
                case 130:
                case 131:
                case 132:
                case 133:
                    coolTime_Flag = false;
                    if (!uiDirector.GetItemList_Num.Contains(num))
                    {
                        uiDirector.GetItemList_Num.Add(num);
                    }
                    itemList.Item[num].Item_Count_UP();
                    break;
                case 134:
                case 135:
                case 136:
                case 137:
                case 138:
                case 139:
                case 140:
                case 141:
                case 142:
                case 143:
                case 145:
                case 146:
                case 147:
                case 148:
                case 149:
                    coolTime_Flag = false;
                    if (!uiDirector.GetItemList_Num.Contains(num))
                    {
                        uiDirector.GetItemList_Num.Add(num);
                    }
                    missionDirector.MaterialCount();
                    itemList.Item[num].Item_Count_UP();
                    break;
            }
        }

        if (coolTime_Flag && !On_ItemList_Test)
        {
            uiDirector.ItemCoolTime_Instantiate(itemList.Item[num]);
        }
    }
}