using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ready_Using_TrainList_Object : MonoBehaviour
{
    public PlayerReadyDirector director;
    [SerializeField]
    bool EmptyTrainFlag;

    [SerializeField]
    int Index;
    [SerializeField]
    int Sub_Index;
    [SerializeField]
    int TrainNum_1;
    [SerializeField]
    int TrainNum_2;

    public Image TrainImage;
    public GameObject Add_Object;
    public GameObject Select_Arrow_Object;
    public Button Btn;

    bool SelectFlag;

    private void Start()
    {
        if (!EmptyTrainFlag)
        {
            TrainImage.gameObject.SetActive(true);
            if(TrainNum_1 == 51 || TrainNum_1 == 52)
            {
                TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1 + "_" + (TrainNum_2/10)*10);
            }
            else
            {
                TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1);
            }
            Add_Object.SetActive(false);
        }
        else
        {
            TrainImage.gameObject.SetActive(false);
            Add_Object.SetActive(true);
        }
        Btn.onClick.AddListener(() => director.Click_Change_Train(Index));
        Btn.interactable = false;
    }


    public void Setting(int _index, int _num1, int _num2, bool empty)
    {
        Index = _index;
        TrainNum_1 = _num1;
        TrainNum_2 = _num2;
        EmptyTrainFlag = empty;
    }

    public void SelectFlag_Change(bool flag)
    {
        if (flag)
        {
            SelectFlag = true;
            Btn.interactable = true;
            Select_Arrow_Object.SetActive(true);
        }
        else
        {
            SelectFlag = false;
            Btn.interactable = false;
            Select_Arrow_Object.SetActive(false);
        }
    }

    public void Change_TrainNum(int num1, int num2)
    {
        if (EmptyTrainFlag)
        {
            EmptyTrainFlag = false;
            Add_Object.SetActive(false);
            TrainImage.gameObject.SetActive(true);
        }

        TrainNum_1 = num1;
        TrainNum_2 = num2;
    }

    public void Change_TrainImage()
    {
        if (TrainNum_1 == 51 || TrainNum_1 == 52)
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1 + "_" + (TrainNum_2 / 10) * 10);
        }
        else
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1);
        }
    }
}
