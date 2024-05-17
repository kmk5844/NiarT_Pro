using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
	public	DialogSystem dialogSystem;

	private IEnumerator Start()
	{
        // 첫 번째 대사 분기 시작
        dialogSystem.gameObject.SetActive(true);
        yield return new WaitUntil(()=> dialogSystem.UpdateDialog());
        dialogSystem.gameObject.SetActive(false);
		yield return new WaitForSeconds(1);
		GameManager.Instance.End_Enter();
	}
}

