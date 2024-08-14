using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainChange_Button_Event : MonoBehaviour
{
    public Button button;
    public int Button_Num;

    public TrainChange_Tooltip Tooltip_Object;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그


    // Update is called once per frame
    void Update()
    {
        if (item_information_Flag)
        {
            Tooltip_Object.Tooltip_ON(Button_Num);
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


    public void OnMouseEnter()
    {
        if(button.interactable == false)
        {
            item_information_Flag = true;
        }
    }

    public void OnMouseExit()
    {
        if (button.interactable == false)
        {
            item_information_Flag = false;
        }
    }
}
