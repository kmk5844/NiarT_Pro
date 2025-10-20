using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PharmacyDirector : MonoBehaviour
{
    [Header("���丮")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject PrarmacyWindow;
    public GameObject SelectStage;
    public GameObject CheckWindow;

    [Header("Data")]
    public SA_PlayerData playerData;
    public Game_DataTable game_DataTable;
    bool startFlag;
    public ItemDataObject Small_HealItem;
    public ItemDataObject Large_HealItem;
    int originPlayerHP;
    int Max_HP;

    int beforePlayerHP;
    int curretPlayerHP;

    [Header("UI")]
    public Button BuyButton;
    public Button StageButton;
    public TextMeshProUGUI playerCoinText;
    public TextMeshProUGUI checkWindowText;

    private void Awake()
    {
        Special_Story.Story_Init(null, 9, 0, 0);
        PrarmacyWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            //Debug.Log("�۵�");
            QualitySettings.vSyncCount = 0;
        }
        playerCoinText.text = playerData.Coin + " G";

        BuyButton.onClick.AddListener(() => ClickHeal());
        StageButton.onClick.AddListener(() => PharmacyEnd());

        originPlayerHP = game_DataTable.Information_Player[playerData.Player_Num].Player_HP;
        curretPlayerHP = ES3.Load<int>("Player_Curret_HP");
        beforePlayerHP = curretPlayerHP;
        Max_HP = originPlayerHP + (((originPlayerHP * playerData.Level_Player_HP) * 10) / 100);

        if (playerData.Coin >= 5000)
        {
            BuyButton.interactable = true;
        }
        else
        {
            BuyButton.interactable= false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }

    private void StartEvent()
    {
        PrarmacyWindow.SetActive(true);
        startFlag = true;
    }

    public void ClickHeal()
    {
        CheckWindow.SetActive(true);
        BuyButton.interactable = false;
        Heal();
        playerData.SA_Buy_Coin(5000);
        playerCoinText.text= playerData.Coin + " G";
        Small_HealItem.Item_Count_UP();
        Large_HealItem.Item_Count_UP();
    }

    void Heal()
    {
        curretPlayerHP += (int)(Max_HP * 0.2f);

        // �ִ� ü�� �ʰ� ����
        if (curretPlayerHP > Max_HP)
        {
            curretPlayerHP = Max_HP;
        }
        ES3.Save<int>("Player_Curret_HP", curretPlayerHP);
        checkWindowText.text = beforePlayerHP + "<color=red>  ->  " + curretPlayerHP + "</color>";
        CheckWindow.SetActive(true);
    }

    public void PharmacyEnd()
    {
        SelectStage.SetActive(true);
    }
}
