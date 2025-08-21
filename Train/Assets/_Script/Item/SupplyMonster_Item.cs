using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupplyMonster_Item : MonoBehaviour
{
    public bool SpawnMonster_Flag;
    public bool MissionMaterialFlag;
    GameObject itemdirector_object;
    ItemDirector itemdirector;
    public List<ItemDataObject> common_supplylist;
    public List<ItemDataObject> rare_supplylist;
    public List<ItemDataObject> unique_supplylist;
    public List<ItemDataObject> epic_supplylist;
    public List<ItemDataObject> epic_supplylist_NoWeapon;
    public List<ItemDataObject> legendary_supplylist;
    [SerializeField]
    ItemDataObject Item;
    UseItem useitemScript;

    public AudioClip GetItemSFX;

    Vector2 SupplyItem_Position;
    bool bounceFlag;

    Material mat;
    private void Start()
    {
        itemdirector_object = GameObject.Find("ItemDirector");
        itemdirector = itemdirector_object.GetComponent<ItemDirector>();
        common_supplylist = itemdirector.itemList.Common_Supply_ItemList.ToList();
        rare_supplylist = itemdirector.itemList.Rare_Supply_ItemList.ToList();
        unique_supplylist = itemdirector.itemList.Unique_Supply_ItemList.ToList();
        epic_supplylist = itemdirector.itemList.Epic_Supply_ItemList.ToList();
        epic_supplylist_NoWeapon = itemdirector.itemList.Epic_Supply_ItemList_NonWeapon.ToList();
        legendary_supplylist = itemdirector.itemList.Legendary_Supply_ItemList.ToList();
        useitemScript = itemdirector_object.GetComponent<UseItem>();
        bounceFlag = false;
        mat = GetComponent<SpriteRenderer>().material;


        if (MissionMaterialFlag)
        {
            if (!SpawnMonster_Flag)
            {
                mat.SetColor("_SolidOutline", Color.green);
            }
            else
            {
                Choice_Item();
            }
        }
        else
        {
            Choice_Item();
        }

        SupplyItem_Position = new Vector2(transform.position.x ,MonsterDirector.MinPos_Ground.y + 0.25f);
    }
    private void FixedUpdate()
    {
        if (!bounceFlag)
        {
            if (transform.position.y > SupplyItem_Position.y)
            {
                transform.Translate(7 * Vector2.down * Time.deltaTime);
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
        num = Random.Range(0, 101);

        Debug.Log(num);

        if (num >= 0 && num < 70) //70%
        {
            Item = common_supplylist[Random.Range(0, common_supplylist.Count)];
            mat.SetColor("_SolidOutline", Color.gray);
        }
        else if (num >= 70 && num < 85) //15%
        {
            Item = rare_supplylist[Random.Range(0, rare_supplylist.Count)];
            mat.SetColor("_SolidOutline", Color.blue);
        }
        else if (num >= 85 && num < 95) //10%
        {
            Item = unique_supplylist[Random.Range(0, unique_supplylist.Count)];
            mat.SetColor("_SolidOutline", new Color(166, 0, 255));
        }
        else if (num >= 95 && num < 99) //3%
        {
            if(!Player.Item_GunFlag)
            {
                Item = epic_supplylist[Random.Range(0, epic_supplylist.Count)];
            }
            else
            {
                Item = epic_supplylist[Random.Range(0, epic_supplylist_NoWeapon.Count)];
            }
            mat.SetColor("_SolidOutline", Color.yellow);
        }
        else if (num >= 99 && num < 101)//1%
        {
            Item = legendary_supplylist[Random.Range(0, legendary_supplylist.Count)];
            mat.SetColor("_SolidOutline", new Color(161, 251, 232));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MMSoundManagerSoundPlayEvent.Trigger(GetItemSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            useitemScript.Get_SupplyItem(Item.Num);
            Destroy(gameObject);
        }
    }

    public void ChangeMaterial(ItemDataObject _item)
    {
        MissionMaterialFlag = true;
        Item = _item;
    }
}
