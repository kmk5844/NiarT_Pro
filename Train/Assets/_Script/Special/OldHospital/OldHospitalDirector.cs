using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldHospitalDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject OldHospitalWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public Game_DataTable gamedatatable;
    public SA_PlayerData playerData;
    int totalcoin;
    int[] Parsent_Heal = { 50, 75, 100 };
    int[] Parsent_Coin = { 10, 30, 50 };
    int[] healHP;
    int[] giveCoin;
    int playerHP;
    int maxHP;
    int damageHP;
    bool startFlag;

    [Header("UI")]
    public Button[] button;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        OldHospitalWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            //Debug.Log("작동");
            QualitySettings.vSyncCount = 0;
        }

        playerHP = ES3.Load<int>("Player_Curret_HP");
        int Level_HP = playerData.Level_Player_HP;
        maxHP = gamedatatable.Information_Player[playerData.Player_Num].Player_HP;
        maxHP = maxHP + (((maxHP * Level_HP) * 10) / 100);
        damageHP = maxHP - playerHP;

        totalcoin = playerData.Coin;
        giveCoin = new int[3];
        healHP = new int[3];
        for(int i = 0; i < 3; i++)
        {
            giveCoin[i] = totalcoin * Parsent_Coin[i] / 100;
            healHP[i] = damageHP * Parsent_Heal[i] / 100;
        }
    }

    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }

    private void StartEvent()
    {
        OldHospitalWindow.SetActive(true);
        startFlag = true;
    }

    public void ClickRewardButton(int i)
    {
        int num = i;
        Heal(num);
        playerData.SA_Buy_Coin(giveCoin[num]);
        CheckWindow.SetActive(true);
        for(int j = 0; j<button.Length; j++)
        {
            button[j].interactable = false;
        }
    }

    void Heal(int i)
    {
        playerHP = Mathf.Min(playerHP + healHP[i], maxHP);
        ES3.Save<int>("Player_Curret_HP", playerHP);
    }

    public void OldHospitalEnd()
    {
        SelectStage.SetActive(true);
    }
}
