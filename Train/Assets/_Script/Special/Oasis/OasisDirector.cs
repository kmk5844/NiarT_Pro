using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class OasisDirector : MonoBehaviour
{
    [Header("���丮")]
    public DialogSystem Special_Story;
    public Dialog dialog;
    bool startFlag = false;

    [Header("������")]
    public SA_PlayerData playerData;
    public SA_Event eventData;

    [Header("Window")]
    public GameObject OasisWindow;
    public GameObject SelectStage;

    [Header("UI")]
    public Button[] oasisButton;
    [SerializeField]
    Image[] oasisButtonimages;
    [SerializeField]
    LocalizeStringEvent[] oasisButtonText;
    public Button NextButton;

    [SerializeField]
    List<int> RandNum = new List<int>();

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        OasisWindow.SetActive(false);
    }
    private void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        SettingInit();
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }

    private void StartEvent()
    {
        OasisWindow.SetActive(true);
        startFlag = true;
    }

    void SettingInit()
    {
        int count = 0;
        while (true)
        {
            int rand = Random.Range(0, 8);
            if (!RandNum.Contains(rand))
            {
                RandNum.Add(rand);
                count++;
            }

            if (count == 3)
            {
                break;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int cardNum = i;
            int x = RandNum[i];
            oasisButton[i].onClick.AddListener(() => oasissButton_Click(cardNum, x));
            oasisButtonText[i].StringReference.TableReference = "SpecialStage_St";


        }
        NextButton.onClick.AddListener(NextStation);
        NextButton.gameObject.SetActive(false);
    }

    /*
       case 0:
          //���� �������� ������� �÷��̾� �̵��ӵ� ����
      case 1:
          //���� �������� ������� �÷��̾� ���� ����
      case 2:
          //���� �������� ������� �÷��̾� ���ݷ� ����
      case 3:
          //���� �������� ������� �÷��̾� ���ݼӵ� ����
      case 4:
          //���� �������� ������� �÷��̾� ���ݷ�, ���ݼӵ� �ټ� ����
      case 5:
          //���� �������� ������� �÷��̾� ����, �̵��ӵ� �ټ� ����
      case 6:
          //�÷��̾� ȸ��
      case 7:
          //���� �������� ������� �÷��̾� ������ ��ȭ
      */

    void oasissButton_Click(int i, int x)
    {
        int randNum = x;
        if(i == 0)
        {
            OpenButton(0, randNum);
            oasisButton[1].gameObject.SetActive(false);
            oasisButton[2].gameObject.SetActive(false);
        }
        else if (i == 1) {
            OpenButton(1, randNum);

            oasisButton[0].gameObject.SetActive(false);
            oasisButton[2].gameObject.SetActive(false);
        }
        else if(i == 2)
        {
            OpenButton(2, randNum);
            oasisButton[0].gameObject.SetActive(false);
            oasisButton[1].gameObject.SetActive(false);
        }

        eventData.Choice_Oasis(randNum);
        NextButton.gameObject.SetActive(true);
    }

    void OpenButton(int index, int randNum)
    {
        oasisButton[index].enabled = false;
        oasisButton[index].interactable = false;
        oasisButtonimages[index].gameObject.SetActive(true);
        oasisButtonText[index].StringReference.TableEntryReference = "Oasis_Reward_" + randNum;
        //oasisButtonText[index].text = randNum + "�� ȿ��";
        oasisButtonText[index].gameObject.SetActive(true);
    }

    void NextStation()
    {
        SelectStage.SetActive(true);
    }
}