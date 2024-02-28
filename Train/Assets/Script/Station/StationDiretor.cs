using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationDirector : MonoBehaviour
{
    Camera mainCam;
    public Transform Train;

    [Header("Click Button -> Open Window")]
    public GameObject BackGround_RepairShop;
    public GameObject Item_Buy_Window;              // 1
    public GameObject Player_Upgrade_Window;        // 2
    public GameObject Train_Upgrade_Window;         // 3
    public GameObject Train_BuyChange_Window;       // 4
    public GameObject Mercenary_Position_Window;    // 5
    public GameObject Mercenary_Buy_Window;         // 6

    int train_num; // 이걸로 이용하여 열차를 사거나 변경이 가능하다.
    int off_num;
    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        train_num = 0;
        off_num = 0;
    }
    public void ClickTrainButton(bool flag)
    {
        if (flag) //left
        {
            if (train_num + 1 > Train.childCount - 1)
            {
                train_num = Train.childCount - 1;
            }
            else
            {
                train_num++;
            }
        }
        else //right
        {
            if(train_num - 1 < 0)
            {
                train_num = 0;
            }
            else
            {
                train_num--;
            }
        }

        if (train_num == 0)
        {
            mainCam.transform.position = new Vector3(0, 2, -10);
        }
        else
        {
            mainCam.transform.position = new Vector3(-9 + ((train_num - 1) * -7), 2, -10);
        }
    }
    public void ClickWindowButton(int num)
    {
        if(BackGround_RepairShop.activeSelf == false)
        {
            BackGround_RepairShop.SetActive(true);
        }
        offWindow();
        
        switch (num)
        {
            case 1:
                Item_Buy_Window.SetActive(true);
                off_num = num;
                break;
            case 2: 
                Player_Upgrade_Window.SetActive(true);
                off_num = num;
                break;
            case 3:
                Train_Upgrade_Window.SetActive(true);
                off_num = num;
                break;
            case 4:
                Train_BuyChange_Window.SetActive(true);
                off_num = num;
                break;
            case 5:
                Mercenary_Position_Window.SetActive(true);
                off_num = num;
                break;
            case 6:
                Mercenary_Buy_Window.SetActive(true);
                off_num = num;
                break;
        }
    }

    void offWindow()
    {
        switch (off_num)
        {
            case 0:
                break;
            case 1:
                Item_Buy_Window.SetActive(false);
                break;
            case 2:
                Player_Upgrade_Window.SetActive(false);
                break;
            case 3:
                Train_Upgrade_Window.SetActive(false);
                break;
            case 4:
                Train_BuyChange_Window.SetActive(false);
                break;
            case 5:
                Mercenary_Position_Window.SetActive(false);
                break;
            case 6:
                Mercenary_Buy_Window.SetActive(false);
                break;
        }
    }
}