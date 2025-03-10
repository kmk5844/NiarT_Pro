using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
	public DialogSystem dialogSystem;
    public GameObject Fade;
    public bool storyEnd_SpecialFlag;

	private IEnumerator Start()
	{
        // 첫 번째 대사 분기 시작
        dialogSystem.gameObject.SetActive(true);
        yield return new WaitUntil(()=> dialogSystem.UpdateDialog());
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