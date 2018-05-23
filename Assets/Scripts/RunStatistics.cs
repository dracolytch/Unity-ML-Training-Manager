using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunStatistics {
    public string runId;
    public int runNumber;
    public int incrementNumber;
    public double finalReward;
    public double averageReward;
}

[System.Serializable]
public class RunData
{
    public string name;
    public List<List<double>> data;
}