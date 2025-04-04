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
    //int mouseClickCount;

    [Header("---------UI--------")]
    public GameObject TreasureWindow;
    //public Image RewardImage;
    //public TextMeshProUGUI RewardText;
    //public GameObject OpenButton;
    public GameObject TotalReward;
    public GameObject[] TotalRewardList;
    public GameObject NextButton;
    public GameObject SelectStage;

    [Header("---------Sprite---------")]
    public Sprite GoldSprite;
    public Sprite FailSprite;

    bool startFlag = false;
    bool treasureFlag = false;

    List<int> List_RandomReward;
    List<int> List_Gold;
    List<ItemDataObject> List_Item;
    int goldCount;
    int itemCount;

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

        if (Input.GetMouseButtonDown(0) && treasureFlag)
        {
            OpenTreasure();
        }
    }

    private void StartEvent()
    {
        TreasureWindow.SetActive(true);
        treasureFlag = true;
    }

    private void RandomReward()
    {
        RewardCount = Random.Range(1, 6);

        for(int i = 0; i < RewardCount; i++)
        {
            int Random_1 = Random.Range(0, 101);
            if (Random_1 < 40)
            {
                int gold = Random.Range(1, 101);
                
                List_Gold.Add(gold * 100);
                List_RandomReward.Add(0);
            }
            else if (Random_1 >= 40 && Random_1 < 80)
            {
                int randomItem;
                if (Random.value < 20f / 32f)
                {
                    randomItem = Random.Range(0, 20);
                    if (randomItem == 0 || randomItem == 12)
                    {
                        randomItem = 1;
                    }
                }
                else
                    randomItem = Random.Range(58, 70);

                ItemDataObject item = itemlist.Item[randomItem];
                List_Item.Add(item);
                List_RandomReward.Add(1);
            }
            else if (Random_1 >= 80 && Random_1 < 101)
            {
                List_RandomReward.Add(2);
                int failReward = Random.Range(0, 3);
                if(failReward == 0){
                    List_Gold.Add(0);
                    List_RandomReward.Add(0);
                }else if(failReward == 1)
                {
                    List_RandomReward.Add(2);
                    playerData.SA_Loss_HP_Persent(5);
                }else if(failReward == 2)
                {
                    List_RandomReward.Add(3);
                    playerData.SA_Loss_Coin_Persent(5);
                }
            }
        }
    }
    
    public void OpenTreasure()
    {
        treasureFlag = false;
        //OpenButton.SetActive(false);
        count = 0;
        TotalReward.SetActive(true);
        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        ChangeTreasure(count);
        yield return new WaitForSeconds(0.2f);
/*        RewardImage.gameObject.SetActive(true);
        RewardText.gameObject.SetActive(true);*/
        TotalRewardList[count].SetActive(true);
        yield return new WaitForSeconds(1f);
/*        RewardImage.gameObject.SetActive(false);
        RewardText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);*/
        count++;
        if(count < RewardCount)
        {
            StartCoroutine(Open());
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            NextButton.SetActive(true);
        }
    }

    void ChangeTreasure(int count)
    {
        if (List_RandomReward[count] == 0)
        {
            TotalRewardList[count].GetComponent<Image>().sprite = GoldSprite;
            //RewardText.text = List_Gold[goldCount] + "G";
            playerData.SA_Get_Coin(List_Gold[goldCount]);
            goldCount++;
        }
        else if (List_RandomReward[count] == 1)
        {
            TotalRewardList[count].GetComponent<Image>().sprite = List_Item[itemCount].Item_Sprite;
            TotalRewardList[count].GetComponentInChildren<TextMeshProUGUI>().text = List_Item[itemCount].Item_Name;
            itemCount++;
        }
        else if (List_RandomReward[count] == 2)
        {
            TotalRewardList[count].GetComponent<Image>().sprite = FailSprite;
            TotalRewardList[count].GetComponentInChildren<TextMeshProUGUI>().text = "HP -5%";
        }
        else if (List_RandomReward[count] == 3)
        {
            TotalRewardList[count].GetComponent<Image>().sprite = FailSprite;
            TotalRewardList[count].GetComponentInChildren<TextMeshProUGUI>().text = "Gold - 5%";
        }
    }

    public void Click_NextButton()
    {
        SelectStage.SetActive(true);
    }
}