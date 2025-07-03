using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowRoomDirector : MonoBehaviour
{
    [Header("Ω∫≈‰∏Æ")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject DrowRoomWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public SA_ItemList itemData;
    public Transform RewardList;
    List<ItemDataObject> rewardItemList;
    bool startFlag;
    bool allFlag;

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
            rewardItemList[i].Item_Count_UP();
        }
        AllOpenButton.gameObject.SetActive(false);
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
        int count = 0;


        for (int i = 0; i < 10; i++)
        {
            DrowRoomButton btn = RewardList.GetChild(i).GetComponent<DrowRoomButton>();

            if (!btn.flag)
            {
                count++;
            }
        }

        if(count == 0)
        {
            NextButton.interactable = true;
        }
        else
        {
            NextButton.interactable = false;
        }
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
}
