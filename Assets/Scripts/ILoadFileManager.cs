
namespace UnityDemo.interfaces.manager
{
    public interface ILoadFileManager
    {
        ILoadFile getFile(string path);
        string getVersion(string path);
    }
}