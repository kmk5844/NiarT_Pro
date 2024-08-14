using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainChange_Button_Event : MonoBehaviour
{
    public Button button;
    public int Button_Num;

    public TrainChange_Tooltip Tooltip_Object;

    bool item_information_Flag; // ���� ��� �÷���
    bool item_mouseOver_Flag; // �̹� �÷��� �ִٴ� �÷���


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
