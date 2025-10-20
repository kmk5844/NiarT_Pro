using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TreeEditor.TreeGroup;

public class DrowRoomDirector : MonoBehaviour
{
    [Header("���丮")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject DrowRoomWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public SA_PlayerData playerData;
    public SA_ItemList itemData;
    public Transform RewardList;
    List<ItemDataObject> rewardItemList;
    bool startFlag;
    bool allFlag;
    public int gold = 100;
    public int openCount = 1;

    [Header("UI")]
    public DrowRoomButton btn;
    public Button DrowButton;
    public Button AllOpenButton;
    public Button NextButton;
    public TextMeshProUGUI PlayerGoldText;
    public TextMeshProUGUI AllOpenGoldText;
    private void Awake()
    {
        Special_Story.Story_Init(null, 14, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 0;
        }
        PlayerGoldText.text = playerData.Coin + " G";
        RewardList.gameObject.SetActive(false);
        RandomSet();
        for (int i = 0; i < 10; i++)
        {
            DrowRoomButton button = Instantiate(btn, RewardList);
            button.SettingItem(rewardItemList[i], this);
        }
        AllOpenButton.gameObject.SetActive(false);

        int coinMat = 1 << 10;
        int PayGold = gold * coinMat;
        AllOpenGoldText.text = PayGold + " G";
        if (playerData.Coin >= PayGold)
        {
            AllOpenButton.interactable = true;
        }
        else
        {
            AllOpenButton.interactable = false;
        }

        NextButton.gameObject.SetActive(false);
        NextButton.interactable = false;
    }

    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }

    void StartEvent()
    {
        RewardList.gameObject.SetActive(true);
        AllOpenButton.gameObject.SetActive(true);
        NextButton.gameObject.SetActive(true);
        startFlag = true;
    }

    void RandomSet()
    {
        rewardItemList = new List<ItemDataObject>();
        for(int i = 0; i < 10; i++)
        {
            int num = Random.Range(0, 29);
            if (num < 29)
            {
                rewardItemList.Add(itemData.Item[num]);
            }
            btn.ChangeGold(gold);
        }
    }

    public void Click_AllOpenBUtton()
    {
        StartCoroutine(Click_AllOpenCorutine());
    }

    IEnumerator Click_AllOpenCorutine()
    {
        AllOpenButton.interactable = false;
        NextButton.interactable = false;

        for(int i = 0; i < 10; i++)
        {
            DrowRoomButton btn = RewardList.GetChild(i).GetComponent<DrowRoomButton>();

            if (!btn.flag)
            {
                btn.ClickButton();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public void CheckCard()
    {
        bool clickflag = false;

        int coinMat = 1 << (openCount - 1);
        int PayGold = gold * coinMat;
        if (playerData.Coin >= PayGold)
        {
            clickflag = false;
        }
        else
        {
            clickflag = true;
        }

        for (int i = 0; i < 10; i++)
        {
            DrowRoomButton btn = RewardList.GetChild(i).GetComponent<DrowRoomButton>();
            if (!clickflag) // ���� ����� ���
            {
                
            }
            else // ���� ������ ���
            {
                btn.button.interactable = false;
            }
            btn.ChangeGold(PayGold);
        }
        NextButton.interactable = true;
    }

    public void DrowRoomEnd()
    {
        SelectStage.SetActive(true);
    }

    public void PlayerCoinPay()
    {
        int coinMat = 1 << (openCount - 1);
        int PayGold = gold * coinMat;
        playerData.SA_Buy_Coin(PayGold);
        PlayerGoldText.text = playerData.Coin + " G";
        openCount++;
    }
}
