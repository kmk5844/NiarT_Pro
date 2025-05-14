using UnityEngine;
using UnityEditor;
using UnityEngine.Localization.PropertyVariants;

public class GameObjectLocalizerCleaner : EditorWindow
{
    [MenuItem("Tools/Remove All GameObjectLocalizer Components")]
    public static void RemoveAllGameObjectLocalizerComponents()
    {
        int count = 0;

        // ���� �ִ� ��� GameObjectLocalizer ������Ʈ ã�� (��Ȱ�� ������Ʈ ����)
        GameObjectLocalizer[] allLocalizers = GameObject.FindObjectsOfType<GameObjectLocalizer>(true);

        foreach (GameObjectLocalizer localizer in allLocalizers)
        {
            Undo.DestroyObjectImmediate(localizer);
            count++;
        }

        Debug.Log($"Removed {count} GameObjectLocalizer components.");
    }
}