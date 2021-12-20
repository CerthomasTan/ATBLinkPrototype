using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contoller for log console
/// </summary>
public class DebugLogConsole : MonoBehaviour
{

    public string output = "";
    public string stack = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    /// <summary>
    /// Very time log is updated in Unity, will add string into output vairable. Will display to text box
    /// </summary>
    /// <param name="logString"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output += logString;
        output += System.Environment.NewLine;
        stack = stackTrace;
        this.gameObject.GetComponent<Text>().text = output;
    }
}
