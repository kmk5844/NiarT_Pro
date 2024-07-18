using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse_Window_Box : MonoBehaviour
{
    Station_Inventory inventory;
    [SerializeField]
    SA_ItemList ItemList;
    [SerializeField]
    ItemDataObject item;

    public Image Item_Icon;
    public TextMeshProUGUI Item_Name;
    public TextMeshProUGUI Item_Count;

    bool Check = false;

    List<ItemDataObject> Random_Material_ItemList;
    List<ItemDataObject> Random_Common_Item_ItemList;
    List<ItemDataObject> Random_Rare_Item_ItemList;
    List<ItemDataObject> Random_Common_Box_ItemList;
    List<ItemDataObject> Random_Rare_Box_ItemList;
    List<ItemDataObject> Random_Unique_Box_ItemList;
    List<ItemDataObject> Random_Epic_Box_ItemList;


    public void Random_Box_Open(int num, GameObject InventoryDirector)
    {
        if(inventory == null)
        {
            inventory = InventoryDirector.GetComponent<Station_Inventory>();
        }    

        if (!Check)
        {
            Random_Material_ItemList = ItemList.Random_Material_ItemList;

            Random_Common_Item_ItemList = ItemList.Common_Item_ItemList;
            Random_Rare_Item_ItemList = ItemList.Rare_Item_ItemList;
/*            Random_Unique_Item_ItemList = ItemList.Unique_Item_ItemList;
            Random_Epic_Item_ItemList = ItemList.Epic_Item_ItemList;*/

            Random_Common_Box_ItemList = ItemList.Common_Box_ItemList;
            Random_Rare_Box_ItemList = ItemList.Rare_Box_ItemList;
            Random_Unique_Box_ItemList = ItemList.Unique_Box_ItemList;
            Random_Epic_Box_ItemList = ItemList.Epic_Box_ItemList;
            Check = true;
        }
            
        if (num == 54)
        {
            //Èñ±Íµµ°¡ µ¿ÀÏ
            item = Random_Material_ItemList[Random.Range(0, Random_Material_ItemList.Count)];
            int Add_ItemCount = Random.Range(1, 11);
            //Item_Icon = ¼³Á¤
            Item_Name.text = item.Item_Name;
            Item_Icon.sprite = item.Item_Sprite;
            Item_Count.text = Add_ItemCount.ToString();
            item.Item_Count_UP(Add_ItemCount);
            inventory.Check_ItemList(true, item, Add_ItemCount);
        }
        else if(num == 55)
        {
            int RarityNum = Random.Range(0, 100);
            
            if (RarityNum >= 0 && RarityNum < 80)
            {
                int rand = Random.Range(0, Random_Rare_Item_ItemList.Count);
                item = Random_Common_Item_ItemList[rand];
            }
            else if (RarityNum >= 90 && RarityNum < 100)
            {
                int rand = Random.Range(0, Random_Rare_Item_ItemList.Count);
                item = Random_Rare_Item_ItemList[rand];
            }


            /*else if(RarityNum >= 80 && RarityNum < 95)
            {
                item = Random_Unique_Item_ItemList[Random.Range(0, Random_Unique_Item_ItemList.Count)];
            }
            else if(RarityNum >= 95)
            {
                item = Random_Epic_Item_ItemList[Random.Range(0, Random_Epic_Item_ItemList.Count)];
            }*/
            //Item_Icon = ¼³Á¤
            Item_Name.text = item.Item_Name;
            Item_Icon.sprite = item.Item_Sprite;
            Item_Count.text = "1";
            item.Item_Count_UP();
            inventory.Check_ItemList(true, item);
        }
        else if(num == 56)
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
            //Item_Icon = ¼³Á¤
            Item_Name.text = item.Item_Name;
            Item_Icon.sprite = item.Item_Sprite;
            Item_Count.text = "1";
            item.Item_Count_UP();
            inventory.Check_ItemList(true, item);
        }
    }
}