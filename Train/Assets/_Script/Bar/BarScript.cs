using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarScript : MonoBehaviour
{
    public DialogSystem Special_Story;
    public Dialog dialog;

    bool startFlag;

    private void Awake()
    {
        Special_Story.Story_Init(null, 1 , 0);
    }


    private void Start()
    {
        
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
            startFlag = true;
        }
    }

    private void StartEvent()
    {

    }
}
