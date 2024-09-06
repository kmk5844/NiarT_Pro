using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using System;

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
        storydirector = StoryDirector_Objcet.GetComponent<StoryDirector>();
		delay = storydirector.delayTime;
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
            if (Input.GetMouseButtonDown(0) && !SkipHit_Flag && !AutoHit_Flag && !BackHit_Flag && !OptionHit_Flag)
            {

                if (isTypingEffect == true)
                {
                    isTypingEffect = false;

                    // Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
                    StopCoroutine("OnTypingText");
                    speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                    // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                    speakers[currentSpeakerIndex].objectArrow.SetActive(true);

                    storydirector.Instantiate_BackLog(currentDialogIndex);

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
		if(speaker.alpha_zero == true)
		{
            color.a = visible == true ? 1 : 0f;
        }
        else
		{
            color.a = visible == true ? 1 : 0.2f;
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
                _data.backLog_Color = EX_Story.Story[i].BackLog_Color;
                dialogs.Add(_data);
                index++;
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
	public	string	dialogue;		// ���
    [HideInInspector]
    public string backLog_Color;
}

