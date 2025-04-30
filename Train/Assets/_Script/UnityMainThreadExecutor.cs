using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UnityMainThreadExecutor : MonoBehaviour
{
    #region �̱���
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

    // Unity�� ���� �����忡�� ����� �۾��� ť�� �߰�
    public static void ExecuteOnMainThread(System.Action action)
    {
        lock (mainThreadActions)
        {
            mainThreadActions.Enqueue(action);
        }
    }

    void Update()
    {
        // ť�� ���� �۾��� ���� �����忡�� ����
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