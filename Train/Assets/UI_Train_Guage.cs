using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_Train_Guage : MonoBehaviour
{
    Train_InGame Train_Data;
    public Image HP_Guage;
    public Image Special_Guage;
    public GameObject ON_Object;

    float timer;

    void Start()
    {
        Train_Data = GetComponentInParent<Train_InGame>();
    }

    void Update()
    {
        HP_Guage.fillAmount = Train_Data.HP_Parsent / 100f;
        if (Train_Data.HP_Parsent < 30f)
        {
            timer += Time.deltaTime;
            if(timer >= 0.3f)
            {
                ON_Object.SetActive(!ON_Object.activeSelf);
                timer = 0f;
            }
        }
        else
        {
            ON_Object.SetActive(false);
        }
    }
}
