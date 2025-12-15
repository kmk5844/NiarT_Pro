using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
	public DialogSystem dialogSystem;
    public GameObject Fade;
    public bool storyEnd_SpecialFlag;
    public bool story_InGame_Flag;
    public GameDirector gameDirector;

    private IEnumerator Start()
	{
        // 첫 번째 대사 분기 시작
        if(!story_InGame_Flag)
        {
            dialogSystem.gameObject.SetActive(true);
            yield return new WaitUntil(() => dialogSystem.UpdateDialog());
            dialogSystem.gameObject.SetActive(false);
            if (!dialogSystem.SpecialFlag)
            {
                Fade.SetActive(true);
            }
            else
            {
                storyEnd_SpecialFlag = true;
            }
        }
	}

    public IEnumerator Story_InGame()
    {
        dialogSystem.gameObject.SetActive(true);
        yield return new WaitUntil(() => dialogSystem.UpdateDialog());
        gameDirector.EndStory();
    }
}