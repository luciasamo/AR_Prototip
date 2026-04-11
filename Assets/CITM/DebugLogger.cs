using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DebugLogger : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private Queue<string> logQueue = new Queue<string>();
    private int maxLines = 10;

    void Awake()
    {
        // También captura logs normales
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string message = $"[{type}] {logString}";
        logQueue.Enqueue(message);
        while (logQueue.Count > maxLines) logQueue.Dequeue();

        if (debugText != null)
            debugText.text = string.Join("\n", logQueue);
    }

    public void AddMessage(string msg)
    {
        logQueue.Enqueue(msg);
        while (logQueue.Count > maxLines) logQueue.Dequeue();
        if (debugText != null)
            debugText.text = string.Join("\n", logQueue);
    }
}