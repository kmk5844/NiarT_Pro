using UnityEngine;
using UnityEditor;
using UnityEngine.Localization.PropertyVariants;

public class GameObjectLocalizerCleaner : EditorWindow
{
    [MenuItem("Tools/Remove All GameObjectLocalizer Components")]
    public static void RemoveAllGameObjectLocalizerComponents()
    {
        int count = 0;

        // 씬에 있는 모든 GameObjectLocalizer 컴포넌트 찾기 (비활성 오브젝트 포함)
        GameObjectLocalizer[] allLocalizers = GameObject.FindObjectsOfType<GameObjectLocalizer>(true);

        foreach (GameObjectLocalizer localizer in allLocalizers)
        {
            Undo.DestroyObjectImmediate(localizer);
            count++;
        }

        Debug.Log($"Removed {count} GameObjectLocalizer components.");
    }
}