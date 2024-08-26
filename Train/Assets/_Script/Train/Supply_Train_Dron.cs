using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Supply_Train_Dron : MonoBehaviour
{
    public SA_ItemList itemList;
    [SerializeField]
    int gradeNum;
    [SerializeField]
    int Data_SupplyCount;

    int SupplyCount;

    ItemDataObject[] item;
    float[] Supply_Pos_X; 

    Rigidbody2D rid;
    Vector3 spawnPosition = new Vector3(MonsterDirector.MinPos_Sky.x - 15f, MonsterDirector.MaxPos_Sky.y + 2f);

    public GameObject SupplyItem_Objcet;

    private void Start()
    {
        transform.position = spawnPosition;    
        rid = GetComponent<Rigidbody2D>();
        rid.velocity = new Vector2(8f, 0f);

        Supply_Pos_X = new float[Data_SupplyCount];
        item = new ItemDataObject[Data_SupplyCount];

        for (int i = 0; i < Data_SupplyCount; i++)
        {
            Supply_Pos_X[i] = Random.Range(MonsterDirector.MinPos_Sky.x + 5f, MonsterDirector.MaxPos_Sky.x - 5f);
            item[i] = Check_Grade();
        }
        System.Array.Sort(Supply_Pos_X);
    }

    private void Update()
    {
        if(SupplyCount < Data_SupplyCount)
        {
            if (transform.position.x > Supply_Pos_X[SupplyCount])
            {
                SupplyItem_Objcet.GetComponent<Supply_Train_Item>().Item = item[SupplyCount];
                Instantiate(SupplyItem_Objcet, transform.position, Quaternion.identity);
                SupplyCount++;
            }
        }

        if(transform.position.x > MonsterDirector.MaxPos_Sky.x + 15f)
        {
            Destroy(gameObject);
        }
    }

    public void SupplyDron_SetData(int num, int count)
    {
        gradeNum = num;
        Data_SupplyCount = count;
    }

    ItemDataObject Check_Grade()
    {
        int Random_num = 0;
        int Random_Rarity = 0;

        if(gradeNum == 1)
        {
            Random_Rarity = Random.Range(0, 2);
        }else if(gradeNum == 2)
        {
            Random_Rarity = Random.Range(0, 3);
        }else if(gradeNum == 3)
        {
            Random_Rarity = Random.Range(0, 4);
        }

        if(Random_Rarity == 0)
        {
            Random_num = Random.Range(0, itemList.Common_Supply_ItemList.Count);
            return itemList.Common_Supply_ItemList[Random_num];
        }
        else if(Random_Rarity == 1)
        {
            Random_num = Random.Range(0, itemList.Rare_Supply_ItemList.Count);
            return itemList.Rare_Supply_ItemList[Random_num];
        }else if(Random_Rarity == 2)
        {
            Random_num = Random.Range(0, itemList.Unique_Supply_ItemList.Count);
            return itemList.Unique_Supply_ItemList[Random_num];

        }else if(Random_Rarity == 3)
        {
            Random_num = Random.Range(0, itemList.Epic_Supply_ItemList.Count);
            return itemList.Epic_Supply_ItemList[Random_num];
        }
        return itemList.Common_Supply_ItemList[1];
    }
}
