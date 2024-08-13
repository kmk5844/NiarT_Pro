using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using static ItemList_Tooltip;

public class ResultItem_Tooltip : MonoBehaviour
{
    public Image Item_Icon;
    public LocalizeStringEvent Item_Name;
    public LocalizeStringEvent Item_Information;

    bool TooltipFlag;

    float halfwidth;
    float halfheight;
    float pivot_x;
    float pivot_y;
    RectTransform rt;

    void Start()
    {
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        halfheight = GetComponentInParent<CanvasScaler>().referenceResolution.y * 0.016f;
        rt = GetComponent<RectTransform>();
        Item_Name.StringReference.TableReference = "ItemData_Table_St";
        Item_Information.StringReference.TableReference = "ItemData_Table_St";

        Tooltip_Off();
    }
    private void Update()
    {
        if (TooltipFlag)
        {
            transform.position = Input.mousePosition + new Vector3(40, -40, 0);
        }

        if (rt.anchoredPosition.x + rt.sizeDelta.x > halfwidth)
        {
            // 왼쪽
            pivot_x = 1;
        }
        else
        {
            // 오른쪽
            pivot_x = 0;
        }

        if (rt.anchoredPosition.y + rt.sizeDelta.y > halfheight)
        {
            // 위
            pivot_y = 1f;
        }
        else
        {
            // 아래
            pivot_y = -0.58f;
        }

        rt.pivot = new Vector2(pivot_x, pivot_y);
    }


    public void Tooltip_ON(Sprite img, int item_Num, bool useFlag, int Pride)
    {
        TooltipFlag = true;
        Item_Icon.sprite = img;
        Item_Name.StringReference.TableEntryReference = "Item_Name_" + item_Num;
        Item_Information.StringReference.TableEntryReference = "Item_Information_" + item_Num;

        gameObject.SetActive(true);
    }

    public void Tooltip_Off()
    {
        TooltipFlag = false;
        Item_Name.StringReference.TableEntryReference = null;
        Item_Information.StringReference.TableEntryReference = null;
        Item_Name.GetComponent<TextMeshProUGUI>().text = "";
        Item_Information.GetComponent<TextMeshProUGUI>().text = "";
        gameObject.SetActive(false);
    }
}
