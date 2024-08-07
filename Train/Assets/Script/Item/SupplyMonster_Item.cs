using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyMonster_Item : MonoBehaviour
{
    GameObject itemdirector_object;
    ItemDirector itemdirector;
    List<ItemDataObject> common_supplylist;
    List<ItemDataObject> rare_supplylist;
    List<ItemDataObject> unique_supplylist;
    List<ItemDataObject> epic_supplylist;
    [SerializeField]
    ItemDataObject Item;
    UseItem useitemScript;

    Vector2 SupplyItem_Position;
    bool bounceFlag;

    Material mat;
    private void Start()
    {
        itemdirector_object = GameObject.Find("ItemDirector");
        itemdirector = itemdirector_object.GetComponent<ItemDirector>();
        common_supplylist = itemdirector.itemList.Common_Supply_ItemList;
        rare_supplylist = itemdirector.itemList.Rare_Supply_ItemList;
        unique_supplylist = itemdirector.itemList.Unique_Supply_ItemList;
        epic_supplylist = itemdirector.itemList.Epic_Supply_ItemList;
        useitemScript = itemdirector_object.GetComponent<UseItem>();
        bounceFlag = false;
        mat = GetComponent<SpriteRenderer>().material;

        Choice_Item();
        SupplyItem_Position = new Vector2(transform.position.x ,MonsterDirector.MinPos_Ground.y + 0.25f);
    }
    private void FixedUpdate()
    {
        if (!bounceFlag)
        {
            if (transform.position.y > SupplyItem_Position.y)
            {
                transform.Translate(2 * Vector2.down * Time.deltaTime);
            }
            else
            {
                if (transform.position.x < MonsterDirector.MinPos_Ground.x || transform.position.x > MonsterDirector.MaxPos_Ground.x)
                {
                    transform.Translate(2 * Vector2.down * Time.deltaTime);
                    if (SupplyItem_Position.y - 2f > transform.position.y)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    bounceFlag = true;
                }
            }
        }
        else
        {
            float offset = Mathf.Sin(Time.time * 10f) * 0.2f; // 2 : 스피드, 1.0f 이동거리
            Vector2 movement = new Vector2(0f, offset);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    private void Choice_Item()
    {
        int num = 0;
        if (!Player.Item_GunFlag)
        {
            num = Random.Range(0, 101);
        }
        else
        {
            num = Random.Range(0, 96);
        }

        if(num >= 0 && num < 70)
        {
            Item = common_supplylist[Random.Range(0, common_supplylist.Count)];
            mat.SetColor("_SolidOutline", Color.gray);
        }
        else if(num >= 70 && num < 85)
        {
            Item = rare_supplylist[Random.Range(0, rare_supplylist.Count)];
            mat.SetColor("_SolidOutline", Color.blue);
        }
        else if(num >= 85 && num < 95)
        {
            Item = unique_supplylist[Random.Range(0, unique_supplylist.Count)];
            mat.SetColor("_SolidOutline", new Color(166,0,255));
        }
        else if(num >= 95 && num < 101)
        {
            Item = epic_supplylist[Random.Range(0, epic_supplylist.Count)];
            mat.SetColor("_SolidOutline", Color.yellow);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            useitemScript.Get_SupplyItem(Item.Num);
            Destroy(gameObject);
        }
    }
}
