using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class UIDirector : MonoBehaviour
{
    public GameObject GameDirector_Object;
    GameDirector gamedirector;
    Player player;
    public SA_ItemList itemList;

    [Header("��ü���� UI ������Ʈ")]
    public GameObject Game_UI;
    public GameObject Pause_UI;
    public GameObject Result_UI;
    public GameObject Option_UI;

    [Header("Game UI")]
    public Image Player_HP_Bar;
    public Slider Distance_Bar;
    public Image TotalFuel_Bar;
    public TextMeshProUGUI Speed_Text;
    public TextMeshProUGUI Fuel_Text;
    public TextMeshProUGUI Score_Text;
    public TextMeshProUGUI Coin_Text;
    public Slider Speed_Arrow;

    [Header("Item UI")]
    public GameObject ItemInformation_Object;
    public Image ItemIcon_Image;
    public LocalizeStringEvent ItemName_Text;
    public LocalizeStringEvent ItemInformation_Text;
    bool ItemInformation_Object_Flag;
    float ItemInformation_Object_Time;
    float ItemInformation_Object_TimeDelay;
    public Transform Equiped_Item_List;
    Image[] Equiped_Item_Image;
    TextMeshProUGUI[] Equiped_Item_Count;

    [Header("Item CoolTime UI")]
    public GameObject ItemCoolTime_Object;
    public Transform ItemCoolTime_List;

    [Header("Result UI ���õ� �ؽ�Ʈ �� ������")]
    public TextMeshProUGUI[] Result_Text_List; //0. Stage, 1. Score, 2. Gold, 3. Rank 4. Point
    public Image Result_Image; //win or lose
    public Sprite Result_Win_Image;
    public Sprite Result_Lose_Image;

    public List<int> GetItemList_Num;
    public Transform GetItemList_Transform;
    public GameObject GetItemList_Object;

    bool PauseFlag;
    bool OptionFlag;

    [Header("������������� ��ư �߰�")]
    public Button Retry_Button;
    public Button Station_Button;

    private void Awake()
    {
        Equiped_Item_Image = new Image[Equiped_Item_List.childCount];
        Equiped_Item_Count = new TextMeshProUGUI[Equiped_Item_List.childCount];

        for (int i = 0; i < Equiped_Item_List.childCount; i++)
        {
            Equiped_Item_Image[i] = Equiped_Item_List.GetChild(i).GetComponent<Image>();
            Equiped_Item_Count[i] = Equiped_Item_List.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gamedirector = GameDirector_Object.GetComponent<GameDirector>();

        ItemName_Text.StringReference.TableReference = "ItemData_Table_St";
        ItemInformation_Text.StringReference.TableReference = "ItemData_Table_St";

        PauseFlag = false;
        OptionFlag = false;

        ItemInformation_Object_Flag = false;
        ItemInformation_Object_TimeDelay = 5f;

        //DemoCheck(); // ���߿� ���� ���濹��
    }

    private void Update()
    {
        if(gamedirector.gameType == GameType.Ending || gamedirector.gameType == GameType.GameEnd)
        {

        }
        else
        {
            if (ItemInformation_Object_Flag)
            {
                if (Time.time > ItemInformation_Object_Time + ItemInformation_Object_TimeDelay)
                {
                    StartCoroutine(ItemInformation_Object_Off());

                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!OptionFlag)
                {
                    ON_OFF_Pause_UI(PauseFlag);
                }
                else
                {
                    Click_Option_Exit();
                }
            }
        }

        Player_HP_Bar.fillAmount = player.Check_HpParsent() / 100f;
        TotalFuel_Bar.fillAmount = gamedirector.Check_Fuel();
        Speed_Text.text = (int)gamedirector.TrainSpeed + " Km/h";
        Fuel_Text.text = (int)(gamedirector.Check_Fuel() * 100f) + "%";
        Speed_Arrow.value = gamedirector.TrainSpeed / gamedirector.MaxSpeed;

        Distance_Bar.value = gamedirector.Check_Distance();
    }

    public void Open_Result_UI(bool Win, int StageNum, int Score, int Coin, string Score_Grade,int Point)
    {
        Result_Text_List[0].text = (StageNum + 1).ToString();
        Result_Text_List[1].text = Score.ToString(); // + "��";
        Result_Text_List[2].text = Coin.ToString(); // + "��";
        if (Win)
        {
            Result_Text_List[3].text = Score_Grade;
            Result_Text_List[4].text = Point.ToString();
            Result_Text_List[4].gameObject.SetActive(true);
            Result_Image.sprite = Result_Win_Image;
        }
        else
        {
            Result_Text_List[3].text = "F";
            Result_Text_List[4].gameObject.SetActive(false);
            Result_Image.sprite = Result_Lose_Image;
        }
        for(int i = 0; i < GetItemList_Num.Count; i++)
        {
            Image img = GetItemList_Object.GetComponentInChildren<Image>();
            img.sprite = itemList.Item[GetItemList_Num[i]].Item_Sprite;
            Instantiate(GetItemList_Object, GetItemList_Transform);
        }

        Game_UI.SetActive(false);
        Result_UI.SetActive(true);
    }

    public void ON_OFF_Pause_UI(bool Flag)
    {
        if (Flag)
        {
            PauseFlag = false;
            Pause_UI.SetActive(false);
        }
        else
        {
            PauseFlag = true;
            Pause_UI.SetActive(true);
        }
    }

    public void Gameing_Text(int Score, int Coin)
    {
        Score_Text.text = Score.ToString();
        Coin_Text.text = Coin.ToString();
    }

    public void Click_Station()
    {
        GameManager.Instance.Start_Enter();
        //LoadingManager.LoadScene("Station");
    }

    public void Demo_Station()
    {
        GameManager.Instance.Demo_End_Enter();
    }
    public void Click_Retry()
    {
        LoadingManager.LoadScene("InGame");
    }

    public void Click_MainMenu()
    {
        LoadingManager.LoadScene("1.MainMenu");
    }

    public void Click_GameExit()
    {
        Application.Quit();
    }

    public void Click_Option()
    {
        gamedirector.GameType_Option(true);
        OptionFlag = true;
        Option_UI.SetActive(true);
    }

    public void Click_Option_Exit()
    {
        gamedirector.GameType_Option(false);
        OptionFlag = false;
        Option_UI.SetActive(false);
    }

    public void Item_EquipedIcon(int equiped_num, Sprite img, int Count)
    {
        Equiped_Item_Image[equiped_num].sprite = img;
        if(Count != 0)
        {
            Equiped_Item_Count[equiped_num].text = Count.ToString();
        }
        else
        {
            Equiped_Item_Count[equiped_num].gameObject.SetActive(false);
        }
    }

    public void ItemInformation_On(ItemDataObject item)
    {
        ItemIcon_Image.sprite = item.Item_Sprite;
        ItemName_Text.StringReference.TableEntryReference = "Item_Name_" + item.Num;
        ItemInformation_Text.StringReference.TableEntryReference = "Item_Information_" + item.Num;
        //ItemName_Text.text = itemName;
        //ItemInformation_Text.text = itemInformation;
        if (!ItemInformation_Object_Flag)
        {
            StartCoroutine(ItemInformation_Object_On());
        }
        else
        {
            ItemInformation_Object_Time = Time.time;
        }
    }

    IEnumerator ItemInformation_Object_On()
    {
        RectTransform ItemInformation = ItemInformation_Object.GetComponent<RectTransform>();
        float startX = ItemInformation.anchoredPosition.x;
        float targetX = -110f;

        float duration = 0.4f;
        float elapsedTime = 0f;

        ItemInformation_Object.SetActive(true);
        ItemInformation_Object_Flag = true;
        ItemInformation_Object_Time = Time.time;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, targetX, t);
            ItemInformation.anchoredPosition = new Vector2(newX, ItemInformation.anchoredPosition.y);
            yield return null;
        }

    }

    IEnumerator ItemInformation_Object_Off()
    {
        RectTransform ItemInformation = ItemInformation_Object.GetComponent<RectTransform>();
        float startX = ItemInformation.anchoredPosition.x;
        float targetX = 120f;

        float duration = 0.4f;
        float elapsedTime = 0f;

        ItemInformation_Object_Flag = false;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, targetX, t);
            ItemInformation.anchoredPosition = new Vector2(newX, ItemInformation.anchoredPosition.y);
            if(ItemInformation_Object_Flag)
            {
                StartCoroutine(ItemInformation_Object_On());
                break;
            }
            yield return null;
        }

        if (!ItemInformation_Object_Flag)
        {
            ItemInformation_Object.SetActive(false);
        }
    }

    public void ItemCoolTime_Instantiate(ItemDataObject item)
    {
        
        ItemCoolTime_Object.GetComponent<ItemCoolTime>().SetSetting(item);
        Instantiate(ItemCoolTime_Object, ItemCoolTime_List);
    }


    private void DemoCheck()
    {
        if (gamedirector.Stage_Num == 5)
        {
            Retry_Button.interactable = false;
            Station_Button.onClick.AddListener(() => Demo_Station());
        }
        else
        {
            Station_Button.onClick.AddListener(() => Click_Station());
        }
    }
}