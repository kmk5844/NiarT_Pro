using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Ready_MercenaryList : MonoBehaviour
{
    [HideInInspector]
    public PlayerReadyDirector Director;
    [HideInInspector]
    public Station_PlayerData playerData;
    [HideInInspector]
    public Station_MercenaryData mercenaryData;

    public int Mercenary_Num;
    public Image Mercenary_Image;
    int mercenary_pride_Buy;
    int mercenary_pride_Upgrade;
    public Button[] MercenaryButtonList;
    TextMeshProUGUI[] MercenaryButtonList_Text;

    public bool BuyFlag;

    private void Start()
    {
        Mercenary_Image.sprite = Resources.Load<Sprite>("Sprite/Mercenary/" + Mercenary_Num);
        mercenary_pride_Buy = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Pride;

        MercenaryButtonList_Text = new TextMeshProUGUI[MercenaryButtonList.Length];

        for (int i = 0; i < MercenaryButtonList.Length; i++)
        {
            MercenaryButtonList_Text[i] = MercenaryButtonList[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        mercenary_pride_Upgrade = mercenaryData.Check_Cost_Mercenary(Mercenary_Num);
        Mercenary_Button_Check();
    }

    public void ChangeState(int num)
    {
        switch (num) {
            case 0:
                MercenaryButtonList[0].gameObject.SetActive(true);
                MercenaryButtonList[1].gameObject.SetActive(false);
                MercenaryButtonList[2].gameObject.SetActive(false);
                break;
            case 1:
                MercenaryButtonList[0].gameObject.SetActive(false);
                MercenaryButtonList[1].gameObject.SetActive(true);
                MercenaryButtonList[2].gameObject.SetActive(false);
                break;
            case 2:
                MercenaryButtonList[0].gameObject.SetActive(false);
                MercenaryButtonList[1].gameObject.SetActive(false);
                MercenaryButtonList[2].gameObject.SetActive(true);
                break;
        }
    }

    public void Mercenary_Buy()
    {
        if(playerData.Player_Coin >= mercenary_pride_Buy)
        {
            playerData.Player_Buy_Coin(mercenary_pride_Buy);
            mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Add(Mercenary_Num);
            Director.check_PlayerCoin();
            BuyFlag = true;
            Mercenary_Button_Check();
        }
        else
        {
            Debug.Log("内牢 何练");
        }
    }

    public void Mercenary_Upgrade()
    {
        if(playerData.Player_Coin >= mercenary_pride_Upgrade)
        {
            playerData.Player_Buy_Coin(mercenary_pride_Upgrade);
            mercenaryData.Mercenary_Level_Up(Mercenary_Num);
            Director.check_PlayerCoin();
            if(mercenaryData.Level_Mercenary[Mercenary_Num] != mercenaryData.Max_Mercenary[Mercenary_Num])
            {
                mercenary_pride_Upgrade = mercenaryData.Check_Cost_Mercenary(Mercenary_Num);
                MercenaryButtonList_Text[1].text = mercenary_pride_Upgrade + "G";
            }
            else
            {
                MercenaryButtonList[1].interactable = false;
                MercenaryButtonList_Text[1].text = "MAX";
            }
        }
        else
        {
            Debug.Log("内牢 何练");
        }
    }

    public void Mercenary_Button_Check()
    {
        if (!BuyFlag)
        {
            MercenaryButtonList[0].interactable = true;
            MercenaryButtonList_Text[0].text = mercenary_pride_Buy + "G";
            MercenaryButtonList[1].interactable = false;
            MercenaryButtonList_Text[1].text = "Not Owned";
            MercenaryButtonList[2].interactable = false;
            MercenaryButtonList_Text[2].text = "X";
        }
        else
        {
            MercenaryButtonList[0].interactable = false;
            MercenaryButtonList_Text[0].text = "In Inventory";
            if (mercenaryData.Level_Mercenary[Mercenary_Num] + 1 != mercenaryData.Max_Mercenary[Mercenary_Num] + 1)
            {
                MercenaryButtonList[1].interactable = true;
                mercenary_pride_Upgrade = mercenaryData.Check_Cost_Mercenary(Mercenary_Num);
                MercenaryButtonList_Text[1].text = mercenary_pride_Upgrade + "G";
            }
            else
            {
                MercenaryButtonList[1].interactable = false;
                MercenaryButtonList_Text[1].text = "MAX";
            }
            MercenaryButtonList[2].interactable = false;
            MercenaryButtonList_Text[2].text = "X";
        }
    }
}