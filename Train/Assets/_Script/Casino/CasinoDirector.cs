using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CasinoDirector : MonoBehaviour
{
    [Header("데이터")]
    public SA_PlayerData playerData;
    public Sprite[] foodImage;
    int money;
    int[] bat = { 2, 4, 8, 16, 32, 64, 128, 256};
    int bat_index = 0;
    bool[] casinoflag = { false, false, false };
    bool casino_end;

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


    private void Start()
    {
        money = 1000;
        UpButton.onClick.AddListener(() => Up_BatMoney());
        DownButton.onClick.AddListener(() => Down_BatMoney());
        BattingButton.onClick.AddListener(() => Bat());
        GoButton.onClick.AddListener(() => Go());  
        StopButton.onClick.AddListener(() => Stop());
        RetryButton.onClick.AddListener(() => retry());
        EndButton.onClick.AddListener(() => casinoEnd());
        Setting_Init();
        CheckText();
    }

    void Setting_Init()
    {
        casino_end = true;
        for (int i = 0; i < 3; i++) { 
            casinoflag[i] = false;
            casinoButton[i].interactable = false;
        }
        GoButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        DownButton.interactable = false;
    }

    private void Update()
    {
        if (!casino_end)
        {
            for (int i = 0; i < 3; i++)
            {
                if (casinoflag[i] != true)
                {
                    int rnd = Random.Range(0, 11);
                    casinoImage[i].sprite = foodImage[rnd];
                }
            }
        }
    }

    void Up_BatMoney()
    {
        money += 1000;
        CheckButton();
        CheckText();
    }

    void Down_BatMoney()
    {
        money -= 1000;
        CheckButton();
        CheckText();
    }

    void CheckButton()
    {
        if (playerData.Coin < money + 1000)
        {
            UpButton.interactable = false;
        }
        else
        {
            UpButton.interactable = true;
        }

        if (money - 1000 < 1000)
        {
            DownButton.interactable = false;
        }
        else
        {
            DownButton.interactable = true;
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
        casino_end = false;
        for(int i = 0; i < 3; i++)
        {
            casinoButton[i].interactable = true;
        }
        BattingButton.gameObject.SetActive(false);
        UpButton.gameObject.SetActive(false);
        DownButton.gameObject.SetActive(false);
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
        UpButton.gameObject.SetActive(true);
        DownButton.gameObject.SetActive(true);
        BattingButton.gameObject.SetActive(true);
        money = 1000;
        bat_index = 0;
        CheckText();
        CheckButton();
    }

    void retry()
    {
        UpButton.gameObject.SetActive(true);
        DownButton.gameObject.SetActive(true);
        BattingButton.gameObject.SetActive(true);
        money = 1000;
        bat_index = 0;
        CheckText();
        CheckButton();
    }

    void casinoEnd()
    {
        Debug.Log("End");
    }

    void Bat_Sucess()
    {
        bat_index++;
        CheckText();
        GoButton.gameObject.SetActive(true);
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
}
