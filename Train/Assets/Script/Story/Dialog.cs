using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
	public	DialogSystem dialogSystem;
    public GameObject Fade;

	private IEnumerator Start()
	{
        // ù ��° ��� �б� ����
        dialogSystem.gameObject.SetActive(true);
        yield return new WaitUntil(()=> dialogSystem.UpdateDialog());
        dialogSystem.gameObject.SetActive(false);
        Fade.SetActive(true);
	}
}

