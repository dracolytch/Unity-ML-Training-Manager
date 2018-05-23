using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

public class TensorboardManager : MonoBehaviour {
    //http://localhost:6006/data/plugin/scalars/scalars?run=BatchSize-inc1-run0&tag=Info/cumulative_reward

    public string TensorboardUrl = "http://localhost:6006";
    public float TensorboardStartupTime = 10f; //  Give Tensorboard some time to start
    public TensorFlowConfig tensorFlowCOnfig;
    public UnityEvent OnTensorboardSuccess = new UnityEvent();
    public UnityEvent OnTensorboardFailure = new UnityEvent();

    public IEnumerator GetRunStats(string runId, int runNumber, int incrementNumber, Action<RunStatistics> action)
    {
        var url = GetRunStatsUrl(runId);
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error) == true && string.IsNullOrEmpty(www.text) == false)
            {
                var runStats = new RunStatistics();
                var buffer = "{\"data\":" + www.text + "}";
                //var runData = JsonUtility.FromJson<RunData>(buffer);
                var runData = JsonConvert.DeserializeObject<RunData>(buffer);

                var accumulator = 0.0;
                foreach (var row in runData.data)
                {
                    accumulator += row[2];
                }
                runStats.runId = runId;
                runStats.runNumber = runNumber;
                runStats.incrementNumber = incrementNumber;
                runStats.averageReward = accumulator / (double)runData.data.Count;
                runStats.finalReward = runData.data[runData.data.Count - 1][2];
                action(runStats);
            }
            else
            {
                Debug.Log("Failure getting stats for " + runId);
            }
        }
    }

    string GetRunStatsUrl(string runId)
    {
        return TensorboardUrl + "/data/plugin/scalars/scalars?run=" + runId + "&tag=Info/cumulative_reward";
    }

    public void CheckTensorboard()
    {
        StartCoroutine(TestTensorboardCo(TensorboardUrl));
    }

    public void StartTensorboard()
    {
        var args = new List<string>();
        args.Add("--logdir=" + tensorFlowCOnfig.SummariesDirectory);
        CommandLineRunner.StartCommandLine(tensorFlowCOnfig.TensorboardDirectory, "tensorboard", args.ToArray());
    }

    IEnumerator TestTensorboardCo(string UrlWithPort)
    {
        using (WWW www = new WWW(UrlWithPort))
        {
            yield return www;
            // if there is no error, and is content
            if (string.IsNullOrEmpty(www.error) == true && string.IsNullOrEmpty(www.text) == false)
            {
                if (OnTensorboardSuccess != null) OnTensorboardSuccess.Invoke();
            }
            else
            {
                Debug.Log("Tensorboard is off, starting TensorBoard and waiting a few seconds for startup");
                StartTensorboard();
                yield return new WaitForSeconds(TensorboardStartupTime);
                if (OnTensorboardFailure != null) OnTensorboardFailure.Invoke();
            }
        }
    }
}
