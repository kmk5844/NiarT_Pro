using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class PlayerLogDirector : MonoBehaviour
{
    public Transform logTransform;
    public static Transform LogTransform { get; private set; }
    public PlayerLogPrefab playerLogPrefab;
    public static PlayerLogPrefab PlayerLogPrefab { get; private set; }

    private void Awake()
    {
        LogTransform = logTransform;
        PlayerLogPrefab = playerLogPrefab;
    }

    public static void MercenaryKill(int mercenaryNum)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("ExcelData_Table_St", "Mercenary_Name_" + mercenaryNum);
        SetPrefab(PlayerLogPrefab.LogType.MercenaryKill, str);
    }

    public static void MercenaryDie(int mercenaryNum)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("ExcelData_Table_St", "Mercenary_Name_" + mercenaryNum);
        SetPrefab(PlayerLogPrefab.LogType.MercenaryDie, str);
    }

    public static void ItemUse(int Item_num)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("ItemData_Table_St", "Item_Name_" + Item_num);
        SetPrefab(PlayerLogPrefab.LogType.ItemUse, str);
    }

    public static void ItemGet(int Item_Num)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("ItemData_Table_St", "Item_Name_" + Item_Num);
        SetPrefab(PlayerLogPrefab.LogType.ItemGet, str);
    }

    public static void ItemBuff()
    {
        SetPrefab(PlayerLogPrefab.LogType.ItemBuff, "스테이터스 안내");
    }

    public static void ItemBuffEnd()
    {
        SetPrefab(PlayerLogPrefab.LogType.ItemBuffEnd,"스테이터스 안내2");
    }

    public static void TrainWarning(float Persent)
    {
        SetPrefab(PlayerLogPrefab.LogType.TrainWarning, Persent.ToString("F1"));
    }

    public static void PlayerHPWarning(float Persent)
    {
        SetPrefab(PlayerLogPrefab.LogType.PlayerHPWarning, Persent.ToString("F1"));
    }

    public static void SpeedWarning(float Persent)
    {
        SetPrefab(PlayerLogPrefab.LogType.SpeedWarning, Persent.ToString("F1"));
    }

    public static void SkillUse(int playerNum, int skillNum)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("MissionSelect_Table_St", "Skill_Name_" + playerNum +"_" + skillNum);
        SetPrefab(PlayerLogPrefab.LogType.SkillUse, str);
    }
    public static void SkillCharge(int playerNum, int skillNum)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("MissionSelect_Table_St", "Skill_Name_" + playerNum +"_" + skillNum);
        SetPrefab(PlayerLogPrefab.LogType.SkillCharge, str);
    }
    public static void WaveStart(int waveNum)
    {
        SetPrefab(PlayerLogPrefab.LogType.WaveStart, waveNum.ToString());
    }

    static void SetPrefab(PlayerLogPrefab.LogType log, string str)
    {
        PlayerLogPrefab pre = Instantiate(PlayerLogPrefab, LogTransform);
        pre.LogTextSet(log, str);
        if (LogTransform.childCount > 4)
        {
            Destroy(LogTransform.GetChild(0).gameObject);
        }
    }
}
