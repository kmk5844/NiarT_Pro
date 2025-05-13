using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class AppQuitHandler : MonoBehaviour
{
    #region �̱���
    private static AppQuitHandler instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static AppQuitHandler Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion
    bool isQuitting = false;
    
    private void OnApplicationQuit()
    {
        isQuitting = true;
/*        LocalizationSettings.StringDatabase.ClearDatabase(); // <-- ��� �ε�� ���ڿ� ���ҽ� ����
        LocalizationSettings.AssetDatabase.ClearDatabase();  // <-- �ε�� ���� ���� (���� ��� ���̶��)*/
        /*        if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }*/
    }

    /*        try
            {
                // Localization�� ���ҽ� ���� ����
                if (LocalizationSettings.Instance != null)
                {
                    LocalizationSettings.Instance.GetType()
                        .GetMethod("ResetState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                        .Invoke(LocalizationSettings.Instance, null);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Localization ���� �� ����: " + e.Message);
            }*/
}
