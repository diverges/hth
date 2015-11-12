using UnityEngine;
using System.Collections;

public class Stopwatch
{

    public bool IsRunning = false;
    private float startTime;
    private float endTime;
    private float currentTime;

    public void Start()
    {
        IsRunning = true;
        startTime = Time.time;
        currentTime = startTime;
        endTime = startTime;
    }

    public void Stop()
    {
        endTime = Time.time;
        IsRunning = false;
    }

    public void Reset()
    {
        startTime = 0.0f;
        endTime = 0.0f;
        IsRunning = false;
    }

    public void Update()
    {
        currentTime = Time.time;
    }

    public float getElapsedSeconds()
    {
        return currentTime - startTime;
    }

    public float getTimeSeconds()
    {
        return endTime - startTime;
    }

}
