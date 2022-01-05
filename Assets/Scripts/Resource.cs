using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Resource
{
    public int current;
    public int max;
    public int criticalThreshold;
    public bool critical
    {
        get
        {
            return current < criticalThreshold;
        }
    }
    public bool depleted
    {
        get
        {
            return current <= 0;
        }
    }

    public void Increment(int value)
    {
        current += value;
        current = Mathf.Clamp(current, 0, max);
    }
}