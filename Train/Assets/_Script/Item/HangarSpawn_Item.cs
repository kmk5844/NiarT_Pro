using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarSpawn_Item : MonoBehaviour
{
    UseItem useitemScript;

    public ItemDataObject Item;
    public AudioClip GetItemSFX;

    SpriteRenderer _sprite;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float offset = Mathf.Sin(Time.time * 10f) * 0.2f; // 2 : 스피드, 1.0f 이동거리
        Vector2 movement = new Vector2(0f, offset);
        transform.Translate(movement * Time.deltaTime);
    }

    public void SetItem(ItemDataObject _item, UseItem use)
    {
        Item = _item;
        Check_Mat();
        useitemScript = use;
    }

    void Check_Mat()
    {
        _sprite = GetComponent<SpriteRenderer>();
        mat = _sprite.material;
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
            useitemScript.Get_CheckItem(Item.Num);
            Destroy(gameObject);
        }
    }
}