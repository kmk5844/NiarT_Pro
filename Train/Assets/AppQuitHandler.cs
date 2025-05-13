using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class AppQuitHandler : MonoBehaviour
{
    #region 싱글톤
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
/*        LocalizationSettings.StringDatabase.ClearDatabase(); // <-- 모든 로드된 문자열 리소스 제거
        LocalizationSettings.AssetDatabase.ClearDatabase();  // <-- 로드된 에셋 제거 (만약 사용 중이라면)*/
        /*        if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }*/
    }

    /*        try
            {
                // Localization의 리소스 수동 해제
                if (LocalizationSettings.Instance != null)
                {
                    LocalizationSettings.Instance.GetType()
                        .GetMethod("ResetState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                        .Invoke(LocalizationSettings.Instance, null);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Localization 정리 중 오류: " + e.Message);
            }*/
}
