using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class PassiveUpgrade_Tooltip : MonoBehaviour
{
    public LocalizeStringEvent Information_Text;
    public GameObject PassiveWindow;

    [SerializeField]
    bool TooltipFlag;

    float halfwidth;
    float halfheight;
    float pivot_x;
    float pivot_y;
    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        halfheight = GetComponentInParent<CanvasScaler>().referenceResolution.y * 0.016f;
        rt = GetComponent<RectTransform>();
        Information_Text.StringReference.TableReference = "Station_Table_St";

        Tooltip_Off();
    }

    // Update is called once per frame
    void Update()
    {
        if (TooltipFlag && PassiveWindow.activeSelf)
        {
            transform.position = Input.mousePosition + new Vector3(50, 20, 0);
        }

        /*if (TooltipFlag)
        {
            transform.position = Input.mousePosition + new Vector3(50, 20, 0);
        }*/

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
            pivot_y = 1f;
        }

        rt.pivot = new Vector2(pivot_x, pivot_y);
    }

    public void Tooltip_ON(int Card_Num)
    {
        TooltipFlag = true;
        Information_Text.StringReference.TableEntryReference = "UI_Passive_Upgrade_Card_" + Card_Num;

        gameObject.SetActive(true);
    }

    public void Tooltip_Off()
    {
        TooltipFlag = false;
        Information_Text.GetComponent<TextMeshProUGUI>().text = "";
        gameObject.SetActive(false);
    }
}