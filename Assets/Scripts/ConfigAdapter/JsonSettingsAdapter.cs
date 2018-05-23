using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class JsonSettingsAdapter : MonoBehaviour {

    //public string ConfigPath;
    public TensorFlowConfig tensorFlowConfig;
    public string ConfigFilename;
    public List<ConfigKvp> settings = new List<ConfigKvp>();

    public void SaveConfigForIncrement(RunManager manager)
    {
        var configFile = Path.Combine(tensorFlowConfig.UnityOutputDirectory, ConfigFilename);
        var outSettings = new List<SimpleSetting>();

        foreach (var setting in settings)
        {
            if (setting.isUsed) outSettings.Add(setting.Interpolate(manager.StartingConfigIncrement, manager.MaxConfigIncrement, manager.GetCurrentStep()));
        }

        var settingsJson = new JsonSettings() { settings = outSettings.ToArray() };
        var buffer = JsonUtility.ToJson(settingsJson);

        File.WriteAllText(configFile, buffer.ToString());
        File.WriteAllText(Path.Combine(tensorFlowConfig.UnityOutputDirectory, manager.RunSetName + "-inc" + manager.GetCurrentStep() + ".json"), buffer.ToString());
    }
}
