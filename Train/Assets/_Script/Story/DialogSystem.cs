using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using MoreMountains.Tools;

public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private GameObject StoryDirector_Objcet;
	StoryDirector storydirector;

    private bool SkipHit_Flag;
    private bool AutoHit_Flag;
    private bool BackHit_Flag;
    private bool OptionHit_Flag;
	private bool Auto_Flag;
    private bool Back_Flag;
    private bool Option_Flag;
	private float delay;

	[SerializeField]
	private int stageNum;
    [SerializeField]
	private int branch;
	[SerializeField]
	private Story_DataTable EX_Story;
	[SerializeField]
	private SA_LocalData SA_Local;
	[SerializeField]
	private	Speaker[] speakers;		// 대화에 참여하는 캐릭터들의 UI
	[SerializeField]
	private	List<DialogData> dialogs;               // 현재 분기의 대사 목록 배열

    [SerializeField]
	private	bool			isAutoStart = true;			// 자동 시작 여부
	private	bool			isFirst = true;				// 최초 1회만 호출하기 위한 변수
	private	int				currentDialogIndex = -1;	// 현재 대사 순번
	private	int				currentSpeakerIndex = 0;	// 현재 말을 하는 화자(Speaker)의 speakers 배열 순번
	private	float			typingSpeed = 0.05f;			// 텍스트 타이핑 효과의 재생 속도
	private	bool			isTypingEffect = false;     // 텍스트 타이핑 효과를 재생중인지

    private void Awake()
	{
        storydirector = StoryDirector_Objcet.GetComponent<StoryDirector>();
		delay = storydirector.delayTime;
        Check_Local();
/*        int index = 0;
        DialogData _data = new DialogData();
        for (int i = 0; i < EX_Story.Story.Count; i++)
        {
            if (EX_Story.Story[i].branch == branch && EX_Story.Story[i].Stage_Num == stageNum)
            {
                //0 -> 기본(영어) , 1 -> 한글 , 2 -> 일본
                _data.speakerIndex = EX_Story.Story[i].Speaker_Index;
                if (SA_Local.Local_Index == 1)
                {
                    _data.name = EX_Story.Story[i].ko_name;
                    _data.dialogue = EX_Story.Story[i].ko_dialog;
                }
                else if (SA_Local.Local_Index == 0)
                {
                    _data.name = EX_Story.Story[i].en_name;
                    _data.dialogue = EX_Story.Story[i].en_dialog;
                }
                else if (SA_Local.Local_Index == 2)
                {
                    _data.name = EX_Story.Story[i].jp_name;
                    _data.dialogue = EX_Story.Story[i].jp_dialog;
                }
                dialogs.Add(_data);
                index++;
            }
        }*/
        Setup();
	}

    private void Update()
    {
        SkipHit_Flag = storydirector.skipHit_Flag;
        AutoHit_Flag = storydirector.toggleHit_Flag;
        BackHit_Flag = storydirector.backHit_Flag;
        OptionHit_Flag = storydirector.optionHit_Flag;
        Auto_Flag = storydirector.Auto_Flag;
        Back_Flag = storydirector.BackLog_Flag;
        Option_Flag = storydirector.Option_Flag;
    }

    public void Story_Init(GameObject StoryDirector_Object, int StageNum, int Branch)
	{
        StoryDirector_Objcet = StoryDirector_Object;
		stageNum = StageNum;
		branch = Branch;
    }

    private void Setup()
	{
        // 모든 대화 관련 게임오브젝트 비활성화
        for ( int i = 0; i < speakers.Length; ++ i )
		{
			SetActiveObjects(speakers[i], false);
			// 캐릭터 이미지는 보이도록 설정
			speakers[i].player_able.gameObject.SetActive(true);
		}
	}

	public bool UpdateDialog()
	{
		// 대사 분기가 시작될 때 1회만 호출
		if ( isFirst == true )
		{
			// 초기화. 캐릭터 이미지는 활성화하고, 대사 관련 UI는 모두 비활성화
			Setup();
			// 자동 재생(isAutoStart=true)으로 설정되어 있으면 첫 번째 대사 재생
			if ( isAutoStart ) SetNextDialog();

			isFirst = false;
		}

        if (!Back_Flag && !Option_Flag)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !SkipHit_Flag && !AutoHit_Flag && !BackHit_Flag && !OptionHit_Flag)
            {
                if (isTypingEffect == true)
                {
                    isTypingEffect = false;

                    // 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
                    StopCoroutine("OnTypingText");
                    speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                    // 대사가 완료되었을 때 출력되는 커서 활성화
                    speakers[currentSpeakerIndex].objectArrow.SetActive(true);

                    storydirector.Instantiate_BackLog(currentDialogIndex);

                    return false;
                }

                // 대사가 남아있을 경우 다음 대사 진행
                if (dialogs.Count > currentDialogIndex + 1)
                {
                    SetNextDialog();
                }
                // 대사가 더 이상 없을 경우 모든 오브젝트를 비활성화하고 true 반환
                else
                {
                    // 현재 대화에 참여했던 모든 캐릭터, 대화 관련 UI를 보이지 않게 비활성화
                    for (int i = 0; i < speakers.Length; ++i)
                    {
                        SetActiveObjects(speakers[i], false);
                        // SetActiveObjects()에 캐릭터 이미지를 보이지 않게 하는 부분이 없기 때문에 별도로 호출
                        speakers[i].player_able.gameObject.SetActive(false);
                    }

                    return true;
                }
            }

            if (Auto_Flag && !isTypingEffect)
            {
                // 대사가 남아있을 경우 다음 대사 진행
                if (dialogs.Count > currentDialogIndex + 1)
                {
                    SetNextDialog();
                }
                // 대사가 더 이상 없을 경우 모든 오브젝트를 비활성화하고 true 반환
                else
                {
                    // 현재 대화에 참여했던 모든 캐릭터, 대화 관련 UI를 보이지 않게 비활성화
                    for (int i = 0; i < speakers.Length; ++i)
                    {
                        SetActiveObjects(speakers[i], false);
                        // SetActiveObjects()에 캐릭터 이미지를 보이지 않게 하는 부분이 없기 때문에 별도로 호출
                        speakers[i].player_able.gameObject.SetActive(false);
                    }
                    return true;
                }
            }
        }

        return false;
	}

    private void SetNextDialog()
	{
		// 이전 화자의 대화 관련 오브젝트 비활성화
		SetActiveObjects(speakers[currentSpeakerIndex], false);
		// 다음 대사를 진행하도록 
		currentDialogIndex ++;
		// 현재 화자 순번 설정
		currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        // 현재 화자의 대화 관련 오브젝트 활성화
		SetActiveObjects(speakers[currentSpeakerIndex], true);

        //커스텀 작동
        CheckCustom();

        // 현재 화자 이름 텍스트 설정
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		// 현재 화자의 대사 텍스트 설정
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");
	}

	private void SetActiveObjects(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);

		// 화살표는 대사가 종료되었을 때만 활성화하기 때문에 항상 false
		speaker.objectArrow.SetActive(false);

		// 캐릭터 알파 값 변경
		Color color = speaker.player_able.color;

        Color UnvisibleColor = new Color(100f / 250f, 100f / 250f, 100f / 250f);

		if(speaker.alpha_zero == true)
		{
            color = visible == true ? Color.white : new Color(0f, 0f, 0f, 0f);
        }
        else
		{
            color = visible == true ? Color.white : UnvisibleColor;
        }
        speaker.player_able.color = color;
	}

	private IEnumerator OnTypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// 텍스트를 한글자씩 타이핑치듯 재생
		while ( index < dialogs[currentDialogIndex].dialogue.Length + 1)
		{
			speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);

			index ++;
		
			yield return new WaitForSeconds(typingSpeed);
		}

        if (Auto_Flag)
        {
            yield return new WaitForSeconds(delay);
        }

        isTypingEffect = false;

		// 대사가 완료되었을 때 출력되는 커서 활성화
		speakers[currentSpeakerIndex].objectArrow.SetActive(true);

        storydirector.Instantiate_BackLog(currentDialogIndex);

    }

    public void Get_Dialogs()
	{
		storydirector.BackLog = dialogs;
	}

	public void Check_Local()
	{
        int index = 0;
        DialogData _data = new DialogData();
        for (int i = 0; i < EX_Story.Story.Count; i++)
        {
            if (EX_Story.Story[i].branch == branch && EX_Story.Story[i].Stage_Num == stageNum)
            {
                //0 -> 기본(영어) , 1 -> 한글 , 2 -> 일본
                _data.speakerIndex = EX_Story.Story[i].Speaker_Index;
                if (SA_Local.Local_Index == 1)
                {
                    _data.name = EX_Story.Story[i].ko_name;
                    _data.dialogue = EX_Story.Story[i].ko_dialog;
                }
                else if (SA_Local.Local_Index == 0)
                {
                    _data.name = EX_Story.Story[i].en_name;
                    _data.dialogue = EX_Story.Story[i].en_dialog;
                }
                else if (SA_Local.Local_Index == 2)
                {
                    _data.name = EX_Story.Story[i].jp_name;
                    _data.dialogue = EX_Story.Story[i].jp_dialog;
                }
                _data.backLog_Color = EX_Story.Story[i].BackLog_Color;
                _data.Sound = EX_Story.Story[i].Sound;
                _data.Player_Animation = EX_Story.Story[i].Player_Animation;
                _data.ChatBox_Animation = EX_Story.Story[i].ChatBox_Animation;
                _data.Sprite = EX_Story.Story[i].Sprite;
                _data.etc = EX_Story.Story[i].Etc;
                dialogs.Add(_data);
                index++;
            }
        }
    }

    private void CheckCustom()
    {
        if(dialogs[currentDialogIndex].Sound != "")
        {
            AudioClip sound = Resources.Load<AudioClip>("Story/Sound/" + dialogs[currentDialogIndex].Sound);
            MMSoundManagerSoundPlayEvent.Trigger(sound, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }

        if (dialogs[currentDialogIndex].Sprite != "")
        {
            Sprite ChangeSprite = Resources.Load<Sprite>("Story/Sprite/" + dialogs[currentDialogIndex].Sprite);
            speakers[currentSpeakerIndex].player_able.sprite = ChangeSprite;
        }

        if (dialogs[currentDialogIndex].Player_Animation != "")
        {
            Animator ani = speakers[currentSpeakerIndex].player_able.GetComponent<Animator>();
            ani.SetTrigger(dialogs[currentDialogIndex].Player_Animation);
        }

        if (dialogs[currentDialogIndex].ChatBox_Animation != "")
        {
            Animator ani = speakers[currentSpeakerIndex].imageDialog.GetComponent<Animator>();

            if (ani != null && ani.isActiveAndEnabled)
            {
                ani.SetTrigger(dialogs[currentDialogIndex].ChatBox_Animation);
            }
            else
            {
                Debug.LogError("Animator is either null or not enabled");
            }
        }
    }
}

[System.Serializable]
public struct Speaker
{
	public	Image			player_able;		// 캐릭터 이미지 (청자/화자 알파값 제어)
	public	Image			imageDialog;		// 대화창 Image UI
	public	TextMeshProUGUI	textName;			// 현재 대사중인 캐릭터 이름 출력 Text UI
	public	TextMeshProUGUI	textDialogue;		// 현재 대사 출력 Text UI
	public	GameObject		objectArrow;        // 대사가 완료되었을 때 출력되는 커서 오브젝트
	public bool alpha_zero;
}

[System.Serializable]
public struct DialogData
{
	public	int		speakerIndex;	// 이름과 대사를 출력할 현재 DialogSystem의 speakers 배열 순번
	public	string	name;			// 캐릭터 이름
	[TextArea(3, 5)]
	public	string dialogue;		// 대사
    [HideInInspector]
    public string backLog_Color;
    public string Sound;
    public string Sprite;
    public string Player_Animation;
    public string ChatBox_Animation;
    public string etc;
}

