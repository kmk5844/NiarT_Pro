using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SkillDirector : MonoBehaviour
{
    public Player player;
    public SA_PlayerData playerData;
    public GameDirector gameDirector;
    public UIDirector uiDirector;
    public Sprite[] Skill_Sprite;
    string skill_cooltime_string;
    string skill_during_string;
    float[] skill_cooltime;
    float[] skill_during;
    int PlayerNum;
    public float[] Item_Skill_CoolTime;

    public bool[] SkillFlag;
    // Start is called before the first frame update
    void Start()
    {
        PlayerNum = playerData.Player_Num;

        skill_cooltime_string = playerData.Skill_CoolTime;
        skill_during_string = playerData.Skill_During;
        SkillFlag = new bool[2];
        skill_cooltime = new float[2];
        skill_during = new float[2];
        Item_Skill_CoolTime = new float[2];
        string[] cool = skill_cooltime_string.Split(',');
        string[] dur = skill_during_string.Split(',');
        for (int i = 0; i < 2; i++)
        {
            SkillFlag[i] = false;
            skill_cooltime[i] = float.Parse(cool[i]);
            skill_during[i] = float.Parse(dur[i]);
            uiDirector.Equiped_Skill_Image[i].sprite = Skill_Sprite[(2 * PlayerNum) + i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameDirector.gameType == GameType.Playing ||
            gameDirector.gameType == GameType.Refreshing ||
            gameDirector.gameType == GameType.Boss ||
            gameDirector.gameType == GameType.Ending)
        {
            if (!gameDirector.SkillLockFlag)
            {
                if (Input.GetKeyDown(KeyCode.Q) && !SkillFlag[0])
                {
                    player.SkillSoundSFX();
                    StartCoroutine(Player_ClickSkill(0));
                }

                if (Input.GetKeyDown(KeyCode.E) && !SkillFlag[1])
                {
                    player.SkillSoundSFX();
                    StartCoroutine(Player_ClickSkill(1));
                }
            }
        }
    }

    IEnumerator Player_ClickSkill(int skill_num)
    {
        Player_Skill(skill_num);
        //CoolTime
        SkillFlag[skill_num] = true;
        float elapsedTime = 0f;
        while (elapsedTime < skill_cooltime[skill_num])
        {
            elapsedTime += Time.deltaTime;
            if(Item_Skill_CoolTime[skill_num] != 0)
            {
                elapsedTime += Item_Skill_CoolTime[skill_num];
                Item_Skill_CoolTime[skill_num] = 0;
            }
            uiDirector.Equiped_CoolTime_Skill_Image[skill_num].fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / skill_cooltime[skill_num]);
            yield return null;
        }
        SkillFlag[skill_num] = false;
    }

    void Player_Skill(int skill_num)
    {
        uiDirector.SkillCoolTime_Instantiate(skill_num, skill_during[skill_num]);
        if (PlayerNum == 0)
        {
            MariGold_Skill(skill_num);
        }
        else if (PlayerNum == 1)
        {
            Peyote_Skill(skill_num);
        }//그 외의 캐릭터 추가 시.
    }

    void MariGold_Skill(int num)
    {
        if(num == 0)
        {
            StartCoroutine(gameDirector.Train_MasSpeedChange(200, skill_during[num]));
            StartCoroutine(gameDirector.Item_Train_SpeedUp(skill_during[num], 1.5f));
        }else if (num == 1)
        {
            StartCoroutine(player.MariGold_Skill2(skill_during[num]));
        }
    }

    void Peyote_Skill(int num)
    {
        if (num == 0)
        {
            gameDirector.Item_Use_Train_Heal_HP(30);
        }
        else if (num == 1)
        {
            gameDirector.Item_Use_Train_Turret_All_SpeedUP(15, skill_during[num]);
        }
    }

    public void Item_Skill_CoolTime_Set(int Persent)
    {
        for (int i = 0; i < 2; i++)
        {
            Item_Skill_CoolTime[i] = skill_cooltime[i] * (1f - Persent / 100f);
        }
    }
}
