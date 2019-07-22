using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class RunManager : MonoBehaviour
{

    public string RunSetName = "MyRunSet";
    public float PauseBeforeRun = 2f;
    public string UnityOutputExeName;
    public int StartingConfigIncrement = 1;
    public int MaxConfigIncrement = 1;
    public int RunsPerConfiguration = 1;
    public TensorFlowConfig tensorFlowConfig;
    public TensorboardManager TbManager;

    public bool ContinueFromSaved = false;
    public string ContinueFromModel = "";

    public List<RunStatistics> RunStats = new List<RunStatistics>();

    public UnityEvent OnNextIncrement = new UnityEvent();
    public UnityEvent OnStartIncrement = new UnityEvent();
    public UnityEvent OnAllIncrementsComplete = new UnityEvent();

    int configIncrement = 0;
    int currentRun = 0;

    public void ExecuteTraining()
    {
        StartCoroutine(DoTraining());
    }

    static void CopyDirectory(string SourcePath, string DestinationPath)
    {
        foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
        }

        foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }

    }

    IEnumerator DoTraining()
    {
        Process currentProcess = null;
        configIncrement = StartingConfigIncrement;
        while (configIncrement <= MaxConfigIncrement)
        {
            if (OnStartIncrement != null) OnStartIncrement.Invoke();

            for (currentRun = 0; currentRun < RunsPerConfiguration; currentRun++)
            {
                yield return new WaitForSeconds(PauseBeforeRun);
                UnityEngine.Debug.Log("Starting run " + currentRun + " in config increment " + configIncrement);

                var myArguments = new List<string>(); // Make a copy of the args
                var runId = RunSetName + "-inc" + configIncrement + "-run" + currentRun;
                myArguments.Add(UnityOutputExeName);
                myArguments.Add("--run-id=" + runId); // Add our own arg
                myArguments.Add("--train");

                if (ContinueFromSaved == true)
                {
                    myArguments.Add("--load");
                    var sourcePath = Path.Combine(tensorFlowConfig.ModelsDirectory, ContinueFromModel);
                    var targetPath = Path.Combine(tensorFlowConfig.ModelsDirectory, runId);
                    Directory.CreateDirectory(targetPath);
                    CopyDirectory(sourcePath, targetPath);
                }

                CommandLineRunner.WorkingDirectory = tensorFlowConfig.MlAgentsConfigDirectory;
                currentProcess = CommandLineRunner.StartCommandLine(tensorFlowConfig.LearnEnvExecute,
                    tensorFlowConfig.MlAgentsConfigDirectory);

                // Coroutine hold until process is complete
                while (currentProcess.HasExited == false)
                {
                    yield return null;
                }

                if (TbManager != null)
                {
                    yield return StartCoroutine(TbManager.GetRunStats(runId, currentRun, configIncrement, new Action<RunStatistics>(AddToStats)));
                }
            }
            currentRun = 0;

            if (OnNextIncrement != null) OnNextIncrement.Invoke();
        }

        if (OnAllIncrementsComplete != null) OnAllIncrementsComplete.Invoke();
    }

    void AddToStats(RunStatistics stats)
    {
        RunStats.Add(stats);
    }

    public int GetCurrentStep()
    {
        return configIncrement;
    }

    public int GetCurrentRun()
    {
        return currentRun;
    }

    public void IncrementLinear()
    {
        configIncrement++;
    }

    public void IncrementDouble()
    {
        configIncrement *= 2;
    }

    public void IncrementSpread()
    {
        switch (configIncrement)
        {
            case 0:
                configIncrement = 1;
                break;

            case 1:
                configIncrement = 5;
                break;

            case 5:
                configIncrement = 10;
                break;

            case 10:
                configIncrement = 20;
                break;

            case 20:
                configIncrement = 50;
                break;

            case 50:
                configIncrement = 100;
                break;

            case 100:
                configIncrement = 250;
                break;

            case 250:
                configIncrement = 500;
                break;

            default:
                configIncrement *= 2;
                break;
        }
    }
}