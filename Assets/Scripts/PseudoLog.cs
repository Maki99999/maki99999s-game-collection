using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoLog
{
    bool isPaused = false;
    float startTime = 0f;
    string log = "";

    public void Add(object logMessage)
    {
        if (isPaused) return;
        if (log == "") startTime = Time.time;
        log += (Time.time - startTime) + "\t" + logMessage + "\n";
    }

    public void AddMoreOneLine(params object[] logMessages)
    {
        if (isPaused) return;
        if (log == "") startTime = Time.time;
        log += (Time.time - startTime) + "\t";

        foreach (object logMessage in logMessages)
            log += logMessage + "\t";

        log = log.Remove(log.Length - 1) + "\n";
    }

    public string getLog()
    {
        string oldLog = log;
        log = "";
        return oldLog;
    }

    public void PauseLogging()
    {
        isPaused = true;
    }

    public void UnpauseLogging()
    {
        isPaused = false;
    }
}
