using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensorFlowConfig : MonoBehaviour
{
    [Tooltip("Path to the root directory for the local Anaconda3 installation" +
        " E.G  C:\\ProgramData\\Anaconda3")]
    public string AnacondaRootDirectory;
    [Tooltip("The name of your Anaconda Environment")]
    public string CondaEnvName;
    [Tooltip("Root directory where your yaml config files are stored")]
    public string MlAgentsConfigDirectory;
    [Tooltip("Path to the summary output directory")]
    public string SummariesDirectory;
    [Tooltip("Path to the NN file output directory")]
    public string ModelsDirectory;
    [Tooltip("Path to the config file output directory")]
    public string ConfigFileOutputDirectory;
    /// <summary>
    /// Starts the Conda Environment and executes ml-agents-learn
    /// </summary>
    public string LearnEnvExecute { get { return $"activate {CondaEnvName} ^&& ml-agents-learn"; } }
    /// <summary>
    /// Starts the Conda Environment and executes tensorboard
    /// </summary>
    public string TensorBoardExecute { get { return $"/K echo Tensorboard is running, do not close.. && {ActivateCondaEnv} python -m tensorboard.main "; } } //conda run -n {CondaEnvName} python -m tensorboard.main
    public string ActivateCondaEnv { get { return $"{AnacondaRootDirectory}\\Scripts\\conda.exe run -n {CondaEnvName}"; } }
}