using System;
using System.Collections.Generic;
using System.Diagnostics;

public static class CommandLineRunner
{
    static List<Process> processes = new List<Process>();
    public static string WorkingDirectory;

    public static Process StartCommandLine(string command, string workingDirectory,
        Action<Exception> errorCallBack = null)
    {
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = command;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = false;

            process.Start();
            return process;

        }
        catch (Exception e)
        {
            if (errorCallBack != null)
            {
                errorCallBack.Invoke(e);
            }
            return null;
        }
    }

    public static void CloseAllProcesses()
    {
        foreach (var process in processes)
        {
            //process.WaitForExit();
            process.Close();
        }
    }
}