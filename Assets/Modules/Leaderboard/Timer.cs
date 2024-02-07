using UnityEngine;
using System;

public class Timer: MonoBehaviour
{
    private DateTime target;
    public void Setup(DateTime targetTime)
    {
        target = targetTime;

    }
}
