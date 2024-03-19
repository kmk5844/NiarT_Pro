using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Station_TrainMaintenance : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;

    [Header("UI에서 나타나는 기차")]
    public Transform UI_TrainList;
    public int UI_Train_Num;
    public TextMeshProUGUI UI_Train_Information;

    [Header("패시브 업그레이드 윈도우")]
    public TextMeshProUGUI Passive_Text_0;
    public Button Passive_Button_0;
    public TextMeshProUGUI Passive_Text_1;   
    public Button Passive_Button_1;
    public TextMeshProUGUI Passive_Text_2;
    public Button Passive_Button_2;
    public TextMeshProUGUI Passive_Text_3;
    public Button Passive_Button_3;

    [Header("기차 변경 윈도우")]
    [SerializeField]
    List<int> Train_Change_Num;
    public Transform Train_Change_Content;
    public GameObject Train_Card;
    public TextMeshProUGUI Train_Card_Information;
    int Engine_Tier_Max_Train;
    [SerializeField]
    List<Toggle> Train_Toggle;
    int Toggle_Train_Num;
    string Toggle_Train_Name;


    private void Start()
    {
        UI_Train_Num = 0;
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        Engine_Tier_Max_Train = trainData.Max_Train_MaxTrain;
        Train_Change_Num = trainData.Train_Change_Num;
        //UI 기차 생성하기
        UI_TrainImage();
        UI_Now_Train_Information();
        //패시브 업그레이드
        Passive_Text(true);
        //기차 변경하기
        Check_Init_TrainCard();
        Train_ToggleStart();
    }
    //UI 기차 생성하기
    private void UI_TrainImage()
    {
        foreach(int trainNum in trainData.Train_Num)
        {
            GameObject train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + trainNum), UI_TrainList);
            train.name = trainData.EX_Game_Data.Information_Train[trainNum].Train_Name;
            if(trainNum != 0)
            {
                train.SetActive(false);
            }
        }
    }
    public void Button_Train_LR(bool flag)
    {
        int beforeNum = UI_Train_Num;
        if (flag) //left
        {
            if (UI_Train_Num + 1 > UI_TrainList.childCount - 1)
            {
                UI_Train_Num = UI_TrainList.childCount - 1;
            }
            else
            {
                UI_Train_Num++;
            }
        }
        else //right
        {
            if (UI_Train_Num - 1 < 0)
            {
                UI_Train_Num = 0;
            }
            else
            {
                UI_Train_Num--;
            }
        }

        UI_TrainList.GetChild(beforeNum).gameObject.SetActive(false);
        UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(true);
        UI_Now_Train_Information();
    }

    private void UI_Now_Train_Information()
    {
        UI_Train_Information.text =
            trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Train_Information;
    }


    //패시브 업그레이드
    private void Passive_Text(bool All, int num = 0)
    {
        if (All)
        {
            if (trainData.Level_Train_EngineTier == trainData.Max_Train_EngineTier)
            {
                Passive_Text_0.text = "Lv.MAX\n0G";
                Passive_Button_0.interactable = false;
            }
            else
            {
                Passive_Text_0.text = "Lv." + trainData.Level_Train_EngineTier + "\n" + trainData.Cost_Train_EngineTier + "G";
            }
            if (trainData.Level_Train_MaxSpeed == trainData.Max_Train_MaxSpeed)
            {
                Passive_Text_1.text = "Lv.MAX\n0G";
                Passive_Button_1.interactable = false;
            }
            else
            {
                Passive_Text_1.text = "Lv." + trainData.Level_Train_MaxSpeed + "\n" + trainData.Cost_Train_MaxSpeed + "G";
            }
            if (trainData.Level_Train_Armor == trainData.Max_Train_Armor)
            {
                Passive_Text_2.text = "Lv.MAX\n0G";
                Passive_Button_2.interactable = false;
            }
            else
            {
                Passive_Text_2.text = "Lv." + trainData.Level_Train_Armor + "\n" + trainData.Cost_Train_Armor + "G";
            }
            if (trainData.Level_Train_Efficient == trainData.Max_Train_Efficient)
            {
                Passive_Text_3.text = "Lv.MAX\n0G";
                Passive_Button_3.interactable = false;
            }
            else
            {
                Passive_Text_3.text = "Lv." + trainData.Level_Train_Efficient + "\n" + trainData.Cost_Train_Efficient + "G";
            }
        }
        else
        {
            if (num == 0)
            {
                if (trainData.Level_Train_EngineTier == trainData.Max_Train_EngineTier)
                {
                    Passive_Text_0.text = "Lv.MAX\n0G";
                    Passive_Button_0.interactable = false;
                }
                else
                {
                    Passive_Text_0.text = "Lv." + trainData.Level_Train_EngineTier + "\n" + trainData.Cost_Train_EngineTier + "G";
                }
            }
            else if (num == 1)
            {
                if (trainData.Level_Train_MaxSpeed == trainData.Max_Train_MaxSpeed)
                {
                    Passive_Text_1.text = "Lv.MAX\n0G";
                    Passive_Button_1.interactable = false;
                }
                else
                {
                    Passive_Text_1.text = "Lv." + trainData.Level_Train_MaxSpeed + "\n" + trainData.Cost_Train_MaxSpeed + "G";
                }
            }
            else if (num == 2)
            {
                if (trainData.Level_Train_Armor == trainData.Max_Train_Armor)
                {
                    Passive_Text_2.text = "Lv.MAX\n0G";
                    Passive_Button_2.interactable = false;
                }
                else
                {
                    Passive_Text_2.text = "Lv." + trainData.Level_Train_Armor + "\n" + trainData.Cost_Train_Armor + "G";
                }
            }
            else if (num == 3)
            {
                if (trainData.Level_Train_Efficient == trainData.Max_Train_Efficient)
                {
                    Passive_Text_3.text = "Lv.MAX\n0G";
                    Passive_Button_3.interactable = false;
                }
                else
                {
                    Passive_Text_3.text = "Lv." + trainData.Level_Train_Efficient + "\n" + trainData.Cost_Train_Efficient + "G";
                }
            }
        }
    }

    public void Click_Passive_Upgrade(int i)//LevelNum : 0 = Tier / 1 = Speed / 2 = Armor / 3 = Efficient
    {
        trainData.Passive_Level_Up(i);
        Passive_Text(false, i);
    }

    //기차 변경하기
    private void Train_ToggleStart()
    {
        foreach(Toggle toggle in Train_Toggle)
        {
            toggle.onValueChanged.AddListener(Train_OnToggleValueChange);
        }
    }

    private void Train_OnToggleValueChange(bool isOn)
    {
        for(int i = 0; i < Train_Toggle.Count; i++)
        {
            if (Train_Toggle[i].isOn)
            {
                Train_Change_Information_Text(i);
            }
        }
    }

    private void Train_Change_Information_Text(int toggle_num)
    {
        Store_Train_Card Card = Train_Change_Content.GetChild(toggle_num).GetComponent<Store_Train_Card>();
        Toggle_Train_Num = Card.Train_Num;
        Toggle_Train_Name = trainData.EX_Game_Data.Information_Train[Toggle_Train_Num].Train_Name;

        Train_Card_Information.text = Toggle_Train_Name;
    }

    private void Check_Init_TrainCard()
    {
        RectTransform ContentSize = Train_Change_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(300 * Train_Change_Num.Count, ContentSize.sizeDelta.y);
        foreach(int num in Train_Change_Num)
        {
            Train_Card.GetComponent<Store_Train_Card>().Train_Num = num;
            GameObject Card = Instantiate(Train_Card, Train_Change_Content);
            Card.name = num.ToString();
            Train_Toggle.Add(Card.GetComponentInChildren<Toggle>());
        }
    }

    public void Button_Train_Change()
    {
        trainData.SA_TrainData.Train_Num[UI_Train_Num] = Toggle_Train_Num;
        Destroy(UI_TrainList.GetChild(UI_Train_Num).gameObject);
        GameObject changeTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + Toggle_Train_Num), UI_TrainList);
        changeTrain.name = Toggle_Train_Name;
        changeTrain.transform.SetSiblingIndex(UI_Train_Num);
        UI_Now_Train_Information();
        GetComponentInParent<StationDirector>().Change_Train_List(Toggle_Train_Num, UI_Train_Num);
    }

    public void Button_Train_Add()
    {
        trainData.SA_TrainData.Train_Num.Add(1); //empty Trian
        GameObject EmptyTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/1"), UI_TrainList);
        EmptyTrain.name = trainData.EX_Game_Data.Information_Train[1].Train_Name;
        EmptyTrain.SetActive(false);
        GetComponentInParent<StationDirector>().Add_Train_List();
    }
}