using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
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

    public GameObject CloseTreasureObj;
    public GameObject OpenTreasureObj;

    [Header("---------Sprite---------")]
    public Sprite GoldSprite;
    public Sprite FailSprite_Gold;
    public Sprite FailSprite_HP;

    bool startFlag = false;
    bool treasureFlag = false;

    List<int> List_RandomReward;
    List<int> List_Gold;
    List<ItemDataObject> List_Item;
    int goldCount;
    int itemCount;

    public AudioClip TreasuerBGM;
    public AudioClip OpenSFX;
    public AudioClip GetSFX;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        TreasureWindow.SetActive(false);
    }

    private void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        List_RandomReward = new List<int>();
        List_Gold = new List<int>();
        List_Item = new List<ItemDataObject>();
        CloseTreasureObj.SetActive(true);
        OpenTreasureObj.SetActive(false);
        RandomReward();
        MMSoundManagerSoundPlayEvent.Trigger(TreasuerBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
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
            MMSoundManagerSoundPlayEvent.Trigger(OpenSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
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
        CloseTreasureObj.SetActive(false);
        OpenTreasureObj.SetActive(true);
        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        ChangeTreasure(count);
        yield return new WaitForSeconds(0.2f);
        if(count != 0)
        {
            MMSoundManagerSoundPlayEvent.Trigger(GetSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }
        TotalRewardList[count].SetActive(true);
        yield return new WaitForSeconds(0.2f);
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
            TotalRewardList[count].transform.GetChild(2).GetComponent<Image>().sprite = GoldSprite;
            TotalRewardList[count].GetComponentInChildren<TextMeshProUGUI>().text = "Gold +" + List_Gold[goldCount] + "G";
            playerData.SA_Get_Coin(List_Gold[goldCount]);
            goldCount++;
        }
        else if (List_RandomReward[count] == 1)
        {
            TotalRewardList[count].transform.GetChild(2).GetComponent<Image>().sprite = List_Item[itemCount].Item_Sprite;
            TotalRewardList[count].GetComponentInChildren<LocalizeStringEvent>().StringReference.TableReference = "ItemData_Table_St";
            TotalRewardList[count].GetComponentInChildren<LocalizeStringEvent>().StringReference.TableEntryReference = "Item_Name_" + List_Item[itemCount].Num;
            itemCount++;
        }
        else if (List_RandomReward[count] == 2)
        {
            TotalRewardList[count].transform.GetChild(2).GetComponent<Image>().sprite = FailSprite_HP;
            TotalRewardList[count].GetComponentInChildren<TextMeshProUGUI>().text = "HP -5%";
        }
        else if (List_RandomReward[count] == 3)
        {
            TotalRewardList[count].transform.GetChild(2).GetComponent<Image>().sprite = FailSprite_Gold;
            TotalRewardList[count].GetComponentInChildren<TextMeshProUGUI>().text = "Gold -5%";
        }
    }
    public void Click_NextButton()
    {
        SelectStage.SetActive(true);
    }
}