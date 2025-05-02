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

    public bool SpecialFlag;
    private GameObject SelectDirector_Object;
    SelectMission selectMission;
    SelectMission.MissionEvent_Flag eventFlag;

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
    private int storyNum;
    [SerializeField]
	private int branch;
	[SerializeField]
	private Story_DataTable EX_Story;
	[SerializeField]
	private SA_LocalData SA_Local;
    [SerializeField]
    Image CutScene_Sprite;
	[SerializeField]
	private	Speaker[] speakers;		// ��ȭ�� �����ϴ� ĳ���͵��� UI
	[SerializeField]
	private	List<DialogData> dialogs;               // ���� �б��� ��� ��� �迭

    [SerializeField]
	private	bool			isAutoStart = true;			// �ڵ� ���� ����
	private	bool			isFirst = true;				// ���� 1ȸ�� ȣ���ϱ� ���� ����
	private	int				currentDialogIndex = -1;	// ���� ��� ����
	private	int				currentSpeakerIndex = 0;	// ���� ���� �ϴ� ȭ��(Speaker)�� speakers �迭 ����
	private	float			typingSpeed = 0.05f;			// �ؽ�Ʈ Ÿ���� ȿ���� ��� �ӵ�
	private	bool			isTypingEffect = false;     // �ؽ�Ʈ Ÿ���� ȿ���� ���������

    private void Awake()
	{
        if(StoryDirector_Objcet != null)
        {
            SpecialFlag = false;
            storydirector = StoryDirector_Objcet.GetComponent<StoryDirector>();
		    delay = storydirector.delayTime;
        }
        else
        {
            SpecialFlag = true;
            SelectDirector_Object = GameObject.Find("SelectMission");
            selectMission = SelectDirector_Object.GetComponent<SelectMission>();
            eventFlag = selectMission.M_Event;
            delay = 1;
        }
        Check_Local();

/*        int index = 0;
        DialogData _data = new DialogData();
        for (int i = 0; i < EX_Story.Story.Count; i++)
        {
            if (EX_Story.Story[i].branch == branch && EX_Story.Story[i].Stage_Num == stageNum)
            {
                //0 -> �⺻(����) , 1 -> �ѱ� , 2 -> �Ϻ�
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
        if(storydirector != null)
        {
            SkipHit_Flag = storydirector.skipHit_Flag;
            AutoHit_Flag = storydirector.toggleHit_Flag;
            BackHit_Flag = storydirector.backHit_Flag;
            OptionHit_Flag = storydirector.optionHit_Flag;
            Auto_Flag = storydirector.Auto_Flag;
            Back_Flag = storydirector.BackLog_Flag;
            Option_Flag = storydirector.Option_Flag;
        }
    }

    public void Story_Init(GameObject StoryDirector_Object, int StageNum, int StoryNum, int Branch)
	{
        StoryDirector_Objcet = StoryDirector_Object;
		stageNum = StageNum;
        storyNum = StoryNum;
		branch = Branch;
    }

    private void Setup()
	{
        // ��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
        for ( int i = 0; i < speakers.Length; ++ i )
		{
			SetActiveObjects(speakers[i], false);
			// ĳ���� �̹����� ���̵��� ����
			speakers[i].player_able.gameObject.SetActive(true);
		}
	}

	public bool UpdateDialog()
	{
		// ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
		if ( isFirst == true )
		{
			// �ʱ�ȭ. ĳ���� �̹����� Ȱ��ȭ�ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
			Setup();
			// �ڵ� ���(isAutoStart=true)���� �����Ǿ� ������ ù ��° ��� ���
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

                    // Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
                    StopCoroutine("OnTypingText");
                    speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                    // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                    speakers[currentSpeakerIndex].objectArrow.SetActive(true);


                    if (!SpecialFlag)
                    {
                        storydirector.Instantiate_BackLog(currentDialogIndex);
                    }

                    return false;
                }

                // ��簡 �������� ��� ���� ��� ����
                if (dialogs.Count > currentDialogIndex + 1)
                {
                    SetNextDialog();
                }
                // ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
                else
                {
                    // ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
                    for (int i = 0; i < speakers.Length; ++i)
                    {
                        SetActiveObjects(speakers[i], false);
                        // SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
                        speakers[i].player_able.gameObject.SetActive(false);
                    }

                    return true;
                }
            }

            if (Auto_Flag && !isTypingEffect)
            {
                // ��簡 �������� ��� ���� ��� ����
                if (dialogs.Count > currentDialogIndex + 1)
                {
                    SetNextDialog();
                }
                // ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
                else
                {
                    // ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
                    for (int i = 0; i < speakers.Length; ++i)
                    {
                        SetActiveObjects(speakers[i], false);
                        // SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
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
		// ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], false);
		// ���� ��縦 �����ϵ��� 
		currentDialogIndex ++;
        // ���� ȭ�� ���� ����
		currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        // ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], true);

        //Ŀ���� �۵�
        CheckCustom();

        // ���� ȭ�� �̸� �ؽ�Ʈ ����
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		// ���� ȭ���� ��� �ؽ�Ʈ ����
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");
	}

	private void SetActiveObjects(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);

		// ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� false
		speaker.objectArrow.SetActive(false);

		// ĳ���� ���� �� ����
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

		// �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
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

		// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
		speakers[currentSpeakerIndex].objectArrow.SetActive(true);

        if (!SpecialFlag)
        {
            storydirector.Instantiate_BackLog(currentDialogIndex);
        }
    }

    public void Get_Dialogs()
	{
        if (!SpecialFlag)
        {
            storydirector.BackLog = dialogs;
        } 
    }

	public void Check_Local()
	{
        int index = 0;
        DialogData _data = new DialogData();

        if (!SpecialFlag)
        {
            for (int i = 0; i < EX_Story.Story.Count; i++)
            {
                if (EX_Story.Story[i].branch == branch && EX_Story.Story[i].Stage_Num == stageNum && EX_Story.Story[i].Story_Num == storyNum)
                {
                    //0 -> �⺻(����) , 1 -> �ѱ� , 2 -> �Ϻ�
                    _data.speakerIndex = EX_Story.Story[i].Speaker_Index;
                    if (SA_Local.Local_Index == 1)
                    {
                        _data.name = EX_Story.Story[i].ko_name;
                        _data.dialogue = EX_Story.Story[i].ko_dialog.Replace("\\n", "\n");
                    }
                    else if (SA_Local.Local_Index == 0)
                    {
                        _data.name = EX_Story.Story[i].en_name;
                        _data.dialogue = EX_Story.Story[i].en_dialog.Replace("\\n", "\n");
                    }
                    else if (SA_Local.Local_Index == 2)
                    {
                        _data.name = EX_Story.Story[i].jp_name;
                        _data.dialogue = EX_Story.Story[i].jp_dialog.Replace("\\n", "\n");
                    }
                    _data.backLog_Color = EX_Story.Story[i].BackLog_Color;
                    _data.Sound = EX_Story.Story[i].Sound;
                    _data.Player_Animation = EX_Story.Story[i].Player_Animation;
                    _data.ChatBox_Animation = EX_Story.Story[i].ChatBox_Animation;
                    _data.CutScene_Sprite = EX_Story.Story[i].CutScene_Sprite;
                    _data.etc = EX_Story.Story[i].Etc;
                    dialogs.Add(_data);
                    index++;
                }
            }
        }
        else
        {
            for(int i = 0; i < EX_Story.Special_Story.Count; i++)
            {
                if (EX_Story.Special_Story[i].branch == branch && EX_Story.Special_Story[i].Specail_Num == stageNum)
                {
                    //0 -> �⺻(����) , 1 -> �ѱ� , 2 -> �Ϻ�
                    _data.speakerIndex = EX_Story.Special_Story[i].Speaker_Index;
                    if (SA_Local.Local_Index == 1)
                    {
                        _data.name = EX_Story.Special_Story[i].ko_name;
                        _data.dialogue = EX_Story.Special_Story[i].ko_dialog.Replace("\\n", "\n");
                    }
                    else if (SA_Local.Local_Index == 0)
                    {
                        _data.name = EX_Story.Special_Story[i].en_name;
                        _data.dialogue = EX_Story.Special_Story[i].en_dialog.Replace("\\n", "\n");
                    }
                    else if (SA_Local.Local_Index == 2)
                    {
                        _data.name = EX_Story.Special_Story[i].jp_name;
                        _data.dialogue = EX_Story.Special_Story[i].jp_dialog.Replace("\\n", "\n");
                    }
                    _data.backLog_Color = EX_Story.Special_Story[i].BackLog_Color;
                    _data.Sound = EX_Story.Special_Story[i].Sound;
                    _data.Player_Sprite = EX_Story.Special_Story[i].Player_Sprite;
                    _data.Player_Animation = EX_Story.Special_Story[i].Player_Animation;
                    _data.ChatBox_Animation = EX_Story.Special_Story[i].ChatBox_Animation;
                    _data.etc = EX_Story.Special_Story[i].Etc;
                    dialogs.Add(_data);
                    index++;
                }
            }
        }

    }

    private void CheckCustom()
    {
        if (CutScene_Sprite.gameObject.activeSelf)
        {
            CutScene_Sprite.gameObject.SetActive(false);
        }

        //�ʱ�ȭ ��
        if (dialogs[currentDialogIndex].Sound != null)
        {
            AudioClip sound = Resources.Load<AudioClip>("Story/Sound/" + dialogs[currentDialogIndex].Sound);
            MMSoundManagerSoundPlayEvent.Trigger(sound, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }

        if (dialogs[currentDialogIndex].Player_Sprite != null)
        {
            Sprite ChangeSprite = Resources.Load<Sprite>("Story/Sprite/" + dialogs[currentDialogIndex].Player_Sprite);
            speakers[currentSpeakerIndex].player_able.sprite = ChangeSprite;
        }

        if (dialogs[currentDialogIndex].Player_Animation != "")
        {
            Animator ani = speakers[currentSpeakerIndex].player_able.GetComponent<Animator>();
            ani.SetTrigger(dialogs[currentDialogIndex].Player_Animation);
        }

        if (dialogs[currentDialogIndex].CutScene_Sprite != "")
        {
            Sprite ChangeSprite = Resources.Load<Sprite>("Story/Sprite/" + dialogs[currentDialogIndex].CutScene_Sprite);
            CutScene_Sprite.sprite = ChangeSprite;
            CutScene_Sprite.gameObject.SetActive(true);
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
	public	Image			player_able;		// ĳ���� �̹��� (û��/ȭ�� ���İ� ����)
	public	Image			imageDialog;		// ��ȭâ Image UI
	public	TextMeshProUGUI	textName;			// ���� ������� ĳ���� �̸� ��� Text UI
	public	TextMeshProUGUI	textDialogue;		// ���� ��� ��� Text UI
	public	GameObject		objectArrow;        // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� ������Ʈ
	public bool alpha_zero;
}

[System.Serializable]
public struct DialogData
{
	public	int		speakerIndex;	// �̸��� ��縦 ����� ���� DialogSystem�� speakers �迭 ����
	public	string	name;			// ĳ���� �̸�
	[TextArea(3, 5)]
	public	string dialogue;		// ���
    [HideInInspector]
    public string backLog_Color;
    public string Sound;
    public string Player_Sprite;
    public string Player_Animation;
    public string ChatBox_Animation;
    public string CutScene_Sprite;
    public string etc;
}

