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
            // ����
            pivot_x = 1;
        }
        else
        {
            // ������
            pivot_x = 0;
        }

        if (rt.anchoredPosition.y + rt.sizeDelta.y > halfheight)
        {
            // ��
            pivot_y = 1f;
        }
        else
        {
            // �Ʒ�
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
                //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "����Ͻ÷��� ��Ŭ�� �����ּ���";
            }
        }
        else if (tooltiptype == TooltipType.Store_Buy)
        {
            Item_Pride.text = "���� ���� : " + Pride;
            UseWindow.SetActive(true);
            UseWindow_Text.StringReference.TableEntryReference = "UI_ToolTip_Buy";
            //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "�����Ͻ÷��� ��Ŭ�� �����ּ���";
        }
        else if(tooltiptype == TooltipType.Store_Sell)
        {
            Item_Pride.text = "�Ǹ� ���� : " + Pride;
            UseWindow.SetActive(true);
            UseWindow_Text.StringReference.TableEntryReference = "UI_ToolTip_Sell";
            //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "�Ǹ��Ͻ÷��� ��Ŭ�� �����ּ���";
        }
        else if(tooltiptype == TooltipType.Equip)
        {
            //�ִ� ���� text
            Item_Pride.text = "";
            UseWindow.SetActive(true);
            UseWindow_Text.StringReference.TableEntryReference = "UI_ToolTip_Equip";
            //UseWindow.GetComponentInChildren<TextMeshProUGUI>().text = "�����Ͻ÷��� ��Ŭ�� �����ּ���";
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
