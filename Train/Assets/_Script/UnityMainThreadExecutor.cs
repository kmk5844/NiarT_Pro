using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UnityMainThreadExecutor : MonoBehaviour
{
    private static readonly Queue<System.Action> mainThreadActions = new Queue<System.Action>();

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