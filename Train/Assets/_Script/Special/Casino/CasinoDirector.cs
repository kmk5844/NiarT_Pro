using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CasinoDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject CasinoWindow;
    public GameObject SelectStage;

    [Header("데이터")]
    public SA_PlayerData playerData;
    public Sprite[] foodImage;
    int money;
    int[] bat = { 1, 2, 4, 8, 16, 32, 64, 128, 256};
    int bat_index = 0;
    bool[] casinoflag = { false, false, false };
    bool casino_end;
    int batMoney = 0;
    bool moneyFlag;
    bool startFlag;

    [Header("배팅")]
    public Button UpButton;
    public Button DownButton;
    public Button BattingButton;
    public Button GoButton;
    public Button StopButton;
    public Button RetryButton;
    public Button EndButton;
    public TextMeshProUGUI Bat_GoldText;
    public TextMeshProUGUI Bat_CountText;

    [Header("카지노")]
    public Image[] casinoImage;
    public Button[] casinoButton;
    public TextMeshProUGUI playerGoldText;

    [Header("---------Sound---------")]
    public AudioClip CasinoBGM;
    public AudioClip MissionSelectBGM;

    private void Awake()
    {
        Special_Story.Story_Init(null, 2, 0, 0);
        CasinoWindow.SetActive(false);
    }


    private void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        moneyCheck();
        UpButton.onClick.AddListener(() => Up_BatMoney());
        DownButton.onClick.AddListener(() => Down_BatMoney());
        BattingButton.onClick.AddListener(() => Bat());
        GoButton.onClick.AddListener(() => Go());  
        StopButton.onClick.AddListener(() => Stop());
        RetryButton.onClick.AddListener(() => retry());
        EndButton.onClick.AddListener(() => casinoEnd());
        Setting_Init();
        CheckButton();
        CheckText();
        playerGoldText.text = playerData.Coin + " G";
        MMSoundManagerSoundPlayEvent.Trigger(CasinoBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID : 10);
    }

    void Setting_Init()
    {
        casino_end = true;
        for (int i = 0; i < 3; i++) { 
            casinoflag[i] = false;
            casinoButton[i].interactable = false;
        }

        if (moneyFlag)
        {
            BattingButton.interactable = false;
            EndButton.gameObject.SetActive(true);
        }

        GoButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        DownButton.interactable = false;
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }

        if (startFlag)
        {
            if (!casino_end)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (casinoflag[i] != true)
                    {
                        int rnd = Random.Range(0, 3);
                        casinoImage[i].sprite = foodImage[rnd];
                    }
                }
            }
        }
    }
    private void StartEvent()
    {
        CasinoWindow.SetActive(true);
        startFlag = true;
    }

    void Up_BatMoney()
    {
        money += batMoney;
        CheckButton();
        CheckText();
    }

    void Down_BatMoney()
    {
        money -= batMoney;
        CheckButton();
        CheckText();
    }
    void moneyCheck()
    {
        moneyFlag = false;
        if (playerData.Coin < 1)
        {
            moneyFlag = true;
            batMoney = 0;
            money = 0;
        }
        else if (playerData.Coin < 10)
        {
            batMoney = 1;
            money = 1;
        }
        else if (playerData.Coin < 100)
        {
            batMoney = 10;
            money = 10;
        }
        else if (playerData.Coin < 1000)
        {
            batMoney = 100;
            money = 100;
        }
        else
        {
            batMoney = 1000;
            money = 1000;
        }
    }
    void CheckButton()
    {
        if (!moneyFlag)
        {
            if (playerData.Coin < money + batMoney)
            {
                UpButton.interactable = false;
            }
            else
            {
                UpButton.interactable = true;
            }

            if (money - batMoney < batMoney)
            {
                DownButton.interactable = false;
            }
            else
            {
                DownButton.interactable = true;
            }
        }
        else
        {
            UpButton.interactable = false;
            DownButton.interactable = false;
        }

    }

    void CheckText()
    {
        Bat_GoldText.text = money.ToString() + 'G';
        Bat_CountText.text = "x " + bat[bat_index];
    }

    void Bat()
    {
        playerData.SA_Buy_Coin(money);
        playerGoldText.text = playerData.Coin + " G";
        casino_end = false;
        for(int i = 0; i < 3; i++)
        {
            casinoflag[i] = false;
            casinoButton[i].interactable = true;
        }
        BattingButton.gameObject.SetActive(false);
        UpButton.gameObject.SetActive(false);
        DownButton.gameObject.SetActive(false);
        EndButton.gameObject.SetActive(false);
    }

    void Go()
    {
        casino_end = false;
        for(int i = 0; i < 3; i++)
        {
            casinoflag[i] = false;
            casinoButton[i].interactable = true;
        }
    }
    void Stop()
    {
        playerData.SA_Get_Coin(money * bat[bat_index]);
        playerGoldText.text = playerData.Coin + " G";
        UpButton.gameObject.SetActive(true);
        DownButton.gameObject.SetActive(true);
        BattingButton.gameObject.SetActive(true);
        GoButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        EndButton.gameObject.SetActive(true);
        moneyCheck();
        bat_index = 0;
        CheckText();
        CheckButton();
        moneyCheck();
    }

    void retry()
    {
        UpButton.gameObject.SetActive(true);
        DownButton.gameObject.SetActive(true);
        BattingButton.gameObject.SetActive(true);
        RetryButton.gameObject.SetActive(false);
        EndButton.gameObject.SetActive(false);
        moneyCheck();
        bat_index = 0;
        CheckText();
        CheckButton();
        Setting_Init();
    }

    void casinoEnd()
    {
        Click_NextButton();
    }

    void Bat_Sucess()
    {
        if(bat_index < bat.Length-1)
        {
            bat_index++;
            GoButton.gameObject.SetActive(true);
        }
        else
        {
            GoButton.gameObject.SetActive(false);
        }
        CheckText();
        StopButton.gameObject.SetActive(true);
        RetryButton.gameObject.SetActive(false);
        EndButton.gameObject.SetActive(false);
    }

    void Bat_Fail()
    {
        GoButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        RetryButton.gameObject.SetActive(true);
        EndButton.gameObject.SetActive(true);
    }

    public void CasinoButton_Click(int i)
    {
        casinoflag[i] = true;
        casinoButton[i].interactable = false;
        CheckFlag();
    }

    void CheckFlag()
    {
        if(casinoflag[0] && casinoflag[1] && casinoflag[2])
        {
            casino_end = true;
            if (casinoImage[0].sprite.name == casinoImage[1].sprite.name &&
                casinoImage[0].sprite.name == casinoImage[2].sprite.name &&
                casinoImage[1].sprite.name == casinoImage[1].sprite.name)
            {
                Bat_Sucess();
            }
            else
            {
                Bat_Fail();
            }
        }
    }

    public void Click_NextButton()
    {
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, 10);
        MMSoundManagerSoundPlayEvent.Trigger(MissionSelectBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: 20);
        SelectStage.SetActive(true);
    }
}
