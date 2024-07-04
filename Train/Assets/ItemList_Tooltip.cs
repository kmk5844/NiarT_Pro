using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList_Tooltip : MonoBehaviour
{
    public TextMeshProUGUI Item_Name;
    public TextMeshProUGUI Item_Information;
    public GameObject UseWindow;
    bool TootipFlag;

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
        if (TootipFlag)
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

    public void Tooltip_ON(string itemName, string itemInformation, bool useFlag)
    {
        TootipFlag = true;
        Item_Name.text = itemName;
        Item_Information.text = itemInformation;
        if (useFlag)
        {
            UseWindow.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    public void Tooltip_Off()
    {
        TootipFlag = false;
        Item_Name.text = "";
        Item_Information.text = "";
        UseWindow.SetActive(false);
        gameObject.SetActive(false);
    }
}
