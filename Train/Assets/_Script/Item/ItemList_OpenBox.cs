using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList_OpenBox : MonoBehaviour
{
    [SerializeField]
    ItemDataObject ItemData;
    public Image ItemIcon;
    public TextMeshProUGUI ItemCountText;

    public void AllBoxOpenList(ItemDataObject item, bool itemCountFlag, int itemcount = -1)
    {
        ItemIcon.sprite = item.Item_Sprite;
        if (!itemCountFlag)
        {
            ItemCountText.gameObject.SetActive(false);
        }
        else
        {
            ItemCountText.gameObject.SetActive(true);
            ItemCountText.text = itemcount.ToString();
        }
    }
    
}
