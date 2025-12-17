using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class CanvasBlackBoxRecorder : MonoBehaviour
{
    struct Snapshot
    {
        public int frame;
        public string info;
    }

    Queue<Snapshot> buffer = new Queue<Snapshot>();
    const int BUFFER_SIZE = 30;

    void OnEnable()
    {
        Application.logMessageReceived += OnLog;
        Canvas.willRenderCanvases += OnWillRender;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= OnLog;
        Canvas.willRenderCanvases -= OnWillRender;
    }

    void LateUpdate()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Frame {Time.frameCount}");

        foreach (var c in FindObjectsOfType<Canvas>())
        {
            sb.AppendLine($"Canvas: {c.name} enabled={c.enabled}");

            var graphics = c.GetComponentsInChildren<UnityEngine.UI.Graphic>(true);
            foreach (var g in graphics)
            {
                if (g == null)
                {
                    sb.AppendLine("  ❌ Graphic NULL");
                    continue;
                }

                var rt = g.rectTransform;
                if (rt == null)
                {
                    sb.AppendLine($"  ❌ {g.name} RectTransform NULL");
                    continue;
                }

                sb.AppendLine(
                    $"  {g.name} " +
                    $"active={g.gameObject.activeSelf} " +
                    $"scale={rt.localScale} " +
                    $"size={rt.rect.size}"
                );
            }
        }

        buffer.Enqueue(new Snapshot
        {
            frame = Time.frameCount,
            info = sb.ToString()
        });

        if (buffer.Count > BUFFER_SIZE)
            buffer.Dequeue();
    }

    void OnWillRender()
    {
        // willRender 호출 자체를 기록
        buffer.Enqueue(new Snapshot
        {
            frame = Time.frameCount,
            info = $"⚠️ willRenderCanvases 호출"
        });

        if (buffer.Count > BUFFER_SIZE)
            buffer.Dequeue();
    }

    void OnLog(string condition, string stackTrace, LogType type)
    {
        if (condition.Contains("Invalid AABB"))
        {
            Debug.LogError("🧨 INVALID AABB DETECTED — BLACKBOX DUMP START");

            foreach (var snap in buffer)
            {
                Debug.Log(snap.info);
            }

            Debug.Break();
        }
    }
}