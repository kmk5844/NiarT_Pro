using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    //public SA_ItemList itemList;
    [Header("영향받는 스크립트")]
    public Player player;
    public GameDirector gameDirector;
    public MercenaryDirector mercenaryDirector;
    public MonsterDirector monsterDirector;

    private void Start()
    {
    }

    public void UseEquipItem(int num)
    {
        switch (num)
        {
            case 0:
                //용병 랜덤 부활
                break;
            case 1:
                player.Item_Player_Heal_HP(10);
                break;
            case 2:
                player.Item_Player_Heal_HP(20);
                break;
            case 3:
                gameDirector.Item_Fuel_Charge(10);
                break;
            case 4:
                gameDirector.Item_Fuel_Charge(20);
                break;
            case 5:
                gameDirector.Item_Use_Heal_TrainHP(10);
                break;
            case 6:
                gameDirector.Item_Use_Heal_TrainHP(20);
                break;
            case 7:
                StartCoroutine(gameDirector.Item_Train_SpeedUp(15));
                break;
            case 8:
                StartCoroutine(player.Item_Player_SpeedUP(1.5f, 20));
                break;
            case 9:
                StartCoroutine(player.Item_Player_SpeedUP(0.5f, 10));
                StartCoroutine(player.Item_Player_Heal_HP_Auto(1f, 10));
                break;
            case 10:
                //mercenaryDirector.Item_Use_Fatigue_Reliever(1, 10);
                break;
            case 11:
                StartCoroutine(player.Item_Player_AtkUP(5, 15));
                break;
            case 12:
                //플레이어 상태이상 제거;
                break;
            case 13:
                player.Item_Player_Ballon_Bullet();
                break;
            case 14:
                StartCoroutine(gameDirector.Item_Coin_Double(30));
                break;
            case 15:
                break;
            case 16:
                break;
            case 17:
                StartCoroutine(player.Item_Player_DoubleAtkUP(6));
                break;
            case 18:
                break;
            case 19:
                break;
            case 20:
                break;
        }
    }

    public void Get_SupplyItem(int num)
    {
        switch (num)
        {
            case 20:
                break;
            case 21:
                break;
            case 22:
                break;
            case 23:
                break;
            case 24:
                StartCoroutine(monsterDirector.Item_Monster_FearFlag(5, 30));
                break;
            case 25:
                StartCoroutine(monsterDirector.Item_Monster_GreedFlag(5, 30));
                break;
            case 26:
                break;
            case 27:
                break;
            case 28:
                player.Item_Player_Giant_Scarecrow();
                break;
            case 29:
                break;
            case 30:
                break;
            case 31:
                break;
            case 32:
                break;
            case 33:
                break;
            case 34:
                break;
            case 35:
                break;
            case 36:
                break;
            case 37:
                break;
            case 38:
                break;
            case 39:
                break;
            case 40:
                break;
            case 41:
                break;
            case 42:
                break;
            case 43:
                break;
            case 44:
                break;
            case 45:
                break;
            case 46:
                break;
            case 47:
                break;
            case 48:
                break;
            case 54:
                break;
            case 55:
                break;
            case 56:
                break;
        }
    }
}