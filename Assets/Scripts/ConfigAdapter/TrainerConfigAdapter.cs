using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

public class TrainerConfigAdapter : MonoBehaviour {

    public string TrainerConfigPath = "E:\\MLAgents-0-3\\python\\";
    public string TrainerConfigFilename = "trainer_config.yaml";
    public string BrainName;
    public List<ConfigKvp> settings = new List<ConfigKvp>();

    string configFile;
	// Use this for initialization
	void Start () {
        configFile = Path.Combine(TrainerConfigPath, TrainerConfigFilename);

        if (File.Exists(configFile))
        {
            File.Copy(configFile, configFile + ".backup-" + System.DateTime.Now.Ticks);
        }
    }
	
    public void SaveConfigForIncrement(RunManager manager)
    {
        var buffer = new StringBuilder();
        buffer.Append("default:\r\n");
        buffer.Append("    " + "trainer: ppo" + "\r\n");
        buffer.Append("    " + "batch_size: 1024" + "\r\n");
        buffer.Append("    " + "beta: 5.0e-3" + "\r\n");
        buffer.Append("    " + "buffer_size: 10240" + "\r\n");
        buffer.Append("    " + "epsilon: 0.2" + "\r\n");
        buffer.Append("    " + "gamma: 0.99" + "\r\n");
        buffer.Append("    " + "hidden_units: 128" + "\r\n");
        buffer.Append("    " + "lambd: 0.95" + "\r\n");
        buffer.Append("    " + "learning_rate: 3.0e-4" + "\r\n");
        buffer.Append("    " + "max_steps: 5.0e4" + "\r\n");
        buffer.Append("    " + "memory_size: 256" + "\r\n");
        buffer.Append("    " + "normalize: false" + "\r\n");
        buffer.Append("    " + "num_epoch: 3" + "\r\n");
        buffer.Append("    " + "num_layers: 2" + "\r\n");
        buffer.Append("    " + "time_horizon: 64" + "\r\n");
        buffer.Append("    " + "sequence_length: 64" + "\r\n");
        buffer.Append("    " + "summary_freq: 1000" + "\r\n");
        buffer.Append("    " + "use_recurrent: false" + "\r\n");
        buffer.Append("\r\n");

        buffer.Append(BrainName + ":\r\n");
        foreach (var setting in settings)
        {
            if (setting.isUsed)
            {
                var outSetting = setting.Interpolate(manager.StartingConfigIncrement, manager.MaxConfigIncrement, manager.GetCurrentStep());
                buffer.Append("    " + outSetting.key + ": " + outSetting.value + "\r\n");
            }
        }

        File.WriteAllText(configFile, buffer.ToString());
        File.WriteAllText(Path.Combine(TrainerConfigPath, manager.RunSetName + "-inc" + manager.GetCurrentStep() + ".yaml"), buffer.ToString());
    }
}
