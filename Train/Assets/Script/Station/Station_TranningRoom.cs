using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Station_TranningRoom : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;

    [Header("플레이어 업그레이드 윈도우")]
    public TextMeshProUGUI PlayerUP_Text_0;
    public Button PlayerUP_Button_0;
    public TextMeshProUGUI PlayerUP_Text_1;
    public Button PlayerUP_Button_1;
    public TextMeshProUGUI PlayerUP_Text_2;
    public Button PlayerUP_Button_2;
    public TextMeshProUGUI PlayerUP_Text_3;
    public Button PlayerUP_Button_3;
    public TextMeshProUGUI PlayerUP_Text_4;
    public Button PlayerUP_Button_4;

    void Start()
    {
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        Player_Text(true);
    }

    private void Player_Text(bool All, int num = 0)
    {
        if (All)
        {
            if (playerData.Level_Player_Atk == playerData.Max_Player_Atk)
            {
                PlayerUP_Text_0.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_0.interactable = false;
            }
            else
            {
                PlayerUP_Text_0.text = "Lv." + playerData.Level_Player_Atk + "\n" + playerData.Cost_Player_Atk + "Pt";
            }
            if (playerData.Level_Player_AtkDelay == playerData.Max_Player_AtkDelay)
            {
                PlayerUP_Text_1.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_1.interactable = false;
            }
            else
            {
                PlayerUP_Text_1.text = "Lv." + playerData.Level_Player_AtkDelay + "\n" + playerData.Cost_Player_AtkDelay + "Pt";
            }
            if (playerData.Level_Player_HP == playerData.Max_Player_HP)
            {
                PlayerUP_Text_2.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_2.interactable = false;
            }
            else
            {
                PlayerUP_Text_2.text = "Lv." + playerData.Level_Player_HP + "\n" + playerData.Cost_Player_HP + "Pt";
            }
            if (playerData.Level_Player_Armor == playerData.Max_Player_Armor)
            {
                PlayerUP_Text_3.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_3.interactable = false;
            }
            else
            {
                PlayerUP_Text_3.text = "Lv." + playerData.Level_Player_Armor + "\n" + playerData.Cost_Player_Armor + "Pt";
            }
            if (playerData.Level_Player_Speed == playerData.Max_Player_Speed)
            {
                PlayerUP_Text_4.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_4.interactable = false;
            }
            else
            {
                PlayerUP_Text_4.text = "Lv." + playerData.Level_Player_Speed + "\n" + playerData.Cost_Player_Speed + "Pt";
            }
        }
        else
        {
            if(num == 0)
            {
                if (playerData.Level_Player_Atk == playerData.Max_Player_Atk)
                {
                    PlayerUP_Text_0.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_0.interactable = false;
                }
                else
                {
                    PlayerUP_Text_0.text = "Lv." + playerData.Level_Player_Atk + "\n" + playerData.Cost_Player_Atk + "Pt";
                }
            }else if(num == 1)
            {
                if (playerData.Level_Player_AtkDelay == playerData.Max_Player_AtkDelay)
                {
                    PlayerUP_Text_1.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_1.interactable = false;
                }
                else
                {
                    PlayerUP_Text_1.text = "Lv." + playerData.Level_Player_AtkDelay + "\n" + playerData.Cost_Player_AtkDelay + "Pt";
                }
            }
            else if(num == 2)
            {
                if (playerData.Level_Player_HP == playerData.Max_Player_HP)
                {
                    PlayerUP_Text_2.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_2.interactable = false;
                }
                else
                {
                    PlayerUP_Text_2.text = "Lv." + playerData.Level_Player_HP + "\n" + playerData.Cost_Player_HP + "Pt";
                }
            }
            else if(num == 3)
            {
                if (playerData.Level_Player_Armor == playerData.Max_Player_Armor)
                {
                    PlayerUP_Text_3.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_3.interactable = false;
                }
                else
                {
                    PlayerUP_Text_3.text = "Lv." + playerData.Level_Player_Armor + "\n" + playerData.Cost_Player_Armor + "Pt";
                }
            }
            else if(num == 4)
            {
                if (playerData.Level_Player_Speed == playerData.Max_Player_Speed)
                {
                    PlayerUP_Text_4.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_4.interactable = false;
                }
                else
                {
                    PlayerUP_Text_4.text = "Lv." + playerData.Level_Player_Speed + "\n" + playerData.Cost_Player_Speed + "Pt";
                }
            }
        }
    }

    public void Click_Player_Upgrade(int i)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = HP / 3 = Armor / 4 = Speed
    {
        playerData.Player_Level_Up(i);
        Player_Text(false, i);
    }
}
