#if (UNITY_EDITOR)


using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace Wakatime
{
    public class GitCliClient : IGitClient
    {
        private TaskCompletionSource<string> _getBranchNameTaskSource;
        
        private Logger Logger { get; }
        public GitCliClient(Logger logger)
        {
            Logger = logger;
        }

        public ValueTask<string> GetBranchNameAsync(string workingDir)
        {
            if (_getBranchNameTaskSource != null)
                return new ValueTask<string>(_getBranchNameTaskSource.Task);

            _getBranchNameTaskSource = new TaskCompletionSource<string>();
            var task = _getBranchNameTaskSource.Task;

            Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo("git"); //No .exe, I assume this work on linux and macos.

                    startInfo.UseShellExecute = false;
                    startInfo.WorkingDirectory = workingDir;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.RedirectStandardInput = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.CreateNoWindow = true;
                    startInfo.Arguments = "rev-parse --abbrev-ref HEAD";

                    using Process process = new Process();
                    process.StartInfo = startInfo;
                    process.Start();

                    string branchname = process.StandardOutput.ReadLine();
                    _getBranchNameTaskSource.SetResult(branchname);
                }
                catch (Exception ex)
                {
                    //Todo, figure out if git exists on this machine.
                    //Also, figure out if this is even a git repo.
                    Logger.Log(LogLevels.Warning, $"Couln't determine branchname, is git installed?\n {ex.Message}");
                    // _getBranchNameTaskSource.SetException(ex);
                    _getBranchNameTaskSource.SetResult(null);
                }
            });

            return new ValueTask<string>(task);
        }
    }
}


#endif
