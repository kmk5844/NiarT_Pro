using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Ready_Mercenary_Information : MonoBehaviour
{
    public LocalizeStringEvent mercenary_name_text;
    public LocalizeStringEvent mercenary_information_text;

    public bool TooltipFlag;

    float halfwidth;
    float halfheight;
    float pivot_x;
    float pivot_y;
    RectTransform rt;

    private void Start()
    {
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        halfheight = GetComponentInParent<CanvasScaler>().referenceResolution.y * 0.016f;
        rt = GetComponent<RectTransform>();
        mercenary_name_text.StringReference.TableReference = "ExcelData_Table_St";
        mercenary_information_text.StringReference.TableReference = "ExcelData_Table_St";
    }

    private void Update()
    {

        if (TooltipFlag)
        {
            transform.position = Input.mousePosition + new Vector3(65, -150f, 0);
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
            pivot_y = 0f;
        }

        rt.pivot = new Vector2(pivot_x, pivot_y);
    }

    public void Tooltip_On(int num)
    {
        TooltipFlag = true;
        mercenary_name_text.StringReference.TableEntryReference = "Mercenary_Name_" + num;
        mercenary_information_text.StringReference.TableEntryReference = "Mercenary_Information_" + num;
        gameObject.SetActive(true);
    }

    public void Tooltip_off()
    {
        TooltipFlag = false;
        gameObject.SetActive(false);
    }
}
