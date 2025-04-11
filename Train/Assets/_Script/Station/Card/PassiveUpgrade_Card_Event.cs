using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveUpgrade_Card_Event : MonoBehaviour
{
    public int Card_Num;

    public PassiveUpgrade_Tooltip Tooltip_Object;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그

    private void Update()
    {
        if (item_information_Flag)
        {
            Tooltip_Object.Tooltip_ON(Card_Num);
            item_mouseOver_Flag = true;
        }
        else
        {
            if (item_mouseOver_Flag)
            {
                Tooltip_Object.Tooltip_Off();
                item_mouseOver_Flag = false;
            }
        }
    }

    public void OnDisable()
    {
        item_information_Flag = false;
    }

    public void OnMouseEnter()
    {
        item_information_Flag = true;
    }

    public void OnMouseExit()
    {
        item_information_Flag = false;
    }
}
