#if (UNITY_EDITOR)


using System.Diagnostics;
using System;

namespace Wakatime
{
    public class GitCliClient : IGitClient
    {
        private Logger Logger { get; }
        public GitCliClient(Logger logger)
        {
            Logger = logger;
        }

        public string GetBranchName(string workingDir)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("git"); //No .exe, I assume this work on linux and macos.

                startInfo.UseShellExecute = false;
                startInfo.WorkingDirectory = workingDir;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.Arguments = "rev-parse --abbrev-ref HEAD";

                using Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                string branchname = process.StandardOutput.ReadLine();
                return branchname;
            }
            catch (Exception ex)
            {
                //Todo, figure out if git exists on this machine.
                //Also, figure out if this is even a git repo.
                Logger.Log(LogLevels.Warning, $"Couln't determine branchname, is git installed?\n {ex.Message}");
            }
            return null;
        }
    }
}


#endif
