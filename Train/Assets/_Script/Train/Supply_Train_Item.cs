using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply_Train_Item : MonoBehaviour
{
    GameObject itemdirector_object;
    UseItem useitemScript;
    public ItemDataObject Item;

    public AudioClip GetItemSFX;

    Vector2 SupplyItem_Position;
    bool bounceFlag;

    public Sprite Box;
    SpriteRenderer _sprite;

    Material mat;

    private void Start()
    {
        itemdirector_object = GameObject.Find("ItemDirector");
        useitemScript = itemdirector_object.GetComponent<UseItem>();
        bounceFlag = false;

        SupplyItem_Position = new Vector2(transform.position.x, MonsterDirector.MinPos_Ground.y + 0.25f);
        _sprite = GetComponent<SpriteRenderer>();
        mat = _sprite.material;
        Check_Mat();
    }

    private void FixedUpdate()
    {
        if (!bounceFlag)
        {
            if(transform.position.y > SupplyItem_Position.y)
            {
                transform.Translate(7f * Vector2.down * Time.deltaTime);
            }
            else
            {
                bounceFlag = true;
                _sprite.sprite = Box;
            }
        }
        else
        {
            float offset = Mathf.Sin(Time.time * 10f) * 0.2f; // 2 : 스피드, 1.0f 이동거리
            Vector2 movement = new Vector2(0f, offset);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    void Check_Mat()
    {
        if (Item.Item_Rarity_Type == Information_Item_Rarity_Type.Common)
        {
            mat.SetColor("_SolidOutline", Color.gray);
        }
        else if (Item.Item_Rarity_Type == Information_Item_Rarity_Type.Rare)
        {
            mat.SetColor("_SolidOutline", Color.blue);
        }
        else if (Item.Item_Rarity_Type == Information_Item_Rarity_Type.Unique)
        {
            mat.SetColor("_SolidOutline", new Color(166, 0, 255));
        }
        else if (Item.Item_Rarity_Type == Information_Item_Rarity_Type.Epic)
        {
            mat.SetColor("_SolidOutline", Color.yellow);
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
}