using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ready_Buy_TrainObject : MonoBehaviour
{
    public PlayerReadyDirector director;

    public int TrainNum_1;
    public int TrainNum_2;
    public Image TrainImage;
    public Button Btn;

    private void Start()
    {
        if (TrainNum_1 == 51 || TrainNum_1 == 52)
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1 + "_" + (TrainNum_2 / 10) * 10);
        }
        else
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1);
        }
        Btn.onClick.AddListener(()=>director.Click_Select_Train(TrainNum_1, TrainNum_2));
    }
}
