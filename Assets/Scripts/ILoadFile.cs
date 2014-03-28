
namespace UnityDemo.interfaces
{
    public interface ILoadFile
    {
        string Path { get; set; }
        int Version { get; set; }
        string Md5 { get; set; }
    }
}