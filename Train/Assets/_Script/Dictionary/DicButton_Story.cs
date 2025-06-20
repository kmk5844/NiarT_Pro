using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class DicButton_Story : MonoBehaviour
{
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
        storyText.StringReference.TableEntryReference = "UI_Dic_CutScene";
        AddButton();
    }

    public void SettingStoryButton(DictionaryDirector _dicDirector, SA_StoryLIst _storylist, int _num)
    {
        dicDirector = _dicDirector;
        sa_storylist = _storylist;
        num = _num;
        
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
        AddButton();
    }

    void AddButton()
    {
        btn.onClick.AddListener(() => dicDirector.Enter_StoryMode(cutSceneFlag, num));
    }
}
