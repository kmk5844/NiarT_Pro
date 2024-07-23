using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;


public class ItemList_Tooltip : MonoBehaviour
{
    public Image Item_Icon;
    public TextMeshProUGUI Item_Name;
    public TextMeshProUGUI Item_Information;
    public TextMeshProUGUI Item_Pride;
    public GameObject UseWindow;
    LocalizeStringEvent UseWindow_Text;
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
        Store_Sell,
        Equip
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
            pivot_y = -0.58f;
        }

        rt.pivot = new Vector2(pivot_x, pivot_y);
    }

    public void Tooltip_ON(Sprite img, string itemName, string itemInformation, bool useFlag, int Pride)
    {
        TooltipFlag = true;
        Item_Icon.sprite = img;
        Item_Name.text = itemName;
        Item_Information.text = itemInformation;
        UseWindow_Text = UseWindow.GetComponentInChildren<LocalizeStringEvent>();
        UseWindow_Text.StringReference.TableReference = "Station_Table_St";
        if (tooltiptype == TooltipType.Inventory)
        {
            Item_Pride.text = "";
            if (useFlag)
            {
                UseWindow.SetActive(true);
                UseWindow_Text.StringReference.TableEntryReference = "UI_ToolTip_Use";
                //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "사용하시려면 좌클릭 눌러주세요";
            }
        }
        else if (tooltiptype == TooltipType.Store_Buy)
        {
            Item_Pride.text = "구매 가격 : " + Pride;
            UseWindow.SetActive(true);
            UseWindow_Text.StringReference.TableEntryReference = "UI_ToolTip_Buy";
            //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "구매하시려면 좌클릭 눌러주세요";
        }
        else if(tooltiptype == TooltipType.Store_Sell)
        {
            Item_Pride.text = "판매 가격 : " + Pride;
            UseWindow.SetActive(true);
            UseWindow_Text.StringReference.TableEntryReference = "UI_ToolTip_Sell";
            //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "판매하시려면 좌클릭 눌러주세요";
        }
        else if(tooltiptype == TooltipType.Equip)
        {
            //최대 개수 text
            Item_Pride.text = "";
            UseWindow.SetActive(true);
            UseWindow_Text.StringReference.TableEntryReference = "UI_ToolTip_Equip";
            //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "장착하시려면 좌클릭 눌러주세요";
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
