using System.Collections;
using UnityEngine;
using TMPro;

public class DialogTest : MonoBehaviour
{
	[SerializeField]
	private	DialogSystem dialogSystem01;
	[SerializeField]
	private	TextMeshProUGUI	textCountdown;
	[SerializeField]
	private DialogSystem dialogSystem02;

	private IEnumerator Start()
	{
		textCountdown.gameObject.SetActive(false);

        // ù ��° ��� �б� ����
        dialogSystem01.gameObject.SetActive(true);
        yield return new WaitUntil(()=> dialogSystem01.UpdateDialog());
        dialogSystem01.gameObject.SetActive(false);
        dialogSystem02.gameObject.SetActive(true);
        /*        // ��� �б� ���̿� ���ϴ� �ൿ�� �߰��� �� �ִ�.
                // ĳ���͸� �����̰ų� �������� ȹ���ϴ� ����.. ����� 5-4-3-2-1 ī��Ʈ �ٿ� ����
                textCountdown.gameObject.SetActive(true);
                int count = 5;
                while (count > 0)
                {
                    textCountdown.text = count.ToString();
                    count--;

                    yield return new WaitForSeconds(1);
                }
                textCountdown.gameObject.SetActive(false);*/

        // �� ��° ��� �б� ����
        yield return new WaitUntil(() => dialogSystem02.UpdateDialog());
        dialogSystem02.gameObject.SetActive(false);

        textCountdown.gameObject.SetActive(true);
		textCountdown.text = "The End";

		yield return new WaitForSeconds(2);

		UnityEditor.EditorApplication.ExitPlaymode();
	}
}

