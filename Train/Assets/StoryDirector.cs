using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoryDirector : MonoBehaviour
{
    [SerializeField]
    private Button SkipButton;
    [SerializeField]
    private Toggle AutoToggle;
    public bool SkipHit_Flag;
    public bool AutoHit_Flag;
    public bool Auto_Flag;
    public float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        SkipButton.onClick.AddListener(() => Click_Skip_Button());
        AutoToggle.onValueChanged.AddListener((value) => Click_Auto_Toggle(value));


        EventTrigger buttonTrigger = SkipButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry buttonEntry = new EventTrigger.Entry();
        buttonEntry.eventID = EventTriggerType.PointerEnter;
        buttonEntry.callback.AddListener((data) => { OnSkipButtonEnter(); });
        buttonTrigger.triggers.Add(buttonEntry);

        EventTrigger.Entry buttonExit = new EventTrigger.Entry();
        buttonExit.eventID = EventTriggerType.PointerExit;
        buttonExit.callback.AddListener((data) => { OnSkipButtonExit(); });
        buttonTrigger.triggers.Add(buttonExit);

        EventTrigger toggleTrigger = AutoToggle.gameObject.AddComponent<EventTrigger>();

        // 토글에 마우스 진입(Enter) 이벤트 리스너 추가
        EventTrigger.Entry toggleEnter = new EventTrigger.Entry();
        toggleEnter.eventID = EventTriggerType.PointerEnter;
        toggleEnter.callback.AddListener((data) => { OnAutoToggleEnter(); });
        toggleTrigger.triggers.Add(toggleEnter);

        // 토글에 마우스 빠져나옴(Exit) 이벤트 리스너 추가
        EventTrigger.Entry toggleExit = new EventTrigger.Entry();
        toggleExit.eventID = EventTriggerType.PointerExit;
        toggleExit.callback.AddListener((data) => { OnAutoToggleExit(); });
        toggleTrigger.triggers.Add(toggleExit);
    }


    private void OnSkipButtonEnter()
    {
        SkipHit_Flag = true;
    }

    private void OnSkipButtonExit()
    {
        SkipHit_Flag = false;
    }

    private void OnAutoToggleEnter()
    {
        AutoHit_Flag = true;
    }

    private void OnAutoToggleExit()
    {
        AutoHit_Flag = false;
    }

    private void Click_Auto_Toggle(bool isOn)
    {
        if (isOn)
        {
            Auto_Flag = true;
        }
        else
        {
            Auto_Flag = false;
        }
    }


    private void Click_Skip_Button()
    {
        LoadingManager.LoadScene("Station");
    }
}
