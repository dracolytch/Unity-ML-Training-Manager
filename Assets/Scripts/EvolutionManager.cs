using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolutionManager : MonoBehaviour {
    public string RunSetNameRoot = "test";
    public int maxGeneration = 2;
    public int currentGeneration = 0;
    public int stepsPerGeneration = 50000;
    public TrainerConfigAdapter trainerConfig;
    public RunManager runManager;

    public UnityEvent OnBeginRun = new UnityEvent();
    public UnityEvent OnDone = new UnityEvent();

	public void DoEvolution()
    {
        currentGeneration++;
        Debug.Log("Generation " + currentGeneration);
        if (currentGeneration == maxGeneration)
        {
            Debug.Log("Max generation reached");
            if (OnDone != null) OnDone.Invoke();
            return;
        }

        var stats = runManager.RunStats;
        if (stats.Count == 0)
        {
            Debug.LogWarning("No entries in stats");
            return;
        }

        // Find the best run
        var bestAverage = double.MinValue;
        var bestAverageIndex = int.MinValue;

        for (var i = 0; i < stats.Count; i++)
        {
            // find the best average reward
            if (stats[i].averageReward > bestAverage)
            {
                bestAverage = stats[i].averageReward;
                bestAverageIndex = i;
            }
        }

        // Set Duplicate settings
        runManager.RunSetName = RunSetNameRoot + "-ev" + currentGeneration;
        runManager.ContinueFromSaved = true;
        runManager.ContinueFromModel = stats[bestAverageIndex].runId;
        runManager.RunStats = new List<RunStatistics>();

        // Change max iterations
        var nextSteps = stepsPerGeneration * (1 + currentGeneration);
        trainerConfig.UpdateSetting("max_steps", nextSteps.ToString(), nextSteps.ToString());

        // Start the run again
        if (OnBeginRun != null) OnBeginRun.Invoke();
    }
}
