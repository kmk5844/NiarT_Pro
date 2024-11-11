using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MoreMountains.Tools;
using TMPro;

public class StoryDirector : MonoBehaviour
{
    [SerializeField]
    private Story_DataTable EX_StoryData;
    [SerializeField]
    private SA_PlayerData SA_PlayerData;
    [SerializeField]
    private SA_LocalData sa_localdata;

    public AudioClip StoryBGM; 

    public Transform Canvas;
    [SerializeField]
    private Button SkipButton;
    [SerializeField]
    private Toggle AutoToggle;
    [SerializeField]
    private Button BackLogButton;
    [SerializeField]
    private Button OptionButton;

    public TextMeshProUGUI Stage_Title;
    public TextMeshProUGUI Stage_Num;

    public bool skipHit_Flag;
    public bool toggleHit_Flag;
    public bool backHit_Flag;
    public bool optionHit_Flag;
    public bool Auto_Flag;
    public bool BackLog_Flag;
    public bool Option_Flag;
    public float delayTime;

    public GameObject[] BranchList;
    public List<DialogData> BackLog;
    DialogSystem branch_DialogSystem;

    public GameObject BackLog_Window;
    public Transform BackLog_Content;
    public GameObject BackLog_Object;
    public GameObject Option_Window;

    public Image BackGround_Image;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (!Option_Flag && !BackLog_Flag)
            {
                Click_Option_Button();
            }
            else if (Option_Flag && !BackLog_Flag)
            {
                Click_Option_Back_Button();
            }

            if (BackLog_Flag)
            {
                Click_BackLog_Back_Button();
            }
        }
    }

    private void Awake()
    {
        int index = SA_PlayerData.Story_Num;
        //int index = EX_StoryData.Story_Branch.FindIndex(x => x.Stage_Index.Equals(SA_PlayerData.New_Stage));
        int Branch_Value = EX_StoryData.Story_Branch[index].Branch_Index;
        BackGround_Image.sprite = Resources.Load<Sprite>("Story/BackGround/" + EX_StoryData.Story_Branch[index].BackGround);
        GameObject Branch = BranchList[Branch_Value]; // stageNum에 따라 Branch 값을 가져온다.
        Branch.GetComponent<DialogSystem>().Story_Init(gameObject, SA_PlayerData.New_Stage, Branch_Value);
        GameObject Branch_Canvas = Instantiate(Branch, Canvas);
        Branch_Canvas.transform.SetSiblingIndex(1);
        branch_DialogSystem = Branch_Canvas.GetComponent<DialogSystem>();
        gameObject.GetComponent<Dialog>().dialogSystem = branch_DialogSystem;
        BackLog = new List<DialogData>();
        branch_DialogSystem.Get_Dialogs();
        if(sa_localdata.Local_Index == 0)
        {
            //영어
            Stage_Title.text = EX_StoryData.Story_Branch[index].Story_Title_En;
            Stage_Num.text = EX_StoryData.Story_Branch[index].Story_Sub_En;
        }
        else if (sa_localdata.Local_Index == 1)
        {
            Stage_Title.text = EX_StoryData.Story_Branch[index].Story_Title_Kr  ;
            Stage_Num.text = EX_StoryData.Story_Branch[index].Story_Sub_Kr;
            //한글
        }
        else if (sa_localdata.Local_Index == 2)
        {
            Stage_Title.text = EX_StoryData.Story_Branch[index].Story_Title_Jp;
            Stage_Num.text = EX_StoryData.Story_Branch[index].Story_Sub_Jp;
            //일본어
        }
        //Stage_Num.text = "Story " + EX_StoryData.Story_Branch[index].Story_Title_Num;
    }
    // Start is called before the first frame update
    void Start()
    {
        MMSoundManagerSoundPlayEvent.Trigger(StoryBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
        SkipButton.onClick.AddListener(() => Click_Skip_Button());
        BackLogButton.onClick.AddListener(() => Click_BackLog_Button());
        OptionButton.onClick.AddListener(() => Click_Option_Button());
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

        EventTrigger backLogTrigger = BackLogButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry backlogEntry = new EventTrigger.Entry();
        backlogEntry.eventID = EventTriggerType.PointerEnter;
        backlogEntry.callback.AddListener((data) => { OnBackLogEnter(); });
        backLogTrigger.triggers.Add(backlogEntry);

        EventTrigger.Entry backlogExit = new EventTrigger.Entry();
        backlogExit.eventID = EventTriggerType.PointerExit;
        backlogExit.callback.AddListener((data) => { OnBackLogExit(); });
        backLogTrigger.triggers.Add(backlogExit);


        EventTrigger OptionTrigger = OptionButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry OptionEntry = new EventTrigger.Entry();
        OptionEntry.eventID = EventTriggerType.PointerEnter;
        OptionEntry.callback.AddListener((data) => { OnOptionEnter(); });
        OptionTrigger.triggers.Add(OptionEntry);

        EventTrigger.Entry OptionExit = new EventTrigger.Entry();
        OptionExit.eventID = EventTriggerType.PointerExit;
        OptionExit.callback.AddListener((data) => { OnOptionExit(); });
        OptionTrigger.triggers.Add(OptionExit);
    }
    private void OnSkipButtonEnter()
    {
        skipHit_Flag = true;
    }

    private void OnSkipButtonExit()
    {
        skipHit_Flag = false;
    }

    private void OnAutoToggleEnter()
    {
        toggleHit_Flag = true;
    }

    private void OnAutoToggleExit()
    {
        toggleHit_Flag = false;
    }

    private void OnBackLogEnter()
    {
        backHit_Flag = true;
    }

    private void OnBackLogExit()
    {
        backHit_Flag = false;
    }

    private void OnOptionEnter()
    {
        optionHit_Flag = true;
    }

    private void OnOptionExit()
    {
        optionHit_Flag = false;
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
        GameManager.Instance.Story_End();
    }

    private void Click_BackLog_Button()
    {
        BackLog_Flag = true;
        BackLog_Window.SetActive(true);
    }

    public void Click_BackLog_Back_Button()
    {
        BackLog_Window.SetActive(false);
        BackLog_Flag = false;

    }

    private void Click_Option_Button()
    {
        Option_Flag = true;
        Option_Window.SetActive(true);
    }

    public void Click_Option_Back_Button()
    {
        Option_Flag = false;
        Option_Window.SetActive(false);
    }

    public void Instantiate_BackLog(int num)
    {
        Vector2 pos = BackLog_Content.GetComponent<RectTransform>().sizeDelta;
        BackLog_Content.GetComponent<RectTransform>().sizeDelta = new Vector2 (pos.x, pos.y + 135);
        GameObject Back = Instantiate(BackLog_Object, BackLog_Content);
        Back.GetComponent<BackLog_object>().SetString(BackLog[num].name, BackLog[num].dialogue, BackLog[num].backLog_Color);
    }
}