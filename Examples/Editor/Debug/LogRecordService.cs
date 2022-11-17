using System.Collections.Generic;
using UnityEngine;

public readonly struct LogEntry
{
    public string LogString { get; }
    public string StackTrace { get; }
    public LogType Type { get; }

    public LogEntry(string logString, string stackTrace, LogType type)
    {
        LogString = logString;
        StackTrace = stackTrace;
        Type = type;
    }
}

public class LogRecordService : MonoBehaviour
{
    private const int MAXIMUM_NUMBER_OF_RECORDED_LOGS = 300;
    private static List<LogEntry> _logEntries;

    public static List<LogEntry> Logs { get { return _logEntries; } }

    protected void Awake()
    {
        _logEntries = new();
        Application.logMessageReceivedThreaded += HandleLogMessageReceived;
    }

    protected void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= HandleLogMessageReceived;
    }

    public void HandleLogMessageReceived(string logString, string stackTrace, LogType type)
    {
        LogEntry newLogEntry = new(logString, stackTrace, type);
        _logEntries.Add(newLogEntry);

        if (_logEntries.Count > MAXIMUM_NUMBER_OF_RECORDED_LOGS)
        {
            _logEntries.RemoveAt(0);
        }
    }
}