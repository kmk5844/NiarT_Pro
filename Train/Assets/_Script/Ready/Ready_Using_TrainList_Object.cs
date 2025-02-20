using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ready_Using_TrainList_Object : MonoBehaviour
{
    public PlayerReadyDirector director;
    public bool EmptyTrainFlag;

    public int TrainNum_1;
    public int TrainNum_2;
    public Image TrainImage;
    public GameObject Add_Object;
    public Button Btn;


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
            Btn.onClick.AddListener(() => director.Click_Change_Train());
        }
        else
        {
            TrainImage.gameObject.SetActive(false);
            Add_Object.SetActive(true);
            Btn.onClick.AddListener(() => director.Click_Add_Train());
        }

        Btn.interactable = false;
    }

}
