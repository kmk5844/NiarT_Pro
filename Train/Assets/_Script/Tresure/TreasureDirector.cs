using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureDirector : MonoBehaviour
{
    public DialogSystem Special_Story;
    public Dialog dialog;

    public SA_PlayerData playerData;
    public SA_ItemList itemlist;
    int RewardCount;
    int count;

    [Header("---------UI--------")]
    public GameObject TreasureWindow;
    public Image RewardImage;
    public TextMeshProUGUI RewardText;
    public GameObject OpenButton;
    public GameObject TotalReward;
    public GameObject[] TotalRewardList;


    [Header("---------Sprite---------")]
    public Sprite GoldSprite;

    bool startFlag = false;

    List<int> List_RandomReward;

    List<int> List_Gold;
    List<ItemDataObject> List_Item;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0);
        TreasureWindow.SetActive(false);
    }

    private void Start()
    {
        List_RandomReward = new List<int>();
        List_Gold = new List<int>();
        List_Item = new List<ItemDataObject>();

        RandomReward();
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
            startFlag = true;
        }
    }

    private void StartEvent()
    {
        TreasureWindow.SetActive(true);
    }

    private void RandomReward()
    {
        RewardCount = Random.Range(1, 5);

        for(int i = 0; i < RewardCount; i++)
        {
            int Random_1 = Random.Range(0, 101);
            if (Random_1 < 40)
            {
                int gold = Random.Range(100, 10001);
                List_Gold.Add(gold);
                List_RandomReward.Add(0);
            }
            else if (Random_1 >= 40 && Random_1 < 80)
            {
                int randomItem;
                if (Random.value < 20f / 32f) 
                    randomItem = Random.Range(0, 20); 
                else
                    randomItem = Random.Range(58, 70);

                ItemDataObject item = itemlist.Item[randomItem];
                List_Item.Add(item);
                List_RandomReward.Add(1);
            }
            else if (Random_1 >= 80 && Random_1 < 101)
            {
                Debug.Log("꽝");
                List_RandomReward.Add(2);
                //아이템
                //0골드
                //현재 체력 -1%
                //현재 체력 -3%
                //현재 체력 -5%
                //골드 -1%
                //골드 -3%
                //골드 -5%
                //현재 기차 체력 -1%
                //현재 기차 체력 -3%
                //현재 기차 체력 -5%
            }
        }
    }
    
    public void OpenTreasure()
    {
        OpenButton.SetActive(false);
        count = 0; 
        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        ChangeTreasure(count);
        yield return new WaitForSeconds(0.2f);
        RewardImage.gameObject.SetActive(true);
        RewardText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        RewardImage.gameObject.SetActive(false);
        RewardText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        count++;
        if(count < RewardCount)
        {
            StartCoroutine(Open());
        }
        else
        {
            TotalReward.SetActive(true);
        }
    }

    void ChangeTreasure(int count)
    {
        if (List_RandomReward[count] == 0)
        {
            RewardText.text = "Gold";
        }
        else if (List_RandomReward[count] == 1)
        {
            RewardText.text = "Item";

        }else if (List_RandomReward[count] == 2)
        {
            RewardText.text = "Fail";
        }
        TotalRewardList[count].SetActive(true);
    }
}