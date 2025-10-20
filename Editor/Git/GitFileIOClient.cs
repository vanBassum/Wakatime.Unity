﻿#if (UNITY_EDITOR)


using System.IO;
using System;
using System.Threading.Tasks;

namespace Wakatime
{
    public class GitFileIOClient : IGitClient
    {
        private TaskCompletionSource<string> _getBranchNameTaskSource;
        
        private Logger Logger { get; }
        public GitFileIOClient(Logger logger) 
        { 
            Logger  = logger;
        }

        public ValueTask<string> GetBranchNameAsync(string path)
        {
            if (_getBranchNameTaskSource != null)
                return new ValueTask<string>(_getBranchNameTaskSource.Task);
            
            _getBranchNameTaskSource = new TaskCompletionSource<string>();
            var task = _getBranchNameTaskSource.Task;

            Task.Run(async () =>
            {
                var result = await GetBranchNameAsyncInternal(path);
                _getBranchNameTaskSource.SetResult(result);
            });
            return new ValueTask<string>(task);
        }


        private async Task<string> GetBranchNameAsyncInternal(string workingDir)
        {
            string gitDir = workingDir, lstDir = "", headFile;
            do
            {
                if (lstDir == gitDir)
                {
                    Logger.Log(LogLevels.Warning, "Couln't determine branchname, git is not initialized.");
                    return null;
                }
                lstDir = gitDir;
                headFile = Path.Combine(gitDir, ".git", "HEAD");
                gitDir = Path.GetFullPath(Path.Combine(gitDir, ".."));
            } while (!File.Exists(headFile));
            try
            {
                string headData = await File.ReadAllTextAsync(headFile);
                if (headData.StartsWith("ref: refs"))
                {
                    // Typically:
                    // ref: refs/xxx/branchname, xxx can be 'for' or 'heads'
                    int index, slash = 0;
                    for (index = 0; slash < 2 && index < headData.Length; index++)
                        if (headData[index] == '/')
                            slash++;
                    if (slash < 2)
                    {
                        // I'm not sure if there is a situation like this
                        Logger.Log(LogLevels.Warning, "Unknown git HEAD, please report this problem:\n" + headData);
                        return null;
                    }
                    return headData.Substring(index);
                }
                else if (headData.StartsWith("ref: "))
                {
                    // I'm not sure if there is a situation like this
                    Logger.Log(LogLevels.Warning, "Unknown git HEAD, please report this problem:\n" + headData);
                    return null;
                }
                else
                {
                    // plain commit id, the HEAD is detached
                    Logger.Log(LogLevels.Warning, "Couln't determine branchname, the git HEAD is in a detached state.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Warning, $"Couln't determine branchname, check if there is problem with file '{headFile}'.\n{ex.Message}");
            }
            return null;
        }
    }
}


#endif
