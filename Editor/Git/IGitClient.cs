using System.Threading.Tasks;

#if (UNITY_EDITOR)


namespace Wakatime
{
    public interface IGitClient
    {
        ValueTask<string> GetBranchNameAsync(string path);
    }
}


#endif
