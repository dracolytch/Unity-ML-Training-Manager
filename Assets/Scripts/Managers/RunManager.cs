using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Diagnostics;

public class RunManager : MonoBehaviour {

    public string RunSetName = "MyRunSet";
    public string ExePath = "";
    public string ExeFile = "";
    public List<string> Arguments = new List<string>();
    public int StartingConfigIncrement = 0;
    public int MaxConfigIncrement = 5;
    public int RunsPerIncrement = 1;
    public UnityEvent OnNextIncrement = new UnityEvent();
    public UnityEvent OnStartIncrement = new UnityEvent();

    int configIncrement = 0;
    int currentRun = 0;

    public void ExecuteTraining()
    {
        StartCoroutine(DoTrainingCo());
    }

    IEnumerator DoTrainingCo()
    {
        Process currentProcess = null;
        configIncrement = StartingConfigIncrement;
        while (configIncrement <= MaxConfigIncrement)
        {
            if (OnStartIncrement != null) OnStartIncrement.Invoke();

            for (currentRun = 0; currentRun < RunsPerIncrement; currentRun++)
            {
                UnityEngine.Debug.Log("Starting run " + currentRun + " in config increment " + configIncrement);

                var myArguments = new List<string>(Arguments); // Make a copy of the args
                var runId = RunSetName + "-inc" + configIncrement + "-run" + currentRun;
                myArguments.Add("--run-id=" + runId); // Add our own arg

                CommandLineRunner.WorkingDirectory = ExePath;
                currentProcess = CommandLineRunner.StartCommandLine(ExePath, ExeFile, myArguments.ToArray());

                // Coroutine hold until process is complete
                while (currentProcess.HasExited == false)
                {
                    yield return null;
                }
            }
            currentRun = 0;

            if (OnNextIncrement != null) OnNextIncrement.Invoke();
        }
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
