using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainList_Button : MonoBehaviour
{
    public Station_TrainMaintenance director;
    public int listNum;
    Button Btn;

    private void Awake()
    {
        Btn = GetComponent<Button>();
    }

    private void Start()
    {
        Btn.onClick.AddListener(() => director.Click_TrainList(listNum));
    }

    public void ChangeButton(bool flag)
    {
        try
        {
            Btn.interactable = flag;
        }
        catch
        {
            Debug.Log("기차 구매 윈도우 떠야 됨");
        }
    }
}