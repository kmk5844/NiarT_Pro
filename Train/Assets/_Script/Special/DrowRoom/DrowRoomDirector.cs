using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TreeEditor.TreeGroup;

public class DrowRoomDirector : MonoBehaviour
{
    [Header("스토리")]
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

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        DrowRoomWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 0;
        }

        RandomSet();
        for (int i = 0; i < 10; i++)
        {
            DrowRoomButton button = Instantiate(btn, RewardList);
            button.SettingItem(rewardItemList[i], this);
        }
        AllOpenButton.gameObject.SetActive(false);

        int coinMat = 1 << 10;
        int PayGold = gold * coinMat;
        Debug.Log(PayGold);
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
        DrowRoomWindow.SetActive(true);
        startFlag = true;
    }

    void RandomSet()
    {
        rewardItemList = new List<ItemDataObject>();
        for(int i = 0; i < 10; i++)
        {
            int num = Random.Range(0, 33);
            if (num < 20)
            {
                rewardItemList.Add(itemData.Item[num]);
            }
            else
            {
                rewardItemList.Add(itemData.Item[num + 37]);
            }
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
            if (!clickflag) // 돈이 충분한 경우
            {
                
            }
            else // 돈이 부족한 경우
            {
                btn.button.interactable = false;
            }
        }

        NextButton.interactable = true;
    }


    public void showDrow()
    {
        DrowButton.gameObject.SetActive(false);
        RewardList.gameObject.SetActive(true);
        AllOpenButton.gameObject.SetActive(true);
        NextButton.gameObject.SetActive(true);
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
        openCount++;
    }
}
