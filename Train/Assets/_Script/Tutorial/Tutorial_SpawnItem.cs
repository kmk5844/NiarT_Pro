using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_SpawnItem : MonoBehaviour
{
    public ItemDataObject Item;
    public AudioClip GetItemSFX;

    GamePlay_Tutorial_Director tutorialDirector;

    float SupplyItem_Position;
    bool bounceFlag;
    public Sprite Box;
    SpriteRenderer _sprite;

    Material mat;

    void Start()
    {
        tutorialDirector = GameObject.Find("TutorialDirector").GetComponent<GamePlay_Tutorial_Director>();
        transform.position = new Vector2(Random.Range(-0.5f, 0.5f), 14.5f);
        bounceFlag = false;
        SupplyItem_Position = -1f;
        _sprite = GetComponent<SpriteRenderer>();
        mat = _sprite.material;
        Check_Mat();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!bounceFlag)
        {
            if (transform.position.y > SupplyItem_Position)
            {
                transform.Translate(6f * Vector2.down * Time.deltaTime);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MMSoundManagerSoundPlayEvent.Trigger(GetItemSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            if (!tutorialDirector.Tutorial_4GetFlag && !tutorialDirector.Tutorial_5GetFlag)
            {
                tutorialDirector.Tutorial_4GetFlag = true;
                tutorialDirector.uiDirector.item_changeIcon(3, 4, true);
                Instantiate(tutorialDirector.SpawnItemObject);
            }
            else if(tutorialDirector.Tutorial_4GetFlag && !tutorialDirector.Tutorial_5GetFlag)
            {
                tutorialDirector.Tutorial_5GetFlag = true;
                tutorialDirector.uiDirector.item_changeIcon(4, 5, true);
                Instantiate(tutorialDirector.SpawnItemObject);
            }
            else if(tutorialDirector.Tutorial_4GetFlag && tutorialDirector.Tutorial_5GetFlag)
            {
                tutorialDirector.UseItem(Item);
            }
            Destroy(gameObject);
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
}
