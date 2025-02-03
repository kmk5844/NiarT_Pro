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
    SA_MercenaryData SaMercenaryData;

    public int Mercenary_Num;
    public Image Mercenary_Image;
    public TextMeshProUGUI Mercenary_Level_Text;
    public Image Merceanary_TypeImage;
    public Sprite[] P_A_Sprite;

    string Mercenary_Name;
    string Mercenary_Information;

    int mercenary_pride_Buy;
    int mercenary_pride_Upgrade;
    public Button[] MercenaryButtonList;
    TextMeshProUGUI[] MercenaryButtonList_Text;
    Sprite mer_sprite;

    bool passiveFlag;
    bool passiveFlag_Ride;
    public bool BuyFlag;

    //드래그 드랍 전용
    public GameObject MercenaryDragObject_List;
    float mouseHoldTime;
    bool MouseHold;
    bool mouseDrag;

    //정보 전용
    public GameObject Information_Tooltip;
    Ready_Mercenary_Information tooltip;
    public bool informationFlag;

    private void Start()
    {
        mer_sprite = Resources.Load<Sprite>("Sprite/Mercenary/" + Mercenary_Num);
        Mercenary_Image.sprite = mer_sprite;
        mercenary_pride_Buy = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Pride;
        MercenaryButtonList_Text = new TextMeshProUGUI[MercenaryButtonList.Length];
        SaMercenaryData = mercenaryData.SA_MercenaryData;

        passiveFlag = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Passive;
        if (passiveFlag)
        {
            Merceanary_TypeImage.sprite = P_A_Sprite[0];
        }
        else
        {
            Merceanary_TypeImage.sprite = P_A_Sprite[1];
        }


        for (int i = 0; i < MercenaryButtonList.Length; i++)
        {
            MercenaryButtonList_Text[i] = MercenaryButtonList[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        mercenary_pride_Upgrade = mercenaryData.Check_Cost_Mercenary(Mercenary_Num);
        Mercenary_Button_Check();
        Check_PassiveFlag();
        ChangeLevel();


        tooltip = Information_Tooltip.GetComponent<Ready_Mercenary_Information>();
        Mercenary_Name = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name;
        Mercenary_Information = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Information;
    }

    private void Update()
    {
        if (MouseHold)
        {
            mouseHoldTime += Time.deltaTime;
        }

        if (!mouseDrag)
        {
            if (mouseHoldTime > 0.1f)
            {
                mouseDrag = true;
                MercenaryDragObject_List.GetComponent<Image>().sprite = mer_sprite;
                MercenaryDragObject_List.SetActive(true);
            }
        }
        else
        {
            MercenaryDragObject_List.transform.position = Input.mousePosition;
        }
    }

    public void Check_PassiveFlag()
    {
        if (passiveFlag)
        {
            foreach(int i in SaMercenaryData.Mercenary_Num)
            {
                if(i == Mercenary_Num)
                {
                    passiveFlag_Ride = true;
                    break;
                }
                else
                {
                    passiveFlag_Ride = false;
                }
            }
        }
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
            Director.Check_PlayerCoin();
            BuyFlag = true;
            Mercenary_Button_Check();
        }
        else
        {
            Director.Open_Mercenary_GoldBen_Window();
        }
    }

    public void Mercenary_Upgrade()
    {
        if(playerData.Player_Coin >= mercenary_pride_Upgrade)
        {
            playerData.Player_Buy_Coin(mercenary_pride_Upgrade);
            mercenaryData.Mercenary_Level_Up(Mercenary_Num);
            Director.Check_PlayerCoin();
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
            ChangeLevel();
        }
        else
        {
            Director.Open_Mercenary_GoldBen_Window();
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

    public void ChangeLevel()
    {
        switch (Mercenary_Num) {
            case 0:
                Mercenary_Level_Text.text = "LV." + SaMercenaryData.Level_Engine_Driver + 1;
                break;
            case 1:
                Mercenary_Level_Text.text = "LV." + SaMercenaryData.Level_Engineer + 1;
                break;
            case 2:
                Mercenary_Level_Text.text = "LV." + SaMercenaryData.Level_Long_Ranged + 1;
                break;
            case 3:
                Mercenary_Level_Text.text = "LV." + SaMercenaryData.Level_Short_Ranged + 1;
                break;
            case 4:
                Mercenary_Level_Text.text = "LV." + SaMercenaryData.Level_Medic + 1;
                break;
            case 5:
                Mercenary_Level_Text.text = "LV." + SaMercenaryData.Level_Bard + 1;
                break;
            case 6:
                Mercenary_Level_Text.text = "LV." + SaMercenaryData.Level_CowBoy + 1;
                break;
        }
    }

    public void OnMouseEnter()
    {
        if (!Director.HoldAndDragFlag_Mercenary && !informationFlag)
        {
            informationFlag = true;
            tooltip.Tooltip_On(Mercenary_Name, Mercenary_Information);
        }
    }

    public void OnMouseExit()
    {
        if(!Director.HoldAndDragFlag_Mercenary && informationFlag)
        {
            informationFlag = false;
            tooltip.Tooltip_off();
        }
    }


    public void OnMouseDown()
    {
        if (informationFlag)
        {
            informationFlag = false;
            tooltip.Tooltip_off();
        }

        if (BuyFlag && !passiveFlag_Ride)
        {
            MouseHold = true;
            Director.MercenaryLIst_Mercenary_Num = Mercenary_Num;
        }
    }

    public void OnMouseUp()
    {
        mouseHoldTime = 0f;
        MercenaryDragObject_List.SetActive(false);
        Director.MercenaryChange();
        Check_PassiveFlag();
        if (informationFlag)
        {
            informationFlag = false;
            tooltip.Tooltip_off();
        }
        MouseHold = false;
        mouseDrag = false;
    }
}