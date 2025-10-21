
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class PlayerLogPrefab : MonoBehaviour
{
    public LocalizeStringEvent LogText;

    public void LogTextSetSingle(LogType type, string str)
    {
        LogText.StringReference.TableReference = "PlayerLogUI_St";
        LogText.StringReference.TableEntryReference = type.ToString();
        LogText.StringReference.Arguments = new object[] { str };
        LogText.RefreshString();
    }

    public void LogTextSetDouble(LogType type, string str, string str2)
    {
        LogText.StringReference.Arguments = new object[] { str , str2 };
        LogText.StringReference.TableReference = "PlayerLogUI_St";
        LogText.StringReference.TableEntryReference = type.ToString();
        LogText.RefreshString();
    }

    public void AniDestroy()
    {
        Destroy(this.gameObject);
    }

    public enum LogType
    {
        MercenaryKill,
        MercenaryDie,
        MercenaryRepair,
        MercenaryRope,
        MercenaryHeal,
        ItemUse,
        ItemGet,
        ItemBuff,
        ItemBuffEnd,
        TrainWarning,
        PlayerHPWarning,
        SpeedWarning,
        SkillUse,
        SkillCharge,
        WaveStart,
        MissionTrainWarning,
        EscortWarning
    }
}
