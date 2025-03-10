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
        // ù ��° ��� �б� ����
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