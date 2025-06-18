using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse_Window_Box_All : MonoBehaviour
{
    Station_Inventory inventory;
    [SerializeField]
    SA_ItemList ItemList;
    ItemDataObject item;

    public GameObject ItemList_Objcet;
    public Transform ItemList_Transform;

    bool Check = false;
    List<ItemDataObject> Random_Material_ItemList;
    List<ItemDataObject> Random_Common_Item_ItemList;
    List<ItemDataObject> Random_Rare_Item_ItemList;
    List<ItemDataObject> Random_Common_Box_ItemList;
    List<ItemDataObject> Random_Rare_Box_ItemList;
    List<ItemDataObject> Random_Unique_Box_ItemList;
    List<ItemDataObject> Random_Epic_Box_ItemList;

    //ÃÑ 24°³±îÁö °¡´É
    public void Random_Box_All_Open(int itemNum,int count, GameObject InventoryDirector)
    {
        if (inventory == null)
        {
            inventory = InventoryDirector.GetComponent<Station_Inventory>();
        }

        if (!Check)
        {
/*            Random_Material_ItemList = ItemList.Random_Material_ItemList;

            Random_Common_Item_ItemList = ItemList.Common_Item_ItemList;
            Random_Rare_Item_ItemList = ItemList.Rare_Item_ItemList;*/
/*            Random_Common_Box_ItemList = ItemList.Common_Box_ItemList;
            Random_Rare_Box_ItemList = ItemList.Rare_Box_ItemList;
            Random_Unique_Box_ItemList = ItemList.Unique_Box_ItemList;
            Random_Epic_Box_ItemList = ItemList.Epic_Box_ItemList;*/
            Check = true;
        }

        if (itemNum == 54)
        {
            for(int i = 0; i < count; i++)
            {
                item = Random_Material_ItemList[Random.Range(0, Random_Material_ItemList.Count)];
                int Add_ItemCount = Random.Range(1, 11);
                GameObject itemlist = Instantiate(ItemList_Objcet, ItemList_Transform);
                itemlist.GetComponent<ItemList_OpenBox>().AllBoxOpenList(item, true, Add_ItemCount);
                item.Item_Count_UP(Add_ItemCount);
                //inventory.Check_ItemList(true, item, Add_ItemCount);
            }
        }
        else if(itemNum == 55)
        {
            for(int i = 0; i < count; i++)
            {
                int RarityNum = Random.Range(0, 101);
                if (RarityNum >= 0 && RarityNum < 80)
                {
                    int rand = Random.Range(0, Random_Rare_Item_ItemList.Count);
                    item = Random_Common_Item_ItemList[rand];
                }
                else if (RarityNum >= 80 && RarityNum < 100)
                {
                    int rand = Random.Range(0, Random_Rare_Item_ItemList.Count);
                    item = Random_Rare_Item_ItemList[rand];
                }
                GameObject itemlist = Instantiate(ItemList_Objcet, ItemList_Transform);
                itemlist.GetComponent<ItemList_OpenBox>().AllBoxOpenList(item, false);
                item.Item_Count_UP();
                //inventory.Check_ItemList(true, item);
            }
        }else if(itemNum == 56)
        {
            for(int i = 0; i < count; i++)
            {
                //Èñ±Íµµ°¡ ´Ù¸§
                int RarityNum = Random.Range(0, 100);
                if (RarityNum >= 0 && RarityNum < 60)
                {
                    item = Random_Common_Box_ItemList[Random.Range(0, Random_Common_Box_ItemList.Count)];
                }
                else if (RarityNum >= 60 && RarityNum < 80)
                {
                    item = Random_Rare_Box_ItemList[Random.Range(0, Random_Rare_Box_ItemList.Count)];
                }
                else if (RarityNum >= 80 && RarityNum < 95)
                {
                    item = Random_Unique_Box_ItemList[Random.Range(0, Random_Unique_Box_ItemList.Count)];
                }
                else if (RarityNum >= 95)
                {
                    item = Random_Epic_Box_ItemList[Random.Range(0, Random_Epic_Box_ItemList.Count)];
                }
                GameObject itemlist = Instantiate(ItemList_Objcet, ItemList_Transform);
                itemlist.GetComponent<ItemList_OpenBox>().AllBoxOpenList(item, false);
                item.Item_Count_UP();
                //inventory.Check_ItemList(true, item);
            }
        }
    }

    public void Item_BoxAll_Init()
    {
        for(int i = 0; i < ItemList_Transform.childCount; i++)
        {
            Destroy(ItemList_Transform.GetChild(i).gameObject);
        }
    }
}
