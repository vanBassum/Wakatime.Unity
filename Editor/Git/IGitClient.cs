#if (UNITY_EDITOR)


namespace Wakatime
{
    public interface IGitClient
    {
        string GetBranchName(string path);
    }
}


#endif
