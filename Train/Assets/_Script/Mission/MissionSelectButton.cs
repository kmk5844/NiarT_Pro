using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionSelectButton : MonoBehaviour
{
    public SA_PlayerData playerData;
    public MissionSelectDirector missionSelectDirector;

    [Header("UI")]
    public LocalizeStringEvent MissionType_Text;
    public LocalizeStringEvent MissionInformation_Text;
    public TextMeshProUGUI MissionReward_Text;

    public int missionNum;
    public int MissionReward;
    string state;

    [Header("�̼� �̹���")]
    public Image missionImage;
    public Sprite[] missionSpriteList;

    Quest_DataTable missionList_Table;
    int missionList_index;

    public void Mission_SetData(int _missionNum, Quest_DataTable _list, int _listIndex)
    {
        missionNum = _missionNum;
        missionList_Table = _list;
        missionList_index = _listIndex;
        //MissionType = _list.Q_List[_listIndex].Quest_Type;
        //MissionInformation = _list.Q_List[_listIndex].Quest_Information;
        CheckMission();
        state = _list.Q_List[_listIndex].Quest_State;
        MissionReward = _list.Q_List[_listIndex].Quest_Reward;
        
        MissionReward_Text.text = MissionReward.ToString() + "G";
        missionImage.sprite = missionSpriteList[missionNum];
    }

    void CheckMission()
    {
        MissionType_Text.StringReference.TableReference = "MissionList_St";
        MissionType_Text.StringReference.TableEntryReference = "Title_" + missionNum;
        MissionInformation_Text.StringReference.TableReference = "MissionList_St";

        string[] missionState = missionList_Table.Q_List[missionList_index].Quest_State.Split(',');

        LocalizedString monsterString = new LocalizedString();
        monsterString.TableReference = "MissionList_St";

        switch (missionNum)
        {
            case 0:
                //�⺻
                MissionInformation_Text.StringReference.TableEntryReference = "Information_" + missionNum;
                break;
            case 1:
                MissionInformation_Text.StringReference.Arguments = new object[] { missionState[0] };
                MissionInformation_Text.StringReference.TableEntryReference = "Information_" + missionNum;
                MissionInformation_Text.RefreshString();
                break;
            case 2:
                monsterString.TableEntryReference = "Monster_" + missionState[0]; // ��: Boss_0, Boss_1 ���� ��
                // monsterString ������ �����ͼ� Argument�� ����
                monsterString.GetLocalizedStringAsync().Completed += handle =>
                {
                    string monsterName = handle.Result;

                    // Argument�� monsterName �ֱ�
                    MissionInformation_Text.StringReference.Arguments = new object[] { monsterName, missionState[1] };
                    MissionInformation_Text.StringReference.TableEntryReference = "Information_" + missionNum;
                    MissionInformation_Text.RefreshString();
                };
                break;
            case 3:
                //�⺻
                MissionInformation_Text.StringReference.TableEntryReference = "Information_" + missionNum;
                break;
            case 4:
                //�⺻
                MissionInformation_Text.StringReference.TableEntryReference = "Information_" + missionNum;
                break;
            case 5:
                monsterString.TableEntryReference = "Boss_" + missionState[0]; // ��: Boss_0, Boss_1 ���� ��

                // monsterString ������ �����ͼ� Argument�� ����
                MissionInformation_Text.StringReference.Arguments = new object[] { -1 };
                monsterString.GetLocalizedStringAsync().Completed += handle =>
                {
                    string monsterName = handle.Result;

                    // Argument�� monsterName �ֱ�
                    MissionInformation_Text.StringReference.Arguments = new object[] { monsterName };
                    MissionInformation_Text.StringReference.TableEntryReference = "Information_" + missionNum;
                    MissionInformation_Text.RefreshString();
                };
                break;
        }
    }


    public void ClickMission()
    {
        if (missionNum == 4)
        {
            missionSelectDirector.Open_Numerical_Settings_Convoy(missionNum, state);
        }
        else
        {
            //playerData.SA_ClickMission(missionNum);
            //playerData.SA_MissionPlaying(true);
            //UnityMainThreadExecutor.ExecuteOnMainThread(() =>  click_Mission_playerData());
            playerData.SA_ClickMission(missionNum);
            StartCoroutine(missionSelectDirector.MissionDataSet());
        }
    }


    async void click_Mission_playerData()
    {
        await Task.Run(() => {
        });
    }
}