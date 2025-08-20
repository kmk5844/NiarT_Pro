using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class DicButton_Story : MonoBehaviour
{
    public TextMeshProUGUI storyNumText;
    public TextMeshProUGUI storyStageText;
    public LocalizeStringEvent storyText;

    DictionaryDirector dicDirector;
    SA_StoryLIst sa_storylist;
    int num;
    bool flag;
    Button btn;
    bool cutSceneFlag;

    private void Start()
    {
        storyText.StringReference.TableReference = "Story_St";
    }

    public void SettingStoryButton_CutScene(DictionaryDirector _dicDirector)
    {
        dicDirector = _dicDirector;
        btn = GetComponent<Button>();
        btn.interactable = true;
        cutSceneFlag = true;
        num = 0;
        storyNumText.text = "0";
        storyText.StringReference.TableEntryReference = "UI_Dic_CutScene";
        storyStageText.text = "CutScene";
        AddButton();
    }

    public void SettingStoryButton(DictionaryDirector _dicDirector, SA_StoryLIst _storylist, int _num)
    {
        dicDirector = _dicDirector;
        sa_storylist = _storylist;
        num = _num;
        storyNumText.text = (num + 1).ToString();

        if (sa_storylist.StoryList[num].Start_Flag && sa_storylist.StoryList[num].End_Flag)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }

        btn = GetComponent<Button>();
        btn.interactable = flag;
        cutSceneFlag = false;
        storyText.StringReference.Arguments = new object[] { num+1 };
        storyText.StringReference.TableEntryReference = "UI_Dic_Story";
        storyText.RefreshString();
       
        if (!sa_storylist.StoryList[num].Dic_Flag)
        {
            storyStageText.text = "¢º Stage " + (sa_storylist.StoryList[num].Stage_Num / 5 + 1)+ "-" + ((sa_storylist.StoryList[num].Stage_Num % 5) + 1) + " 1";
        }
        else
        {
            storyStageText.text = "¢º Stage " + (sa_storylist.StoryList[num].Stage_Num / 5 + 1) + "-" + ((sa_storylist.StoryList[num].Stage_Num % 5) + 1) + " 2";
        }

        AddButton();
    }

    void AddButton()
    {
        if (cutSceneFlag)
        {
            btn.onClick.AddListener(() => dicDirector.SetStory(cutSceneFlag, 0));
        }
        else
        {
            btn.onClick.AddListener(() => dicDirector.SetStory(cutSceneFlag, num + 1));
        }
    }
}
