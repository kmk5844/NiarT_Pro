using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class PlayerLogDirector : MonoBehaviour
{
    public static bool OnOff;
    [Header("옵션에 추가된 토글 : 인게임에서만 적용")]
    public Toggle logToggle;

    public Transform logTransform;
    public static Transform LogTransform { get; private set; }
    public PlayerLogPrefab playerLogPrefab;
    public static PlayerLogPrefab PlayerLogPrefab { get; private set; }

    private void Awake()
    {
        LogTransform = logTransform;
        PlayerLogPrefab = playerLogPrefab;
        OnOff = ES3.Load<bool>("Setting_PlayerLogUI", true);
    }

    private void Start()
    {
        logToggle.isOn = OnOff;
        logToggle.onValueChanged.AddListener(delegate { ChnageToggle(); });
    }

    void ChnageToggle()
    {
        OnOff = logToggle.isOn;
        ES3.Save<bool>("Setting_PlayerLogUI", OnOff);
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

    public static void MercenaryRepair(int mercenaryNum)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("ExcelData_Table_St", "Mercenary_Name_" + mercenaryNum);
        SetPrefab(PlayerLogPrefab.LogType.MercenaryRepair, str);
    }

    public static void MercenaryRope(int mercenaryNum)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("ExcelData_Table_St", "Mercenary_Name_" + mercenaryNum);
        SetPrefab(PlayerLogPrefab.LogType.MercenaryRope, str);
    }

    public static void MercenaryHeal(int mercenaryNum, int healMercenaryNum)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString("ExcelData_Table_St", "Mercenary_Name_" + mercenaryNum);
        string str2 = LocalizationSettings.StringDatabase.GetLocalizedString("ExcelData_Table_St", "Mercenary_Name_" + healMercenaryNum);
        SetPrefab_Double(PlayerLogPrefab.LogType.MercenaryHeal, str, str2);
    }

    public static void MissionTrainWarning(float Persent)
    {
        SetPrefab(PlayerLogPrefab.LogType.MissionTrainWarning, Persent.ToString("F1"));
    }
    public static void EscortWarning(float Persent)
    {
        SetPrefab(PlayerLogPrefab.LogType.EscortWarning, Persent.ToString("F1"));
    }

    static void SetPrefab(PlayerLogPrefab.LogType log, string str)
    {
        if (OnOff)
        {
            PlayerLogPrefab pre = Instantiate(PlayerLogPrefab, LogTransform);
            pre.LogTextSetSingle(log, str);
            if (LogTransform.childCount > 4)
            {
                Destroy(LogTransform.GetChild(0).gameObject);
            }
        }
    }

    static void SetPrefab_Double(PlayerLogPrefab.LogType log, string str, string str2)
    {
        if (OnOff)
        {
            PlayerLogPrefab pre = Instantiate(PlayerLogPrefab, LogTransform);
            pre.LogTextSetDouble(log, str, str2);
            if (LogTransform.childCount > 4)
            {
                Destroy(LogTransform.GetChild(0).gameObject);
            }
        }
    }
}
