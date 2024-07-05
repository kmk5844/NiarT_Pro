using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList_Tooltip : MonoBehaviour
{
    public TextMeshProUGUI Item_Name;
    public TextMeshProUGUI Item_Information;
    public TextMeshProUGUI Item_Pride;
    public GameObject UseWindow;
    bool TooltipFlag;

    float halfwidth;
    float halfheight;
    float pivot_x;
    float pivot_y;
    RectTransform rt;

    public enum TooltipType
    {
        Inventory,
        Store_Buy,
        Store_Sell
    }

    public TooltipType tooltiptype;

    private void Start()
    {
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        halfheight = GetComponentInParent<CanvasScaler>().referenceResolution.y * 0.016f;
        rt = GetComponent<RectTransform>();
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
            pivot_y = -0.5f;
        }

        rt.pivot = new Vector2(pivot_x, pivot_y);
    }

    public void Tooltip_ON(string itemName, string itemInformation, bool useFlag, int Pride)
    {
        TooltipFlag = true;
        Item_Name.text = itemName;
        Item_Information.text = itemInformation;
        if (tooltiptype == TooltipType.Inventory)
        {
            Item_Pride.text = "";
            if (useFlag)
            {
                UseWindow.SetActive(true);
                UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "사용하시려면 좌클릭 눌러주세요";
            }
        }
        else if (tooltiptype == TooltipType.Store_Buy)
        {
            Item_Pride.text = "구매 가격 : " + Pride;
            UseWindow.SetActive(true);
            UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "구매하시려면 좌클릭 눌러주세요";
        }else if(tooltiptype == TooltipType.Store_Sell)
        {
            Item_Pride.text = "판매 가격 : " + Pride;
            UseWindow.SetActive(true);
            UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "판매하시려면 좌클릭 눌러주세요";
        }

        gameObject.SetActive(true);
    }

    public void Tooltip_Off()
    {
        TooltipFlag = false;
        Item_Name.text = "";
        Item_Information.text = "";
        UseWindow.SetActive(false);
        gameObject.SetActive(false);
    }
}
