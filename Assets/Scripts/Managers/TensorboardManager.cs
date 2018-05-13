using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TensorboardManager : MonoBehaviour {
    public string TensorboardUrl = "http://localhost:6006";
    public string TensorboardPath = "";
    public string SummariesDirectory = "E:\\MLAgents-0-3\\python\\summaries\\";
    public UnityEvent OnTensorboardSuccess = new UnityEvent();
    public UnityEvent OnTensorboardFailure = new UnityEvent();

    float checkDelay = 5f; //  Give Tensorboard some time to start

    public void CheckTensorboard()
    {
        StartCoroutine(TestTensorboardCo(TensorboardUrl));
    }

    public void StartTensorboard()
    {
        var args = new List<string>();
        args.Add("--logdir=" + SummariesDirectory);
        CommandLineRunner.StartCommandLine(TensorboardPath, "tensorboard", args.ToArray());
    }

    IEnumerator TestTensorboardCo(string UrlWithPort)
    {
        Debug.Log("Checking Tensorboard in " + checkDelay + " seconds");
        yield return new WaitForSeconds(checkDelay);
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
                if (OnTensorboardFailure != null) OnTensorboardFailure.Invoke();
            }
        }
    }
}
