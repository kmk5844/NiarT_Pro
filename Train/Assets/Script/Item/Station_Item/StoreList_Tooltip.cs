using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreList_Tooltip : MonoBehaviour
{
    public TextMeshProUGUI Train_Name;
    public TextMeshProUGUI Train_Information;
    public TextMeshProUGUI Train_Pride;
    bool TooltipFlag;

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
            pivot_y = -0.5f;
        }

        rt.pivot = new Vector2(pivot_x, pivot_y);
    }

    public void Tooltip_ON(string storeName, string storeInformation, int Pride)
    {
        TooltipFlag = true;
        Train_Name.text = storeName;
        Train_Information.text = storeInformation;
        Train_Pride.text = Pride.ToString();

        gameObject.SetActive(true);
    }

    public void Tooltip_Off()
    {
        TooltipFlag = false;
        Train_Name.text = "";
        Train_Information.text = "";

        gameObject.SetActive(false);
    }
}
