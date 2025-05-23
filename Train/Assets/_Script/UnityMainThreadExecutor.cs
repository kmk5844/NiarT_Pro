using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UnityMainThreadExecutor : MonoBehaviour
{
    #region 싱글톤
    private static UnityMainThreadExecutor instance = null;
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
    public static UnityMainThreadExecutor Instance
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
    private static readonly Queue<System.Action> mainThreadActions = new Queue<System.Action>();
/*    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }*/

    // Unity의 메인 스레드에서 실행될 작업을 큐에 추가
    public static void ExecuteOnMainThread(System.Action action)
    {
        lock (mainThreadActions)
        {
            mainThreadActions.Enqueue(action);
        }
    }

    void Update()
    {
        // 큐에 쌓인 작업을 메인 스레드에서 실행
        while (mainThreadActions.Count > 0)
        {
            System.Action action;
            lock (mainThreadActions)
            {
                action = mainThreadActions.Dequeue();
            }
            action.Invoke();
        }
    }
}