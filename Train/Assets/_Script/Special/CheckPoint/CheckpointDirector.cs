using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointDirector : MonoBehaviour
{
    public Slider slider;
    public Image slider_Handle;
    RectTransform slider_Handle_Ract;
    public bool clickflag;
    bool InsertFlag;
    bool RewardFlag;
    bool StopFlag;
    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider_Handle_Ract = slider_Handle.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!StopFlag)
        {
            if (!InsertFlag)
            {
                if (slider.value > slider.minValue && !clickflag)
                {
                    slider.value -= 0.0005f;
                }

                if (slider.value > slider.maxValue - 0.1f)
                {
                    slider_Handle.raycastTarget = false;
                    InsertFlag = true;
                }
            }

            if (InsertFlag)
            {
                if (slider_Handle_Ract.pivot.x > 0.05f)
                {
                    Vector2 vec = slider_Handle_Ract.pivot - new Vector2(0.005f, 0f);
                    slider_Handle_Ract.pivot = vec;
                }
                else
                {
                    RewardFlag = true;
                }
            }
        }


        if (RewardFlag)
        {
            Reward();
            RewardFlag = false;
            StopFlag = true;
        }
    }

    public void Reward()
    {
        Debug.Log("º¸»ó");
    }
}
