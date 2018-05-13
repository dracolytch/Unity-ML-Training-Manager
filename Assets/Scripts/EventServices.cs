using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventServices : MonoBehaviour {

    public UnityEvent OnStart = new UnityEvent();

    // Use this for initialization
    void Start()
    {
        if (OnStart != null) OnStart.Invoke();
    }

    public void Log(string target)
    {
        Debug.Log(target);
    }
}
