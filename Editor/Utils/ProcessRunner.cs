#if (UNITY_EDITOR)
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEditor.PackageManager;

namespace Wakatime
{
    //https://github.com/wakatime/nuget-wakatime-shared-extension-utils/blob/0f376c8d6bbe9579ec72d47c122ad728c1965ae8/src/WakaTime.Shared.ExtensionUtils/RunProcess.cs
    public class ProcessRunner
    {
        private Logger Logger { get; set; }
        private readonly string _program;
        private readonly string _arguments;
        private string _stdin;
        private bool _captureOutput;

        internal ProcessRunner(Logger logger, string program, params string[] arguments)
        {
            _program = program;
            _arguments = arguments
                .Aggregate(string.Empty, (current, arg) => current + "\"" + arg + "\" ")
                .TrimEnd(' ');
            _captureOutput = true;
            Logger = logger;
        }

        internal void RunInBackground()
        {
            _captureOutput = false;
            Run();
        }

        internal void RunInBackground(string stdin)
        {
            _captureOutput = false;
            _stdin = stdin;
            Run();
        }

        internal void Run(string stdin)
        {
            _stdin = stdin;
            Run();
        }

        internal string Output { get; private set; }

        internal string Error { get; private set; }

        internal bool Success => Exception == null;

        private Exception Exception { get; set; }

        internal void Run()
        {
            try
            {
                var procInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardError = _captureOutput,
                    RedirectStandardOutput = _captureOutput,
                    RedirectStandardInput = _stdin != null,
                    FileName = _program,
                    CreateNoWindow = true,
                    Arguments = _arguments,
                };

                using (var process = Process.Start(procInfo))
                {
                    // run background process at lower priority to prevent lagging GUI
                    try
                    {
                        if (process != null) process.PriorityClass = ProcessPriorityClass.BelowNormal;
                    }
                    catch (Exception) { /* Might fail if not Admin */ }

                    if (_stdin != null)
                    {
                        process?.StandardInput.WriteLine($"{_stdin}\n");
                    }

                    if (_captureOutput)
                    {
                        var stdOut = new StringBuilder();
                        var stdErr = new StringBuilder();

                        if (process != null)
                        {
                            process.OutputDataReceived += (s, e) => stdOut.Append(e.Data);
                            process.ErrorDataReceived += (s, e) => stdErr.Append(e.Data);
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                            process.WaitForExit(1000 * 60 * 10); // 10 minutes
                        }

                        Output = stdOut.ToString().Trim(Environment.NewLine.ToCharArray()).Trim('\r', '\n');
                        Error = stdErr.ToString().Trim(Environment.NewLine.ToCharArray()).Trim('\r', '\n');
                    }

                    Exception = null;
                }
            }
            catch (Exception ex)
            {
                Output = null;
                Error = ex.Message;
                Exception = ex;
                Logger.Log(LogLevels.Warning, $"Exception when running process{_program}\n{ex.Message}");
            }
        }
    }
}


#endif
