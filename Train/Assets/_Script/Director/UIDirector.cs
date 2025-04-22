using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using PixelCrushers.DialogueSystem;
using static PixelCrushers.AnimatorSaver;
using UnityEngine.Localization;

public class UIDirector : MonoBehaviour
{
    public GameObject GameDirector_Object;
    GameDirector gamedirector;
    Player player;
    public SA_ItemList itemList;
    SA_PlayerData playerData;

    [Header("전체적인 UI 오브젝트")]
    public GameObject Game_UI;
    public GameObject Pause_UI;
    public GameObject Result_UI;
    public GameObject Option_UI;

    [Header("Playing UI")]
    public Image Player_Blood;
    Color Player_Blood_Color;
    bool isBloodFlag;
    float blood_delayTime;
    float blood_lastTime;

    [Header("Game UI")]
    public Image Player_Head;
    public Sprite[] Player_Head_Sprite;
    public Image Player_HP_Bar;
    public Slider Distance_Bar;
    public Image TotalFuel_Bar;
    public TextMeshProUGUI Speed_Text;
    public TextMeshProUGUI Fuel_Text;
    public TextMeshProUGUI Score_Text;
    public TextMeshProUGUI Coin_Text;
    public Slider Speed_Arrow;
    public GameObject WarningObject;

    [Header("Clear UI")]
    public GameObject Clear_UI;

    [Header("Boss UI")]
    public GameObject BossHP_Object;
    public Image BossHP_Guage;

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
    public Image[] Equiped_CoolTime_Item_Image;
    TextMeshProUGUI[] Equiped_Item_Count;

    [Header("Pause + Item UI")]
    public GameObject itemobject_pause;
    public Transform GetItemList_Transform_Pause;

    [Header("CoolTime UI")]
    public Transform CoolTime_List;
    public GameObject ItemCoolTime_Object;
    public GameObject SkillCoolTime_Object;

    [Header("Skill UI")]
    public Transform Equiped_Skill_List;
    public Image[] Equiped_Skill_Image;
    public Image[] Equiped_CoolTime_Skill_Image;

    [Header("Result UI 관련된 텍스트 및 아이템")]
    public TextMeshProUGUI[] Result_Text_List; //0. Stage, 1. Score, 2. Gold, 3. Rank 4. Point
    public GameObject WinWindow;
    public GameObject LoseWindow;
    public LocalizeStringEvent LoseText;

    public LocalizeStringEvent missionTitle;
    public TextMeshProUGUI missionInformation;

    [Header("SubSelectStage UI")]
    public GameObject UIObject_SubSelectStage;

    public List<int> GetItemList_Num;
    public Transform GetItemList_Transform_Result;
    public GameObject GetItemList_Object;
    public ResultItem_Tooltip Tooltip_Object;

    bool OptionFlag;

    [HideInInspector]
    public bool LoseFlag;
    [HideInInspector]
    public int LoseText_Num;

    [Header("미션 정보")]
    public LocalizeStringEvent missionTextInformation_text;
    public TextMeshProUGUI missionCountText_text;

    [Header("Wave")]
    public Sprite[] Refresh_Item_Sprite;
    public GameObject WaveObject;
    int WaveCount;

    private void Awake()
    {
        isBloodFlag = false;
        blood_delayTime = 0.4f;
        Equiped_Item_Image = new Image[Equiped_Item_List.childCount];
        Equiped_Item_Count = new TextMeshProUGUI[Equiped_Item_List.childCount];

        for (int i = 0; i < Equiped_Item_List.childCount; i++)
        {
            Equiped_Item_Image[i] = Equiped_Item_List.GetChild(i).GetComponent<Image>();
            Equiped_Item_Count[i] = Equiped_Item_List.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }

        Equiped_Skill_Image = new Image[Equiped_Item_List.childCount];
        for(int i = 0; i < Equiped_Skill_List.childCount; i++)
        {
            Equiped_Skill_Image[i] = Equiped_Skill_List.GetChild(i).GetComponent<Image>();
        }
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player_Head.sprite = Player_Head_Sprite[player.PlayerNum];
        gamedirector = GameDirector_Object.GetComponent<GameDirector>();
        Player_Blood_Color = Player_Blood.GetComponent<Image>().color;
        playerData = gamedirector.SA_PlayerData;

        ItemName_Text.StringReference.TableReference = "ItemData_Table_St";
        ItemInformation_Text.StringReference.TableReference = "ItemData_Table_St";

        LoseFlag = false;   
        OptionFlag = false;

        ItemInformation_Object_Flag = false;
        ItemInformation_Object_TimeDelay = 5f;
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
                ON_OFF_Option_UI(OptionFlag);
            }
        }



        Player_HP_Bar.fillAmount = player.Check_HpParsent() / 100f;
        TotalFuel_Bar.fillAmount = gamedirector.Check_Fuel();
        Speed_Text.text = "<size=21>" + (int)gamedirector.TrainSpeed + "</size> Km/h";
        Fuel_Text.text = (int)(gamedirector.Check_Fuel() * 100f) + "%";
        Speed_Arrow.value = gamedirector.TrainSpeed / gamedirector.MaxSpeed;

        Distance_Bar.value = gamedirector.Check_Distance();
    }

    public void Open_Result_UI(bool Win, int Score, int Coin, SelectMission mission, int LoseNum = -1)
    {
        //Result_Text_List[0].text = Score.ToString(); // + "점";
        Result_Text_List[1].text = Coin + "G"; // + "원";
        if (Win)
        {
            WinWindow.SetActive(true);
            LoseWindow.SetActive(false);
            
            Result_Text_List[2].text = mission.MissionReward + "G";
            Result_Text_List[3].text = "+" + (Coin + mission.MissionReward) + "G";

            missionTitle.StringReference.TableReference = "MissionList_St";
            missionTitle.StringReference.TableEntryReference = "Title_" + mission.MISSIONNUM;
            missionInformation.text = missionTextInformation_text.StringReference.GetLocalizedString();

            for (int i = 0; i < GetItemList_Num.Count; i++)
            {
                ResultObject _resultobject = GetItemList_Object.GetComponent<ResultObject>();
                _resultobject.item = itemList.Item[GetItemList_Num[i]];
                _resultobject.item_tooltip_object = Tooltip_Object;
                Instantiate(GetItemList_Object, GetItemList_Transform_Result);
            }
        }
        else
        {
            LoseFlag = true;
            LoseText_Num = LoseNum;
            //LoseNum은 Lose_UI Director에 있다.
            WinWindow.SetActive(false);
            LoseWindow.SetActive(true);

            Result_Text_List[2].text = "0G";
            Result_Text_List[3].text = "-" + (int)(playerData.Coin * (mission.MissionCoinLosePersent / 100f));
            LoseText.StringReference.TableReference = "InGame_Table_St";
            LoseText.StringReference.TableEntryReference = "UI_Lose_Text_" + LoseText_Num;
        }

        Game_UI.SetActive(false);
        Result_UI.SetActive(true);
    }

    public void ON_OFF_Option_UI(bool Flag)
    {
        if (Flag)
        {
            OptionFlag = false;
            Option_UI.SetActive(false);
        }
        else
        {
            OptionFlag = true;
            Option_UI.SetActive(true);
        }
    }

    public void Gameing_Text(int Score, int Coin)
    {
        //Score_Text.text = Score.ToString();
        Coin_Text.text = Coin.ToString();
    }

    public void Open_SubSelect()
    {
        UIObject_SubSelectStage.SetActive(true);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void Click_Station()
    {
        GameManager.Instance.BeforeStation_Enter();
        //SceneManager.LoadScene("Station");
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
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

    public void View_ItemList(Sprite sprite)
    {
        GameObject item = Instantiate(itemobject_pause, GetItemList_Transform_Pause);
        item.GetComponent<Image>().sprite = sprite;
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

    public void ItemInformation_On(ItemDataObject item, bool Supply = false, int SupplyNum = -1, int SupplyPersent = -1)
    {
        if (!Supply)
        {
            ItemIcon_Image.sprite = item.Item_Sprite;
            ItemName_Text.StringReference.TableEntryReference = "Item_Name_" + item.Num;
            ItemInformation_Text.StringReference.TableEntryReference = "Item_Information_" + item.Num;
        }
        else
        {
            ItemIcon_Image.sprite = Refresh_Item_Sprite[SupplyNum];
            ItemName_Text.StringReference.TableEntryReference = "Item_RefreshSupply_" + SupplyNum;
            ItemInformation_Text.StringReference.TableEntryReference = "Item_RefreshSupply_Information";
            ItemInformation_Text.StringReference.Arguments = new object[] { SupplyPersent };
            ItemInformation_Text.RefreshString();
        }
        
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
        Instantiate(ItemCoolTime_Object, CoolTime_List);
    }

    public void SkillCoolTime_Instantiate(int skillNum, float during)
    {
        SkillCoolTime_Object.GetComponent<SkillCoolTime>().SetSetting(Equiped_Skill_Image[skillNum].sprite, during);
        Instantiate(SkillCoolTime_Object, CoolTime_List);
    }

    public void Player_Blood_Ani()
    {
        StartCoroutine(Blood_On());
    }

    IEnumerator Blood_On()
    {
        float duration = 0.2f;
        float elapsedTime = 0f;

        Player_Blood.gameObject.SetActive(true);
        isBloodFlag = true;
        blood_lastTime = Time.time;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Player_Blood_Color.a = Mathf.Lerp(0f, 1f, t);
            Player_Blood.color = Player_Blood_Color;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Player_Blood_Color.a = Mathf.Lerp(1f, 0f, t);
            Player_Blood.color = Player_Blood_Color;
            yield return null;
        }

        if (!ItemInformation_Object_Flag)
        {
            Player_Blood.gameObject.SetActive(false);
        }
        isBloodFlag = false;
    }

    public IEnumerator GameClear()
    {
        Clear_UI.SetActive(true);
        yield return new WaitForSeconds(3f);
        Clear_UI.SetActive(false);
    }

    public void CheckMissionInformation(int missionNum, Quest_DataTable DataList, int missionList_Index)
    {
        missionTextInformation_text.StringReference.TableReference = "MissionList_St";

        string[] missionState = DataList.Q_List[missionList_Index].Quest_State.Split(',');

        LocalizedString monsterString = new LocalizedString();
        monsterString.TableReference = "MissionList_St";

        switch (missionNum)
        {
            case 0:
                //기본
                missionTextInformation_text.StringReference.TableEntryReference = "Information_" + missionNum;
                break;
            case 1:
                missionTextInformation_text.StringReference.Arguments = new object[] { missionState[0] };
                missionTextInformation_text.StringReference.TableEntryReference = "Information_" + missionNum;
                missionTextInformation_text.RefreshString();
                break;
            case 2:
                missionTextInformation_text.StringReference.Arguments = new object[] { -1, -1 };
                monsterString.TableEntryReference = "Monster_" + missionState[0]; // 예: Boss_0, Boss_1 같은 식
                // monsterString 번역값 가져와서 Argument로 전달
                monsterString.GetLocalizedStringAsync().Completed += handle =>
                {
                    string monsterName = handle.Result;

                    // Argument에 monsterName 넣기
                    missionTextInformation_text.StringReference.Arguments = new object[] { monsterName, missionState[1] };
                    missionTextInformation_text.StringReference.TableEntryReference = "Information_" + missionNum;
                    missionTextInformation_text.RefreshString();
                };
                break;
            case 3:
                //기본
                missionTextInformation_text.StringReference.TableEntryReference = "Information_" + missionNum;
                break;
            case 4:
                //기본
                missionTextInformation_text.StringReference.TableEntryReference = "Information_" + missionNum;
                break;
            case 5:
                missionTextInformation_text.StringReference.Arguments = new object[] { -1 };
                monsterString.TableEntryReference = "Boss_" + missionState[0]; // 예: Boss_0, Boss_1 같은 식
                // monsterString 번역값 가져와서 Argument로 전달
                monsterString.GetLocalizedStringAsync().Completed += handle =>
                {
                    string monsterName = handle.Result;

                    // Argument에 monsterName 넣기
                    missionTextInformation_text.StringReference.Arguments = new object[] { monsterName };
                    missionTextInformation_text.StringReference.TableEntryReference = "Information_" + missionNum;
                    missionTextInformation_text.RefreshString();
                };
                break;
        }
    }


    public IEnumerator WaveInformation()
    {
        yield return StartCoroutine(Wave_Object_On());
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Wave_Object_Off());
    }


    IEnumerator Wave_Object_On()
    {
        RectTransform waveobject = WaveObject.GetComponent<RectTransform>();
        float startX = waveobject.anchoredPosition.x;
        float targetX = -60f;

        float duration = 0.4f;
        float elapsedTime = 0f;
        WaveCount++;
        WaveObject.GetComponentInChildren<TextMeshProUGUI>().text = "WABE-" + WaveCount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, targetX, t);
            waveobject.anchoredPosition = new Vector2(newX, waveobject.anchoredPosition.y);
            yield return null;
        }
    }
    IEnumerator Wave_Object_Off()
    {
        RectTransform waveobject = WaveObject.GetComponent<RectTransform>();
        float startX = waveobject.anchoredPosition.x;
        float targetX = 100f;

        float duration = 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, targetX, t);
            waveobject.anchoredPosition = new Vector2(newX, waveobject.anchoredPosition.y);
            yield return null;
        }
    }
}