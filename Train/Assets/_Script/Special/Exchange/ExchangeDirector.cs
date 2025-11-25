using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ExchangeDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject ExchangeWindow;
    public GameObject SelectStage;
    public GameObject RewardWindow;

    [Header("데이터")]
    public SA_ItemList ItemData;
    List<ItemDataObject> Reward_Items;
    ItemDataObject RewardItem;
    public ItemDataObject SelectItem;
    bool exchangeflag = false;
    bool startFlag;

    [Header("UI")]
    public Image RewardImage;
    public Transform ItemTransform;
    public GameObject Itemobject;
    public Image SelectItemImage;
    public LocalizeStringEvent SelectName;

    [Header("---------Sound---------")]
    public AudioClip ExchangeBGM;
    public AudioClip MissionSelectBGM;

    private void Awake()
    {
        Special_Story.Story_Init(null, 7, 0, 0);
        ExchangeWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        ItemSetting();
        RewardSetting();
        MMSoundManagerSoundPlayEvent.Trigger(ExchangeBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID : 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }

        if (exchangeflag)
        {
            EXCHANGE();
            exchangeflag = false;
        }
    }

    private void StartEvent()
    {
        ExchangeWindow.SetActive(true);
        startFlag = true;
    }

    void ItemSetting()
    {
        Reward_Items = new List<ItemDataObject>();

        foreach(ItemDataObject item in ItemData.Item)
        {
            if (item.Item_Count > 0)
            {
                GameObject obj = Instantiate(Itemobject, ItemTransform);
                obj.GetComponent<ExchangeList>().SettingItem(item, this);
            }

            if (item.Sell_Flag)
            {
                Reward_Items.Add(item);
            }
        }
    }

    void RewardSetting()
    {
        int num = Random.Range(0, Reward_Items.Count);
        RewardItem = Reward_Items[num];
        RewardImage.sprite = RewardItem.Item_Sprite;
    }

    public void Click_Item(ItemDataObject item)
    {
        SelectItem = item;
        SelectItemImage.sprite = SelectItem.Item_Sprite;
        SelectName.StringReference.TableReference = "ItemData_Table_St";
        SelectName.StringReference.TableEntryReference = "Item_Name_" + item.Num;
    }

    public void Click_Exchange()
    {
        RewardWindow.SetActive(true);
        exchangeflag = true;
    }

    void EXCHANGE()
    {
        int count = SelectItem.Item_Count;
        SelectItem.Item_Count_Down(count);
        RewardItem.Item_Count_UP(count);
    }

    public void Click_Check()
    {
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, 10);
        MMSoundManagerSoundPlayEvent.Trigger(MissionSelectBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: 20);
        SelectStage.SetActive(true);
    }
}