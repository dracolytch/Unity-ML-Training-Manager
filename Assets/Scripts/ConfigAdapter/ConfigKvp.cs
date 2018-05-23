using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class ConfigKvp
{
    public string key;
    public string startValue;
    public string endValue;
    public bool isUsed = true;

    public SimpleSetting Interpolate(int start, int end, int current)
    {
        if (start == end)
        {
            return new SimpleSetting() { key = key, value = startValue };
        }

        double startDouble;
        double endDouble;
        int startInt;
        int endInt;

        int totalSteps = end - start;
        int currentSteps = current - start;

        if (int.TryParse(startValue, out startInt) && int.TryParse(endValue, out endInt)) // Ints
        {
            int amountPerStep = (endInt - startInt) / totalSteps;
            int finalVal = startInt + (amountPerStep * currentSteps);
            return new SimpleSetting() { key = key, value = finalVal.ToString() };
        }

        // If this parses as a float
        if (double.TryParse(startValue, out startDouble) && double.TryParse(endValue, out endDouble))
        {
            double amountPerStep = (endDouble - startDouble) / (double)totalSteps;
            double finalVal = startDouble + (amountPerStep * currentSteps);
            return new SimpleSetting() { key = key, value = finalVal.ToString() };
        }

        return new SimpleSetting() { key = key, value = startValue };
    }
}

[System.Serializable]
public class JsonSettings
{
    public SimpleSetting[] settings;
}

[System.Serializable]
public class SimpleSetting
{
    public string key;
    public string value;
}