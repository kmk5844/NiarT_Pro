using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionDirector : MonoBehaviour
{
    public bool isKeyBoardFlag;
    public bool isCreditFlag;

    public GameObject KeyBoard_Object;
    public GameObject Credit_Object;

    void Start()
    {
        isKeyBoardFlag = false;
        isCreditFlag = false;
    }

    private void OnDisable()
    {
        isKeyBoardFlag = false;
        isCreditFlag = false;
        KeyBoard_Object.SetActive(false);
        Credit_Object.SetActive(false);
    }

    public void Click_OpenKeyBoard()
    {
        isKeyBoardFlag = true;
        KeyBoard_Object.SetActive(true);
    }

    public void Click_OpenCredit()
    {
        isCreditFlag = true;
        Credit_Object.SetActive(true);
    }

    public void Click_CloseKeyBoard()
    {
        isKeyBoardFlag = false;
        KeyBoard_Object.SetActive(false);
    }

    public void Click_CloseCredit()
    {
        isCreditFlag = false;
        Credit_Object.SetActive(false);
    }
}
