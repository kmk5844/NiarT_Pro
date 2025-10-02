using MoreMountains.Tools;
using System.Collections;
using UnityEngine;

public class ItemDirector : MonoBehaviour
{
    public GameDirector gameDirector;
    public UIDirector uiDirector;
    public SA_ItemList itemList;
    public SA_ItemData itemData;
    UseItem useitem;

    [SerializeField]
    bool[] EquipedItemFlag;
    [SerializeField]
    bool[] SupplyItemFlag;
    [SerializeField]
    float Duration;

    public GameObject Dron;

    bool player_revival_flag;
    int player_revival_num = 0;

    [Header("Sound")]
    public AudioClip UseItemSound;

    public int[] SupplyItem;

    private void Start()
    {
        try
        {
            itemData.Load();
            //itemData.Test(0, 1, 1);
        }
        catch
        {
            itemData.Init();
        }

        EquipedItemFlag = new bool[5];
        SupplyItemFlag = new bool[2];
        player_revival_flag = false;

        SupplyItem = new int[2];
        SupplyItem[0] = -1;
        SupplyItem[1] = -1;

        for (int i = 0; i < itemData.Equiped_Item.Count; i++)
        {
            if (itemData.Equiped_Item[i] == -1)
            {
                uiDirector.Item_EquipedIcon(i, itemData.EmptyObject.Item_Sprite, itemData.Equiped_Item_Count[i]);
                EquipedItemFlag[i] = false;
            }else if (itemData.Equiped_Item[i] == 0)
            {
                //��������
                uiDirector.Item_EquipedIcon(i, itemList.Item[itemData.Equiped_Item[i]].Item_Sprite, itemData.Equiped_Item_Count[i]);
                player_revival_flag = true;
                EquipedItemFlag[i] = false;
                player_revival_num = i;
            }
            else
            {
                uiDirector.Item_EquipedIcon(i, itemList.Item[itemData.Equiped_Item[i]].Item_Sprite, itemData.Equiped_Item_Count[i]);
                EquipedItemFlag[i] = true;
            }
        }

        for(int i = 3; i < 5; i++)
        {
            uiDirector.Item_EquipedIcon(i, itemData.EmptyObject.Item_Sprite, -2);
            EquipedItemFlag[i] = true;
            SupplyItemFlag[i-3] = false;
        }

        if (player_revival_flag)
        {
            gameDirector.Revival_PlayerSet();
            //���� ���Ϳ��� ��ȣ�� �˷������.
        }

        useitem = GetComponent<UseItem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && EquipedItemFlag[0])
        { 
            Change_EquipedItem(0);
            StartCoroutine(EquipItemCoolTime(0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && EquipedItemFlag[1])
        {
            Change_EquipedItem(1);
            StartCoroutine(EquipItemCoolTime(1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && EquipedItemFlag[2])
        {
            Change_EquipedItem(2);
            StartCoroutine(EquipItemCoolTime(2));
        }else if(Input.GetKeyDown(KeyCode.Alpha4) && SupplyItemFlag[0] && EquipedItemFlag[3])
        {
            Use_SupplyItem(0);
            StartCoroutine(EquipItemCoolTime(3));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5) && SupplyItemFlag[1] && EquipedItemFlag[4])
        {
            Use_SupplyItem(1);
            StartCoroutine(EquipItemCoolTime(4));
        }
    }

    IEnumerator EquipItemCoolTime(int i)
    {
        EquipedItemFlag[i] = false;
        float elapsedTime = 0f;
        while(elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            uiDirector.Equiped_CoolTime_Item_Image[i].fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / Duration);
            yield return null;
        }
        EquipedItemFlag[i] = true;
    }

    private void Change_EquipedItem(int num)
    {
        MMSoundManagerSoundPlayEvent.Trigger(UseItemSound, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        if (itemData.Equiped_Item[num] != -1)
        {
            useitem.UseEquipItem(itemData.Equiped_Item[num]);
            itemData.UseEquipedItem(num);
            if(itemData.Equiped_Item[num] == -1)
            {
                uiDirector.Item_EquipedIcon(num, itemData.EmptyObject.Item_Sprite, itemData.Equiped_Item_Count[num]);
            }
            else
            {
                uiDirector.Item_EquipedIcon(num, itemList.Item[itemData.Equiped_Item[num]].Item_Sprite, itemData.Equiped_Item_Count[num]);
            }
        }
    }

    private void Use_SupplyItem(int num)
    {
        int changeNum = -1;
        if(num == 0)
        {
            changeNum = 3;
        }else if(num == 1)
        {
            changeNum = 4;
        }
        MMSoundManagerSoundPlayEvent.Trigger(UseItemSound, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        useitem.Get_SupplyItem(SupplyItem[num]);
        SupplyItem[num] = -1;
        uiDirector.Item_EquipedIcon(changeNum, itemData.EmptyObject.Item_Sprite, -2);
    }

    public void Get_Supply_Item_Information(ItemDataObject Item)
    {
        uiDirector.ItemInformation_On(Item);
    }

    public void Item_Reward_Equiped()
    {
        int num = Random.Range(0, itemList.Equiped_Item_List.Count);
        ItemDataObject item = itemList.Equiped_Item_List[num];
        item.Item_Count_UP();
    }

    public void Item_UseDron()
    {
        int num = Random.Range(1, 4);
        Dron.GetComponentInChildren<Supply_Train_Dron>().SupplyDron_SetData(num, 3);
        Instantiate(Dron);
    }

    public void RevivalUse()
    {
        player_revival_flag = false;
        Change_EquipedItem(player_revival_num);
    }

    public void SupplySet(int index, int itemNum)
    {
        int changeNum = -1;
        if (index == 0)
        {
            changeNum = 3;
        }
        else if (index == 1)
        {
            changeNum = 4;
        }
        SupplyItem[index] = itemNum;
        uiDirector.Item_EquipedIcon(changeNum, itemList.Item[itemNum].Item_Sprite, -2);
        SupplyItemFlag[index] = true;
    }
}