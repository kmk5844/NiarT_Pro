using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PharmacyDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject PrarmacyWindow;
    public GameObject SelectStage;
    public GameObject CheckWindow;

    [Header("Data")]
    public SA_PlayerData playerData;
    bool startFlag;
    public ItemDataObject Small_HealItem;
    public ItemDataObject Large_HealItem;
    int playerHP;

    [Header("UI")]
    public Button BuyButton;
    public Button StageButton;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        PrarmacyWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            //Debug.Log("작동");
            QualitySettings.vSyncCount = 0;
        }

        BuyButton.onClick.AddListener(() => ClickHeal());
        StageButton.onClick.AddListener(() => PharmacyEnd());
        playerHP = ES3.Load<int>("Player_Curret_HP");

        if(playerData.Coin >= 5000)
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
        Small_HealItem.Item_Count_UP();
        Large_HealItem.Item_Count_UP();
    }

    void Heal()
    {
        Debug.Log(playerHP);
        playerHP = playerHP * 120 / 100;
        Debug.Log(playerHP);
        ES3.Save<int>("Player_Curret_HP",playerHP);
        CheckWindow.SetActive(true);
    }

    public void PharmacyEnd()
    {
        SelectStage.SetActive(true);
    }
}
