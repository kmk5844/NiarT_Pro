using MoreMountains.Tools;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

public class ItemDirector : MonoBehaviour
{
    public bool On_ItemList_Test;

    public UIDirector uiDirector;
    public SA_ItemList itemList;
    public SA_ItemList_Test itemList_Test;
    public SA_ItemData itemData;
    UseItem useitem;

    [SerializeField]
    bool[] EquipedItemFlag;
    [SerializeField]
    float Duration;

    [Header("Sound")]
    public AudioClip UseItemSound;


    private void Start()
    {
        try
        {
            itemData.Load();
        }
        catch
        {
            itemData.Init();
        }

        EquipedItemFlag = new bool[3];

        for (int i = 0; i < itemData.Equiped_Item.Count; i++)
        {
            if (itemData.Equiped_Item[i] == -1)
            {
                uiDirector.Item_EquipedIcon(i, itemData.EmptyObject.Item_Sprite, itemData.Equiped_Item_Count[i]);
                EquipedItemFlag[i] = false;
            }
            else
            {
                if (!On_ItemList_Test)
                {
                    uiDirector.Item_EquipedIcon(i, itemList.Item[itemData.Equiped_Item[i]].Item_Sprite, itemData.Equiped_Item_Count[i]);
                }
                else
                {
                    uiDirector.Item_EquipedIcon(i, itemList_Test.Item[itemData.Equiped_Item[i]].Item_Sprite, itemData.Equiped_Item_Count[i]);
                }
                EquipedItemFlag[i] = true;
            }
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
                if (!On_ItemList_Test)
                {
                    uiDirector.Item_EquipedIcon(num, itemList.Item[itemData.Equiped_Item[num]].Item_Sprite, itemData.Equiped_Item_Count[num]);
                }
                else
                {
                    uiDirector.Item_EquipedIcon(num, itemList_Test.Item[itemData.Equiped_Item[num]].Item_Sprite, itemData.Equiped_Item_Count[num]);
                }

            }
        }
    }

    public void Get_Supply_Item_Information(ItemDataObject Item)
    {
        uiDirector.ItemInformation_On(Item);
    }

    public void Item_Reward_Equiped()
    {
        int num = Random.Range(0, itemList_Test.Equiped_Item_List.Count);
        ItemDataObject_Test item = itemList_Test.Equiped_Item_List[num];
        Debug.Log(item.Item_Count);
        item.Item_Count_UP();
        Debug.Log(item.Item_Count);
    }
}