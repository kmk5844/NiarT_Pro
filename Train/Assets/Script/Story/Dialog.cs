using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
	public	DialogSystem dialogSystem;
    public GameObject Fade;

	private IEnumerator Start()
	{
        // 첫 번째 대사 분기 시작
        dialogSystem.gameObject.SetActive(true);
        yield return new WaitUntil(()=> dialogSystem.UpdateDialog());
        dialogSystem.gameObject.SetActive(false);
        Fade.SetActive(true);
	}
}

