using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private GameObject StroyDirector_Objcet;
	StoryDirector stroydirector;

    private bool SkipHit_Flag;
    private bool AutoHit_Flag;
	private bool Auto_Flag;
	private float delay;
	Time time;
    private bool isDelaying = false;

    [SerializeField]
	private int branch;
	[SerializeField]
	private Test_Local test;
	[SerializeField]
	private SA_LocalData SA_Local;
	[SerializeField]
	private	Speaker[] speakers;		// ��ȭ�� �����ϴ� ĳ���͵��� UI
	[SerializeField]
	private	List<DialogData> dialogs;                   // ���� �б��� ��� ��� �迭

    [SerializeField]
	private	bool			isAutoStart = true;			// �ڵ� ���� ����
	private	bool			isFirst = true;				// ���� 1ȸ�� ȣ���ϱ� ���� ����
	private	int				currentDialogIndex = -1;	// ���� ��� ����
	private	int				currentSpeakerIndex = 0;	// ���� ���� �ϴ� ȭ��(Speaker)�� speakers �迭 ����
	private	float			typingSpeed = 0.1f;			// �ؽ�Ʈ Ÿ���� ȿ���� ��� �ӵ�
	private	bool			isTypingEffect = false;     // �ؽ�Ʈ Ÿ���� ȿ���� ���������

	private void Awake()
	{
        stroydirector = StroyDirector_Objcet.GetComponent<StoryDirector>();
		delay = stroydirector.delayTime;
        int index = 0;
        DialogData data = new DialogData();
        for (int i = 0; i < test.Test.Count; i++)
		{
			if (test.Test[i].branch == branch)
			{
				data.speakerIndex = test.Test[i].Speaker_Index;
                if (SA_Local.Local_Index == 0)
                {
                    data.name = test.Test[i].ko_name;
                    data.dialogue = test.Test[i].ko_dialog;
                }
                else if (SA_Local.Local_Index == 1)
                {
                    data.name = test.Test[i].en_name;
                    data.dialogue = test.Test[i].en_dialog;
                }
                dialogs.Add(data);
                index++;
			}
		}
		Setup();
	}

    private void Update()
    {
		SkipHit_Flag = stroydirector.SkipHit_Flag;
        AutoHit_Flag = stroydirector.AutoHit_Flag;
        Auto_Flag = stroydirector.Auto_Flag;

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

        if (Input.GetMouseButtonDown(0) && !SkipHit_Flag && !AutoHit_Flag)
        {
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                // Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);

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

		if(Auto_Flag && !isTypingEffect)
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
		color.a = visible == true ? 1 : 0.2f;
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

		isTypingEffect = false;

		// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
		speakers[currentSpeakerIndex].objectArrow.SetActive(true);
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
}

[System.Serializable]
public struct DialogData
{
	public	int		speakerIndex;	// �̸��� ��縦 ����� ���� DialogSystem�� speakers �迭 ����
	public	string	name;			// ĳ���� �̸�
	[TextArea(3, 5)]
	public	string	dialogue;		// ���
}

