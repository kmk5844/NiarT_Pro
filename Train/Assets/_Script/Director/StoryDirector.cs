using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MoreMountains.Tools;

public class StoryDirector : MonoBehaviour
{
    [SerializeField]
    private Story_DataTable EX_StoryData;
    [SerializeField]
    private SA_StoryData SA_StoryData;
    [SerializeField]
    private SA_PlayerData SA_PlayerData;

    public AudioClip StoryBGM; 

    public Transform Canvas;
    [SerializeField]
    private Button SkipButton;
    [SerializeField]
    private Toggle AutoToggle;
    public bool SkipHit_Flag;
    public bool AutoHit_Flag;
    public bool Auto_Flag;
    public float delayTime;

    public GameObject[] BranchList;

    private void Awake()
    {
        int index = EX_StoryData.Story_Branch.FindIndex(x => x.Stage_Index.Equals(SA_PlayerData.New_Stage));
        int Branch_Value = EX_StoryData.Story_Branch[index].Branch_Index;
        GameObject Branch = BranchList[Branch_Value]; // stageNum에 따라 Branch 값을 가져온다.
        Branch.GetComponent<DialogSystem>().Story_Init(gameObject, SA_PlayerData.New_Stage, Branch_Value);
        GameObject Branch_Canvas = Instantiate(Branch, Canvas);
        Branch_Canvas.transform.SetSiblingIndex(1);
        gameObject.GetComponent<Dialog>().dialogSystem = Branch_Canvas.GetComponent<DialogSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        MMSoundManagerSoundPlayEvent.Trigger(StoryBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
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

        //토글에 마우스 진입(Enter) 이벤트 리스너 추가
        EventTrigger.Entry toggleEnter = new EventTrigger.Entry();
        toggleEnter.eventID = EventTriggerType.PointerEnter;
        toggleEnter.callback.AddListener((data) => { OnAutoToggleEnter(); });
        toggleTrigger.triggers.Add(toggleEnter);

        //토글에 마우스 빠져나옴(Exit) 이벤트 리스너 추가
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
        GameManager.Instance.End_Enter();
    }
}