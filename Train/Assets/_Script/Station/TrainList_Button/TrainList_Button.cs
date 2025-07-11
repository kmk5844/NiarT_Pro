using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainList_Button : MonoBehaviour
{
    public Station_TrainMaintenance director;
    public int listNum;
    public Button Btn;

    private void Start()
    {
        Btn.onClick.AddListener(() => director.Click_TrainList(listNum));
    }

    public void ChangeButton(bool flag)
    {
        Btn.interactable = flag;
    }

}